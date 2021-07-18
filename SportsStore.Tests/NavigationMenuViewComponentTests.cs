using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            // Организация
            Mock<IStoreRepository> mok = new Mock<IStoreRepository>();
            mok.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category="Apples"},
                new Product {ProductID = 2, Name = "P2", Category="Apples"},
                new Product {ProductID = 3, Name = "P3", Category="Plums"},
                new Product {ProductID = 4, Name = "P4", Category="Oranges"},
            }).AsQueryable<Product>());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mok.Object);

            // Действие - получение категорий
            string [] result = ((IEnumerable<string>)(target.Invoke()
                as ViewViewComponentResult).ViewData.Model).ToArray();

            // Утверждение
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, result));            
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            // Организация
            string categoryToSelect = "Apples";
            Mock<IStoreRepository> mok = new Mock<IStoreRepository>();
            mok.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category="Apples"},
                new Product {ProductID = 2, Name = "P2", Category="Oranges"},
            }).AsQueryable<Product>());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mok.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            // Действие - получение категорий
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            // Утверждение
            Assert.Equal(categoryToSelect, result);
        }
    }
}
