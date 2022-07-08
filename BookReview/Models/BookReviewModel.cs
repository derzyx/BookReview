using BookReview.ClassLibrary;
using BookReview.ClassLibrary.DTO;

namespace BookReview.Models
{
    public class BookReviewModel
    {
        public Book ReviewedBook { get; set; }
        public Review Review { get; set; }
        public List<Review> Reviews { get; set; }

        // Authors of reviews for given book
        public List<string> ReviewAuthors { get; set; }
    }
}
