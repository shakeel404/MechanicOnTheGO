//using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Mech.Models
{
    class Location
    {
        public Location()
        {
            Initialize();
        }
       public async void Initialize()
        { 
            //var locator = CrossGeolocator.Current;

            //var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(1));

            //Debug.WriteLine("Position Status: {0}", position.Timestamp);
            //Debug.WriteLine("Position Latitude: {0}", position.Latitude);
            //Debug.WriteLine("Position Longitude: {0}", position.Longitude);
            //this.Latitude = position.Latitude;
            //this.Longitude = position.Longitude; 
        }


        

      public  double Latitude { get; set; }
       public double Longitude { get; set; }
      // public List<Address> Addresses { get; set; }

    }
}
