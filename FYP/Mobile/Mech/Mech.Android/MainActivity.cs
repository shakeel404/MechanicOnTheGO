
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Widget;
using Firebase.Iid;
using Firebase.Messaging;
using ImageCircle.Forms.Plugin.Droid;
using System;
using System.Threading.Tasks; 
namespace Mech.Droid
{
    [Activity(Label = "Mech", Icon = "@drawable/MechIcon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(this);
            Xamarin.FormsMaps.Init(this, bundle);
            Lottie.Forms.Droid.AnimationViewRenderer.Init();
            ImageCircleRenderer.Init(); 
            LoadApplication(new App());


        }
       
    }

   

}

