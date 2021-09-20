using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Model.DTO
{
    public class UserDTO : LoginDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
       
    }
}