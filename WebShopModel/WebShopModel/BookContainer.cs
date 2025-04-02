using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WebShopModel
{
    public class BookContainer
    {
        private readonly List<Book> _books = new();
        public void AddBook(Book book)
        {
            if (_books.Any(b => b.ISBN == book.ISBN))

                throw new ArgumentException("Book with same ISBN already exists");

            _books.Add(book);

        }
        public Book GetBookByISBN(string isbn)
        {
            return _books.FirstOrDefault(b => b.ISBN == isbn);
        }
        public List<Book> getAllBooks()
        {
            return _books.ToList();
        }

        public void DeleteBook(string isbn)
        {
            var book = GetBookByISBN(isbn);

            if (book != null && !book.IsDeleted)
            {
                book.MarkAsDeleted();
            }

        }
        public List<Book> Search(string keyword)
        {
            keyword = keyword.ToLower();

            return _books
                .Where(b =>
                    !b.IsDeleted &&
                    (b.Title.ToLower().Contains(keyword) ||
                     b.Author.ToLower().Contains(keyword)))
                .ToList();
        }

    }
}

    


