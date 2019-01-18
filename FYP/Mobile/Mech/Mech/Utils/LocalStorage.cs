using Mech.Models;
using Mech.Services;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mech.Utils
{
    public static class LocalStorage
    {

        public static void SetMechanic(Mechanic mechanic, bool isExist = true)
        {
            CrossSettings.Current.AddOrUpdateValue(Server.TITLE, mechanic.Title);
            CrossSettings.Current.AddOrUpdateValue(Server.NAME, mechanic.Name);
            CrossSettings.Current.AddOrUpdateValue(Server.CONTACTNO, mechanic.ContactNo);
            CrossSettings.Current.AddOrUpdateValue(Server.ADDRESS, mechanic.Address);
            CrossSettings.Current.AddOrUpdateValue(Server.LAT, mechanic.Latitude);
            CrossSettings.Current.AddOrUpdateValue(Server.LONG, mechanic.Longitude);

            CrossSettings.Current.AddOrUpdateValue(Server.ID, mechanic.Id);
            CrossSettings.Current.AddOrUpdateValue(Server.CONNECTIONID, mechanic.CurrentConnection);
            CrossSettings.Current.AddOrUpdateValue(Server.IMAGEURL, mechanic.ImageUrl);

            CrossSettings.Current.AddOrUpdateValue(Server.ISEXIST, isExist);
            CrossSettings.Current.AddOrUpdateValue(Server.REGISTRATION, Server.MECHANIC);
        }

        public static void SetCustomer(Customer customer, bool isExist = true)
        { 
            CrossSettings.Current.AddOrUpdateValue(Server.NAME, customer.Name);
            CrossSettings.Current.AddOrUpdateValue(Server.MODEL, customer.Model);
            CrossSettings.Current.AddOrUpdateValue(Server.VECHILE, customer.Name);
            CrossSettings.Current.AddOrUpdateValue(Server.CONTACTNO, customer.ContactNo); 
            CrossSettings.Current.AddOrUpdateValue(Server.LAT, customer.Latitude);
            CrossSettings.Current.AddOrUpdateValue(Server.LONG, customer.Longitude);

            CrossSettings.Current.AddOrUpdateValue(Server.ID, customer.Id);
            CrossSettings.Current.AddOrUpdateValue(Server.CONNECTIONID, customer.CurrentConnection);
            CrossSettings.Current.AddOrUpdateValue(Server.IMAGEURL, customer.ImageUrl);

            CrossSettings.Current.AddOrUpdateValue(Server.ISEXIST, isExist);
            CrossSettings.Current.AddOrUpdateValue(Server.REGISTRATION, Server.CUSTOMER);
        }

        public static Mechanic GetMechanic()
        {
            var defaultValue = string.Empty;

            var mechanic = new Mechanic
            {
                Id = CrossSettings.Current.GetValueOrDefault(Server.ID, 0),
                Title = CrossSettings.Current.GetValueOrDefault(Server.TITLE, defaultValue),
                Name = CrossSettings.Current.GetValueOrDefault(Server.NAME, defaultValue),
                ContactNo = CrossSettings.Current.GetValueOrDefault(Server.CONTACTNO, defaultValue),
                Address = CrossSettings.Current.GetValueOrDefault(Server.ADDRESS, defaultValue),
                Latitude = CrossSettings.Current.GetValueOrDefault(Server.LAT, 0d),
                Longitude = CrossSettings.Current.GetValueOrDefault(Server.LONG, 0d),
                CurrentConnection = CrossSettings.Current.GetValueOrDefault(Server.CONNECTIONID, defaultValue),
                ImageUrl = CrossSettings.Current.GetValueOrDefault(Server.IMAGEURL, defaultValue)

            };
            return mechanic;
        }
        public static Customer GetCustomer()
        {
            var defaultValue = string.Empty;

            var customer = new Customer
            {
                Id = CrossSettings.Current.GetValueOrDefault(Server.ID, 0),
                
                Name = CrossSettings.Current.GetValueOrDefault(Server.NAME, defaultValue),
                ContactNo = CrossSettings.Current.GetValueOrDefault(Server.CONTACTNO, defaultValue),
                
                Latitude = CrossSettings.Current.GetValueOrDefault(Server.LAT, 0),
                Longitude = CrossSettings.Current.GetValueOrDefault(Server.LONG, 0),
                CurrentConnection = CrossSettings.Current.GetValueOrDefault(Server.CONNECTIONID, defaultValue),
                ImageUrl = CrossSettings.Current.GetValueOrDefault(Server.IMAGEURL, defaultValue)

            };
            return customer;
        }

        public static int Id
        {
            get { return CrossSettings.Current.GetValueOrDefault(Server.ID, 0); }
        }


        public static string Title { get { return CrossSettings.Current.GetValueOrDefault(Server.TITLE, string.Empty); } }

        public static string Name { get { return CrossSettings.Current.GetValueOrDefault(Server.NAME, string.Empty); } }

        public static string ContactNo { get { return CrossSettings.Current.GetValueOrDefault(Server.CONTACTNO, string.Empty); } }
        public static string Address { get { return CrossSettings.Current.GetValueOrDefault(Server.ADDRESS, string.Empty); } }
        public static double Latitude { get { return CrossSettings.Current.GetValueOrDefault(Server.LAT, 34.78680923d); } }
        public static double Longitude { get { return CrossSettings.Current.GetValueOrDefault(Server.LONG, 72.35325037d); } }
        public static string CurrentConnection
        {
            get { return CrossSettings.Current.GetValueOrDefault(Server.CONNECTIONID, string.Empty); }
            set
            {
                CrossSettings.Current.AddOrUpdateValue(Server.CONNECTIONID, value);
            }
        }
        public static string ImageUrl { get { return CrossSettings.Current.GetValueOrDefault(Server.IMAGEURL, string.Empty); } }

        public static bool IsExist { get { return CrossSettings.Current.GetValueOrDefault(Server.ISEXIST, false); } }

        public static string Registration { get { return CrossSettings.Current.GetValueOrDefault(Server.REGISTRATION, string.Empty); } }

    }
}
