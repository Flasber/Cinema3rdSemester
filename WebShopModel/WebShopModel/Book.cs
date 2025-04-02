using System.Dynamic;

namespace WebShopModel
{
    public class Book
    {
        public string ISBN { get; }
        public string Title { get; }
        public string Author { get; }
        public decimal Price { get; }
        public bool IsDeleted { get; private set; }
        private readonly List<StockEntry> _stockEntries = new();

        public Book(string isbn, string title, string author, decimal price)
        {
            if (string.IsNullOrWhiteSpace(isbn) || isbn.Length < 2)
             throw new ArgumentException("Invalid ISBN"); 
            if (string.IsNullOrWhiteSpace(title) || title.Length < 2)
             throw new ArgumentException("Invalid Title"); 
            if (string.IsNullOrWhiteSpace(author) || author.Length < 2)
             throw new ArgumentException("invalid Author"); 
            if (price < 0)
    
                
                   throw new ArgumentException("Stock must be non-negative");
                ISBN = isbn;
            Title = title;
            Author = author;
            Price = price; 
           
            IsDeleted = false;
            }
        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
        public void AddStock(string location, int quantity)
        {
            var existing = _stockEntries.FirstOrDefault(s => s.Location == location);
            if (existing != null)
                existing.Quantity += quantity;
            else
                _stockEntries.Add(new StockEntry(location, quantity));
        }

        public int GetStockAt(string location)
        {
            return _stockEntries.FirstOrDefault(s => s.Location == location)?.Quantity ?? 0;
        }

        public int GetTotalStock()
        {
            return _stockEntries.Sum(s => s.Quantity);
        }
    }
    }




