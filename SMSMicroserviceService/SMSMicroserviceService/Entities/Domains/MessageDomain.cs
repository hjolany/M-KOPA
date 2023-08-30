﻿using MediatR;

namespace SMSMicroService.Entities.Domains
{
    public class MessageDomain : INotification
    {
        public string PhoneNumber { get; set; }
        public string Content { get; set; }
        public override string ToString()
        {
            return $"{PhoneNumber}=>\t{Content}";
        }
    }
}