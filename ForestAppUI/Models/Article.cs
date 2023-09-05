namespace ForestAppUI.Models
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PhotoUrl { get; set; }
        public string SeoUrl { get; set; }
        public int ViewCount { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ArticleTag> ArticleTag { get; set; }
    }
}
