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
        public async Task<IActionResult> AddReview([Bind("ReviewedBook, Review")] BookReviewModel bookParams)
        {
            Book? currentBook = new Book();
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
                return new RedirectToActionResult("AddReview", "Book", new { bookId = currentBook.BookId });
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

            return new RedirectToActionResult("Reviews", "Book", new {bookId = currentBook.BookId});
        }


        public async Task<IActionResult> AddReview(int? id, Book? book)
        {
            Book? currentBook = new Book();

            if(id != null)
            {
                currentBook = await _context.Book.FindAsync(id);
            }
            else
            {
                currentBook = _context.Book.Where(x =>
                    x.Author == book.Author &&
                    x.Title == book.Title &&
                    x.IdentyficationCode == book.IdentyficationCode &&
                    x.PublishDate == book.PublishDate)
                .FirstOrDefault();
            }


            if (currentBook == null)
            {
                //return View(bookReviewModel);
                return View(new BookReviewModel { ReviewedBook = book });
            }
            else
            {
                return View(new BookReviewModel { ReviewedBook = currentBook });
            }
        }


        public async Task<IActionResult> Reviews(int? bookId)
        {
            Book? book = new Book();
            List<Review>? reviews = new List<Review>();
            List<string>? reviewAuthors = new List<string>();

            if (bookId != null)
            {
                book = await _context.Book.FindAsync(bookId);
                reviews = await _context.Review
                    .Where(x => x.BookId_FK == bookId)
                    .ToListAsync();
                foreach(Review review in reviews)
                {
                    reviewAuthors.Add(
                        _context.User
                            .Where(x => x.UserId == review.AuthorId_FK)
                            .Select(x => x.Nick)
                            .FirstOrDefault()
                    );
                }
                return View(new BookReviewModel { ReviewedBook = book, Reviews = reviews, ReviewAuthors = reviewAuthors});
            }
            return View();
        }

        public async Task<IActionResult> UpdateReview(int reviewId)
        {
            if(reviewId != 0)
            {
                Review review = await _context.Review.FindAsync(reviewId);
                Book book = await _context.Book.FindAsync(review.BookId_FK);

                return View(new BookReviewModel { Review = review, ReviewedBook = book });
            }
            else
            {
                return new RedirectToActionResult("Index", "Home", null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReview([Bind("Review")] BookReviewModel model)
        {
            List<Review> reviews = new List<Review>();
            Review review = await _context.Review.FindAsync(model.Review.ReviewId);
            Book book = await _context.Book.FindAsync(review.BookId_FK);

            review.Summary = model.Review.Summary;
            review.Score = (int)model.Review.Score;
            review.Date = DateTime.UtcNow.AddHours(-2);

            await _context.SaveChangesAsync();

            reviews = await _context.Review
                .Where(x => x.BookId_FK == book.BookId)
                .ToListAsync();

            book.AvgScore = (float)reviews.Average(x => x.Score);

            await _context.SaveChangesAsync();

            return new RedirectToActionResult("MyAccount", "Account", new { userId = review.AuthorId_FK });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveReview(int reviewId)
        {
            List<Review> reviews = new List<Review>();

            Review review = await _context.Review.FindAsync(reviewId);
            Book book = await _context.Book.FindAsync(review.BookId_FK);
            int authorId = review.AuthorId_FK;

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            reviews = await _context.Review
                .Where(x => x.BookId_FK == book.BookId)
                .ToListAsync();

            if (reviews.Count > 0)
            {
                book.ReviewsCount = reviews.Count();
                book.AvgScore = (float)reviews.Average(x => x.Score);
            }
            else
            {
                book.ReviewsCount = 0;
                book.AvgScore = 0;
            }

            await _context.SaveChangesAsync();

            return new RedirectToActionResult("MyAccount", "Account", new { userId = authorId});
        }
    }
}
