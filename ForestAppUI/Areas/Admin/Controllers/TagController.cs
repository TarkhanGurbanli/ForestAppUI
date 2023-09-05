using ForestAppUI.Data;
using ForestAppUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace ForestAppUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tags = _context.Tags.ToList();
            return View(tags);
        }


        public async Task<IActionResult> TagSearch(string Empsearch)
        {
            ViewData["Gettagnames"] = Empsearch;
            var emoquery = from x in _context.Tags select x;
            if (!String.IsNullOrEmpty(Empsearch))
            {
                emoquery = emoquery.Where(x => x.TagName.Contains(Empsearch));
            }
            return View(await emoquery.AsNoTracking().ToListAsync());
        }

     

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            var tagName = _context.Tags.FirstOrDefault(x => x.TagName == tag.TagName);

            if(tagName != null)
            {
                ModelState.AddModelError("Error", "Tag adı mövcuddur!");
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    tag.CreatedDate = DateTime.Now;
                    _context.Tags.Add(tag);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index), nameof(Tag));
                }
                else
                {
                    return View();
                }
            }
        }

        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var tagName = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tagName == null) return NotFound();
            return View(tagName);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var tagName = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tagName == null) return NotFound();
            return View(tagName);
        }

        [HttpPost]
        public IActionResult Delete(Tag tag)
        {
            tag.DeletedDate = DateTime.Now;
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), nameof(Tag));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var tagName = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tagName == null) return NotFound();
            return View(tagName);
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(tag);
                }
                else
                {
                    tag.UpdatedDate = DateTime.Now;
                    _context.Tags.Update(tag);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index), nameof(Tag));
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
