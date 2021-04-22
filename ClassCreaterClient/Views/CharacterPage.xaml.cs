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
    public partial class CharacterPage : ContentPage
    {
        App thisApp;
        List<Class> classes;

        public CharacterPage()
        {
            InitializeComponent();
            classes = new List<Class>();
            thisApp = Application.Current as App;
            thisApp.needCharacterRefresh = true;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await showClasses();
            if (thisApp.needCharacterRefresh)
            {
                refreshCharacters();
            }
            else
            {
                characterList.IsVisible = true;
            }
            characterList.SelectedItem = null;
        }

        private void refreshCharacters()
        {
            thisApp = Application.Current as App;
            if (pickerClasses.SelectedIndex < 1)
            {
                showCharacters(null);
            }
            else
            {
                string selectedClass = pickerClasses.Items[pickerClasses.SelectedIndex];
                int? id = thisApp.AllClasses.Where(c => c.Name == selectedClass)
                    .SingleOrDefault().ID;
                showCharacters(id.GetValueOrDefault());
            }
            thisApp.needCharacterRefresh = false;
        }

        private async Task showClasses()
        {
            if (!(thisApp.AllClasses?.Count > 0))
            {
                ClassRepository c = new ClassRepository();
                try
                {
                    classes.Add(new Class { ID = 0, Name = "All Classes" });
                    thisApp.AllClasses = await c.GetClasses();
                    foreach (Class item in thisApp.AllClasses.OrderBy(cl => cl.Name))
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

        private async void showCharacters(int? ClassID)
        {
            CharacterRepository ch = new CharacterRepository();
            try
            {
                List<Character> characters;
                if (ClassID.GetValueOrDefault() > 0)
                {
                    characters = await ch.GetCharactersByClass(ClassID.GetValueOrDefault());
                }
                else
                {
                    characters = await ch.GetCharacters();
                    charactersResult.Text = "Characters";
                }
                characterList.ItemsSource = characters;
                characterList.IsVisible = true;
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
            refreshCharacters();
        }

        private void AddClicked(object sender, EventArgs e)
        {
            Character newCharacter = new Character();
            var characterDetailPage = new CharacterDetailsPage();
            characterDetailPage.BindingContext = newCharacter;
            characterList.IsVisible = false;
            Navigation.PushAsync(characterDetailPage);
        }

        private void characterSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var characterSelected = (Character)e.SelectedItem;
                var characterDetailPage = new CharacterDetailsPage();
                characterDetailPage.BindingContext = characterSelected;
                characterList.IsVisible = false;
                Navigation.PushAsync(characterDetailPage);
            }
        }
    }
}