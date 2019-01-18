using Android.Content;
using Android.Telephony;
using Mech.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Mech.Droid.PhoneRequester))]
namespace Mech.Droid
{
    public class PhoneRequester : IPhoneRequester
    {
        public string RequestPhoneNumber()
        {
            TelephonyManager mgr = Android.App.Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;
            return mgr.Line1Number;
        }
    }
}