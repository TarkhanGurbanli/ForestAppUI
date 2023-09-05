using ForestAppUI.Data;
using ForestAppUI.Helper;
using ForestAppUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ForestAppUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _enw;

        public ArticleController(AppDbContext contex, IHttpContextAccessor contextAccessor, IWebHostEnvironment enw)
        {
            _context = contex;
            _contextAccessor = contextAccessor;
            _enw = enw;
        }

        public IActionResult Index()
        {
            var articles = _context.Articles.Include(x => x.User).Include(x => x.Category).Include(x => x.ArticleTag).ThenInclude(x => x.Tag).ToList();
            return View(articles);
        }

  

        public IActionResult Create(IFormFile Photo, List<int> tagIds)
        {
            var catgories = _context.Categories.ToList();
            var tags = _context.Tags.ToList();
            ViewBag.categories = new SelectList(catgories, "Id", "CategoryName");
            ViewData["Tags"] = tags;
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Article article, List<int> tagIds, IFormFile Photo)
        {
            try
            {
                var catgories = await _context.Categories.ToListAsync();
                var tags = await _context.Tags.ToListAsync();
                ViewBag.categories = new SelectList(catgories, "Id", "CategoryName");
                ViewData["Tags"] = tags;

                var userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                article.UserId = userId;
                
                article.CreatedDate = DateTime.Now;

                article.SeoUrl = article.Title.ReplaceInvalidChars();

                article.PhotoUrl = await Photo.SaveFileAsync(_enw.WebRootPath);

                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();


                List<ArticleTag> articleTags = new();


                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleTag articleTag = new()
                    {
                        ArticleId = article.Id,
                        TagId = tagIds[i]
                    };
                    articleTags.Add(articleTag);
                }
                await _context.ArticleTags.AddRangeAsync(articleTags);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(x => x.Id == id);
            _context.Articles.Remove(article);
           await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == null) return NotFound();
            var article = _context.Articles.Include(x => x.ArticleTag).ThenInclude(x => x.Tag).FirstOrDefault(x => x.Id == id);
            if (article == null) return NotFound();
            var categories = _context.Categories.ToList();
            var tags = _context.Tags.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            ViewData["Tags"] = tags;
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Article article, List<int> tagIds, IFormFile Photo)
        {
            try
            {
                article.UpdatedDate = DateTime.Now;
                article.SeoUrl = article.Title.ReplaceInvalidChars();

                if (Photo != null)
                {
                    article.PhotoUrl = await Photo.SaveFileAsync(_enw.WebRootPath);
                }

                var art = _context.ArticleTags.Where(x => x.ArticleId == article.Id).ToList();
                _context.ArticleTags.RemoveRange(art);
                _context.SaveChanges();

                List<ArticleTag> articleTags = new();

                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleTag articleTag = new()
                    {
                        ArticleId = article.Id,
                        TagId = tagIds[i]
                    };
                    articleTags.Add(articleTag);

                }
                await _context.ArticleTags.AddRangeAsync(articleTags);

                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
