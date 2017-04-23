namespace ALS.Models.Ad
{
    using Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class AdEditModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public Cities City { get; set; }

        [Required]
        public string Content { get; set; }

        public decimal Price { get; set; }

        //check for proper declaration
        public List<Pictures> Pictures { get; set; }
    }
}