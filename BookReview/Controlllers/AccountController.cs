using BookReview.ClassLibrary;
using BookReview.ClassLibrary.API_request_models;
using BookReview.ClassLibrary.KeyStorage;
using BookReview.Data;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace BookReview.Controlllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        //GET login page
        public IActionResult Login(string? ErrorMessage)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeys.CurrentUser)))
            {
                return new RedirectToActionResult("Index", "Home", null);
            }

            ViewBag.ErrorMessage = (string.IsNullOrEmpty(ErrorMessage)) ? "" : ErrorMessage;
            return View();
        }

        //POST user to log in
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email, Password")] LoginModel userToLogin)
        {
            User? user = _context.User
                .Where(x => x.Password == userToLogin.Password && x.Email == userToLogin.Email)
                .FirstOrDefault();

            if (user != null)
            {
                HttpContext.Session.SetString(SessionKeys.CurrentUser, user.UserId.ToString());
                return new RedirectToActionResult("Index", "Home", null);
            }
            else
            {
                return RedirectToAction(nameof(Login), new { ErrorMessage = "Nieprawidłowe dane logowania" });
            }
        }

        //GET register page
        public IActionResult Register(string? ErrorMessage, string? SuccessMessage)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeys.CurrentUser)))
            {
                return new RedirectToActionResult("Index", "Home", null);
            }

            ViewBag.ErrorMessage = ErrorMessage ?? "";
            ViewBag.SuccessMessage = SuccessMessage ?? "";
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
                    return RedirectToAction("Register", new { SuccessMessage = "Pomyślnie dodano konto." });
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeys.CurrentUser)))
            {
                return new RedirectToActionResult("Index", "Home", null);
            }

            if(userId == 0)
            {
                return new RedirectToActionResult("Index", "Home", null);
            }
            else
            {
                string nick = _context.User.FindAsync(userId).Result.Nick;
                List<Book> reviewedBooks = new List<Book>();
                List<Review> reviews = _context.Review.Where(x => x.AuthorId_FK == userId).ToList();
                foreach(Review review in reviews)
                {
                    reviewedBooks.Add(_context.Book.Where(x => x.BookId == review.BookId_FK).First());
                }

                return View(new MyAccountModel {CurrentUser = nick, Reviews = reviews, ReviewedBooks = reviewedBooks });
            }
        }

        public IActionResult Logout()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeys.CurrentUser)))
            {
                return new RedirectToActionResult("Index", "Home", null);
            }
            else
            {
                HttpContext.Session.Remove(SessionKeys.CurrentUser);
                return new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
