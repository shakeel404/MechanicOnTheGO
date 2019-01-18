using GEO = Geocoding;
using GEOGoogle = Geocoding.Google;
using Plugin.Geolocator;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Plugin.Connectivity;
using System.Net.Http;
using Newtonsoft.Json;
using Mech.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using Mech.Services;
using Plugin.Settings;
using Mech.Utils;

namespace Mech.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        private Mechanic SelectedMechanic { get; set; }
        
         Map MechMap = null;

        private int ViewIndex = 1;
        public MainPage()
        {
            InitializeComponent();
            Mechanics = new ObservableCollection<Mechanic>();
            NavigationPage.SetHasNavigationBar(this, true);

            var exist = CrossSettings.Current.GetValueOrDefault(Server.ISEXIST, false);

            if (exist)
            {
                this.LogoutButton.IsVisible = true;
                this.LoginButton.IsVisible = false;
            }
            else
            {
                this.LogoutButton.IsVisible = false;
                this.LoginButton.IsVisible = true;
            }

        }

        private bool IsLoaded { get; set; }
        private ObservableCollection<Mechanic> Mechanics { get; set; }

         

 

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!IsLoaded)
            {
                InitMap();
            }
            IsLoaded = true;
        }

         

        private  void RefreshMap(object sender, EventArgs e)
        {
            InitMap();
        }

        public  void OnToolBarItemClicked(object sender,EventArgs e)
        {
            if (MechMap == null)
                return;

            
            
            switch (ViewIndex)
            {
                case 1:
                    MechMap.MapType = MapType.Satellite;
                    ViewToolBar.Icon = "satellite.png"; 
                    ViewIndex++;
                    break;
                case 2:
                    MechMap.MapType = MapType.Hybrid;
                    ViewToolBar.Icon = "hybrid.png";

                    ViewIndex++;
                    break;
                case 3:
                    MechMap.MapType = MapType.Street;
                    ViewToolBar.Icon = "mark.png";
                    ViewIndex = 1;
                    break;
                default:
                    break;

            }


        }

        //public  void OnClearClicked(object sender,EventArgs e)
        //{
        //    CrossSettings.Current.Clear();
        //}
        private async void InitMap()
        {
            var position = new Position();

            var dialog = UserDialogs.Instance.Loading("Loading Map...");
            
                try
                {
                dialog.Title = "Locating your position";
                    var locator = CrossGeolocator.Current;
                
                    if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                    {
                       var locatedPosition = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(10000));

                    // For Test Purpose Only
                    //34.78680923/72.35325037/
                   // var mylat = 34.78680923d;
                  // var mylong = 72.35325037d;

                    position = new Position(locatedPosition.Latitude, locatedPosition.Longitude);

                        /// For Test Purpos Only 
                       //  position = new Position(mylat, mylong);


                        MechMap = new Utils.MechMap(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1)))
                        {
                            IsShowingUser = true,
                            MapType = MapType.Street
                        };


                        MechMap.HorizontalOptions = LayoutOptions.Fill;
                        MechMap.VerticalOptions = LayoutOptions.Fill;  
                        MapGrid.Children.Add(MechMap);
                        MechMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1)));

                    }
                    else
                    {
                        AddressesText.Text = "Location not available";
                    }

                }
                catch (Exception ex)
                {
                AddressesText.Text = "GPS is unable to locate your location.";
                }

            try
            {
                dialog.Title = "Searching Mechanics...";
                if (CrossConnectivity.Current.IsConnected)
                {
                    var client = new HttpClient();
                    var server = new Server();

                    var EndPoint = string.Format(server.NearestMechanicEndPoint, position.Latitude, position.Longitude); 

                    var response = await client.GetAsync(EndPoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<Mechanic>>(content);

                        Mechanics = new ObservableCollection<Mechanic>(list);


                        foreach(var mech in Mechanics)
                        {
 
                            Pin pin = new Pin()
                            {
                                Position = new Position(mech.Latitude, mech.Longitude),
                                Address = mech.Address,
                                 Label = $"{mech.Name}, {mech.Title}",
                                 Type= PinType.Place
                            };
                            pin.BindingContext = mech;
                            pin.Clicked += Pin_Clicked; 

                            if(MechMap != null)
                            {
                                MechMap.Pins.Add(pin);
                            }
                            
                        }
                    }
                    else
                    {
                       await DisplayAlert("Mech Server", "Service not available", "OK");
                    }
                }
                else
                {
                    dialog.Title = "Connection Problem";
                    await DisplayAlert("Internet", "Please connect to internet", "OK");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Mech Server", "Unable to connect server", "OK");
            } 

            try
            {
                dialog.Title = "Searching your address";
                if (CrossConnectivity.Current.IsConnected)
                {
                    var geocoder = new GEOGoogle.GoogleGeocoder();

                    var addresses = await geocoder.ReverseGeocodeAsync(position.Latitude, position.Longitude);

                    var approximateAddress = addresses.FirstOrDefault();

                    if (approximateAddress != null)
                    {
                        this.AddressesText.Text = approximateAddress.FormattedAddress;
                    }
                    else
                    {
                        this.AddressesText.Text = "Address not found for this location";
                    }
                }
                else
                {
                    this.AddressesText.Text = "Please connect to internet to locate your location address.";
                }
                
            }
            catch (Exception ex)
            {
                this.AddressesText.Text = "Address not found.";
            }

            dialog.Dispose();
        }

        private async void Pin_Clicked(object sender, EventArgs e)
        {
           
            Pin pin = sender as Pin;

            if (pin == null)
                return;


            if (LocalStorage.IsExist)
            {
                SelectedMechanic = pin.BindingContext as Mechanic;

                ContractPage contractPage = new ContractPage();

                contractPage.BindingContext = SelectedMechanic;

                contractPage.Init();
                await Navigation.PushAsync(new NavigationPage(contractPage));
            }
            else
            {
              await  Navigation.PushAsync(new CustomerRegistrationPage());
            }
           
          
        } 


        private async void OnLogOutClicked(object sender, EventArgs e)
        {
            CrossSettings.Current.AddOrUpdateValue(Server.ISEXIST, false);
            this.LogoutButton.IsVisible = false;
            this.LoginButton.IsVisible = true; 
        }

        private async void OnLoginClicked(object sender,EventArgs e)
        {
            await Navigation.PushAsync(new TermsAndConditions());
        }

        //private async void OnCallClicked(object sender,EventArgs e)
        //{
        //    if (SelectedMechanic == null)
        //        return;


        //   if(Plugin.Messaging.CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
        //    {
        //        Plugin.Messaging.CrossMessaging.Current.PhoneDialer.MakePhoneCall(SelectedMechanic.ContactNo);
        //    }
        //    else
        //    {
        //        await DisplayAlert("Mech", "Can not make call ", "OK");
        //    }

        //}
        //private async void OnSmsClicked(object sender,EventArgs e)
        //{
        //    //if (SelectedMechanic == null)
        //    //    return;

        //    //if (Plugin.Messaging.CrossMessaging.Current.SmsMessenger.CanSendSms)
        //    //{
        //    //    Plugin.Messaging.CrossMessaging.Current.SmsMessenger.SendSms(SelectedMechanic.ContactNo,"Hello, I need your help");
        //    //}
        //    //else
        //    //{
        //    //    await DisplayAlert("Mech", "Can not send message ", "OK");
        //    //}

        //    await Navigation.PushAsync(new Chat());
        //}


    }




}