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
    public partial class HomePage : TabbedPage
    {
        public HomePage ()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<object>(this, "Accepted", (sender) =>
            {
                this.CurrentPage = this.Children[1];
            });
        }
    }
}