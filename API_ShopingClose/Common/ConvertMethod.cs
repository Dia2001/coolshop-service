using API_ShopingClose.Entities;
using API_ShopingClose.Model;

namespace API_ShopingClose.Common;

public class ConvertMethod
{
    public static Product convertProductModleToProduct(ProductModel productModel)
    {
        Product product = new Product();

        product.Image = productModel.image == null ? "" : productModel.image;
        product.ProductName = productModel.name;
        product.Slug = productModel.slug == null ? "" : productModel.slug;
        product.Description = productModel.description;
        product.Price = productModel.price;
        product.BrandID = productModel.brandId;
        product.Rate = productModel.rate;
        product.ProductID = productModel.productId;

        return product;
    }

    public static ProductInCategory convertProductInCategoryModelToProductInCategory(
        ProductInCategoryModel productInCategoryModel)
    {
        ProductInCategory productInCategory = new ProductInCategory();

        productInCategory.productId = productInCategoryModel.productId;
        productInCategory.categoryId = productInCategoryModel.categoryId;

        return productInCategory;
    }
}
