using BookReview.ClassLibrary;
using BookReview.ClassLibrary.DTO;

namespace BookReview.Models
{
    public class BookReviewModel
    {
        //public BookDTO ReviewedBook { get; set; }
        public Book ReviewedBook { get; set; }

        public Review Review { get; set; }
        public List<Review> Reviews { get; set; }

        public List<string> ReviewAuthors { get; set; }
    }
}
