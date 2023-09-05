using ForestAppUI.Models;

namespace ForestAppUI.ViewModels
{
    public class HomeVM
    {
        public List<Article> Articles { get; set; }
        public Article FirstSlot { get; set; }
        public List<Article> AllSlot { get; set; }
        public List<Article> PopularArticle { get; set; }
        public List<KeyValuePair<string, int>> PopularCategories { get; set; }

    }
}
