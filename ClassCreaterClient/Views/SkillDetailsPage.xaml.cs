using ClassCreaterClient.Data;
using ClassCreaterClient.Models;
using ClassCreaterClient.Moldes;
using ClassCreaterClient.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassCreaterClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkillDetailsPage : ContentPage
    {
        Skill skill;
        App thisApp;

        public SkillDetailsPage()
        {
            InitializeComponent();
            thisApp = Application.Current as App;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            skill = (Skill)this.BindingContext;
            setClass();

            // Add new Character
            if (skill.ID == 0)
            {
                this.Title = "New Skill Creation";
                btnDelete.IsEnabled = false;
            }
            else
            {
                this.Title = "Edit Skill Menu";
                btnDelete.IsEnabled = true;
            }
        }

        private void setClass()
        {
            int classAssigned = 0;
            int count = 0;
            foreach (Class item in thisApp.AllClassesToSkills.OrderBy(c => c.Name))
            {
                pickerClass.Items.Add(item.Name);

                if (item.ID == skill.ClassID)
                {
                    classAssigned = count;
                }

                count++;
            }

            if (skill.Class != null)
            {
                pickerClass.SelectedItem = skill.Class.Name;
            }
            else
            {
                pickerClass.SelectedItem = "Select Skill Class";
            }
        }

        int getSkillClassID()
        {
            if (pickerClass.SelectedIndex == -1)
            {
                return -1;
            }

            string selectedClass = pickerClass.Items[pickerClass.SelectedIndex];
            if (selectedClass != "Select Skill Class")
            {
                return thisApp.AllClassesToSkills.Where(cl => cl.Name == selectedClass).SingleOrDefault().ID;
            }
            else
            {
                return 0;
            }
        }

        async void SaveClicked(object sender, EventArgs e)
        {
            try
            {
                skill.ClassID = getSkillClassID();
                if (skill.ClassID > 0)
                {
                    SkillRepository s = new SkillRepository();
                    if (skill.ID == 0)
                    {
                        await s.AddSkill(skill);
                    }
                    else
                    {
                        await s.UpdateSkill(skill);
                    }
                    thisApp.needClassRefresh = true;
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Class Not Selected:", "You must set the class.", "Ok");
                }
            }
            catch (AggregateException ex)
            {
                string errMsg = "";
                foreach (var exception in ex.InnerExceptions)
                {
                    errMsg += Environment.NewLine + exception.Message;
                }
                await DisplayAlert("One or more exceptions has occurred:", errMsg, "Ok");
            }
            catch (ApiException apiEx)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Errors:");
                foreach (var error in apiEx.Errors)
                {
                    sb.AppendLine("-" + error);
                }
                thisApp.needSkillRefresh = true;
                await DisplayAlert("Problem Saving the Skill:", sb.ToString(), "Ok");
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    await DisplayAlert("Error", "No connection with the server.", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "Could not complete operation.", "Ok");
                }
            }
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        async void DeleteClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirm Delete", "Are you certain that you want to delete " + skill.SkillName + "?", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    SkillRepository s = new SkillRepository();
                    await s.DeleteSkill(skill);
                    thisApp.needSkillRefresh = true;
                    await Navigation.PopAsync();
                }
                catch (AggregateException ex)
                {
                    string errMsg = "";
                    foreach (var exception in ex.InnerExceptions)
                    {
                        errMsg += Environment.NewLine + exception.Message;
                    }
                    await DisplayAlert("One or more exceptions has occurred:", errMsg, "Ok");
                }
                catch (ApiException apiEx)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Errors:");
                    foreach (var error in apiEx.Errors)
                    {
                        sb.AppendLine("-" + error);
                    }
                    await DisplayAlert("Problem Deleting the Skill:", sb.ToString(), "Ok");
                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException().Message.Contains("connection with the server"))
                    {
                        await DisplayAlert("Error", "No connection with the server.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Could not complete operation.", "Ok");
                    }
                }
            }
        }
    }
}