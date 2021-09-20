using System.ComponentModel.DataAnnotations;

namespace ContactManager.Model.DTO
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}