using Mech.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mech.ViewModels
{
    public class ContractItem : BaseViewModel
    {


        private bool confirmed;
        private bool isWorkStarted;
        private bool isWorkFinished;
        private DateTime? workStarted;
        private DateTime? workFinished;
        private DateTime? time;
        private int total;
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Time
        {
            get { return time.Value.ToLocalTime(); }
            set
            {
                SetProperty(ref time, value);
            }
        }
        public string Title { get; set; } 
        public int InitialCharges { get; set; }
        public int RatePerKM { get; set; }
        public int RatePerHour { get; set; }
        public int KM { get; set; }
        public string ImageUrl
        {
            get { return $"{Server.ImagesEndPoint}Images/{Name[0].ToString()}.jpg"; }
        }
        public string ContactNo { get; set; }

        public DateTime? WorkStarted
        {
            get { return workStarted.Value.ToLocalTime(); }
            set
            {
                SetProperty(ref workStarted, value);
            }
        }


        public bool IsWorkStarted
        {
            get { return isWorkStarted; }
            set
            {
                SetProperty(ref isWorkStarted, value);
            }
        }

        public bool IsWorkFinished
        {
            get { return isWorkFinished; }
            set
            {
                SetProperty(ref isWorkFinished, value);
            }
        }

        public DateTime? WorkFinished
        {
            get { return workFinished.Value.ToLocalTime(); }
            set
            {
                SetProperty(ref workFinished, value);
            }
        }


        public bool IsConfirmed
        {
            get { return confirmed; }
            set
            {
                SetProperty(ref confirmed, value);
            }
        }

        public bool IsPending
        {
            get { return !IsConfirmed; }
        }
         
        public int Total
        {
            get
            {
                total = CalculateTotal();
                return total;
            }
            set
            {
                var val = value;
                SetProperty(ref total, CalculateTotal());
            }
        } 

        public int CalculateTotal()
        {
            if (IsWorkFinished)
            {
                var time = WorkFinished - WorkStarted;

                var hours = time.Value.Hours;
                var minuts = time.Value.Minutes;
              
                double totalTime = 0.0d;

                double.TryParse($"{hours}.{minuts}", out totalTime);

                return (InitialCharges + ((RatePerKM * KM) * 2) + (int)(totalTime * RatePerHour));
                
            }
            else 
            {
                return (InitialCharges + ((RatePerKM * KM) * 2));
            }
        }
        
    }
}
