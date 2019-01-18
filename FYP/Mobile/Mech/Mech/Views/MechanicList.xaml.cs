using Mech.Models;
using Mech.ViewModels;
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
	public partial class MechanicList : ContentPage
	{
		public MechanicList ()
		{
			InitializeComponent (); 
		}
         

        public async void OnItemClicked(object sender,ItemTappedEventArgs e)
        {
            var mechanic = e.Item as Mechanic;

            MechanicDetails page = new MechanicDetails(mechanic); 

          await  Navigation.PushAsync(page);

            MechanisList.SelectedItem = null;
        }
    }
}