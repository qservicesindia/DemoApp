using LoginForm.Attributes;
using LoginForm.Data;
using LoginForm.Helpers;
using LoginForm.Model.Product;
using LoginForm.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoginForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class ProductController : BaseApiController
    {

        #region fields

        private readonly AppSettings _appSettings;


        #endregion

        #region constructor

        public ProductController(AppDbContext context, IOptions<AppSettings> appSettings) : base(context)
        {
            _appSettings = appSettings.Value;

        }

        #endregion

        #region Product
        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct(ProductViewModel model)
        {
            try
            {
                using (AppUnitOfWork unitOfWork = new AppUnitOfWork(_context))
                {
                    ProductEntity product = unitOfWork.productRepository.FindOneReadOnly(m => m.ProductName.ToLower() == model.ProductName.ToLower());

                    if (product == null)
                    {
                        product = new ProductEntity()
                        {
                            ProductName = model.ProductName,
                            Quantity = model.Quantity,
                            Price = model.Price

                        };
                        unitOfWork.productRepository.InsertOrUpdate(product);
                        unitOfWork.Commit();

                        return ApiResponse(true, "Product Saved Successfully");
                    }

                    return ApiResponse(false, "Product Name already exists!");
                }
            }
            catch (Exception ex)
            {
                LogError(typeof(UserController), ex);
                return ApiResponse(false, GlobalConstants.MESSAGE_EXCEPTION, ex);
            }
        }

        [HttpPut("UpdateProduct/{productId}")]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductViewModel model)
        {
            try
            {
                using (AppUnitOfWork unitOfWork = new AppUnitOfWork(_context))
                {
                    if (productId > 0)
                    {
                        ProductEntity product = unitOfWork.productRepository.FindOneReadOnly(m => m.Id != productId && m.ProductName.ToLower() == model.ProductName.ToLower());
                        if (product == null)
                        {
                            product = unitOfWork.productRepository.FindOneReadOnly(m => m.Id == productId);
                            if (product != null)
                            {
                                product.ProductName = model.ProductName;
                                product.Quantity = model.Quantity;
                                product.Price = model.Price;
                                unitOfWork.productRepository.InsertOrUpdate(product);
                                unitOfWork.Commit();
                                return ApiResponse(true, "Product Updated Successfully");
                            }
                        }
                    }
                    return ApiResponse(false, "Product Name already exists!");
                }
            }
            catch (Exception ex)
            {
                LogError(typeof(UserController), ex);
                return ApiResponse(false, GlobalConstants.MESSAGE_EXCEPTION, ex);
            }
        }


        [HttpDelete("DeleteProduct/{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                using (AppUnitOfWork unitOfWork = new AppUnitOfWork(_context))
                {
                    if (productId > 0)
                    {
                        ProductEntity product = unitOfWork.productRepository.FindOneReadOnly(m => m.Id == productId);
                        if (product != null)
                        {
                            unitOfWork.productRepository.Delete(product);
                            unitOfWork.Commit();
                            return ApiResponse(true, "Product Deleted Successfully");
                        }
                    }
                    return ApiResponse(false, "Product Not Found!");
                }
            }
            catch (Exception ex)
            {
                LogError(typeof(UserController), ex);
                return ApiResponse(false, GlobalConstants.MESSAGE_EXCEPTION, ex);
            }
        }

        [HttpGet("GetAllProduct")]
        public IActionResult GetAllProduct(string productName)
        {
            try
            {
                ProductViewModel dmodel;
                IList<ProductViewModel> productList = new List<ProductViewModel>();
                using (AppUnitOfWork unitOfWork = new AppUnitOfWork(_context))
                {
                    {
                        var allProducts = unitOfWork.productRepository.GetAll();

                        ///Filters
                        if (!string.IsNullOrEmpty(productName))
                            allProducts = allProducts.Where(f => f.ProductName.ToLower().Contains(productName.ToLower())).ToList();

                        foreach (var item in allProducts)
                        {
                            dmodel = new ProductViewModel();
                            dmodel.Id = item.Id;
                            dmodel.ProductName = item.ProductName;
                            dmodel.Quantity = item.Quantity;
                            dmodel.Price = item.Price;
                            productList.Add(dmodel);
                        }

                        return ApiResponse(true, GlobalConstants.MESSAGE_SUCCESS, productList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(typeof(UserController), ex);
                return ApiResponse(false, GlobalConstants.MESSAGE_EXCEPTION, ex);
            }
        }
        #endregion
    }
}
