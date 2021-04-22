using ClassCreaterClient.Data;
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
    public partial class CharacterDetailsPage : ContentPage
    {
        Character character;
        App thisApp;

        public CharacterDetailsPage()
        {
            InitializeComponent();
            thisApp = Application.Current as App;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            character = (Character)this.BindingContext;
            setClass();

            // Add new Character
            if (character.ID == 0)
            {
                this.Title = "New Character Creation";
                btnDelete.IsEnabled = false;
                pickerGender.SelectedItem = "Male";
                pickerRace.SelectedItem = "Human";
            }
            else
            {
                this.Title = "Edit Character Menu";
                pickerGender.SelectedItem = character.Gender;
                pickerRace.SelectedItem = character.Race;
                btnDelete.IsEnabled = true;
            }
        }

        private void setClass()
        {
            int classAssigned = 0;
            int count = 0;
            foreach (Class item in thisApp.AllClasses.OrderBy(c => c.Name))
            {
                pickerClass.Items.Add(item.Name);

                if (item.ID == character.ClassID)
                {
                    classAssigned = count;
                }

                count++;
            }

            if (character.Class != null)
            {
                pickerClass.SelectedItem = character.Class.Name;
            }
            else
            {
                pickerClass.SelectedItem = "Select Character Class";
            }
        }

        int getCharacterClassID()
        {
            if (pickerClass.SelectedIndex == -1)
            {
                return -1;
            }

            string selectedClass = pickerClass.Items[pickerClass.SelectedIndex];
            if (selectedClass != "Select Character Class")
            {
                return thisApp.AllClasses.Where(cl => cl.Name == selectedClass).SingleOrDefault().ID;
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
                character.ClassID = getCharacterClassID();
                character.Race = pickerRace.SelectedItem.ToString();
                character.Gender = pickerGender.SelectedItem.ToString();
                if (character.ClassID > 0)
                {
                    CharacterRepository ch = new CharacterRepository();
                    if (character.ID == 0)
                    {
                        await ch.AddCharacter(character);
                    }
                    else
                    {
                        await ch.UpdateCharacter(character);
                    }
                    thisApp.needCharacterRefresh = true;
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
                thisApp.needCharacterRefresh = true;
                await DisplayAlert("Problem Saving the Character:", sb.ToString(), "Ok");
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
            var answer = await DisplayAlert("Confirm Delete", "Are you certain that you want to delete " + character.CharacterName + "?", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    CharacterRepository ch = new CharacterRepository();
                    await ch.DeleteCharacter(character);
                    thisApp.needCharacterRefresh = true;
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
                    await DisplayAlert("Problem Deleting the Character:", sb.ToString(), "Ok");
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