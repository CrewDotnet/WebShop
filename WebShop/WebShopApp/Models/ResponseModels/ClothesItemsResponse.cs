using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShopApp.Models.ResponseModels
{
    public class ClothesItemsResponse
    {
        public Guid Id {get; set;} = Guid.NewGuid();
        public string? Name {get; set;}
        public decimal Price {get; set;}
        public Guid ClothesTypeId {get; set;}
    }
            
}