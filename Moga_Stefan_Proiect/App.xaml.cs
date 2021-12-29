//using Moga_Stefan_Proiect.Services;
using Moga_Stefan_Proiect.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Moga_Stefan_Proiect
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();


            MainPage = new AppShell();
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
