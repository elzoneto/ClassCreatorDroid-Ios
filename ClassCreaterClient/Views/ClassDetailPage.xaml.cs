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
    public partial class ClassDetailPage : ContentPage
    {
        Class theClass;
        App thisApp;

        public ClassDetailPage()
        {
            InitializeComponent();
            thisApp = Application.Current as App;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            theClass = (Class)this.BindingContext;
            setClassType();

            // Start adding a new class
            if (theClass.ID == 0)
            {
                this.Title = "New Class";
                btnDelete.IsEnabled = false;
            }
            else
            {
                this.Title = "Edit Class";
                btnDelete.IsEnabled = true;
            }
        }

        private void setClassType()
        {
            int classTypeAssigned = 0;
            int count = 0;
            foreach (ClassType item in thisApp.AllClassTypes.OrderBy(n => n.Name))
            {
                pickerClassType.Items.Add(item.Name);

                if (item.ID == theClass.TypeID)
                {
                    classTypeAssigned = count;
                }

                count++;
            }

            if (theClass.Type != null)
            {
                pickerClassType.SelectedItem = theClass.Type.Name;
            }
            else
            {
                pickerClassType.SelectedItem = "Select a class Type";
            }
        }

        int getClassTypeID()
        {
            if (pickerClassType.SelectedIndex == -1)
            {
                return -1;
            }

            string selectedType = pickerClassType.Items[pickerClassType.SelectedIndex];
            if (selectedType != "Select a class Type")
            {
                return thisApp.AllClassTypes.Where(t => t.Name == selectedType).SingleOrDefault().ID;
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
                theClass.TypeID = getClassTypeID();
                if (theClass.TypeID > 0)
                {
                    ClassRepository cl = new ClassRepository();
                    if (theClass.ID == 0)
                    {
                        await cl.AddClass(theClass);
                    }
                    else
                    {
                        await cl.UpdateClass(theClass);
                    }
                    thisApp.needClassRefresh = true;
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Class Type Not Selected:", "You must set the type of your class.", "Ok");
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
                thisApp.needClassRefresh = true;
                await DisplayAlert("Problem Saving the Class:", sb.ToString(), "Ok");
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
            var answer = await DisplayAlert("Confirm Delete", "Are you certain that you want to delete " + theClass.Name + "?", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    ClassRepository cl = new ClassRepository();
                    await cl.DeleteClass(theClass);
                    thisApp.needClassRefresh = true;
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
                    await DisplayAlert("Problem Deleting the Class:", sb.ToString(), "Ok");
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