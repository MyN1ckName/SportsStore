using System.Linq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // Организация - создание новой карзины
            Cart target = new Cart();

            // Действие
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            CartLine[] result = target.Lines.ToArray();

            // Утверждение
            Assert.Equal(2, result.Length);
            Assert.Equal(p1, result[0].Product);
            Assert.Equal(p2, result[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            // Организация - создание новой карзины
            Cart target = new Cart();

            // Действие
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);

            CartLine[] result = target.Lines
               .OrderBy(c => c.Product.ProductID).ToArray();

            // Утверждение
            Assert.Equal(2, result.Length);
            Assert.Equal(11, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            // Организация - создание новой карзины
            Cart target = new Cart();

            // Организация - добавление нескольких элементов в карзину
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            // Действие
            target.RemoveLine(p2);

            // Утверждение
            Assert.Empty(target.Lines.Where(c => c.Product == p2));
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2" , Price = 50M };

            // Организация - создание новой карзины
            Cart target = new Cart();

            // Организация - добавление нескольких элементов в карзину
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);

            // Действие
            decimal result = target.ComputeTotalValue();

            // Утверждение
            Assert.Equal(450M, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // Организация - создание новой карзины
            Cart target = new Cart();

            // Организация - добавление нескольких элементов в карзину
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Действие
            target.Clear();

            // Утверждение
            Assert.Empty(target.Lines);
        }
    }
}