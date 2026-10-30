using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NovaCart.Application.DTOs;
using NovaCart.Application.Interfaces;
using NovaCart.Domain.Entities;

namespace NovaCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 1. جلب كل التصنيفات (GET: api/categories)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            // استخدام AutoMapper لتحويل قائمة الـ Entities إلى قائمة DTOs
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }

        // 2. جلب تصنيف بالـ ID (GET: api/categories/{id})
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            // تحويل الـ Entity إلى DTO فردي
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        // 3. إضافة تصنيف جديد (POST: api/categories) - محمي بواسطة الـ JWT Token
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null || string.IsNullOrWhiteSpace(createCategoryDto.Name))
            {
                return BadRequest(new { message = "Invalid category data. Name is required." });
            }

            // استخدام AutoMapper لتحويل الـ CreateCategoryDto إلى الـ Entity الأساسية
            var category = _mapper.Map<Category>(createCategoryDto);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            // تحويل الـ Entity الناتجة إلى DTO لإرجاعها في الـ Response
            var categoryDto = _mapper.Map<CategoryDto>(category);

            return CreatedAtAction(nameof(GetById), new { id = categoryDto.Id }, categoryDto);
        }
        // 4. تحديث تصنيف موجود (PUT: api/categories/{id}) - محمي بواسطة الـ JWT Token
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (updateCategoryDto == null)
            {
                return BadRequest(new { message = "Invalid category data." });
            }

            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            // استخدام AutoMapper لتحديث بيانات التصنيف الموجود
            _mapper.Map(updateCategoryDto, existingCategory);

            _unitOfWork.Categories.Update(existingCategory);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Category updated successfully" });
        }

        // 5. حذف تصنيف (DELETE: api/categories/{id}) - محمي بواسطة الـ JWT Token
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Category deleted successfully" });
        }
    }
}