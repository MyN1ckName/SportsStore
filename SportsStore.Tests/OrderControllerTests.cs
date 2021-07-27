using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Органиация - создание имитированного хранилища заказов
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Органиация - создание пустой карзины
            Cart cart = new Cart();

            // Органиация - создание заказа
            Order order = new Order();

            // Органиация - создание экземляра котроллера
            OrderController target = new OrderController(mock.Object, cart);

            // Действие
            ViewResult result = target.Checkout(order) as ViewResult;

            // Утверждение - проверка, что заказ не был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // Утверждение - проверка, что метод возвращает
            // стандартное представления
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            // Утверждение - проверка, что представлению передана
            // недопустимая модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_Shipping_Details()
        {
            // Органиация - создание имитированного хранилища заказов
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Органиация - создание пустой карзины
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Органиация - создание экземляра котроллера
            OrderController target = new OrderController(mock.Object, cart);

            // Организация - добавление ошибки в модель
            target.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            // Утверждение - проверка, что заказ не был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // Утверждение - проверка, что метод возвращает
            // стандартное представления
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            // Утверждение - проверка, что представлению передана
            // недопустимая модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Order()
        {
            // Органиация - создание имитированного хранилища заказов
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            // Органиация - создание пустой карзины
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Органиация - создание экземляра котроллера
            OrderController target = new OrderController(mock.Object, cart);

            // Действие - попытка перехода к оплате
            RedirectToPageResult result = target.Checkout(new Order()) as RedirectToPageResult;

            // Утверждение - проверка, что заказ был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

            // Утверждение - проверка, что метод перенаправляется
            // на действие Complited
            Assert.Equal("/Complited", result.PageName);
        }
    }
}