using BookReview.ClassLibrary;

namespace BookReview.Models
{
    public class MyAccountModel
    {
        public string CurrentUser { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Book> ReviewedBooks { get; set; }

    }
}
