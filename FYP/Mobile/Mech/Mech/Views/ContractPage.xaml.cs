using Acr.UserDialogs; 
using Mech.Models;
using Mech.Services;
using Mech.Utils;
using Microsoft.AspNet.SignalR.Client;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Messaging;
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
    public partial class ContractPage : ContentPage
    {
        public ContractPage()
        {
            InitializeComponent();

            Connection = new HubConnection(Server.HubEndPoint);

            Proxy = Connection.CreateHubProxy(Server.ContractHub);
            //MessagingCenter.Subscribe<object, object>(this, "MessageReceived", (sender, arg) => {
            //    //ChatListView.ScrollTo(arg, ScrollToPosition.End, true);
            //}); 
        }


        private async void OnHelpClicked(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;
            Position position = null;
            if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
            {
                 position = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(10000));
                

                //Test Purpose only
               // position = new Position(34.82917452d, 72.35345203d);
            }
            else
            {
                position = new Position(LocalStorage.Latitude, LocalStorage.Longitude);
            }

            await Proxy.Invoke("SendRequest", LocalStorage.Name, Mechanic.ContactNo, position.Latitude, position.Longitude);
            HelpButton.Image = "";
            HelpButton.IsEnabled = false;
            HelpButton.FontSize = 12;
            HelpButton.Text = "Sent";


        }


        private HubConnection Connection { get; set; }
        private IHubProxy Proxy { get; set; }

        private string ConnectionTo { get; set; }
        private string CustomerConnection { get; set; }
        private int Amount { get; set; }
        private int Initial_Charges { get; set; }
        private int PerKmRate { get; set; }
        private int PerHourRate { get; set; }
        private int KM { get; set; }
        private Mechanic Mechanic { get; set; }

        public async void Init()
        {
            Mechanic = this.BindingContext as Mechanic;

            var Id = LocalStorage.Id;

            var server = new Server(); 

            Proxy.On<int, int, int, int>("conferm", (initialCharges, chargesPerKM, chargesPerHour, km) => { 

                Device.BeginInvokeOnMainThread(() => {

                    int total = initialCharges + ((chargesPerKM*km) * 2) + chargesPerHour;

                    Amount = total;
                    Initial_Charges = initialCharges;
                    PerKmRate = chargesPerKM;
                    PerHourRate = chargesPerHour;
                    KM = km;
                    var aggrementText = $"{Mechanic.Name} has accepted your request and want an aggrement based on service charges. The service charges he proposed is {Amount} PKR.\nThis means you will pay {Amount} to {Mechanic.Name} once you accept the aggrement.";
                    var aggrementTitle = $"{Mechanic.Name}  ({km} Kilometer Away)";
                    this.AggrementText.Text = aggrementText;
                    this.AggrementTitle.Text = aggrementTitle;
                    this.InitialCharges.Text = initialCharges.ToString();
                    this.ChargesPerKm.Text = chargesPerKM.ToString();
                    this.ChargesPerHour.Text = chargesPerHour.ToString();
                    this.TotalCharges.Text = Amount.ToString();
                    AggrementFrame.IsVisible = true;
                    AggrementAcceptanceFrame.IsVisible = true;

                });

            });

            Proxy.On("accepted", () => {

                Device.BeginInvokeOnMainThread(() =>
                {



                    DisplayAlert("Agreement", "You and " + Mechanic.Name + " have accepted the aggrement", "OK");

                    CallGrid.IsVisible = true;
                    SmsGrid.IsVisible = true;
                    AggrementAcceptanceFrame.IsVisible = false;


                });
            });

            Proxy.On("discard", () => {

                Device.BeginInvokeOnMainThread(() =>
                {



                    DisplayAlert("Request", Mechanic.Name + " is unavailable right now", "OK");

                    CallGrid.IsVisible = false;
                    SmsGrid.IsVisible = false;
                    AggrementAcceptanceFrame.IsVisible = false;


                });

            });
            Connection.StateChanged += Connection_StateChanged;
            await Connection.Start();
        }

        private async void OnCallClicked(object sender, EventArgs e)
        {
            if (CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
            {
                CrossMessaging.Current.PhoneDialer.MakePhoneCall(Mechanic.ContactNo);
            }
        }

        private async void OnSmsClicked(object sender, EventArgs e)
        {
            if (CrossMessaging.Current.SmsMessenger.CanSendSms)
            {
                var name = LocalStorage.Name;

                CrossMessaging.Current.SmsMessenger.SendSms(Mechanic.ContactNo, $"Hello {Mechanic.Name}, My name is {name}. I need your service, Please contact me via app");
            }
        }

        private void Connection_StateChanged(StateChange obj)
        {
            if (obj == null)
            {
                return;
            }

            Device.BeginInvokeOnMainThread(() => {

               RequestFrame.IsVisible = HelpGrid.IsVisible = obj.NewState == ConnectionState.Connected;

            });
        }

        private void OnAcceptRequest(object sender, EventArgs e)
        {
            //AcceptRequest
            Proxy.Invoke("AcceptRequest", Mechanic.Id, LocalStorage.Id, Initial_Charges,PerKmRate,PerHourRate,KM);
            this.RequestFrame.IsVisible = false;
            this.AggrementFrame.IsVisible = false;
            this.AggrementAcceptanceFrame.IsVisible = false;
        }


    }
}