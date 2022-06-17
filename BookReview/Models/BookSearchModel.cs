using BookReview.ClassLibrary;
using BookReview.ClassLibrary.API_request_models;
using BookReview.ClassLibrary.DTO;

namespace BookReview.Models
{
    public class BookSearchModel
    {
        public List<Book>? InternalBooks { get; set; }
        public BookItems? Books { get; set; }
        public BookDTO? BookSearchParams { get; set; }
        public Book? Book { get; set; }
    }
}
