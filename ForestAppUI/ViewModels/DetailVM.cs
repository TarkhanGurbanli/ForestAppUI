using ForestAppUI.Models;

namespace ForestAppUI.ViewModels
{
    public class DetailVM
    {
        public Article Article { get; set; }
        public Article NextArticle { get; set; }
        public Article PrevArticle { get; set; }
        public List<Article> SimilarArticle { get; set; }
        public List<Article> PopularArticle { get; set; }
        public List<KeyValuePair<string, int>> PopularCategories { get; set; }
        public List<ArticleComment> articleComments { get; set; }
        public List<Article> PopulatArticle { get; internal set; }
    }
}
