namespace ALS.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Pictures
    {
        [Key]
        public int Id { get; set; }

        public string FilePath { get; set; }

        public virtual Ad Ad { get; set; }
    }
}