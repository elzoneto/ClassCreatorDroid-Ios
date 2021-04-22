using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassCreaterClient.Views;
using System.Collections.Generic;
using ClassCreaterClient.Moldes;
using ClassCreaterClient.Models;

namespace ClassCreaterClient
{
    public partial class App : Application
    {
        public List<ClassType> AllClassTypes;
        public List<Class> AllClasses;
        public List<Class> AllClassesToSkills;
        public List<Character> AllCharacters;
        public List<Skill> AllSkills;

        public bool needClassRefresh;
        public bool needCharacterRefresh;
        public bool needSkillRefresh;

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            //var nav = new NavigationPage(new ClassPage());
            //MainPage = nav;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
