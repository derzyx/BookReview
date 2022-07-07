using BookReview.ClassLibrary;
using BookReview.Data;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;
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
            List<Book> topRatedBooks = _context.Book
                .OrderByDescending(x => x.AvgScore)
                .Take(3)
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