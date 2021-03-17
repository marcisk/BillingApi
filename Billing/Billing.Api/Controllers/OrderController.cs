using Billing.Api.Data;
using Billing.Api.Data.Repository;
using Billing.Api.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Billing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public OrderController(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("/api/order/process")]
        public IActionResult ProcessOrder(ProcessOrderIn input)
        {
            var result = new ProcessOrderOut()
            {
                HasErrors = false,
                ErrorMessage = string.Empty
            };

            // Check input
            // Check order number not empty
            if (string.IsNullOrWhiteSpace(input.OrderNumber))
            {
                result.HasErrors = true;
                result.ErrorMessage = "Wrong order number specified";
                return UnprocessableEntity(result);
            }

            // Check that order exists
            var order = _orderRepository.GetOrder(input.OrderNumber);
            if (order == null)
            {
                result.HasErrors = true;
                result.ErrorMessage = "Order with specified number doesn't exist";
                return UnprocessableEntity(result);
            }

            // Check amount
            if (order.Amount != input.PayableAmount)
            {
                result.HasErrors = true;
                result.ErrorMessage = "Payable amount is not correct for specified order";
                return UnprocessableEntity(result);
            }

            // Check user exists
            var user = _userRepository.Get(input.UserId);
            if (user == null)
            {
                result.HasErrors = true;
                result.ErrorMessage = "Specified user doesn't exist";
                return UnprocessableEntity(result);
            }

            // Check payment gateway. In this task only one payment gateway id is used - 1
            if(input.PaymentGateway != 1)
            {
                result.HasErrors = true;
                result.ErrorMessage = "Wrong payment gateway";
                return UnprocessableEntity(result);
            }

            // Process payment. For this task just generating receipt and storing order billing in DB. 
            try
            {
                var receipe = ProcessPayment(input);
                if (receipe == null || receipe.Length == 0)
                {
                    // As generic exception is catched, just thow exception
                    throw new Exception("Error processing receipe.");
                }

                result.HasErrors = false;
                result.ErrorMessage = string.Empty;
                result.Receipt = receipe;
            }
            catch (Exception ex)
            {
                // Catch there general exception
                result.HasErrors = true;
                result.ErrorMessage = ex.Message;
                return this.StatusCode(500, result);
            }

            return new JsonResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processOrderIn"></param>
        /// <returns></returns>
        private byte[] ProcessPayment(ProcessOrderIn processOrderIn)
        {
            var paymentDate = DateTime.Now;
            var receiptTemplate = Properties.Resources.Receipt
                .Replace("{PaymentDate}", paymentDate.ToString("dd.MM.yyyy HH:mm:ss"))
                .Replace("{PaymentAmount}", processOrderIn.PayableAmount.ToString())
                .Replace("{OrderNumber}", processOrderIn.OrderNumber);

            var renderer = new IronPdf.HtmlToPdf();
            var receipt = renderer.RenderHtmlAsPdf(receiptTemplate);

            // Save order billing to database
            var orderBilling = new OrderBilling
            {
                Description = processOrderIn.Description,
                OrderNumber = processOrderIn.OrderNumber,
                PayableAmount = processOrderIn.PayableAmount,
                PaymentGateway = 1,
                UserId = processOrderIn.UserId
            };
            _orderRepository.CreateOrderBilling(orderBilling);
            
            return receipt.BinaryData;
        }
    }
}
