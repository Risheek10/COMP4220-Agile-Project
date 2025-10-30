using Microsoft.VisualStudio.TestTools.UnitTesting;
using ywBookStoreLIB;

namespace ShoppingCartTests
{
    [TestClass]
    public class ShoppingCartTests
    {
        [TestMethod]
        public void AddItem_ShouldAddNewItemToCart()
        {
            // Arrange
            var order = new BookOrder();
            var item = new OrderItem("123", "Test Book", 10.0, 1);

            // Act
            order.AddItem(item);

            // Assert
            Assert.AreEqual(1, order.OrderItemList.Count);
            Assert.AreEqual("123", order.OrderItemList[0].BookID);
        }

        [TestMethod]
        public void AddItem_ShouldIncreaseQuantityIfBookAlreadyExists()
        {
            // Arrange
            var order = new BookOrder();
            var item1 = new OrderItem("123", "Test Book", 10.0, 1);
            var item2 = new OrderItem("123", "Test Book", 10.0, 2);

            // Act
            order.AddItem(item1);
            order.AddItem(item2);

            // Assert
            Assert.AreEqual(1, order.OrderItemList.Count);
            Assert.AreEqual(3, order.OrderItemList[0].Quantity);
        }

        [TestMethod]
        public void RemoveItem_ShouldRemoveCorrectItem()
        {
            // Arrange
            var order = new BookOrder();
            order.AddItem(new OrderItem("111", "Book A", 10.0, 1));
            order.AddItem(new OrderItem("222", "Book B", 15.0, 1));

            // Act
            order.RemoveItem("111");

            // Assert
            Assert.AreEqual(1, order.OrderItemList.Count);
            Assert.AreEqual("222", order.OrderItemList[0].BookID);
        }

        [TestMethod]
        public void RemoveItem_ShouldDoNothingIfItemNotFound()
        {
            // Arrange
            var order = new BookOrder();
            order.AddItem(new OrderItem("111", "Book A", 10.0, 1));

            // Act
            order.RemoveItem("999");

            // Assert
            Assert.AreEqual(1, order.OrderItemList.Count);
            Assert.AreEqual("111", order.OrderItemList[0].BookID);
        }

        [TestMethod]
        public void QuantityChange_ShouldUpdateSubtotal()
        {
            // Arrange
            var item = new OrderItem("123", "Book", 20.0, 1);

            // Act
            item.Quantity = 3;

            // Assert
            Assert.AreEqual(60.0, item.SubTotal);
        }

        [TestMethod]
        public void GetOrderTotal_ShouldReturnSumOfSubtotals()
        {
            // Arrange
            var order = new BookOrder();
            order.AddItem(new OrderItem("111", "Book A", 10.0, 2)); // 20
            order.AddItem(new OrderItem("222", "Book B", 15.0, 1)); // 15

            // Act
            double total = order.GetOrderTotal();

            // Assert
            Assert.AreEqual(35.0, total);
        }
    }
}
