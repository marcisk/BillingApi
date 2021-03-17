using Billing.Api.Controllers;
using Billing.Api.Models.ApiModels;
using Billing.Tests.MockClasses;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Billing.Tests
{
    public class TestOrders
    {
        [Fact(DisplayName = "Check error for wrong order number")]
        public void TestOrderProcessing_EmptyOrderNumber()
        {
            var orderController = new OrderController(new OrderRepositoryMock(), new UserRepositoryMock());
            var processOrderIn = new ProcessOrderIn()
            {
                Description = string.Empty,
                OrderNumber = string.Empty,
                PayableAmount = 0.0M,
                PaymentGateway = 1,
                UserId = 1
            };

            var result = orderController.ProcessOrder(processOrderIn);
            Assert.IsType<UnprocessableEntityObjectResult>(result);

            var objectResult = result as UnprocessableEntityObjectResult;
            Assert.IsType<ProcessOrderOut>(objectResult.Value);

            var processOrderOut = objectResult.Value as ProcessOrderOut;
            Assert.True(processOrderOut.HasErrors);
            Assert.Equal("Wrong order number specified", processOrderOut.ErrorMessage);
        }

        [Fact(DisplayName = "Check error for not existing order")]
        public void TestOrderProcessing_NotExistingOrderNumber()
        {
            var orderController = new OrderController(new OrderRepositoryMock(), new UserRepositoryMock());
            var processOrderIn = new ProcessOrderIn()
            {
                Description = string.Empty,
                OrderNumber = "12",
                PayableAmount = 0.0M,
                PaymentGateway = 1,
                UserId = 1
            };

            var result = orderController.ProcessOrder(processOrderIn);
            Assert.IsType<UnprocessableEntityObjectResult>(result);

            var objectResult = result as UnprocessableEntityObjectResult;
            Assert.IsType<ProcessOrderOut>(objectResult.Value);

            var processOrderOut = objectResult.Value as ProcessOrderOut;
            Assert.True(processOrderOut.HasErrors);
            Assert.Equal("Order with specified number doesn't exist", processOrderOut.ErrorMessage);
        }

        [Fact(DisplayName = "Check error for wrong order amount")]
        public void TestOrderProcessing_WrongOrderAmount()
        {
            var orderController = new OrderController(new OrderRepositoryMock(), new UserRepositoryMock());
            var processOrderIn = new ProcessOrderIn()
            {
                Description = string.Empty,
                OrderNumber = "1",
                PayableAmount = 101.0M,
                PaymentGateway = 1,
                UserId = 1
            };

            var result = orderController.ProcessOrder(processOrderIn);
            Assert.IsType<UnprocessableEntityObjectResult>(result);

            var objectResult = result as UnprocessableEntityObjectResult;
            Assert.IsType<ProcessOrderOut>(objectResult.Value);

            var processOrderOut = objectResult.Value as ProcessOrderOut;
            Assert.True(processOrderOut.HasErrors);
            Assert.Equal("Payable amount is not correct for specified order", processOrderOut.ErrorMessage);
        }

        [Fact(DisplayName = "Check error for non existing user")]
        public void TestOrderProcessing_NotExistingUser()
        {
            var orderController = new OrderController(new OrderRepositoryMock(), new UserRepositoryMock());
            var processOrderIn = new ProcessOrderIn()
            {
                Description = string.Empty,
                OrderNumber = "1",
                PayableAmount = 100.0M,
                PaymentGateway = 1,
                UserId = 2
            };

            var result = orderController.ProcessOrder(processOrderIn);
            Assert.IsType<UnprocessableEntityObjectResult>(result);

            var objectResult = result as UnprocessableEntityObjectResult;
            Assert.IsType<ProcessOrderOut>(objectResult.Value);

            var processOrderOut = objectResult.Value as ProcessOrderOut;
            Assert.True(processOrderOut.HasErrors);
            Assert.Equal("Specified user doesn't exist", processOrderOut.ErrorMessage);
        }

        [Fact(DisplayName = "Check error for non existing payment gateway")]
        public void TestOrderProcessing_NotExistingGateway()
        {
            var orderController = new OrderController(new OrderRepositoryMock(), new UserRepositoryMock());
            var processOrderIn = new ProcessOrderIn()
            {
                Description = string.Empty,
                OrderNumber = "1",
                PayableAmount = 100.0M,
                PaymentGateway = 2,
                UserId = 2
            };

            var result = orderController.ProcessOrder(processOrderIn);
            Assert.IsType<UnprocessableEntityObjectResult>(result);

            var objectResult = result as UnprocessableEntityObjectResult;
            Assert.IsType<ProcessOrderOut>(objectResult.Value);

            var processOrderOut = objectResult.Value as ProcessOrderOut;
            Assert.True(processOrderOut.HasErrors);
            Assert.Equal("Specified user doesn't exist", processOrderOut.ErrorMessage);
        }

        [Fact(DisplayName = "Check order billing processed and billing done")]
        public void TestOrderProcessing_OrderBillingDone()
        {
            var orderController = new OrderController(new OrderRepositoryMock(), new UserRepositoryMock());
            var processOrderIn = new ProcessOrderIn()
            {
                Description = string.Empty,
                OrderNumber = "1",
                PayableAmount = 100.0M,
                PaymentGateway = 1,
                UserId = 1
            };

            var result = orderController.ProcessOrder(processOrderIn);
            Assert.IsType<JsonResult>(result);

            var objectResult = result as JsonResult;
            Assert.IsType<ProcessOrderOut>(objectResult.Value);

            var processOrderOut = objectResult.Value as ProcessOrderOut;
            Assert.False(processOrderOut.HasErrors);
            Assert.NotNull(processOrderOut.Receipt);
            Assert.NotEmpty(processOrderOut.Receipt);
        }
    }
}
