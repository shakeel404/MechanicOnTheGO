using Mech.Models;
using GEOGoogle = Geocoding.Google;
using Plugin.Connectivity;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Prism.Commands;
using Acr.UserDialogs;
using System.Net.Http;
using Newtonsoft.Json;
using Mech.Services;
using Mech.Utils;
using System.Linq;
using Mech.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Mech.ViewModels
{
    public class CustomerRegisterViewModel : BaseViewModel
    {

        public CustomerRegisterViewModel()
        {
            RegisterCommand = new DelegateCommand(Register, CanRegister);

            RequestPhoneNumber();
            InitLocation();
            PopulateLists();
        }

        
        string name;
        string contactno;
       
        double _lat;
        double _long; 
        string address;
        string selectedVechile;
        string selectedModel;
        public ObservableCollection<string> Vechiles { get; private set; }
        public ObservableCollection<string> Models { get; private set; }


        private void PopulateLists()
        {
            var vechiles = new List<string>() { "Car", "Jeep", "Truck", "Suzuki", "Rikshaw", "Moter Car", "Flying Coach", "Coster", "Other", "Moter Cycle" };
            var models = new List<string>() { "2001", "2002", "2003", "2004", "2005", "2008", "2012", "2014", "2015", "2017", "2018" };

            Vechiles = new ObservableCollection<string>(vechiles);
            Models = new ObservableCollection<string>(models);
        }


        public string SelectedVechile
        {
            get { return selectedVechile; }
            set
            {
                SetProperty(ref selectedVechile, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }
        public string SelectedModel
        {
            get { return selectedModel; }
            set
            {
                SetProperty(ref selectedModel, value);
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
        

        public string Address
        {
            get { return address; }
            set
            {
                SetProperty(ref address, value);
            }
        }


        public DelegateCommand RegisterCommand { get; private set; }


        private bool CanRegister()
        {
            return  
               !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(ContactNo) &&
               !string.IsNullOrWhiteSpace(SelectedModel) &&
               !string.IsNullOrWhiteSpace(SelectedVechile);// && (Latitude > 0 && Longitude > 0); 
        }

        private async void Register()
        {


            using (UserDialogs.Instance.Loading("Registering..."))
            {
                var client = new HttpClient();

                var customer = new Customer
                {
                    Name = this.Name,
                    Model = this.SelectedModel,
                    ContactNo = this.ContactNo,
                    Latitude = this.Latitude,
                    Longitude = this.Longitude,
                    Vehicle = this.SelectedVechile,
                    CurrentConnection = LocalStorage.CurrentConnection
                };

                var json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                if (CrossConnectivity.Current.IsConnected)
                {


                    try
                    {
                        Server server = new Server();
                        var response = await client.PostAsync(server.CustomerEndPoint, content);



                        if (response.IsSuccessStatusCode)
                        {
                            var customerRowContent = await response.Content.ReadAsStringAsync();

                            var responseCustomer = JsonConvert.DeserializeObject<Customer>(customerRowContent);

                            Message = "Successfuly Registerd";

                            LocalStorage.SetCustomer(responseCustomer);

                            App.Current.MainPage = new NavigationPage(new HomePage());
 
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


            //try
            //{

            //    if (CrossConnectivity.Current.IsConnected)
            //    {
            //        var geocoder = new GEOGoogle.GoogleGeocoder();

            //        var addresses = await geocoder.ReverseGeocodeAsync(Latitude, Longitude);

            //        var approximateAddress = addresses.FirstOrDefault();

            //        if (approximateAddress != null)
            //        {
            //            Address = approximateAddress.FormattedAddress;
            //        }
            //        else
            //        {
            //            Address = "Address not found please enter manually";
            //        }
            //    }
            //    else
            //    {
            //        Message = "Please connect to internet to locate your location address.";
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Message = "An error occured while connecting to end point.";
            //}

        }

        private void RequestPhoneNumber()
        {
            IPhoneRequester phoneRequester = DependencyService.Get<IPhoneRequester>();
            var number = phoneRequester.RequestPhoneNumber();
        }


    }
}
