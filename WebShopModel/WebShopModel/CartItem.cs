using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopModel
{
    public class CartItem
    {
        public Book Book { get; }
        public int Quantity { get; set; }
        public CartItem(Book book, int quantity)
        {
            Book = book;
            Quantity = quantity;

        }
        public decimal SubTotal => Book.Price * Quantity;
    }
}
