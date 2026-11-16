using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaCart.Application.DTOs;
using NovaCart.Application.Interfaces;
using NovaCart.Domain.Entities;

namespace NovaCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 1. جلب كل المنتجات (GET: api/products)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // جلب المنتجات مع التصنيف الخاص بها لضمان ظهور اسم التصنيف (CategoryName)
            var products = await _unitOfWork.Products.GetAllAsync();

            // تحويل قائمة المنتجات إلى ProductDto باستخدام AutoMapper
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productDtos);
        }

        // 2. جلب منتج بالـ ID (GET: api/products/{id})
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        // 3. إضافة منتج جديد (POST: api/products)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            if (createProductDto == null)
            {
                return BadRequest(new { message = "Invalid product data." });
            }

            // التحقق من أن التصنيف (CategoryId) موجود فعلاً قبل إضافة المنتج
            var category = await _unitOfWork.Categories.GetByIdAsync(createProductDto.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = "The specified Category does not exist." });
            }

            // تحويل CreateProductDto إلى Product Entity
            var product = _mapper.Map<Product>(createProductDto);

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            // تحويل المنتج الناتج إلى ProductDto لإرجاع البيانات كاملة للعميل
            var productDto = _mapper.Map<ProductDto>(product);
            productDto.CategoryName = category.Name; // تعيين اسم التصنيف مباشرة

            return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
        }
        // 4. تحديث منتج موجود (PUT: api/products/{id}) - محمي بواسطة JWT Token
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null)
            {
                return BadRequest(new { message = "Invalid product data." });
            }

            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // التأكد من أن التصنيف الجديد (إذا تم تغييره) موجود في القاعدة
            var category = await _unitOfWork.Categories.GetByIdAsync(updateProductDto.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = "The specified Category does not exist." });
            }

            // استخدام AutoMapper لتحديث بيانات المنتج الموجود مباشرة
            _mapper.Map(updateProductDto, existingProduct);

            _unitOfWork.Products.Update(existingProduct);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Product updated successfully" });
        }

        // 5. حذف منتج (DELETE: api/products/{id}) - محمي بواسطة JWT Token
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Product deleted successfully" });
        }
    }
}