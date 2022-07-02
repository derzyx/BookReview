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
        public async Task<IActionResult> AddReview(int? id, IFormCollection? formCollection)
        {
            BookDTO? bookDTO = new BookDTO();

            if(id == null || id == 0)
            {
                bookDTO = new BookDTO
                {
                    BookId = Convert.ToInt32(formCollection["BookId"]),
                    Title = formCollection["Title"],
                    Author = formCollection["Author"],
                    Summary = formCollection["Summary"],
                    IdentyficationCode = formCollection["IdentyficationCode"],
                    ImgUrl = formCollection["ImgUrl"],
                    PublishDate = formCollection["PublishDate"]
                };
            }
            else
            {

            }


            return AddReview(new BookReviewModel { ReviewedBook = bookDTO });
        }


        public IActionResult AddReview(BookReviewModel? bookReviewModel)
        {
            Book? currentBook = new Book();

            currentBook = _context.Book.Where(x =>
                x.Author == bookReviewModel.ReviewedBook.Author &&
                x.Title == bookReviewModel.ReviewedBook.Title &&
                x.IdentyficationCode == bookReviewModel.ReviewedBook.IdentyficationCode &&
                x.PublishDate == bookReviewModel.ReviewedBook.PublishDate)
            .FirstOrDefault();

            if(currentBook == null)
            {
                return View(bookReviewModel);
            }
            else
            {
                return View(new BookReviewModel
                {
                    ReviewedBook = new BookDTO
                    {
                        Title = currentBook.Title,
                        Author = currentBook.Author,
                        PublishDate = currentBook.PublishDate,
                        Summary = currentBook.Summary,
                        IdentyficationCode= currentBook.IdentyficationCode,
                        ImgUrl = currentBook.ImgUrl,
                        AvgScore = currentBook.AvgScore,
                        ReviewsCount = currentBook.ReviewsCount,
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
