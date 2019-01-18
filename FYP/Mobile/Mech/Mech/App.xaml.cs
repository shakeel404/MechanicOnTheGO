using System;

using Mech.Views;
using Xamarin.Forms;
using Plugin.Settings;
using Mech.Services;
using Mech.Utils;
using Plugin.Connectivity;
using Acr.UserDialogs;

namespace Mech
{
	public partial class App : Application
	{

		public App ()
		{
			InitializeComponent();

            ViewModels.BaseViewModel baseViewModel = new ViewModels.BaseViewModel();



            var exist = LocalStorage.IsExist;
            var isMechanic = LocalStorage.Registration == Server.MECHANIC;

            

            if (exist && isMechanic)
            {
                MainPage = new NavigationPage(new DashBoard());
            }
            else
            {
                MainPage =  new NavigationPage(new HomePage());
            }


        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
