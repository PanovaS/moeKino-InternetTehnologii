using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace moeKino.Models
{
    public class Film
    {
        public int Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Display(Name="Image")]
        public string Url { set; get; }
        public string Genre { set; get; }
        public string Director { set; get; }
        [Display(Name="Release Date")]
        public string ReleaseDate { set; get; }
        [Display(Name = "Short Description")]
        public string ShortDescription { set; get; }
        public string Stars { set; get; }
        public int Rating { set; get; }
        [Display(Name = "Visitors")]
        public int Audience { set; get; }
        public virtual List<Client> clients { get; set; }

        public Film() {
            clients = new List<Client>();
        }

    }
}