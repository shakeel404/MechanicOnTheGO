using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Web.Api.Models;
using Web.Api.Models.MechModels;

namespace Web.Api
{
    public class MechHub : Hub
    {
       

        public override Task OnDisconnected(bool stopCalled)
        {

            var connection = this.Context.ConnectionId; 

            Clients.Caller.disconnect();

          
            return base.OnDisconnected(stopCalled);
        }


        public void Connect(string contactNo)
        {  
            Clients.Others.newUserConnected(contactNo, this.Context.ConnectionId);  
        } 
       

        public void EchoNewUser(string contact,string connection)
        {
            Clients.Client(connection).echo(contact, this.Context.ConnectionId);
        }

        public void Send(Chat message)
        {
            var to = message.RecieverId;

            Clients.Client(to).recieveMessage(message);
        }



        
    }
}