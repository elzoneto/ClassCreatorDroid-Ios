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
    public partial class ClassPage : ContentPage
    {
        App thisApp;
        List<ClassType> classTypes;

        public ClassPage()
        {
            InitializeComponent();
            classTypes = new List<ClassType>();
            thisApp = Application.Current as App;
            thisApp.needClassRefresh = true;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (thisApp.needClassRefresh)
            {
                refreshClasses();
            }
            else
            {
                classList.IsVisible = true;
            }
            classList.SelectedItem = null;
        }

        private async void refreshClasses()
        {
            thisApp = Application.Current as App;
            showClasses();
            classResult.Text = "All Classes";

            ClassTypeRepository ct = new ClassTypeRepository();

            try
            {
                classTypes.Add(new ClassType { ID = 0, Name = "All Class Types" });
                thisApp.AllClassTypes = await ct.GetClassTypes();
                foreach (ClassType c in thisApp.AllClassTypes.OrderBy(n => n.Name))
                {
                    classTypes.Add(c);
                }

                // I decide not to have a picker on class view but buttons
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

        private async void showClasses()
        {
            // Getting all Classes
            ClassRepository c = new ClassRepository();
            try
            {
                List<Class> classes;
                classes = await c.GetClasses();
                
                classList.ItemsSource = classes;
                classList.IsVisible = true;
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

        private async void Tank_Clicked(object sender, EventArgs e)
        {
            ClassRepository c = new ClassRepository();
            try
            {
                List<Class> tankClasses;
                tankClasses = await c.GetClassesByTypes(1);

                classList.ItemsSource = tankClasses;
                classList.IsVisible = true;
                classResult.Text = "Tank Classes";
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

        private async void Melee_Clicked(object sender, EventArgs e)
        {
            ClassRepository c = new ClassRepository();
            try
            {
                List<Class> meleeClasses;
                meleeClasses = await c.GetClassesByTypes(3);

                classList.ItemsSource = meleeClasses;
                classList.IsVisible = true;
                classResult.Text = "Melee Classes";
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

        private async void Range_Clicked(object sender, EventArgs e)
        {
            ClassRepository c = new ClassRepository();
            try
            {
                List<Class> rangeClasses;
                rangeClasses = await c.GetClassesByTypes(4);

                classList.ItemsSource = rangeClasses;
                classList.IsVisible = true;
                classResult.Text = "Range Classes";
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

        private async void Healer_Clicked(object sender, EventArgs e)
        {
            ClassRepository c = new ClassRepository();
            try
            {
                List<Class> healerClasses;
                healerClasses = await c.GetClassesByTypes(2);

                classList.ItemsSource = healerClasses;
                classList.IsVisible = true;
                classResult.Text = "Healer Classes";
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

        private void AddClicked(object sender, EventArgs e)
        {
            Class newClass = new Class();
            var classDetailPage = new ClassDetailPage();
            classDetailPage.BindingContext = newClass;
            classList.IsVisible = false;
            Navigation.PushAsync(classDetailPage);
        }

        private void classSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var classSelected = (Class)e.SelectedItem;
                var classDetailPage = new ClassDetailPage();
                classDetailPage.BindingContext = classSelected;
                classList.IsVisible = false;
                Navigation.PushAsync(classDetailPage);
            }
        }

    }
}