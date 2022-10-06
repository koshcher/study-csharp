using BlogDbLibrary;
using BlogDbLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace StrongBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly BlogDbContext _db;

        public PostController(BlogDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Post> objPostList = _db.Posts.ToList();
            return View(objPostList);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post obj)
        {
            if (obj.Title == obj.Publisher)
            {
                ModelState.AddModelError("Title", "Title cannot equals Publisher");
            }
            if (ModelState.IsValid)
            {
                _db.Posts.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Post added successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var postFromDb = _db.Posts.Find(id);

            if (postFromDb == null)
            {
                return NotFound();
            }

            return View(postFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post obj)
        {
            if (obj.Title == obj.Publisher)
            {
                ModelState.AddModelError("Title", "Title cannot equals Publisher");
            }
            if (ModelState.IsValid)
            {
                _db.Posts.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Post updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var postFromDb = _db.Posts.Find(id);

            if (postFromDb == null)
            {
                return NotFound();
            }

            return View(postFromDb);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Posts.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Posts.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Post deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteNow(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Posts.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Posts.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Post deleted successfully";

            return RedirectToAction("Index");
        }
    }
}