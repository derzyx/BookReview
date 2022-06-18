using BookReview.ClassLibrary;
using BookReview.ClassLibrary.DTO;

namespace BookReview.Models
{
    public class BookReviewModel
    {
        public BookDTO ReviewedBook { get; set; }

        public ReviewDTO Review { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
