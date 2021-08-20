using AutoFixture;
using AutoMapper;
using FakeItEasy;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace GameStore.UnitTests.BL.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task ShouldReturnOrdersWhenSendsUserIdAsync()
        {
            //Arrange
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var orders = new List<Order>() { fixture.Create<Order>() };
            var userId = fixture.Create<int>();

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).Returns(orders);


            //Act
            var result = await orderService.GetOrdersAsync(userId);

            //Assert
            Assert.NotNull(result);

            Assert.IsType<List<OutputOrderDto>>(result);

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnOrdersWhenSendsOrdersIdAsync()
        {
            //Arrange
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var orders = new List<Order>() { fixture.Create<Order>() };
            var userId = fixture.Create<int>();
            var ordersId = fixture.Create<int[]>();

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).Returns(orders);


            //Act
            var result = await orderService.GetOrdersAsync(userId, ordersId);

            //Assert
            Assert.NotNull(result);

            Assert.IsType<List<OutputOrderDto>>(result);

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task ShouldBeCalledOneTimeInDeleteOrderAsync()
        {
            //Arrange
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var orders = new List<ExtendedOrderDto>() { fixture.Create<ExtendedOrderDto>() };
            var userId = fixture.Create<int>();

            A.CallTo(() => orderRepository.SoftRangeRemoveAsync(A<ICollection<Order>>._));


            //Act
            await orderService.DeleteOrdersAsync(orders);

            //Assert
            A.CallTo(() => orderRepository.SoftRangeRemoveAsync(A<ICollection<Order>>._)).MustHaveHappenedOnceExactly();
        }





        [Fact]
        public async Task ShouldCompleteAllOrders()
        {
            //Arrange
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };


            var userId = fixture.Create<int>();
            var orders = new List<Order>() { fixture.Build<Order>().With(o => o.ApplicationUserId, userId).Create() };

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).Returns(orders);
            A.CallTo(() => orderRepository.UpdateItemsAsync(A<List<Order>>._)).Returns(orders);


            //Act
            var result = await orderService.CompleteOrdersAsync(userId);

            //Assert

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => orderRepository.UpdateItemsAsync(A<List<Order>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => libraryService.AddItemsToLibraryAsync(A<List<ProductLibraries>>._)).MustHaveHappenedOnceExactly();
            Assert.Equal(ServiceResultType.Success, result.Result);
        }

        [Fact]
        public async Task ShouldThrowsErrorInTransactionFromCompleteOrdersAsync()
        {
            //Arrange
            const string exceptionstring = "Transaction failed";
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };


            var userId = fixture.Create<int>();
            var orders = new List<Order>() { fixture.Build<Order>().With(o => o.ApplicationUserId, userId).Create() };
            var excpectedException = new Exception(exceptionstring);


            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).Returns(orders);
            A.CallTo(() => orderRepository.UpdateItemsAsync(A<List<Order>>._)).Throws(excpectedException);


            //Act
            var result = await orderService.CompleteOrdersAsync(userId);

            //Assert
            Assert.Equal(exceptionstring, result.ErrorMessage);

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => orderRepository.UpdateItemsAsync(A<List<Order>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => libraryService.AddItemsToLibraryAsync(A<List<ProductLibraries>>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ShouldReturnInternalErrorOnCompletedOrdersFromCompleteOrdersAsync()
        {
            //Arrange
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };


            var userId = fixture.Create<int>();
            var orders = new List<Order>() { fixture.Build<Order>().With(o => o.ApplicationUserId, userId).With(o => o.Status, OrderStatus.Completed).Create() };


            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).Returns(orders);


            //Act
            var result = await orderService.CompleteOrdersAsync(userId);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => orderRepository.UpdateItemsAsync(A<List<Order>>._)).MustNotHaveHappened();
            A.CallTo(() => libraryService.AddItemsToLibraryAsync(A<List<ProductLibraries>>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ShouldUpdateOrder()
        {
            //Arrange
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var extendedOrder = fixture.Create<ExtendedOrderDto>();
            var orders = JsonConvert.DeserializeObject<List<Order>>(JsonConvert.SerializeObject(new List<ExtendedOrderDto>() { extendedOrder }));

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).Returns(orders);

            //Act
            var result = await orderService.UpdateItemAsync(extendedOrder);

            //Assert
            Assert.NotNull(result);

            A.CallTo(() => orderRepository.GetOrdersAsync(A<Expression<Func<Order, bool>>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => orderRepository.UpdateItemAsync(A<Order>._, A<Expression<Func<Order, object>>>._)).MustHaveHappenedOnceExactly();
        }



        [Fact]
        public async Task ShouldCreateOrder()
        {
            //Arrange
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicOrder = fixture.Create<BasicOrderDto>();

            A.CallTo(() => orderRepository.SearchForSingleItemAsync(A<Expression<Func<Order, bool>>>._)).Returns(Task.FromResult<Order>(null));

            //Act
            var result = await orderService.CreateOrderAsync(basicOrder);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => orderRepository.SearchForSingleItemAsync(A<Expression<Func<Order, bool>>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => orderRepository.CreateItemAsync(A<Order>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnInternalErrorOnCreateOrder()
        {
            //Arrange
            var libraryService = A.Fake<IProductLibraryService>();
            var mapper = A.Fake<IMapper>();
            var orderRepository = A.Fake<IOrderRepository>();

            var orderService = new OrderService(orderRepository, mapper, libraryService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicOrder = fixture.Create<BasicOrderDto>();

            var order = fixture.Create<Order>();

            A.CallTo(() => orderRepository.SearchForSingleItemAsync(A<Expression<Func<Order, bool>>>._)).Returns(Task.FromResult(order));

            //Act
            var result = await orderService.CreateOrderAsync(basicOrder);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => orderRepository.SearchForSingleItemAsync(A<Expression<Func<Order, bool>>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => orderRepository.CreateItemAsync(A<Order>._)).MustNotHaveHappened();
        }
    }
}
