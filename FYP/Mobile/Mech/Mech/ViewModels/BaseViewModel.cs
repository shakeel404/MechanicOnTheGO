using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mech.ViewModels
{
  public  class BaseViewModel: BindableBase
    {
        
        
        string message;

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
    }
}
