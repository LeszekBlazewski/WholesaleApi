using AutoMapper;
using Wholesale.BL.Models;
using Wholesale.BL.Models.Dto;
using Wholesale.BL.Models.Query;

namespace WholesaleApi.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<RegisterQuery, User>();
            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<ProductCategoryDto, ProductCategory>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderDetails, OrderDetailsDto>();
            CreateMap<OrderDetailsDto, OrderDetails>();
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<User, UpdateUserQuery>();
            CreateMap<UpdateUserQuery, User>();
            CreateMap<Order, UpdateOrderStatusQuery>();
            CreateMap<UpdateOrderStatusQuery, Order>();
        }
    }
}
