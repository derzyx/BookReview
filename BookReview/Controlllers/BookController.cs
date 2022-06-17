using BookReview.ClassLibrary;
using BookReview.ClassLibrary.DTO;
using BookReview.Data;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookReview.Controlllers
{
    public class BookController : Controller
    {

        private readonly DataContext _context;

        public BookController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Add([Bind("BookSearchParams")] BookSearchModel? book)
        public async Task<IActionResult> Details(IFormCollection formCollection)
        {
            foreach(string key in formCollection.Keys)
            {
                Console.WriteLine("Key=" + key + " ");
                Console.WriteLine("Value=" + formCollection[key]);
            }
            return new RedirectToActionResult("Add", "Reviews", new BookDTO { 
                Title = formCollection["Title"],
                Author = formCollection["Author"],
                Summary = formCollection["Summary"],
                IdentyficationCode = formCollection["IdentyficationCode"],
                ImgUrl = formCollection["ImgUrl"],
                PublishDate = formCollection["PublishDate"]
            });
        }


        public IActionResult Details(BookDTO? book)
        {
            //Internal search returns Book with set ID (not 0). Books from external search don't exist in internal DB so they don't have an ID.
            if(book.BookId == 0)
            {
                return View(new BookReviewModel { ReviewedBook = book});
            }
            else
            {
                Book existingBook = _context.Book.Find(book.BookId);
                return View(new BookReviewModel { ReviewedBook = new BookDTO { 
                    BookId = book.BookId,
                    Title = book.Title,
                    Author = book.Author,
                    Summary = book.Summary,
                    IdentyficationCode = book.IdentyficationCode,
                    ImgUrl = book.ImgUrl,
                    PublishDate = book.PublishDate,
                    AvgScore = book.AvgScore,
                    ReviewsCount = book.ReviewsCount
                } });
            }
        }
    }
}
