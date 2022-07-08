using BookReview.ClassLibrary.API_request_models;
using BookReview.Controlllers;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace BookReview.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow(null, null, null)]
        [DataRow("Pan Tadeusz", null, null)]
        [DataRow("Pan Tadeusz", "Adam Mickiewicz", null)]
        [DataRow("Pan Tadeusz", "Stanis³aw Wyspiañski", null)]
        [DataRow("Pan Tadeusz", null, "6351564")]
        [DataRow(null, null, "6351564")]
        public async Task TestPostingBookParamsAsync(string? title, string? author, string? isbn)
        {
            var controller = new FindBookController(context: null);

            var result = await controller.ExternalSearch(title, author, isbn) as ViewResult;

            var books = (BookSearchModel)result.ViewData.Model;

            Assert.IsNotNull(books.Books.items);
        }
    }
}