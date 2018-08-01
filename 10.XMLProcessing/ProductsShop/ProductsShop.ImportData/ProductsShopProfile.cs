using AutoMapper;
using ProductsShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.ImportData
{
    public class ProductsShopProfile : Profile
    {
        public ProductsShopProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
