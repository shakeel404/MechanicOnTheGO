using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mech.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TermsAndConditions : ContentPage
	{
		public TermsAndConditions ()
		{
			InitializeComponent ();
		}

        private async void OnNextButtonClicked(object sender,EventArgs e)
        {
           await Navigation.PushAsync(new Register());
        }

    }
}