using BookReview.ClassLibrary;

namespace BookReview.Models
{
    public class MyAccountModel
    {
        public List<Review> Reviews { get; set; }
        public List<Book> ReviewedBooks { get; set; }

    }
}
