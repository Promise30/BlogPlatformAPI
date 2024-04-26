namespace BloggingAPI.Entities{
    public class Comment
    {
        public int Id { get; set;}
        public string Content { get; set;}
        public DateTime CreatedOn { get; set;}
        public DateTime DateModified { get; set;}
        public int PostId { get; set;}
        public Post Post { get; set;}
    }
}