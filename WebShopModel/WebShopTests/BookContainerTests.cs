using NUnit.Framework;
using WebShopModel;

namespace WebShopTests
{
    public class BookContainerTests
    {
        [Test]
        public void AddBook_InvalidPrice_ThrowsArgumentException()
        {
            var container = new BookContainer();

            Assert.Throws<ArgumentException>(() =>
            {
                var invalidBook = new Book("222", "Bog", "Forfatter", -10);
                container.AddBook(invalidBook);
            });
        }

        [Test]
        public void DeleteBook_MarksBookAsDeleted()
        {
            var container = new BookContainer();
            var book = new Book("123", "Bog", "Forfatters", 100);
            container.AddBook(book);

            container.DeleteBook("123");
            var result = container.GetBookByISBN("123");

            Assert.IsTrue(result.IsDeleted);
        }

        [Test]
        public void DeleteBook_AlreadyDeleted_DoesNothing()
        {
            var container = new BookContainer();
            var book = new Book("999", "bog", "forfatter", 100);
            container.AddBook(book);

            container.DeleteBook("999");
            container.DeleteBook("999");
            var result = container.GetBookByISBN("999");
            Assert.IsTrue(result.IsDeleted);
        }

        [Test]
        public void DeleteBook_NonExistingBook_DoesNotThrow()
        {
            var container = new BookContainer();
            Assert.DoesNotThrow(() =>
            {
                container.DeleteBook("404");
            });
        }

        [Test]
        public void Search_KeywordInTitle_ReturnMatchingBook()
        {
            var container = new BookContainer();
            var book1 = new Book("111", "taber for bogen", "forfatters", 150);
            var book2 = new Book("222", "vindere for bogen", "forfatteren", 250);
            container.AddBook(book1);
            container.AddBook(book2);

            var result = container.Search("taber");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].ISBN, Is.EqualTo("111"));
        }

        [Test]
        public void Search_KeyWordInAuthor_ReturnMatchingBook()
        {
            var container = new BookContainer();
            var book1 = new Book("111", "taber for bogen", "forfatters", 150);
            var book2 = new Book("222", "vindere for bogen", "forfatteren", 250);
            container.AddBook(book1);
            container.AddBook(book2);

            var result = container.Search("forfatters");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].ISBN, Is.EqualTo("111"));
        }

        [Test]
        public void Search_KeywordInTItleAndAuthor_ReturnBothBooks()
        {
            var container = new BookContainer();
            var book1 = new Book("111", "taber for bogen", "forfatters", 150);
            book1.AddStock("Standard", 5);
            var book2 = new Book("222", "vindere for bogen", "forfatteren taber", 250);
            container.AddBook(book1);
            container.AddBook(book2);

            var result = container.Search("taber");

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(b => b.ISBN == "111"), Is.True);
            Assert.That(result.Any(b => b.ISBN == "222"), Is.True);
        }

        [Test]
        public void Search_KeywordNotInanyBook_ReturnEmptyList()
        {
            var container = new BookContainer();
            var book1 = new Book("111", "taber for bogen", "forfatters", 150);
            var book2 = new Book("222", "vindere for bogen", "forfatteren taber", 250);
            container.AddBook(book1);
            container.AddBook(book2);

            var result = container.Search("aab");

            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        public void AddStock_OneLocation_ReturnsCorrectStock()
        {
            var book = new Book("111", "taber for bogen", "forfatters", 150);
            book.AddStock("Aalborg", 5);

            Assert.That(book.GetStockAt("Aalborg"), Is.EqualTo(5));
            Assert.That(book.GetTotalStock(), Is.EqualTo(5));
        }
        [Test]
        public void AddStock_TwoLocations_TotalStockIsSummed()
        {
            var book = new Book("111", "taber for bogen", "forfatters", 150);
            book.AddStock("Aalborg", 3);
            book.AddStock("Stockholm", 7);

            Assert.That(book.GetStockAt("Aalborg"), Is.EqualTo(3));
            Assert.That(book.GetStockAt("Stockholm"), Is.EqualTo(7));
            Assert.That(book.GetTotalStock(), Is.EqualTo(10));
        }
        [Test]
        public void AddStock_SameLocationTwice_AddsUpCorrectly()
        {
            var book = new Book("111", "taber for bogen", "forfatters", 150);
            book.AddStock("Aalborg", 2);
            book.AddStock("Aalborg", 4);

            Assert.That(book.GetStockAt("Aalborg"), Is.EqualTo(6));
            Assert.That(book.GetTotalStock(), Is.EqualTo(6));
        }

    }

}
