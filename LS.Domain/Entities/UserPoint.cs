namespace LS.Domain.Entities
{
    public class UserPoint
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
