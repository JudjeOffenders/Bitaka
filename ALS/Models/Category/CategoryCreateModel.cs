namespace ALS.Models.Category
{
    using ALS.Data;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CategoryCreateModel
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(20)]
        public string Name { get; set; }

    }
}