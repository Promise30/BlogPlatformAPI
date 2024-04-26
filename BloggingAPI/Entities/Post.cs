namespace BloggingAPI.Entities{
    public class Post
    {
        public int Id   { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime DateModified { get; set; }
        public ICollection<Comment>Comment{get;} = new List<Comment>();
    }
}