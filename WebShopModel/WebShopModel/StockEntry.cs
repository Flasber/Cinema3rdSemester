using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopModel
{
    public class StockEntry
    {
        public string Location { get; set; }
        public int Quantity { get; set; }
    
    public StockEntry(string location, int quantity)
        {
            Location = location;
            Quantity = quantity;
        }
    } 
}
