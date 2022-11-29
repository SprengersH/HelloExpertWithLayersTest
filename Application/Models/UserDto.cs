using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string? LastName { get; set; }
        
        public string Email { get; set; }
    }
}
