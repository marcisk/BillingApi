using Billing.Api.Controllers;
using Billing.Api.Data;
using Billing.Api.Data.Repository;
using Billing.Api.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Billing.Tests
{
    public class TestOrders
    {
        private (IUserRepository, IOrderRepository) GetMockObjects()
        {
            var userMock = new Mock<IUserRepository>();
            userMock.Setup(u => u.Get(It.IsAny<int>())).Returns<int>(id =>
            {
                if (id == 1)
                {
                    return new User
                    {
                        Id = 1,
                        UserName = "test"
                    };
                }

                return null;
            });

            var orderMock = new Mock<IOrderRepository>();
            orderMock.Setup(o => o.GetOrder(It.IsAny<string>())).Returns<string>(s =>
            {
                if (s == "1")
                {
                    return new Order
                    {
                        Amount = 100.0M,
                        OrderNumber = "1",
                        Id = 1
                    };
                }

                return null;
            });

            return (userMock.Object, orderMock.Object);
        }

        [Fact(DisplayName = "Check error for wrong order number")]
        public void TestOrderProcessing_EmptyOrderNumber()
        {
            (IUserRepository userMock, IOrderRepository orderMock) = GetMockObjects();
            var orderController = new OrderController(orderMock, userMock);
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
            (IUserRepository userMock, IOrderRepository orderMock) = GetMockObjects();
            var orderController = new OrderController(orderMock, userMock);
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
            (IUserRepository userMock, IOrderRepository orderMock) = GetMockObjects();
            var orderController = new OrderController(orderMock, userMock);
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
            (IUserRepository userMock, IOrderRepository orderMock) = GetMockObjects();
            var orderController = new OrderController(orderMock, userMock);
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
            (IUserRepository userMock, IOrderRepository orderMock) = GetMockObjects();
            var orderController = new OrderController(orderMock, userMock);
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
            (IUserRepository userMock, IOrderRepository orderMock) = GetMockObjects();
            var orderController = new OrderController(orderMock, userMock);
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
