using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.ClassLibrary.DTO
{
    public class ReviewDTO
    {
        public int? ReviewId { get; set; }
        public int? BookId { get; set; }
        public int? AuthorId { get; set; }
        public string? Summary { get; set; }
        public int? Score { get; set; }
        public DateTime? Date { get; set; }
    }
}
