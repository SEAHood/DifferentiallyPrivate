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
    /// <summary>
    /// User - Represents a user of the system
    /// Encodes password and checks against database for login
    /// </summary>
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
            //Check if user is valid through DBInterface
            EncodePassword();
            var DBI = new DBInterface();
            return DBI.ValidateUser(this);
        }

        
        private void EncodePassword()
        {            
            //Basic SHA1 encryption
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