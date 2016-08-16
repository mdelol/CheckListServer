namespace DAL.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        public bool IsCompleted { get; set; }

        public string Description { get; set; }
    }
}