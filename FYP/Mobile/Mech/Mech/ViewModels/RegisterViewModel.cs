using System;
using System.Collections.Generic;

using GEOGoogle = Geocoding.Google;
using System.Net.Http;
using System.Text; 
using Mech.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Prism.Commands;
using System.Linq;
using Xamarin.Forms;
using Plugin.Connectivity;
using Acr.UserDialogs;
using Mech.Views;
using Plugin.Settings;
using Mech.Services;
using Mech.Utils;

namespace Mech.ViewModels
{
   public class RegisterViewModel :BaseViewModel
    {

        public RegisterViewModel()
        {
            RegisterCommand = new DelegateCommand(Register, CanRegister);

            RequestPhoneNumber();
            InitLocation();
        }

        string title;
        string name;
        string contactno;
        string address;
        double _lat;
        double _long;
       
        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        } 

        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        } 

        public string ContactNo
        {
            get { return contactno; }
            set
            {
                SetProperty(ref contactno, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                SetProperty(ref address, value);
                RegisterCommand.RaiseCanExecuteChanged();
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

       

        public DelegateCommand RegisterCommand { get; private set; }


        private bool CanRegister()
        {
            return   !string.IsNullOrWhiteSpace(Title) &&
               !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(ContactNo) &&
               !string.IsNullOrWhiteSpace(Address);// && (Latitude > 0 && Longitude > 0); 
        }

        private async void Register()
        {


            using (UserDialogs.Instance.Loading("Registering..."))
            { 
                var client = new HttpClient();

                var mechanic = new Mechanic
                {
                    Name = this.name,
                    Title = this.title,
                    ContactNo = this.ContactNo,
                    Address = this.Address,
                    Latitude = this.Latitude,
                    Longitude = this.Longitude,
                    CurrentConnection = LocalStorage.CurrentConnection
                
            };

            var json = JsonConvert.SerializeObject(mechanic);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            if (CrossConnectivity.Current.IsConnected)
            {
            
                    
                try
                {
                        Server server = new Server();
                    var response = await client.PostAsync(server.MechanicEndPoint, content);

                        

                    if (response.IsSuccessStatusCode)
                    {
                            var mechicRowContent = await response.Content.ReadAsStringAsync();

                            var responseMechanic = JsonConvert.DeserializeObject<Mechanic>(mechicRowContent);

                            Message = "Successfuly Registerd"; 

                            LocalStorage.SetMechanic(responseMechanic);

                            App.Current.MainPage = new NavigationPage( new DashBoard());
                    }
                    else
                    {
                        Message = "Registeration Failed.";
                    }

                }
                catch
                {
                    Message = "Unable to connect server";
                } 
            }
            else
            {
                Message = "Please connect to the internet.";
            }

            }


        }


        private async void InitLocation()
        {
          
            try
            {
                var locator = CrossGeolocator.Current;

                if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                {
                    var locatedPosition = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(5000));

                    Latitude = locatedPosition.Latitude;
                    Longitude = locatedPosition.Longitude;
                }
                else
                {
                   Message = "Location not available";
                }

            }
            catch (Exception ex)
            {
                Message = "A problem occure while locating your position.";
            }


            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    var geocoder = new GEOGoogle.GoogleGeocoder();

                    var addresses = await geocoder.ReverseGeocodeAsync(Latitude, Longitude);

                    var approximateAddress = addresses.FirstOrDefault();

                    if (approximateAddress != null)
                    {
                        Address = approximateAddress.FormattedAddress;
                    }
                    else
                    {
                         Address = "Address not found please enter manually";
                    }
                }
                else
                {
                   Message = "Please connect to internet to locate your location address.";
                }

            }
            catch (Exception ex)
            {
                Message = "An error occured while connecting to end point.";
            }
             
        }

        private void RequestPhoneNumber()
        {
            IPhoneRequester phoneRequester = DependencyService.Get<IPhoneRequester>();
           var number =  phoneRequester.RequestPhoneNumber(); 
        }
    }
}
