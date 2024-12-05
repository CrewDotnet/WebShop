using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShopApp.Models.ResponseModels
{
    public class OrderResponse
    {
        //public List<Order> Orders {get; set;} = new List<Order>();
        public Guid Id {get; set;} = Guid.NewGuid();
        public decimal TotalPrice {get; set;}
        public Guid CustomerId {get;set;}
    }
}