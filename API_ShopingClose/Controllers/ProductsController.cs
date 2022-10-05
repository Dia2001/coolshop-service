﻿using API_ShopingClose.Service;
using API_ShopingClose.Entities;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Model;
using API_ShopingClose.Common;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ProductDeptService _productservice;
        ProductInCategoryDeptService _productInCategoryService;
        ProductDetailsDeptService _productDetailService;

        public ProductsController(ProductDeptService productservice,
            ProductInCategoryDeptService productInCategoryService,
            ProductDetailsDeptService productDetailsDeptService)
        {
            _productservice = productservice;
            _productInCategoryService = productInCategoryService;
            _productDetailService = productDetailsDeptService;
        }

        // Lấy tất cả các sản phẩm
        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> getAllProducts()
        {
            try
            {
                List<Product> allProducts = (await _productservice.getAllProducts()).ToList();
                List<ProductModel> products = new List<ProductModel>();

                foreach (var oneproduct in allProducts)
                {
                    ProductModel product = new ProductModel();
                    product.productId = oneproduct.ProductID;
                    product.name = oneproduct.ProductName;
                    product.price = oneproduct.Price;
                    product.description = oneproduct.Description;
                    product.slug = oneproduct.Slug;
                    product.image = oneproduct.Image;
                    product.brandId = oneproduct.BrandID;
                    product.rate = oneproduct.Rate;

                    List<ProductInCategory> allCategoryProducts = (await
                  _productInCategoryService.getProductInCategoryByProductID(oneproduct.ProductID)).ToList();

                    Guid[] listIdCategoryInProduct = new Guid[allCategoryProducts.Count()];
                    int i = 0;
                    foreach (var oneproductincategory in allCategoryProducts)
                    {
                        listIdCategoryInProduct[i] = oneproductincategory.categoryId;
                        i++;
                    }
                    product.categories = listIdCategoryInProduct;

                    List<ProductDetails> allProductDetails = (await
                        _productDetailService.getAllProductDetailByProductId(oneproduct.ProductID)).ToList();

                    List<ProductDetails> listSizeInProduct = new List<ProductDetails>();
                    List<ProductDetails> listColorInProduct = new List<ProductDetails>();

                    int totalQuantity = 0;

                    if (allProductDetails.Count() > 0)
                    {
                        int colorI = 0;
                        int sizeI = 0;
                        foreach (var oneproductdetail in allProductDetails)
                        {
                            totalQuantity += oneproductdetail.quantity;

                            bool isNotEmptyColor = false;
                            bool isNotEmptySize = false;

                            foreach (var productDetailTmp in listColorInProduct)
                            {
                                if (oneproductdetail.colorId.Equals(productDetailTmp.colorId))
                                {
                                    isNotEmptyColor = true;
                                    break;
                                }
                            }
                            if (!isNotEmptyColor)
                            {
                                listColorInProduct.Add(oneproductdetail);
                                colorI++;
                            }

                            foreach (var productDetailTmp in listSizeInProduct)
                            {
                                if (oneproductdetail.sizeId.Equals(productDetailTmp.sizeId))
                                {
                                    isNotEmptySize = true;
                                    break;
                                }
                            }
                            if (!isNotEmptySize)
                            {
                                listSizeInProduct.Add(oneproductdetail);
                                sizeI++;
                            }
                        }
                    }

                    Detail detailp = new Detail();

                    Guid[] listSizeId = new Guid[listSizeInProduct.Count()];
                    Guid[] listColorId = new Guid[listColorInProduct.Count()];

                    int sizeCount = 0;
                    int colorCount = 0;

                    foreach (var proudctSize in listSizeInProduct)
                    {
                        listSizeId[sizeCount] = proudctSize.sizeId;
                        sizeCount++;
                    }

                    foreach (var proudctColor in listColorInProduct)
                    {
                        listColorId[colorCount] = proudctColor.colorId;
                        colorCount++;
                    }

                    detailp.sizes = listSizeId;
                    detailp.colors = listColorId;
                    product.detail = detailp;
                    product.totalQuantity = totalQuantity;
                    product.rate = 0;
                    products.Add(product);
                }
                return StatusCode(StatusCodes.Status200OK, products);

            }
            catch (Exception)
            {
                dynamic response = new
                {
                    status = 500,
                    message = "Call servser faile!",
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }



        [HttpPost]
        [Route("products")]
        public async Task<IActionResult> createProduct([FromForm] ProductModel productModel, [FromForm] IFormFile? file)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            Product product = ConvertMethod.convertProductModleToProduct(productModel);
            if (file != null)
            {
                if (!Validate.ValidateImageFileNameUpload(file.ContentType))
                {
                    response = new
                    {
                        status = 400,
                        message = "File không đúng định dạng!"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                if (file.Length > Constants.MAX_FILE_IMAGE_UPLOAD)
                {
                    response = new
                    {
                        status = 400,
                        message = "Kích thước file không được vượt quá 4Mb!"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                product.Image = await UploadFile(file);
            }

            product.Slug = await getSlug(productModel.slug, productModel.name);
            product.Rate = 0;

            try
            {
                // Insert du lieu vao bang prooduct
                var insert = _productservice.InsertProduct(product);
                if (insert != null)
                {
                    if (productModel.categories != null)
                    {
                        // Tao moi entity productInCategory chua thong tin productId va categoryId
                        ProductInCategory productInCategory = new ProductInCategory();
                        productInCategory.productId = insert;

                        // Lap qua danh sach categoryId roi them data vao
                        foreach (Guid categoryId in productModel.categories)
                        {
                            productInCategory.categoryId = categoryId;
                            await _productInCategoryService.InsertPrductInCategory(productInCategory);
                        }
                    }

                    if (productModel.detail != null && productModel.detail.sizes != null)
                    {
                        ProductDetails productDetails = new ProductDetails();
                        productDetails.quantity = 0;
                        productDetails.productId = insert;

                        // Voi moi mot colorId va sizeId thi chen du lieu vao
                        foreach (Guid sizeId in productModel.detail.sizes)
                        {
                            foreach (Guid colorId in productModel.detail.colors)
                            {
                                productDetails.sizeId = sizeId;
                                productDetails.colorId = colorId;
                                await _productDetailService.InsertProductDetails(productDetails);
                            }
                        }
                    }

                    response = new
                    {
                        status = 201,
                        message = "Tạo sản phẩm thành công!",
                        data = insert
                    };
                    return StatusCode(StatusCodes.Status201Created, response);
                }
                else
                {
                    response = new
                    {
                        status = 400,
                        message = "Có lỗi xảy ra trong quá trình tạo dữ liệu!",
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        [HttpPut]
        [Route("products/{productId}")]
        public async Task<IActionResult> updateProduct([FromRoute] Guid productId,
            [FromForm] IFormFile? file, [FromForm] ProductModel productModel)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            if (!ModelState.IsValid || !productId.Equals(productModel.productId))
            {
                response = new
                {
                    status = 400,
                    message = "Dữ liệu gửi lên không hợp lệ!"
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            try
            {
                Product product = ConvertMethod.convertProductModleToProduct(productModel);

                if (file != null)
                {
                    if (!Validate.ValidateImageFileNameUpload(file.ContentType))
                    {
                        response = new
                        {
                            status = 400,
                            message = "File không đúng định dạng!"
                        };
                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    if (file.Length > Constants.MAX_FILE_IMAGE_UPLOAD)
                    {
                        response = new
                        {
                            status = 400,
                            message = "Kích thước file không được vượt quá 4Mb!"
                        };
                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }

                    // Xoa file anh goc (Tam thoi an di do may bi loi khong cho xoa)
                    // System.IO.File.Delete(Constants.ROOT_PATH_IMAGE_PRODUCT + productModel.image);

                    product.Image = await UploadFile(file);

                }

                if (productModel.slug == null || productModel.slug.Equals(""))
                {
                    product.Slug = await getSlug(null, product.ProductName);
                }
                else
                {
                    product.Slug = await getSlug(null, productModel.slug);
                }

                if (await _productservice.UpdateProduct(product, productId))
                {
                    response = new
                    {
                        status = 200,
                        message = "Cập nhật sản phẩm thành công!"
                    };

                    return StatusCode(StatusCodes.Status200OK, response);

                }
                else
                {
                    response = new
                    {
                        status = 400,
                        message = "Cập nhật sản phẩm thất bại!"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("products/quantity")]
        public async Task<IActionResult> addQuantityBySizeAndColor([FromBody] ProductDetailModel productDetailModel)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            if (!ModelState.IsValid)
            {
                response = new
                {
                    status = 400,
                    message = "Dữ liệu gửi lên không hợp lệ!"
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            try
            {
                if (await _productDetailService.updateQuantityByProductIdAndSizeIdAndColorId(
                      productDetailModel.productId, productDetailModel.sizeId, productDetailModel.colorId,
                      productDetailModel.quantity))
                {
                    response = new
                    {
                        status = 200,
                        message = "Thêm số lượng sản phẩm thành công!"
                    };
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    response = new
                    {
                        status = 400,
                        message = "Thêm số lượng sản phẩm thất bại!"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("products/size")]
        public async Task<IActionResult> addSize([FromBody] AddSizeToProductModel addSizeToProductModel)
        {

            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            if (!ModelState.IsValid)
            {
                response = new
                {
                    status = 400,
                    message = "Dữ liệu gửi lên không hợp lệ!"
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            try
            {

                List<ProductDetails> productDetails = (await
                  _productDetailService.getAllProductDetailByProductId(addSizeToProductModel.productId)).ToList();

                List<Guid> listColorId = new List<Guid>();

                foreach (var productDT in productDetails)
                {
                    bool isNotEmpty = false;
                    foreach (var colorId in listColorId)
                    {
                        if (productDT.colorId.Equals(colorId))
                        {
                            isNotEmpty = true;
                            break;
                        }
                    }
                    if (!isNotEmpty)
                    {
                        listColorId.Add(productDT.colorId);
                    }
                }

                ProductDetails productDetail = new ProductDetails();
                productDetail.productId = addSizeToProductModel.productId;
                productDetail.sizeId = addSizeToProductModel.sizeId;

                foreach (var colorId in listColorId)
                {
                    productDetail.colorId = colorId;
                    try
                    {
                        await _productDetailService.InsertProductDetails(productDetail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        response = new
                        {
                            status = 400,
                            message = "Thêm size cho sản phẩm thất bại!"
                        };
                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }
                }

                response = new
                {
                    status = 200,
                    message = "Thêm size cho sản phẩm thành công!"
                };
                return StatusCode(StatusCodes.Status200OK, response);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("products/size/delete")]
        public async Task<IActionResult> deleteSize([FromBody] AddSizeToProductModel addSizeToProductModel)
        {

            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            if (!ModelState.IsValid)
            {
                response = new
                {
                    status = 400,
                    message = "Dữ liệu gửi lên không hợp lệ!"
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            try
            {
                try
                {
                    await _productDetailService.deleteAllProductDetailByProductIdAndSizeId(
                        addSizeToProductModel.productId, addSizeToProductModel.sizeId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    response = new
                    {
                        status = 400,
                        message = "Xóa size cho sản phẩm thất bại!"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                response = new
                {
                    status = 200,
                    message = "Xóa size cho sản phẩm thành công!"
                };
                return StatusCode(StatusCodes.Status200OK, response);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("products/color")]
        public async Task<IActionResult> addColor([FromBody] AddColorToProductModel addColorToProductModel)
        {

            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            if (!ModelState.IsValid)
            {
                response = new
                {
                    status = 400,
                    message = "Dữ liệu gửi lên không hợp lệ!"
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            try
            {

                List<ProductDetails> productDetails = (await
                  _productDetailService.getAllProductDetailByProductId(addColorToProductModel.productId)).ToList();

                List<Guid> listSizeId = new List<Guid>();

                foreach (var productDT in productDetails)
                {
                    bool isNotEmpty = false;
                    foreach (var sizeId in listSizeId)
                    {
                        if (productDT.sizeId.Equals(sizeId))
                        {
                            isNotEmpty = true;
                            break;
                        }
                    }
                    if (!isNotEmpty)
                    {
                        listSizeId.Add(productDT.sizeId);
                    }
                }

                ProductDetails productDetail = new ProductDetails();
                productDetail.productId = addColorToProductModel.productId;
                productDetail.colorId = addColorToProductModel.colorId;

                foreach (var sizeId in listSizeId)
                {
                    productDetail.sizeId = sizeId;
                    try
                    {
                        await _productDetailService.InsertProductDetails(productDetail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        response = new
                        {
                            status = 400,
                            message = "Thêm color cho sản phẩm thất bại!"
                        };
                        return StatusCode(StatusCodes.Status400BadRequest, response);
                    }
                }

                response = new
                {
                    status = 200,
                    message = "Thêm color cho sản phẩm thành công!"
                };
                return StatusCode(StatusCodes.Status200OK, response);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("products/color/delete")]
        public async Task<IActionResult> deleteColor([FromBody] AddColorToProductModel addColorToProductModel)
        {

            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            if (!ModelState.IsValid)
            {
                response = new
                {
                    status = 400,
                    message = "Dữ liệu gửi lên không hợp lệ!"
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            try
            {
                try
                {
                    await _productDetailService.deleteAllProductDetailByProductIdAndColorId(
                        addColorToProductModel.productId, addColorToProductModel.colorId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    response = new
                    {
                        status = 400,
                        message = "Xóa color cho sản phẩm thất bại!"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                response = new
                {
                    status = 200,
                    message = "Xóa color cho sản phẩm thành công!"
                };
                return StatusCode(StatusCodes.Status200OK, response);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("products/categories")]
        public async Task<IActionResult> createProductInCategory([FromBody] ProductInCategoryModel productInCategoryModel)
        {

            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            if (!ModelState.IsValid)
            {
                response = new
                {
                    status = 400,
                    message = "Dữ liệu gửi lên không hợp lệ!"
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            try
            {
                if (await _productInCategoryService.InsertPrductInCategory(
                    ConvertMethod.convertProductInCategoryModelToProductInCategory(productInCategoryModel)))
                {
                    response = new
                    {
                        status = 201,
                        message = "Thêm danh mục cho sản phẩm thành công!"
                    };
                    return StatusCode(StatusCodes.Status201Created, response);
                }
                else
                {
                    response = new
                    {
                        status = 400,
                        message = "Thêm danh mục cho sản phẩm thất bại!"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("products/categories/delete")]
        public async Task<IActionResult> deleteProductInCategory([FromBody] ProductInCategoryModel productInCategoryModel)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            if (!ModelState.IsValid)
            {
                response = new
                {
                    status = 400,
                    message = "Dữ liệu gửi lên không hợp lệ!"
                };
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            try
            {
                if (await _productInCategoryService.deleteProductInCategory(
                    ConvertMethod.convertProductInCategoryModelToProductInCategory(productInCategoryModel)))
                {
                    response = new
                    {
                        status = 200,
                        message = "Xóa danh mục cho sản phẩm thàn công!"
                    };
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                else
                {
                    response = new
                    {
                        status = 400,
                        message = "Xóa danh mục cho sản phẩm thất bại!"
                    };
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        private async Task<string> getSlug(string? slugParam, string productName)
        {
            string slug;

            if (slugParam != null)
            {
                slug = slugParam;
            }
            else
            {
                slug = Utils.getSlugFromName(productName);
            }

            int indexSlug = 1;
            string slugTmp = slug;
            while (await _productservice.checkSlugEmpty(slugTmp))
            {
                slugTmp = slug + "-" + indexSlug;
                indexSlug++;
            }

            return slugTmp;
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + file.FileName;
                string path = Constants.ROOT_PATH_IMAGE_PRODUCT + fileName;

                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }
}
