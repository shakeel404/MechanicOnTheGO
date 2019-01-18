using Acr.UserDialogs;
using Mech.Models;
using Mech.Services;
using Mech.Utils;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace Mech.ViewModels
{
   public class ContractsViewModel: BaseViewModel
    {

        public ContractsViewModel()
        { 
            RefreshCommand = new DelegateCommand(Init);

            Contracts = new ObservableCollection<ContractItem>();
            if (LocalStorage.IsExist)
            {
                Init();
            }
           
        }

        private string EndPoint; 
        private bool isRefreshing; 

        public ObservableCollection<ContractItem> Contracts { get; private set; }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                SetProperty(ref isRefreshing, value);
            }
        }

        private void CheckUser()
        {
            Server server = new Server();
            if (LocalStorage.Registration == Server.MECHANIC)
            {
                EndPoint = server.MechanicContractEndPoint + $"/{LocalStorage.Id}";
            }
            else if (LocalStorage.Registration == Server.CUSTOMER)
            {
                EndPoint = server.CustomerContractEndPoint + $"/{LocalStorage.Id}";
            }
        }
        public async void Init()
        {
              CheckUser();
            if(!LocalStorage.IsExist)
            {
                await UserDialogs.Instance.AlertAsync("Please login to continue", "Mech Login", "Ok");

                return;
            }

            IsRefreshing = true;

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var client = new HttpClient(); 

                    var response = await client.GetAsync(EndPoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<Contract>>(content);

                        if (list != null)
                        {
                            Contracts.Clear();
                        }

                        foreach(var contract in list)
                        {
                           

                            if (contract.Mechanic != null)
                            {

                                var contractitem = new ContractItem()
                                {
                                    Id = contract.Id,
                                    Time = contract.Time,
                                    IsConfirmed = contract.PaidByCustomer,
                                    Name = contract.Mechanic.Name,
                                    Title = contract.Mechanic.Title,
                                     InitialCharges = contract.InitialCharges,
                                     RatePerKM = contract.RatePerKM,
                                     RatePerHour = contract.RatePerHour,
                                     KM = contract.KM,
                                     ContactNo = contract.Mechanic.ContactNo,
                                     IsWorkStarted = contract.IsWorkStarted ,
                                     WorkStarted = contract.WorkStarted,
                                     IsWorkFinished = contract.IsWorkFinished,
                                     WorkFinished = contract.WorkFinished
                                     
                                    
                                };
                                Contracts.Add(contractitem);
                            }
                            else if(contract.Customer != null)
                            {
                                var contractitem = new ContractItem()
                                {
                                    Id = contract.Id,
                                    Time = contract.Time,
                                    IsConfirmed = contract.RecievedByMehanic,
                                    Name = contract.Customer.Name,
                                    Title = $"{contract.Customer.Vehicle} {contract.Customer.Model}",
                                    InitialCharges = contract.InitialCharges,
                                    RatePerKM = contract.RatePerKM,
                                    RatePerHour = contract.RatePerHour,
                                    KM = contract.KM,
                                    ContactNo = contract.Customer.ContactNo,
                                    IsWorkStarted = contract.IsWorkStarted,
                                    WorkStarted = contract.WorkStarted,
                                    IsWorkFinished = contract.IsWorkFinished,
                                    WorkFinished = contract.WorkFinished

                                };
                                Contracts.Add(contractitem);
                            }
                            
                           
                             
                        }
                         
                    }
                    IsRefreshing = false;
                }
            }
            catch(Exception ex)
            {
                IsRefreshing = false;
            }

            IsRefreshing = false;
        }




        public DelegateCommand RefreshCommand { get; private set; }
    }
}
