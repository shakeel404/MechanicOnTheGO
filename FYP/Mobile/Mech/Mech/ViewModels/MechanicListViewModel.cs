using Acr.UserDialogs;
using Mech.Models;
using Mech.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace Mech.ViewModels
{
    public class MechanicListViewModel : BaseViewModel
    {
        public MechanicListViewModel()
        {

            Mechanics = new ObservableCollection<Mechanic>();
            RefreshMechanicsCommand = new DelegateCommand(GetMechanics);

            RefreshMechanicsCommand.Execute();
        }
        bool isrefreshing;
        public bool IsRefreshing
        {
            get { return isrefreshing; }
            set
            {
                SetProperty(ref isrefreshing, value);
            }
        }
        public ObservableCollection<Mechanic> Mechanics { get; private set; }

        public DelegateCommand RefreshMechanicsCommand { get; private set; }


        private async void GetMechanics()
        {
            IsRefreshing = true;
            //var client = new HttpClient();
            //Server server = new Server();
            //var uri = new Uri(string.Format(server.MechanicEndPoint,));

            //if (CrossConnectivity.Current.IsConnected)
            //{

            //    try
            //    {

            //        var locator = CrossGeolocator.Current;
                     
            //        if (!locator.IsGeolocationAvailable && !locator.IsGeolocationEnabled)
            //        {
            //            UserDialogs.Instance.Alert("GPS is not available", "Mech", "OK");
            //        }
            //        else
            //        {
            //            var locatedPosition = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(5000)); 

            //            var response = await client.GetAsync(uri);
            //            if (response.IsSuccessStatusCode)
            //            {
            //                var content = await response.Content.ReadAsStringAsync();
            //                var list = JsonConvert.DeserializeObject<List<Mechanic>>(content);

            //                foreach (var mech in list)
            //                {
            //                    Mechanics.Add(mech);
            //                }

            //            }
            //            else
            //            {
            //                Message = "Registeration Failed.";
            //            }
            //        }

            //    }
            //    catch
            //    {
            //        Message = "Unable to connect server";
            //    }
            //}
            //else
            //{
            //    Message = "Please connect to the internet.";
            //}

            IsRefreshing = false;
        }
    }
}
