using AutoMapper;
using BasketApii.Entities;
using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApii.Mapping
{
    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
