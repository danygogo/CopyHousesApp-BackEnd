using System.ComponentModel.DataAnnotations;

namespace ApiForSale.Models
{
    public class Login
    {
        [EmailAddress]
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}

