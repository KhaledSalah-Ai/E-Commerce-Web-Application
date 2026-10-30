using AutoMapper;
using NovaCart.Application.DTOs;
using NovaCart.Domain.Entities;

namespace NovaCart.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // تحويل الـ Category إلى CategoryDto والعكس
            CreateMap<Category, CategoryDto>().ReverseMap();

            // تحويل الـ CreateCategoryDto إلى Category الأصلية عند الإضافة
            CreateMap<CreateCategoryDto, Category>();

            // تحويل الـ Product إلى ProductDto والعكس
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

            // تحويل الـ CreateProductDto إلى Product الأصلية عند الإضافة
            CreateMap<CreateProductDto, Product>();

            // تحويل الـ Order إلى OrderDto والعكس
            CreateMap<Order, OrderDto>().ReverseMap();

            // تحويل الـ OrderItem إلى OrderItemDto والعكس (مع جلب اسم المنتج تلقائياً)
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ReverseMap();

            // تحويل CreateOrderDto إلى Order Entity
            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderItemDto, OrderItem>();
        }
    }
}