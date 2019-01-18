using Mech.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mech.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationsPage : ContentPage
	{
		public NotificationsPage ()
		{
			InitializeComponent ();
		}

        

        private async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var contract = e.Item as ContractItem;

            if (contract != null)
            {
                var vm = new ContractDetailsViewModel(contract);
                var page = new ContractDetailsPage();
                page.BindingContext = vm;

                await Navigation.PushAsync(page);
            }
        }
    }
}