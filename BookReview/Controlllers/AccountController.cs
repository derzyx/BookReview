using BookReview.ClassLibrary;
using BookReview.ClassLibrary.API_request_models;
using BookReview.Data;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace BookReview.Controlllers
{
    public class AccountController : Controller
    {
        private const string baseUri = "https://www.googleapis.com/books/v1/volumes?q=intitle:tatry&langRestrict=pl&key=AIzaSyAS7p0rjmwhrfXYF0Ps5Vc4jvI_r9mnO5g";
        private const string parameters = "?q=intitle:tatry&langRestrict=pl&key=AIzaSyAS7p0rjmwhrfXYF0Ps5Vc4jvI_r9mnO5g";
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        //GET login page
        public async Task<IActionResult> Login(string? ErrorMessage)
        {
            ViewBag.ErrorMessage = (string.IsNullOrEmpty(ErrorMessage)) ? "" : ErrorMessage;
            return View();
        }

        //POST user to log in
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email, Password")] LoginModel userToLogin)
        {
            int count = await _context.User
                .Where(x => x.Password == userToLogin.Password && x.Email == userToLogin.Email)
                .CountAsync();

            if(count == 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Login), new {ErrorMessage = "Nieprawidłowe dane logowania"});
            }
        }

        //GET register page
        public IActionResult Register(string? ErrorMessage)
        {
            ViewBag.ErrorMessage = (string.IsNullOrEmpty(ErrorMessage)) ? "" : ErrorMessage;
            return View();
        }

        //POST user to register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Nick, Email, Password")] User user)
        {
            if (ModelState.IsValid)
            {
                int nicks = await _context.User
                    .Where(x => x.Nick == user.Nick)
                    .CountAsync();

                if(nicks == 0)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return RedirectToAction("Register", new { ErrorMessage = "Ktoś już używa tego nicku"});
                }
            }
            return View();
        }

        public IActionResult MyAccount(int userId)
        {
            if(userId == 0)
            {
                return new RedirectToActionResult("Index", "Home", null);
            }
            else
            {
                List<Book> reviewedBooks = new List<Book>();
                List<Review> reviews = _context.Review.Where(x => x.AuthorId_FK == userId).ToList();
                foreach(Review review in reviews)
                {
                    reviewedBooks.Add(_context.Book.Where(x => x.BookId == review.BookId_FK).First());
                }

                return View(new MyAccountModel { Reviews = reviews, ReviewedBooks = reviewedBooks });
            }
        }
    }
}
