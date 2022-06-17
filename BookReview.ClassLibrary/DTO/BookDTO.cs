using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.ClassLibrary.DTO
{
    public class BookDTO
    {
        public int? BookId { get; set; } = 0;
        public string? Title { get; set; } = "";
        public string? Author { get; set; } = "";
        public string? PublishDate { get; set; } = "";
        public string? Summary { get; set; } = "";
        public string? IdentyficationCode { get; set; } = "";
        public string? ImgUrl { get; set; } = "";
        public float? AvgScore { get; set; } = 0;
        public int? ReviewsCount { get; set; } = 0;
    }
}
