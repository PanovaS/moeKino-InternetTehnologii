using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace moeKino.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        [Display(Name="Name and Surname")]
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual List<Film> movies { get; set; }
        public Client() {
            movies = new List<Film>();
        }
    }
}