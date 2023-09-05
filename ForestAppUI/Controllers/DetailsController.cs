using ForestAppUI.Data;
using ForestAppUI.Helper;
using ForestAppUI.Models;
using ForestAppUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ForestAppUI.Controllers
{
    public class DetailsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DetailsController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Detail(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            }
            if (id == null) return NotFound();

            var article = _context.Articles.Include(x => x.Category).Include(x => x.User).Include(x => x.ArticleTag).ThenInclude(x => x.Tag).SingleOrDefault(x => x.Id == id);



            var cookie = _httpContextAccessor.HttpContext.Request.Cookies[$"Views"];
            string[] findCookie = { "" };

            if (cookie != null)
            {
                findCookie = cookie.Split('-').ToArray();
            }

            if (!findCookie.Contains(article.Id.ToString()))
            {
                Response.Cookies.Append($"Views", $"{cookie}-{article.Id}",
                    new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                        Expires = DateTime.Now.AddYears(1),
                    });

                article.ViewCount += 1;
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
            }




            if (article == null) return NotFound();
            var popArt = _context.Articles.OrderByDescending(X => X.ViewCount).Take(3).ToList();
            var comments = _context.ArticleComments.Include(x => x.User).Where(x => x.ArticleId == id).ToList();
            var nextArticle = _context.Articles.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Id > article.Id);
            var prevArticle = _context.Articles.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Id < article.Id);
            var similarArticle = _context.Articles.Include(x => x.Category).OrderByDescending(x => x.Id).Include(x => x.Category).Where(x => x.CategoryId == article.CategoryId && x.Id != article.Id).Take(2).ToList();
            var popularCategories = _context.Articles.GroupBy(article => article.Category.CategoryName).AsEnumerable().Select(group => new KeyValuePair<string, int>(group.Key, group.Count())).OrderByDescending(pair => pair.Value).Take(3).ToList();

            foreach (var comment in comments)
            {
                ViewData[$"PublishDateAgo_{comment.Id}"] = CommentTime.GetTimeAgo(comment.PublishDate);
            }

            DetailVM detailVM = new()
            {
                Article = article,
                PopulatArticle = popArt,
                articleComments = comments,
                NextArticle = nextArticle,
                PrevArticle = prevArticle,
                SimilarArticle = similarArticle,
                PopularCategories = popularCategories,
            };

            return View(detailVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(ArticleComment articleComment, int articleId)
        {
            articleComment.PublishDate = DateTime.Now;


            articleComment.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _context.ArticleComments.AddAsync(articleComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), "Details", new { id = articleId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId, int articleId)
        {
            var artCom = _context.ArticleComments.FirstOrDefault(x => x.Id == commentId);
            _context.ArticleComments.Remove(artCom);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Detail), "Details", new { id = articleId });

        }
    }
}
