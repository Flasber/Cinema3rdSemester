using System.Collections.Generic;
using System.Linq;

namespace WebShopModel
{
    public class ShoppingCart
    {
        private readonly List<CartItem> _items = new();

        public void AddToCart(Book book, int quantity)
        {
            var existing = _items.FirstOrDefault(i => i.Book.ISBN == book.ISBN);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem(book, quantity));
            }
        }

        public List<CartItem> GetItems()
        {
            return _items.ToList();
        }

        public decimal GetTotal()
        {
            return _items.Sum(i => i.SubTotal);
        }
        public void ChangeQuantity(string isbn, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.Book.ISBN == isbn);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    _items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

            }
        }
    }
}

