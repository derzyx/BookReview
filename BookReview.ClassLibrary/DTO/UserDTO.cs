using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.ClassLibrary.DTO
{
    public class UserDTO
    {
        public int? UserId { get; set; } = 0;

        public string? Nick { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
