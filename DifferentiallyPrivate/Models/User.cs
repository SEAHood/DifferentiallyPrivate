using DifferentiallyPrivate.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DifferentiallyPrivate.Models
{
    public class User
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool IsValid()
        {
            EncodePassword();
            var DBI = new DBInterface();
            return DBI.ValidateUser(this);
        }

        
        private void EncodePassword()
        {            
            byte[] bytes = Encoding.UTF8.GetBytes(Password);
 
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var c = b.ToString();
                sb.Append(c);
            }

            Password = sb.ToString();
        }
    }
}