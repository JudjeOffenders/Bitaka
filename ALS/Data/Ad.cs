namespace ALS.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ad
    {

        [Key]
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

        public DateTime DateAdded { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public Ad()
        {
            Pictures = new List<Pictures>();
        }

        public virtual ICollection<Pictures> Pictures { get; set; }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }

        public AdStatus Status { get; set; }

    }
}