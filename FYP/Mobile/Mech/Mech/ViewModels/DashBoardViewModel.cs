using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using Geocoding;
using Mech.Services;
using Mech.Utils;
using Mech.Views;
using Microsoft.AspNet.SignalR.Client;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Settings;
using Prism.Commands;
using Xamarin.Forms;

namespace Mech.ViewModels
{
    public class DashBoardViewModel : BaseViewModel
    {
        public DashBoardViewModel()
        {
            LogoutCommand = new DelegateCommand(Logout);
            DiscardCommand = new DelegateCommand(OnDisCard);
            ConfermationCommand = new DelegateCommand(SendConfermation, CanSendConfermation);
            
            Connection = new HubConnection(Server.HubEndPoint);

            Proxy = Connection.CreateHubProxy(Server.ContractHub);

            Connection.StateChanged += Connection_StateChanged;
            Init();

        }

        private void Connection_StateChanged(StateChange obj)
        {
            if (obj == null)
            {
                return;
            }

            if (obj.NewState == ConnectionState.Connected)
            {
                Proxy.Invoke("Connect", this.ContactNo);
            }
        }

        private HubConnection Connection { get; set; }
        private IHubProxy Proxy { get; set; }
        string title;
        string name;
        string contactno;
        string address;
        double _lat;
        double _long;
        string senderTitle; 
        bool accepted;
        bool confermation;
        string aggrementText;
        int initialCharges;
        int chargesPerKm;
        int chargesPerhour;
        int totalCharges;
        bool isVisible = true;
        string pageTitle = "Contract";
        public string AggrementText
        {
            get { return aggrementText; }
            set
            {
                SetProperty(ref aggrementText, value);
            }
        }

        public string SenderTitle
        {
            get { return senderTitle; }
            set
            {
                SetProperty(ref senderTitle, value);
            }
        }

        public int InitialCharges
        {
            get { return initialCharges; }
            set
            {
                SetProperty(ref initialCharges, value);
                ConfermationCommand.RaiseCanExecuteChanged();
                Calculate();
            }
        }

        public int ChargesPerKM
        {
            get { return chargesPerKm; }
            set
            {
                SetProperty(ref chargesPerKm, value);
                ConfermationCommand.RaiseCanExecuteChanged();
                Calculate();
            }
        }

        public int ChargesPerHour
        {
            get { return chargesPerhour; }
            set
            {
                SetProperty(ref chargesPerhour, value);
                ConfermationCommand.RaiseCanExecuteChanged();
                Calculate();
            }
        }

        public int TotalCharges
        {
            get { return totalCharges; }
            set
            {
                SetProperty(ref totalCharges, value);
                ConfermationCommand.RaiseCanExecuteChanged();
            }
        }



        public bool Accepted
        {
            get { return accepted; }
            set
            {
                SetProperty(ref accepted, value);
            }
        }

        public bool Confermation
        {
            get { return confermation; }
            set
            {
                SetProperty(ref confermation, value);
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }

        public string ContactNo
        {
            get { return contactno; }
            set
            {
                SetProperty(ref contactno, value);

            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                SetProperty(ref address, value);

            }
        }

        public double Latitude
        {
            get { return _lat; }
            set
            {
                SetProperty(ref _lat, value);
            }
        }

        public double Longitude
        {
            get { return _long; }
            set
            {
                SetProperty(ref _long, value);
            }
        } 

        public string ImageUrl { get; set; }

        public int Distance { get; set; }

        public string CustomerName { get; set; }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                SetProperty(ref isVisible, value);
            }
        }

        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                SetProperty(ref pageTitle, value);
            }
        }

        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand ConfermationCommand { get; private set; }
        public DelegateCommand DiscardCommand { get; private set; }
      

        
        private void Calculate()
        {
            TotalCharges = InitialCharges + ((ChargesPerKM*Distance)*2) + ChargesPerHour;
        }

        private void Logout()
        {
            CrossSettings.Current.AddOrUpdateValue(Server.ISEXIST, false);

            CrossSettings.Current.Clear();

            App.Current.MainPage = new NavigationPage(new HomePage());
        }

        private void Init()
        {
            Title = LocalStorage.Title;
            Name = LocalStorage.Name;
            ContactNo = LocalStorage.ContactNo;
            Address = LocalStorage.Address;
            Latitude = LocalStorage.Latitude;
            Longitude = LocalStorage.Longitude;
            ImageUrl =  $"{Server.ImagesEndPoint}Images/{Name[0].ToString()}.jpg"; 


            Proxy.On<string,double,double>("requested", async (name,lati,longi)  =>  
            {
                CustomerName = name;
                var locator = CrossGeolocator.Current;
                Position position = null;
                if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                {
                    position = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(10000));
                    

                    //Test Purpose only
                   // position = new Position(LocalStorage.Latitude, LocalStorage.Longitude);
                }
                else
                {
                    position = new Position(LocalStorage.Latitude, LocalStorage.Longitude);
                }


                var customerLocation = new Location(lati, longi);

                var location = new Location(position.Latitude, position.Longitude);

                var distance = location.DistanceBetween(customerLocation, DistanceUnits.Kilometers);

                var km = Math.Ceiling(distance.Value);
                Distance = (int)km;
                SenderTitle = $"{name} ({km} {distance.Units} Away)";
                AggrementText = $"{name} is {km} {distance.Units} away from your position and requested you for your services.\nPleae enter your service charges and conferm request.";

             
                
                Device.BeginInvokeOnMainThread(() =>
                { 
                    Confermation = true;
                    PageTitle = "Contract *";
                });

            });


            Proxy.On("accepted", () => {


                UserDialogs.Instance.Alert("You and " + CustomerName + " have accepted the agreement", "Agreement", "Ok");
                IsVisible = false;
                PageTitle = "Contract";

            });

           
            Connection.Start();

           
        }

        
        private bool CanSendConfermation()
        {
            return this.TotalCharges > 0;
        }
        private void SendConfermation()
        {
            //public void ConfermRequest(string connectionIdOf,string amount)

            Proxy.Invoke("ConfermRequest", InitialCharges,ChargesPerKM,ChargesPerHour,Distance.ToString());
        }

        private void OnDisCard()
        {
            Proxy.Invoke("DisCard", LocalStorage.ContactNo);
            Confermation = false;
            PageTitle = "Contract";
        }
    }
}
