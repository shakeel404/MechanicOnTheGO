using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Firebase.Messaging;
using Mech.Models;
using Mech.Services;
using Mech.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Mech.Droid
{
    [Activity(Label = "Mech", Icon = "@drawable/MechIcon", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnResume()
        {
            base.OnResume();

            StartActivity(typeof(MainActivity));

            IsGooglePlayServiceAvailable();

#if DEBUG
            // Force refresh of the token. If we redeploy the app, no new token will be sent but the old one will
            // be invalid.
            Task.Run(() =>
            {
                // This may not be executed on the main thread.
                FirebaseInstanceId.Instance.DeleteInstanceId();
                Console.WriteLine("Forced token: " + FirebaseInstanceId.Instance.Token);
            });
#endif
        

    }

        public bool IsGooglePlayServiceAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);

            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Console.WriteLine($"Error:{GoogleApiAvailability.Instance.GetErrorString(resultCode)}");
                }
                else
                {
                    Console.WriteLine("Error: Google Play Service not supported");
                    Finish();
                }

                return false;
            }
            else
            {
                Console.WriteLine("Google Play Service available.");

                return true;
            }
        }

    }



    /// This service handles the device registration with FCM(Firebase cloud messaging)
    ///

    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var freshToken = FirebaseInstanceId.Instance.Token;
           
            Console.WriteLine($"Token Recieved: {freshToken}");

            if (LocalStorage.IsExist)
            {
                SendRegistrationToServer(freshToken);
            }
            else
            {
                LocalStorage.CurrentConnection = freshToken;
            }
            
        }

        private async void SendRegistrationToServer(string token)
        { 
            var client = new HttpClient();
            var server = new Server(); 

            var item = new NotificationPayLoad( token, LocalStorage.Registration);
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var endPoint = $"{server.NotificationEndPoint}/{LocalStorage.Id}";
            var response = await client.PutAsync(endPoint,content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Token Successfuly Registerd");
            }
            else
            {
                Console.WriteLine("Token Failed to Registerd");
            }
        }
    }

    // This service is used if app is in the foreground and a message is received.

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            Console.WriteLine("Received: " + message);

          
            try
            {
                var title = message.GetNotification().Title;

                if (title == "Contract")
                {
                    Device.BeginInvokeOnMainThread(() => {

                        MessagingCenter.Send<object>(this, "Requested");
                    });
                    
                }
               else if(title =="Accepted")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send<object>(this, "Accepted");
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting message: " + ex);
            }
        }
    }
}