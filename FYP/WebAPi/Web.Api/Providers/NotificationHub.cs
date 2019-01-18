using FCM.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Web.Api.Providers
{
    public class NotificationHub
    {
        const string ServerKey = "Your Fire Base Server Key";

        public async Task SendAsync(string recieverToken,string message,string title="Contract")
        {
            using (var sender = new Sender(ServerKey))
            {
                var msg = new Message
                {
                    RegistrationIds = new List<string> { recieverToken },
                    Notification = new Notification
                    {
                        Title = title,
                        Body = message,
                        Sound= "Default"
                    }
                };
                var result = await sender.SendAsync(msg);  
            }

        }

        public async void SendAsJSONAsync(string recieverToken, string message, string title = "Contract")
        {
            using (var sender = new Sender(ServerKey))
            {
                var json = "{\"notification\":{\"title\":\"" + title + "\",\"body\":\"" + message + "\"},\"to\":\"" + recieverToken + "\"}";
                var result = await sender.SendAsync(json);
            }
        }
    }
}