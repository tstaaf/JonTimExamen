using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JonTimExamen.RequestObjects
{
    public class RegisterRequestObject
    {
        [Required, MinLength(3), MaxLength(15)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
