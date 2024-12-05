using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShopApp.Models.ResponseModels
{
    public class ClothesTypesResponse
    {
        //public List<ClothesType> ClothesTypes {get; set;} = new List<ClothesType>();
        public Guid Id {get; set;} = Guid.NewGuid();
        public string? Type {get; set;}
    }
}