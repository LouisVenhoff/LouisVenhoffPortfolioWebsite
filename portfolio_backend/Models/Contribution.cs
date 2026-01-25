namespace portfolio_backend.Models
{
    public class Contribution
    {

        [Key]
        public DateTime time { get; set; }

        [Required]
        public int Count { get; set; }
        

    }
}