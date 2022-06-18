using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.ClassLibrary
{
    public class Review
    {
        public int ReviewId { get; set; } = 0;
        public int BookId_FK { get; set; }
        public int AuthorId_FK { get; set; }
        public string Summary { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}
