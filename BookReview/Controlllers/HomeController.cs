using BookReview.ClassLibrary;
using BookReview.Data;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookReview.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            // Get 3 top rated books
            List<Book> topRatedBooks = _context.Book
                .FromSqlRaw("SELECT TOP 3 * FROM dbo.Book ORDER BY AvgScore DESC, ReviewsCount DESC")
                .ToList();

            return View(new BookSearchModel { InternalBooks = topRatedBooks});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}