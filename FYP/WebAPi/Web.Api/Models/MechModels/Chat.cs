using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Api.Models.MechModels
{
    public class Chat
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
        public string SenderImageUrl { get; set; }
        public string Status { get; set; }
    }
}