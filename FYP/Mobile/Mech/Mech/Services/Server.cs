  using System;
using System.Collections.Generic;
using System.Text;

namespace Mech.Services
{
    public class Server
    {
/// Change ApiHost According to your server
        public const string ApiHost =        "http://localhost:3435/";  
        public const string ApiUrl = ApiHost + "api/";  
        public const string HubEndPoint = ApiHost;      
        public const string ImagesEndPoint = ApiHost;    
        public const string ChatHub = "MechHub";
        public const string ContractHub = "ContractHub";
           



        public readonly string CustomerEndPoint = ApiUrl + "Customers";
        public readonly string MechanicEndPoint = ApiUrl + "Mechanics";
        public readonly string NotificationEndPoint = ApiUrl + "Notification";
        public readonly string MechanicContractEndPoint = ApiUrl + "MechanicContract";
        public readonly string CustomerContractEndPoint = ApiUrl + "CustomerContract";
        public readonly string WorkStartedEndPoint = ApiUrl + "WorkTracking/WorkStarted/";
        public readonly string WorkFinishedEndPoint = ApiUrl + "WorkTracking/WorkFinished/";


        /// <summary>
        /// Get End Point for retriving nearest mechanics to the users locations. It lacks two param arguments.
        /// 1. Latitude
        /// 2. Longitude
        /// Usage: var string endpoint = string.Formate(MechanicEndPoint,lat,long)
        /// </summary>
        public readonly string NearestMechanicEndPoint = ApiUrl + @"Mechanics/GetMechanic/{0}/{1}/";

        public const string ISEXIST = "IsExist";
        public const string ID = "Id";
        public const string TITLE = "Title";
        public const string NAME = "Name";
        public const string CONTACTNO = "ContactNo";
        public const string ADDRESS = "Address";
        public const string LAT = "Lat";
        public const string LONG = "Long";
        public const string CONNECTIONID = "ConnectionId";
        public const string IMAGEURL = "ImageUrl";
        public const string MODEL = "Model";
        public const string VECHILE = "Vechile";
        public const string CUSTOMER = "Customer";
        public const string MECHANIC = "Mechanic";
        public const string REGISTRATION = "Registration";

    }
}
