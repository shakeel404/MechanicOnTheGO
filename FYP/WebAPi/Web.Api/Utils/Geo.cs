using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Api.Utils
{
   public class Geo
    {
        //public double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        //{
        //    double distance = 0;

        //    double dLat = (lat2 - lat1) / 180 * Math.PI;
        //    double dLong = (long2 - long1) / 180 * Math.PI;

        //    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
        //                + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
        //    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)); 
           
        //    double radiusE = 6378135; 
        //    double radiusP = 6356750; 
             
        //    double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);

        //    double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
        //                    + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
        //    double radius = Math.Sqrt(nr / dr); 
           
        //    distance = radius * c;
        //    return distance;
        //}


        public System.Linq.Expressions.Expression<Func<double,double,double,double,double>> GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        {  

            return (_lat1, _long1, _lat2, _long2) => 

               ((Math.Sqrt(Math.Pow(6378135d * 6356750d * Math.Cos(_lat1 / 180 * Math.PI), 2) / (Math.Pow(6378135 * Math.Cos(_lat1 / 180 * Math.PI), 2)
                            + Math.Pow(6356750 * Math.Sin(_lat1 / 180 * Math.PI), 2)))) * (2 * Math.Atan2(Math.Sqrt(Math.Sin(((_lat2 - _lat1) / 180 * Math.PI) / 2) * Math.Sin(((_long2 - _long1) / 180 * Math.PI) / 2)
                        + Math.Cos(_lat2) * Math.Sin(((_long2 - _long1) / 180 * Math.PI) / 2) * Math.Sin(((_long2 - _long1) / 180 * Math.PI) / 2)), Math.Sqrt(1 - Math.Sin(((_lat2 - _lat1) / 180 * Math.PI) / 2) * Math.Sin(((_long2 - _long1) / 180 * Math.PI) / 2)
                        + Math.Cos(_lat2) * Math.Sin(((_long2 - _long1) / 180 * Math.PI) / 2) * Math.Sin(((_long2 - _long1) / 180 * Math.PI) / 2)))));
            
        }
    }
}