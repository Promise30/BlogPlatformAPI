public class PostDto{
    public int Id {get;set;}
    public string Title {get;set;}
    public string Content {get;set;}
    public ICollection<CommentDto> Comment {get;} = new List<CommentDto>();
}