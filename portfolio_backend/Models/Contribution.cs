namespace portfolio_backend.Models
{
    public class Contribution
    {

        public Contribution(DateTime time, int Count)
        {
            this.time = time;
            this.Count = Count;
        }

        [Key]
        public DateTime time { get; set; }

        [Required]
        public int Count { get; set; }
        

    }
}