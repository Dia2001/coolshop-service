using API_ShopingClose.Service;
using API_ShopingClose.Entities;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Model;
using API_ShopingClose.Common;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> createProduct([FromForm] ProductModel productModel, [FromForm] IFormFile? file)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            Product product = new Product();
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

                product.ProductName = productModel.name;
                product.Image = await UploadFile(file);
                product.Slug = await getSlug(productModel.slug, productModel.name);
                product.Description = productModel.description;
                product.Price = productModel.price;
                product.BrandID = productModel.brandId;
                product.Rate = 0;

                try
                {

                    // Insert du lieu vao bang prooduct
                    var insert = _productservice.InsertProduct(product);
                    if (insert != null)
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

            }

            return StatusCode(StatusCodes.Status500InternalServerError, response);
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
