using ForestAppUI.Data;
using ForestAppUI.Helper;
using ForestAppUI.Models;
using ForestAppUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace ForestAppUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index(int pg = 1)
        {
            const int pageSize = 9;
            if (pg < 1)
            {
                pg = 1;
            }

            int articleCount = _context.Articles.Count();

            var pager = new Pager(articleCount, pg, pageSize);

            int artSkip = (pg - 1) * pageSize;

            var articles = _context.Articles
                .Include(x => x.User)
                .Include(x => x.Category)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(artSkip)
                .Take(pager.PageSize)
                .ToList();
            var firstArticle = _context.Articles
                 .Include(x => x.Category).Include(x => x.User)
                 .OrderByDescending(x => x.Id)
                 .FirstOrDefault();
            var popularCategories = _context.Articles.GroupBy(article => article.Category.CategoryName).AsEnumerable().Select(group => new KeyValuePair<string, int>(group.Key, group.Count())).OrderByDescending(pair => pair.Value).Take(3).ToList();
            var popArt = _context.Articles.OrderByDescending(X => X.ViewCount).Take(3).ToList();


            HomeVM homeVM = new()
            {
                Articles = articles,
                FirstSlot = firstArticle,
                PopularCategories = popularCategories,
                PopularArticle = popArt

            };
            ViewBag.Pager = pager;

            var recentPosts = _context.Articles
               .Include(x => x.User)
               .Include(x => x.Category)
               .OrderByDescending(x => x.CreatedDate)
               .Take(5)
               .ToList();

            ViewBag.RecentPosts = recentPosts;

            return View(homeVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}