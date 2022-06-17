using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.ClassLibrary.API_request_models
{
    public class BookItems
    {
        public List<BookJSON> items { get; set; }
    }

    public class BookJSON
    {
        public VolumeInfo volumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string[] authors { get; set; }
        public ImageLinks? imageLinks { get; set; }
        public List<ISBN> industryIdentifiers { get; set; }
        public string? publishedDate { get; set; }
    }

    public class ImageLinks
    {
        public string? thumbnail { get; set; }
    }

    public class ISBN
    {
        public string? type { get; set; }
        public string? identifier { get; set; }
    }
}
