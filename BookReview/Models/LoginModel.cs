using BookReview.ClassLibrary.DTO;
using System.ComponentModel.DataAnnotations;

namespace BookReview.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
