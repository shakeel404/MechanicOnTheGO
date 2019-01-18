using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.SignalR.Client;
using Mech.Services;
using System.Collections.ObjectModel;
using Mech.Models;
using Plugin.Settings;
using Prism.Commands;
using Mech.Utils;
using Xamarin.Forms;

namespace Mech.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {

        private HubConnection Connection { get; set; }
        private IHubProxy Proxy { get; set; }


        private string messageText;

        private bool connected;


        

        public ChatViewModel()
        {
            Connection = new HubConnection(Server.HubEndPoint);

            Proxy = Connection.CreateHubProxy(Server.ChatHub);
            Messages = new ObservableCollection<Chat>();

            SendCommand = new DelegateCommand(Send, CanSend);
            Name = LocalStorage.Name; 
          
        }

       

        public ChatViewModel(string recieverContact, string name)
        {
            RecieverContact = recieverContact;
            RecieverName = name;
            Connection = new HubConnection(Server.HubEndPoint);

            Proxy = Connection.CreateHubProxy(Server.ChatHub);
            Messages = new ObservableCollection<Chat>();

            SendCommand = new DelegateCommand(Send, CanSend);

            Name = LocalStorage.Name;
        }



        public string ConnectionId { get; private set; }
        public string RecieverConnection { get; private set; }
        public string RecieverContact { get; private set; }
        public string RecieverName { get; private set; }
        public bool Connected
        {

            get { return connected; }
            set
            {
                SetProperty(ref connected, value);
            }
        }
        public string Name { get; private set; }

        public ObservableCollection<Chat> Messages { get; private set; }

        public string MessageText
        {
            get { return messageText; }
            set
            {
                SetProperty(ref messageText, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }



        private void TestMessages()
        {
            var messageSend = new Chat()
            {
                SenderId = ConnectionId,
                RecieverId = RecieverConnection,
                ToUser = "Me",
                FromUser = "Unknown",
                DateSent = DateTime.Now,
                SenderImageUrl = $"{Server.ImagesEndPoint}Images/{Name[0].ToString().ToUpper()}.jpg",
                Message = "Hi, How is it going.",
                Status = "Sent"

            };
            var messageRecieved = new Chat()
            {
                SenderId = ConnectionId,
                RecieverId = RecieverConnection,
                ToUser = "Me",
                FromUser = "Test",
                DateSent = DateTime.Now,
                SenderImageUrl = $"{Server.ImagesEndPoint}Images/{Name[0].ToString().ToUpper()}.jpg",
                Message = "How Are you.",
                Status = "Recieved"

            };

            Messages.Add(messageRecieved);
            Messages.Add(messageSend);
        }


        public async void Init()
        {
            Proxy.On<Chat>("recieveMessage", msg =>
            {
                msg.Status = "Recieved";

                msg.SenderImageUrl = $"{Server.ImagesEndPoint}Images/{msg.FromUser[0].ToString().ToUpper()}.jpg";
                Messages.Add(msg);
                MessagingCenter.Send<object, object>(this, "MessageReceived", msg);
            });

            Proxy.On<string, string>("newUserConnected", (contact, connection) =>
            {

                if (LocalStorage.Registration == Server.MECHANIC)
                {
                    RecieverConnection = connection;


                    Proxy.Invoke("EchoNewUser", LocalStorage.ContactNo, connection);
                }
                else if (RecieverContact == contact)
                {
                    RecieverConnection = connection;


                    Proxy.Invoke("EchoNewUser", LocalStorage.ContactNo, connection);
                }


            });



            Proxy.On<string, string>("echo", (contact, connection) =>
            {
                if (LocalStorage.Registration == Server.MECHANIC)
                {
                    RecieverConnection = connection;
                    Proxy.Invoke("EchoNewUser", LocalStorage.ContactNo, connection);
                }
                else if (RecieverContact == contact)
                {
                    RecieverConnection = connection;
                }
            });

            Connection.StateChanged += Connection_StateChanged;


            await Connection.Start();

            Connect();
        }

        private void Connection_StateChanged(StateChange obj)
        {
            if (obj == null)
            {
                Connected = false;
                return;
            }

            Connected = obj.NewState == ConnectionState.Connected;
        }

        public DelegateCommand SendCommand { get; private set; }

        private void Connect()
        {
            var contact = LocalStorage.ContactNo;

            if (string.IsNullOrWhiteSpace(contact))
            {
                return;
            }

            Proxy.Invoke<string>("Connect", contact);
        }


        private bool CanSend()
        {
            return !string.IsNullOrWhiteSpace(MessageText);
        }

        private void Send()
        {
            var text = MessageText;
            var message = new Chat()
            {
                SenderId = ConnectionId,
                RecieverId = RecieverConnection,
                ToUser = RecieverName,
                FromUser = Name,
                DateSent = DateTime.Now,
                SenderImageUrl = $"{Server.ImagesEndPoint}Images/{Name[0].ToString().ToUpper()}.jpg",
                Message = text,
                Status = "Sent"

            };

            Proxy.Invoke("Send", message);

            Messages.Add(message);
            MessageText = string.Empty;
            MessagingCenter.Send<object, object>(this, "MessageReceived", message);
        }

        
    }
}
