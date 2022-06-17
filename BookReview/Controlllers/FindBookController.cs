using BookReview.ClassLibrary;
using BookReview.ClassLibrary.API_request_models;
using BookReview.ClassLibrary.DTO;
using BookReview.Data;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.Controlllers
{
    public class FindBookController : Controller
    {

        private const string baseUri = "https://www.googleapis.com/books/v1/volumes";
        private string parameters = "?q=intitle:tatry&langRestrict=pl";
        private const string apiKey = "key=AIzaSyAS7p0rjmwhrfXYF0Ps5Vc4jvI_r9mnO5g";
        private readonly DataContext _context;

        public FindBookController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> InternalSearch(string? title, string? author, string? isbn)
        {
            if (title != null || author != null || isbn != null)
            {
                List<Book> books = _context.Book
                    .Where(x =>
                        x.Title.ToLower().Contains((string.IsNullOrEmpty(title)) ? "" : title.ToLower()) &&
                        x.Author.ToLower().Contains((string.IsNullOrEmpty(author)) ? "" : author.ToLower()) &&
                        x.IdentyficationCode.Contains(isbn ?? ""))
                    .ToList();

                return View(new BookSearchModel { InternalBooks = books });
            }

            return View(new BookSearchModel { InternalBooks = null });
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InternalSearchPost([Bind("Book")] BookSearchModel? bookParams)
        {
            return new RedirectToActionResult("InternalSearch", "FindBook",
                new
                {
                    title = bookParams.Book.Title,
                    author = bookParams.Book.Author,
                    isbn = bookParams.Book.IdentyficationCode
                });

        }

        public async Task<IActionResult> ExternalSearch(string? title, string? author, string? isbn)
        {
            if(title != null || author != null || isbn != null)
            {
                string titleParam = (string.IsNullOrEmpty(title)) ? "" : $"intitle:{title}&";
                string authorParam = (string.IsNullOrEmpty(author)) ? "" : $"inauthor:{author}&";
                string isbnParam = (string.IsNullOrEmpty(isbn)) ? "" : $"isbn:{isbn}&";
                parameters = $"?q={titleParam}{authorParam}{isbnParam}langRestrict=pl";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUri);
                BookItems books = new BookItems();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = await client.GetAsync(baseUri + parameters + apiKey))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        books = await response.Content.ReadAsAsync<BookItems>();
                    }
                }
                //return new RedirectToActionResult("Search", "Book", books);
                return View(new BookSearchModel { Books = books });
            }
            return View(new BookSearchModel { Books = null});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalSearchPost([Bind("BookSearchParams")] BookSearchModel? bookParams)
        {

            return new RedirectToActionResult("ExternalSearch", "FindBook", 
                new { 
                    title = bookParams.BookSearchParams.Title, 
                    author = bookParams.BookSearchParams.Author,
                    isbn = bookParams.BookSearchParams.IdentyficationCode
                });
        }
    }
}
