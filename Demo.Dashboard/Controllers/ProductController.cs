using AutoMapper;
using Demo.Core.Domain.Contracts.Persistence;
using Demo.Core.Domain.Entities.Products;
using Demo.Dashboard.Helpers;
using Demo.Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Demo.Dashboard.Controllers
{
    public class ProductController(IUnitOfWork _unitOfWork,IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.GetRepository<Product,int>().GetAllAsync();
            var mappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);

            return View(mappedProducts);
        }

		public IActionResult Create()
		{
            return View();
		}
        [HttpPost]
		public async Task<IActionResult> Create(ProductViewModel productViewModel)
		{
            if (ModelState.IsValid)
            {
                if (productViewModel.Image != null)
                {
                    productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");
                }
                else
                    productViewModel.PictureUrl = "blueberry-cheesecake";

                var mappedProduct = _mapper.Map<ProductViewModel, Product>(productViewModel);
                await _unitOfWork.GetRepository<Product, int>().AddAsync(mappedProduct);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");
            }
            else
                return View(productViewModel);
 		}

		public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
            var mappedProduct = _mapper.Map<Product,ProductViewModel>(product);
            return View(mappedProduct);
        }
		[HttpPost]
		public async Task<IActionResult> Edit(int id,ProductViewModel productViewModel)
		{
            if (id != productViewModel.Id)
            { 
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                if(productViewModel.Image != null)
                {
                    if (productViewModel.PictureUrl != null)
                    {
                        PictureSettings.DeleteFile(productViewModel.PictureUrl, "products");
                    }
                    productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");
				}
                else
				  productViewModel.PictureUrl = PictureSettings.UploadFile(productViewModel.Image, "products");

                var mappedProduct = _mapper.Map<ProductViewModel,Product>(productViewModel);
                _unitOfWork.GetRepository<Product, int>().Update(mappedProduct);
                var result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                    return RedirectToAction("Index");
			} 
            
                return View(productViewModel); 
		}

		public async Task<IActionResult> Delete(int id)
		{
			var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
			var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
			return View(mappedProduct);
		}
		[HttpPost]
		public async Task<IActionResult> Delete(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
                return NotFound();

            try
            {
				var product = await _unitOfWork.GetRepository<Product, int>().GetAsync(id);
                if(product.PictureUrl != null)
                {
                    PictureSettings.DeleteFile(product.PictureUrl, "products");
                }
                _unitOfWork.GetRepository<Product, int>().Delete(product);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
			}
            catch(System.Exception ) 
            {
				return View(productViewModel);
			}

		}
	}
}
