using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.ClassLibrary
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string Nick { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength (20, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
