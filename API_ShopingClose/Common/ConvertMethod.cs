using API_ShopingClose.Entities;
using API_ShopingClose.Model;
using API_ShopingClose.Models;

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

    public static ProductModel convertProductToProductModel(Product product)
    {
        ProductModel productModel = new ProductModel();

        productModel.productId = product.ProductID;
        productModel.name = product.ProductName;
        productModel.slug = product.Slug;
        productModel.description = product.Description;
        productModel.price = product.Price;
        productModel.brandId = product.BrandID;
        productModel.rate = product.Rate;
        productModel.image = product.Image;

        return productModel;
    }

    public static ProductInCategory convertProductInCategoryModelToProductInCategory(
        ProductInCategoryModel productInCategoryModel)
    {
        ProductInCategory productInCategory = new ProductInCategory();

        productInCategory.productId = productInCategoryModel.productId;
        productInCategory.categoryId = productInCategoryModel.categoryId;

        return productInCategory;
    }

    public static Product OverrideProduct(Product productOld, Product productNew)
    {
        Product productTmp = productOld;

        productTmp.ModifiedDate = DateTime.Now;
        productTmp.Rate = productNew.Rate;
        productTmp.Slug = productNew.Slug;
        productTmp.BrandID = productNew.BrandID;
        productTmp.Image = productNew.Image;
        productTmp.Description = productNew.Description;
        productTmp.ProductName = productNew.ProductName;

        return productTmp;
    }
    public static Cart convertCartModelToCart(CartModel cartmodel)
    {
        Cart cart = new Cart();

        cart.productId = cartmodel.productId;
        cart.sizeId = cartmodel.sizeId;
        cart.colorId = cartmodel.colorId;
        cart.quantity = cartmodel.quantity;

        return cart;
    }
    public static Order convertOrderModelToOrder(OrderModel orderModel)
    {
        Order order = new Order();
        order.OrderstatusID = orderModel.orderStatusId;
        order.PhoneShip = orderModel.phoneShip;
        order.AddresShip = orderModel.addressShip;
        order.NameShip = orderModel.nameShip;
        order.Note = orderModel.note;
        order.CreateDate = orderModel.createDate == null ? DateTime.Now : orderModel.createDate;
        order.UpdateDate = orderModel.updateDate == null ? DateTime.Now : orderModel.updateDate;

        return order;
    }

    public static OrderModel convertOrderToOrderModel(Order order)
    {
        OrderModel orderModel = new OrderModel();
        orderModel.orderId = order.OrderID;
        orderModel.userId = order.UserID;
        orderModel.orderStatusId = order.OrderstatusID;
        orderModel.phoneShip = order.PhoneShip;
        orderModel.addressShip = order.AddresShip;
        orderModel.nameShip = order.NameShip;
        orderModel.note = order.Note;
        orderModel.createDate = order.CreateDate;
        orderModel.updateDate = order.UpdateDate;

        return orderModel;
    }

    public static OrderDetailsModel convertOrderDetailsToOrderDetailModel(OrderDetails orderDetailTmp)
    {
        OrderDetailsModel orderDetailModel = new OrderDetailsModel();
        orderDetailModel.orderDetailId = orderDetailTmp.OrderdetailID;
        orderDetailModel.productId = orderDetailTmp.ProductID;
        orderDetailModel.sizeId = orderDetailTmp.SizeID;
        orderDetailModel.colorId = orderDetailTmp.ColorID;
        orderDetailModel.quantity = orderDetailTmp.Qunatity;
        orderDetailModel.price = orderDetailTmp.Price;
        orderDetailModel.promotion = orderDetailTmp.Promotion;
        orderDetailModel.orderId = orderDetailTmp.OrderID;

        return orderDetailModel;
    }
}
