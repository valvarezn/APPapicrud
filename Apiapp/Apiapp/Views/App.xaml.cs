using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Apiapp.API;

namespace Apiapp
{
    public partial class App : Application
    {
        public static API.HttpClient RestClient { get; private set; }
        public static string BaseUrl { get; private set; }

        public App()
        {
            InitializeComponent();

            App.RestClient = new API.HttpClient(new Dictionary<string, string>
            {
                {"API-key", "9a004c81-29e7-4d22-a9c4-f1e0763bc7d0"}
            });

            App.BaseUrl = "http://192.168.1.69/API/index.php";

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            MainPage = new NavigationPage(new UserPage());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
