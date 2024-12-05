using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopData.Models;

namespace WebShopApp.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping from Request models to Domain models
            CreateMap<ClothesItemRequest, ClothesItem>();
            CreateMap<ClothesTypeRequest, ClothesType>();
            CreateMap<CustomerRequest, Customer>();
            CreateMap<OrderRequest, Order>();

            // Mapping from Domain models to Response models
            CreateMap<ClothesItem, ClothesItemsResponse>();
            CreateMap<ClothesType, ClothesTypesResponse>();
            CreateMap<Customer, CustomerResponse>();
            CreateMap<Order, OrderResponse>();
        }
    }
}