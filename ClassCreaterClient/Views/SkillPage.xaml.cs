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
    public partial class SkillPage : ContentPage
    {
        App thisApp;
        List<Class> classes;

        public SkillPage()
        {
            InitializeComponent();
            classes = new List<Class>();
            thisApp = Application.Current as App;
            thisApp.needSkillRefresh = true;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await showClasses();
            if (thisApp.needSkillRefresh)
            {
                refreshSkills();
            }
            else
            {
                skillList.IsVisible = true;
            }
            skillList.SelectedItem = null;
        }

        private void refreshSkills()
        {
            thisApp = Application.Current as App;
            if (pickerClasses.SelectedIndex < 1)
            {
                showSkills(null);
            }
            else
            {
                string selectedClass = pickerClasses.Items[pickerClasses.SelectedIndex];
                int? id = thisApp.AllClassesToSkills.Where(c => c.Name == selectedClass)
                    .SingleOrDefault().ID;
                showSkills(id.GetValueOrDefault());
            }
            thisApp.needSkillRefresh = false;
        }

        private async Task showClasses()
        {
            if (!(thisApp.AllClassesToSkills?.Count > 0))
            {
                ClassRepository c = new ClassRepository();
                try
                {
                    classes.Add(new Class { ID = 0, Name = "All Classes" });
                    thisApp.AllClassesToSkills = await c.GetClasses();
                    foreach (Class item in thisApp.AllClassesToSkills.OrderBy(cl => cl.Name))
                    {
                        classes.Add(item);
                    }

                    pickerClasses.ItemsSource = classes;
                    pickerClasses.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.GetBaseException().Message.Contains("connection with the server"))
                        {

                            await DisplayAlert("Error", "No connection with the server. Check that the Web Service is running and available and then click the Refresh button.", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Error", "If the problem persists, please call your system administrator.", "Ok");
                        }
                    }
                    else
                    {
                        if (ex.Message.Contains("NameResolutionFailure"))
                        {
                            await DisplayAlert("Internet Access Error ", "Cannot resolve the Uri: " + Jeeves.DBUri.ToString(), "Ok");
                        }
                        else
                        {
                            await DisplayAlert("General Error ", ex.Message, "Ok");
                        }

                    }
                }
            }
        }

        private async void showSkills(int? ClassID)
        {
            SkillRepository s = new SkillRepository();
            try
            {
                List<Skill> skills;
                if (ClassID.GetValueOrDefault() > 0)
                {
                    skills = await s.GetSkillByClass(ClassID.GetValueOrDefault());
                }
                else
                {
                    skills = await s.GetSkills();
                }
                skillList.ItemsSource = skills;
                skillList.IsVisible = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.GetBaseException().Message.Contains("connection with the server"))
                    {

                        await DisplayAlert("Error", "No connection with the server. Check that the Web Service is running and available and then click the Refresh button.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Error", "If the problem persists, please call your system administrator.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("General Error", "If the problem persists, please call your system administrator.", "Ok");
                }

            }
        }

        private void pickerClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshSkills();
        }

        private void AddClicked(object sender, EventArgs e)
        {
            Skill newSkill = new Skill();
            var skillDetailPage = new SkillDetailsPage();
            skillDetailPage.BindingContext = newSkill;
            skillList.IsVisible = false;
            Navigation.PushAsync(skillDetailPage);
        }

        private void skillSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var skillSelected = (Skill)e.SelectedItem;
                var skillDetailPage = new SkillDetailsPage();
                skillDetailPage.BindingContext = skillSelected;
                skillList.IsVisible = false;
                Navigation.PushAsync(skillDetailPage);
            }
        }
    }
}