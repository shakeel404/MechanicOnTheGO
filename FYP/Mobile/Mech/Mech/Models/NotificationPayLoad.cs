using System;
using System.Collections.Generic;
using System.Text;

namespace Mech.Models
{
    public class NotificationPayLoad
    {
        public NotificationPayLoad(  string token, string sender)
        {
             
            Token = token;
            Sender = sender;
        }

        
        public string Token { get; private set; }
        public string Sender { get; private set; }
    }
}
