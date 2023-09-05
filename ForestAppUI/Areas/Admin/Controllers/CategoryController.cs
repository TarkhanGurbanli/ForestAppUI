using ForestAppUI.Data;
using ForestAppUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForestAppUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize]

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        category.CreatedDate = DateTime.Now;
                        _context.Categories.Add(category);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var categoryName = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (categoryName == null) return NotFound();
            return View(categoryName);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var categoryName = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (categoryName == null) return NotFound();
            return View(categoryName);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            category.DeletedDate = DateTime.Now;
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var tagName = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (tagName == null) return NotFound();
            return View(tagName);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        category.UpdatedDate = DateTime.Now;
                        _context.Categories.Update(category);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
