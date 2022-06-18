using BookReview.ClassLibrary;
using BookReview.ClassLibrary.DTO;
using BookReview.Data;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> AddReview(IFormCollection formCollection)
        {
            foreach (string key in formCollection.Keys)
            {
                Console.WriteLine("Key=" + key + " ");
                Console.WriteLine("Value=" + formCollection[key]);
            }
            BookDTO bookDTO = new BookDTO
            {
                BookId = Convert.ToInt32(formCollection["BookId"]),
                Title = formCollection["Title"],
                Author = formCollection["Author"],
                Summary = formCollection["Summary"],
                IdentyficationCode = formCollection["IdentyficationCode"],
                ImgUrl = formCollection["ImgUrl"],
                PublishDate = formCollection["PublishDate"]
            };

            return View(bookDTO);
        }


        public IActionResult AddReview(int? bookId, BookDTO? book)
        {
            //Internal search returns Book with set ID (not 0). Books from external search don't exist in internal DB so they don't have an ID.
            if (book.BookId == 0)
            {
                return View(new BookReviewModel { ReviewedBook = book });
            }
            else
            {
                Book existingBook = _context.Book.Find(book.BookId);
                return View(new BookReviewModel
                {
                    ReviewedBook = new BookDTO
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        Author = book.Author,
                        Summary = book.Summary,
                        IdentyficationCode = book.IdentyficationCode,
                        ImgUrl = book.ImgUrl,
                        PublishDate = book.PublishDate,
                        AvgScore = book.AvgScore,
                        ReviewsCount = book.ReviewsCount
                    }
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reviews(int? bookId, [Bind("ReviewedBook, Review")] BookReviewModel? bookParams)
        {
            Book? currentBook = new Book();
            BookDTO currentBookDTO = new BookDTO();
            List<Review> reviews = new List<Review>();

            currentBook = _context.Book.Where(x =>
                x.Author == bookParams.ReviewedBook.Author &&
                x.Title == bookParams.ReviewedBook.Title &&
                x.IdentyficationCode == bookParams.ReviewedBook.IdentyficationCode &&
                x.PublishDate == bookParams.ReviewedBook.PublishDate)
            .FirstOrDefault();

            if (currentBook == null)
            {
                _context.Book.Add(new Book
                {
                    Title = bookParams.ReviewedBook.Title,
                    Author = bookParams.ReviewedBook.Author,
                    PublishDate = bookParams.ReviewedBook.PublishDate,
                    Summary = bookParams.ReviewedBook.Summary,
                    IdentyficationCode = bookParams.ReviewedBook.IdentyficationCode,
                    ImgUrl = bookParams.ReviewedBook.ImgUrl
                });
                await _context.SaveChangesAsync();

                currentBook = _context.Book.Where(x =>
                    x.Author == bookParams.ReviewedBook.Author &&
                    x.Title == bookParams.ReviewedBook.Title &&
                    x.IdentyficationCode == bookParams.ReviewedBook.IdentyficationCode &&
                    x.PublishDate == bookParams.ReviewedBook.PublishDate)
                .FirstOrDefault();
            }
            else
            {
                return new RedirectToActionResult("AddReview", "Book", new { bookId = currentBook.BookId});
            }


            if (bookParams.Review != null)
            {
                _context.Review.Add(new Review
                {
                    AuthorId_FK = 1,
                    BookId_FK = currentBook.BookId,
                    Summary = bookParams.Review.Summary,
                    Score = (int)bookParams.Review.Score,
                    Date = DateTime.UtcNow.AddHours(-2)
                });
                await _context.SaveChangesAsync();
            }

            reviews = await _context.Review
                .Where(x => x.BookId_FK == currentBook.BookId)
                .ToListAsync();

            currentBook.ReviewsCount = reviews.Count();
            currentBook.AvgScore = (float)reviews.Average(x => x.Score);
            await _context.SaveChangesAsync();

            currentBookDTO = new BookDTO
            {
                BookId = currentBook.BookId,
                Title = currentBook.Title,
                Author = currentBook.Author,
                Summary = currentBook.Summary,
                PublishDate = currentBook.PublishDate,
                IdentyficationCode = currentBook.IdentyficationCode,
                ImgUrl = currentBook.ImgUrl,
                AvgScore = currentBook.AvgScore,
                ReviewsCount = currentBook.ReviewsCount
            };

            return View(new BookReviewModel { ReviewedBook = currentBookDTO, Reviews = reviews });
        }

        public IActionResult Reviews(BookReviewModel? newBookAndReview)
        {
            return View();
        }
    }
}
