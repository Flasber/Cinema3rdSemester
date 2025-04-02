using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopModel;

namespace WebShopTests
{
    public class ShoppingCartTests

    {
        [Test]
        public void AddToCart_Singlebook_AddedCorrectlyWithCorrectTotal()
        {
            var cart = new ShoppingCart();
            var book = new Book("111", "taber for bogen", "forfatters", 150, 5);
            cart.AddToCart(book, 1);
            var items = cart.GetItems();
            Assert.That(items[0].Book.ISBN, Is.EqualTo("111"));
            Assert.That(items[0].Quantity, Is.EqualTo(1));
            Assert.That(items[0].SubTotal, Is.EqualTo(150));
            Assert.That(cart.GetTotal(), Is.EqualTo(150));
        }
        [Test]
        public void AddToCart_TwoDifferentBooks_TotalSumOfBoth()
        {
            var cart = new ShoppingCart();

            var book1 = new Book("111", "taber for bogen", "forfatters", 150);
            var book2 = new Book("222", "vindere for bogen", "forfatteren", 250);

            cart.AddToCart(book1, 1);
            cart.AddToCart(book2, 1);
            var items = cart.GetItems();

            Assert.That(items.Count, Is.EqualTo(2));
            Assert.That(cart.GetTotal(), Is.EqualTo(400));
        }
        [Test]
        public void changeQuantity_UpdateSubTotalAdnTotalCorrectly()
        {
            var cart = new ShoppingCart();
            var book = new Book("111", "taber for bogen", "forfatters", 150);
            cart.AddToCart(book, 1);
            cart.ChangeQuantity("111", 3);
            var items = cart.GetItems();
            Assert.That(items[0].Quantity, Is.EqualTo(3));
            Assert.That(items[0].SubTotal, Is.EqualTo(450));
            Assert.That(cart.GetTotal(), Is.EqualTo(450));
        }
        [Test]
        public void ChangeQuantity_SetToZero_RemovesItemFromCart()
        {
            var cart = new ShoppingCart();
            var book = new Book("111", "Taber for bogen", "Forfatters", 150);

            cart.AddToCart(book, 2);
            cart.ChangeQuantity("111", 0);

            var items = cart.GetItems();

            Assert.That(items.Count, Is.EqualTo(0));
            Assert.That(cart.GetTotal(), Is.EqualTo(0));
        }

    }
}
