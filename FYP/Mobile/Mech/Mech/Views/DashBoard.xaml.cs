
using Acr.UserDialogs;
using Mech.Models;
using Mech.Services;
using Mech.Utils;
using Mech.ViewModels;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Mech.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DashBoard : TabbedPage
	{
        public DashBoard()
        {
            InitializeComponent();

            //MessagingCenter.Subscribe<object, object>(this, "MessageReceived", (sender, arg) =>
            //{
            //  //  ChatListView.ScrollTo(arg, ScrollToPosition.End, true);
            //});

            MessagingCenter.Subscribe<object>(this, "Requested", (sender) =>
            {
                this.CurrentPage = this.Children[1];
            });

            MessagingCenter.Subscribe<object>(this, "Accepted", (sender) =>
            {
                this.CurrentPage = this.Children[2];
            }); 
        }
        

        private async void OnEditClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditMechanicPage());
        }



        //private  void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var contract = e.SelectedItem as ContractItem; 

        //    if (contract != null)
        //    {
        //        ConfirmConfig config = new ConfirmConfig()
        //        {
        //            Title="Mech Service",
        //            Message="Did you recieved service charges?",
        //             OkText = "Yes, Recieved",
        //             CancelText="No",
        //             OnAction = OnRecieved,

        //        };
        //     UserDialogs.Instance.Confirm(config); 
        //    }

        //}


        //private async void OnRecieved( bool yes)
        //{
        //    if (yes)
        //    { 
        //        var client = new HttpClient();
        //        var server = new Server();
        //        var contract = this.ContractsListView.SelectedItem as ContractItem;

        //        var id = contract.Id;

        //        var endpoint = $"{server.MechanicContractEndPoint}/{id}";


        //        var response = await client.PutAsync(endpoint,null);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            contract.IsConfirmed = true;
        //        }
        //    }


        //}
    }
}