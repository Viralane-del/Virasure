

namespace VirasureYouAI.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string CommentDetail { get; set; }
        public DateTime CommentDate { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string? Status { get; set; }
    }
}
