using Mech.Models;
using Mech.ViewModels;
using Plugin.Geolocator;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace Mech.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MechanicDetails : ContentPage
    {
        Mechanic Mechanic = null;
        public MechanicDetails(Mechanic mechanic)
        {
            Mechanic = mechanic;
            this.BindingContext = Mechanic;
            InitializeComponent(); 
            InitMap();
        }

        private async void InitMap()
        {
            try
            {

            
            
            var locator = CrossGeolocator.Current;

            if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
            {
                var locatedPosition = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(5000));
               var myPosition = new Position(locatedPosition.Latitude, locatedPosition.Longitude);



                var myPin = new Pin();
                myPin.Position = myPosition;
                    myPin.Label = "You are here";

                var mechPin = new Pin(); 
                mechPin.Position = new Position(Mechanic.Latitude, Mechanic.Longitude);
                    mechPin.Label = Mechanic.Name + ", '" + Mechanic.Address + "'";

               var map = new Map(MapSpan.FromCenterAndRadius(myPosition, Distance.FromMiles(1)))
                {
                    IsShowingUser = true,
                    MapType = MapType.Hybrid
                };

                map.Pins.Add(myPin);
                map.Pins.Add(mechPin);

                MapGrid.Children.Clear();
                MapGrid.Children.Add(map);
            }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Map", ex.Message + "\n" + ex.Source, "OK");
            }
        }

        private async void OnCallClicked(object sender,EventArgs e)
        {
            var dialer = CrossMessaging.Current.PhoneDialer;
            if (dialer.CanMakePhoneCall)
            {
                dialer.MakePhoneCall(Mechanic.ContactNo);
            }

        }

        private async void OnSMSClicked(object sender, EventArgs e)
        {
            var messanger = CrossMessaging.Current.SmsMessenger;

            if (messanger.CanSendSms)
            {

                var config = new PromptConfig()
                {
                    OnTextChanged = args => {


                        args.IsValid = true;
                        args.Value = "I need your help.";

                    },
                    OkText = "Send",
                    CancelText = "Cancel",
                    Title = Mechanic.Name,
                    Text = "I need your help." 
                    
                };
                string messageToSend = string.Empty;
                using (UserDialogs.Instance.Prompt(config))
                {
                    messageToSend = config.Text;
                }

                messanger.SendSms(Mechanic.ContactNo, messageToSend);

            }

        }

    }
}