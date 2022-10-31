using API_ShopingClose.Common;
using API_ShopingClose.Entities;
using API_ShopingClose.Models;
using API_ShopingClose.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class OrdersController : AuthController
    {
        private readonly IConfiguration _config;
        OrderDeptService _orderService;
        OrderDetailDeptService _orderDetailService;
        ProductDeptService _productservice;
        public OrdersController(IConfiguration config, ILogger<UsersController> logger,
            OrderDeptService orderService, OrderDetailDeptService orderDetailService, ProductDeptService productservice) : base(logger)
        {
            _config = config;
            _orderService =orderService;
            _orderDetailService = orderDetailService;
            _productservice = productservice;
        }

        [HttpPost]
        [Route("Orders")]
        public async Task<IActionResult> addOrder([FromBody] OrderModel orderModel)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            try
            {
                Order order = ConvertMethod.convertOrderModelToOrder(orderModel);
                order.UserID = Guid.Parse(GetUserId().ToString());
                var orderId=await _orderService.InsertOrder(order);
                if (orderId != null)
                {
                    foreach(var orderDetail in orderModel.OrderDetail)
                    {
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.ProductID = orderDetail.ProductID;
                        orderDetails.SizeID = orderDetail.SizeID;
                        orderDetail.ColorID = orderDetail.ColorID;
                        orderDetail.Quantity = orderDetails.Qunatity;
                        orderDetail.Price = orderDetails.Price;
                        orderDetail.Promotion = orderDetails.Promotion;
                        orderDetail.OrderID =Guid.Parse(orderId.ToString());
                        await _orderDetailService.InsertOrderDetail(orderDetails);
                    }
                }
                response = new
                {
                    status = 201,
                    message = "Tạo sản phẩm thành công!",
                    data = orderId
                };
                return StatusCode(StatusCodes.Status201Created, response);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
}
