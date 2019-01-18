using Acr.UserDialogs;
using Mech.Models;
using Mech.Services;
using Mech.Utils;
using Newtonsoft.Json;
using Plugin.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Mech.ViewModels
{
   public class ContractDetailsViewModel :BaseViewModel
    {
        public ContractDetailsViewModel(ContractItem contractItem)
        {
            this.Item = contractItem;
            PayCommand = new DelegateCommand(OnPay,CanPay);
            StartCommand = new DelegateCommand(OnStart);
            FinishCommand = new DelegateCommand(OnFinish);
            CallCommand = new DelegateCommand(OnCall);
            SmsCommand = new DelegateCommand(OnSms);

            Init();
        }

        bool canRequest=true;
        bool isPending;
        string statusText;
        bool canStart;
        bool canFinish;
        bool istotalVisible;
        bool canPayIsVisible;
        public string AmountText { get; set; }
        public string ButtonText { get; set; }
        public bool Pending
        {
            get { return isPending; }
            set
            {
                SetProperty(ref isPending, value);
            }
        }
        public bool CanRequest
        {
            get { return canRequest; }
            set
            {
                SetProperty(ref canRequest, value);
                PayCommand.RaiseCanExecuteChanged();
            }
        }
        public bool CanStart
        {
            get { return canStart; }
            set
            {
                SetProperty(ref canStart, value);
            }
        }
        public bool CanPayIsVisible
        {
            get { return canPayIsVisible; }
            set
            {
                SetProperty(ref canPayIsVisible, value);
            }
        }
        public bool CanFinish
        {
            get { return canFinish; }
            set
            {
                SetProperty(ref canFinish, value);
            }
        }

        public bool IsTotalVisible
        {
            get { return istotalVisible; }
            set
            {
                SetProperty(ref istotalVisible, value);
            }
        }

        public ContractItem Item { get; private set; }

        

        public DelegateCommand StartCommand { get; private set; }
        public DelegateCommand FinishCommand { get; private set; }
        public DelegateCommand PayCommand { get; private set; } 
        public DelegateCommand CallCommand { get; private set; } 
        public DelegateCommand SmsCommand { get; private set; }

        public string StatusText
        {
            get { return statusText; }
            set
            {
                SetProperty(ref statusText, value);
            }
        }

        //private void WorkStarted()
        //{
        //    CanStart = false;
        //    CanFinish = true;
        //    StatusText = CalculateHourse();
        //    Pending = !Item.IsConfirmed;
        //}


        //private void WorkFinished()
        //{
        //    CanStart = false;
        //    CanFinish = false;
        //    StatusText = CalculateHourse();
        //    Pending = !Item.IsConfirmed;
        //}

        private void SetValues()
        {
            StatusText = CalculateHourse(); 
            Pending = !Item.IsConfirmed;
            CanStart = !Item.IsWorkStarted;
            CanFinish = Item.IsWorkStarted && !Item.IsWorkFinished;
            IsTotalVisible = Item.IsWorkFinished;
            CanPayIsVisible = (Item.IsWorkFinished && !Item.IsConfirmed) ? true : false;
            Item.Total = 0;
        }
        private void SetValues(Contract contract)
        {
            Item.IsWorkStarted = contract.IsWorkStarted;
            Item.WorkStarted = contract.WorkStarted;
            Item.IsWorkFinished = contract.IsWorkFinished;
            Item.WorkFinished = contract.WorkFinished;
            Item.Total = 0;

        }
        private string CalculateHourse()
        { 

            if (Item.IsWorkFinished)
            {
                var time = Item.WorkFinished - Item.WorkStarted;

                var hours = time.Value.Hours;
                var minuts = time.Value.Minutes;

                return $"Work Finished in {hours} Hours and {minuts} Minuts";
            }
            else if (Item.IsWorkStarted)
            {
                return "Work Started";
            }
            else
            {
                return "Work Not Started Yet";
            }





        }
         
        private void OnCall()
        {
            if (CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
            {
                CrossMessaging.Current.PhoneDialer.MakePhoneCall(Item.ContactNo);
            }
        }
       
        private void OnSms()
        {
            if (CrossMessaging.Current.SmsMessenger.CanSendSms)
            { 
                CrossMessaging.Current.SmsMessenger.SendSms(Item.ContactNo, $"Hello {Item.Name} [Type your message]");
            }
        }


        private async void OnStart()
        {
           using(UserDialogs.Instance.Loading("Starting Work..."))
            {
                var client = new HttpClient();

                var server = new Server();
                var id = Item.Id;
                var endPoint = $"{server.WorkStartedEndPoint}{id}";



                var response = await client.PutAsync(endPoint, null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContract = await response.Content.ReadAsStringAsync();

                    var contract = JsonConvert.DeserializeObject<Contract>(responseContract);

                    SetValues(contract);
                    SetValues();
                   // WorkStarted();

                }
            } 
            
        }

        private async void OnFinish()
        {
            using (UserDialogs.Instance.Loading("Finishing Work..."))
            {
                var client = new HttpClient();

                var server = new Server();
                var id = Item.Id;
                var endPoint = $"{server.WorkFinishedEndPoint}{id}";


                var response = await client.PutAsync(endPoint, null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContract = await response.Content.ReadAsStringAsync();

                    var contract = JsonConvert.DeserializeObject<Contract>(responseContract);

                    SetValues(contract);
                   // WorkFinished();
                    SetValues();

                }
            }
        }

        private async void OnPay()
        {
            using (UserDialogs.Instance.Loading("Confirming..."))
            {
                CanRequest = false;
                var client = new HttpClient();
                var server = new Server();


                var id = Item.Id;

                var EndPoint = string.Empty;

                if (LocalStorage.Registration == Server.MECHANIC)
                {
                    EndPoint = $"{server.MechanicContractEndPoint}/{id}";
                }
                else if (LocalStorage.Registration == Server.CUSTOMER)
                {
                    EndPoint = $"{server.CustomerContractEndPoint}/{id}";
                }
                else
                {
                    return;
                }


                var response = await client.PutAsync(EndPoint, null);

                if (response.IsSuccessStatusCode)
                {

                    var responseContract = await response.Content.ReadAsStringAsync();

                    var contract = JsonConvert.DeserializeObject<Contract>(responseContract);
                    Item.IsConfirmed = true;
                    SetValues(contract); 
                    SetValues(); 
                   
                }

                CanRequest = true;
            }
        }



        private bool CanPay()
        {
            return CanRequest;
        }
        private void Init()
        {
            if (LocalStorage.Registration == Server.MECHANIC)
            {
                AmountText = "Recieved";
                ButtonText = "Recieved";
            }
            else if (LocalStorage.Registration == Server.CUSTOMER)
            {
                AmountText = "Paid";
                ButtonText = "Paid";
            }

            SetValues();
        }

         
    }
}
