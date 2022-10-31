using API_ShopingClose.Common;
using API_ShopingClose.Entities;
using API_ShopingClose.Models;
using API_ShopingClose.Service;
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
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _productservice = productservice;
        }

        [HttpPost]
        [Route("orders")]
        public async Task<IActionResult> createOrder([FromBody] OrderModel orderModel)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            try
            {
                orderModel.OrderstatusID = OrderContants.CREATED;
                Order order = ConvertMethod.convertOrderModelToOrder(orderModel);
                order.UserID = Guid.Parse(GetUserId().ToString());

                var orderId = await _orderService.InsertOrder(order);
                if (orderId != null)
                {
                    foreach (var orderDetailTmp in orderModel.OrderDetail)
                    {
                        OrderDetails orderDetail = new OrderDetails();
                        orderDetail.ProductID = orderDetailTmp.ProductID;
                        orderDetail.SizeID = orderDetailTmp.SizeID;
                        orderDetail.ColorID = orderDetailTmp.ColorID;
                        orderDetail.Qunatity = orderDetailTmp.Quantity;
                        orderDetail.Promotion = orderDetailTmp.Promotion;
                        orderDetail.Price = orderDetailTmp.Price;
                        orderDetail.OrderID = Guid.Parse(orderId.ToString());
                        await _orderDetailService.InsertOrderDetail(orderDetail);
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
