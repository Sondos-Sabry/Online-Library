using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_4.Models;
namespace mvc_4.Controllers
{
    public class bookController : Controller
    {
        BlogContext db;
        public bookController(BlogContext db)
        {
            this.db = db;
        }

        public ActionResult addbook()
        {
            if (HttpContext.Session.GetInt32("userid") == null)
            {
                return RedirectToAction("login", "user");
            }
            else
            {

                List<Catalog> ct = db.Catalogs.ToList();
                ViewBag.cat = new SelectList(ct, "cat_id", "cat_name");
                return View();
            }
        }

        [HttpPost]
        public ActionResult addbook (book b, IFormFile img)
        {
            //upload
            string path = $"wwwroot/media/{img.FileName}";
            FileStream f = new FileStream(path, FileMode.Create);
            img.CopyTo(f);

            //save
            b.photo = $"/media/{img.FileName}"; ;
            b.user_id = HttpContext.Session.GetInt32("user_id");
            b.date = DateTime.Now;
            db.books.Add(b);
            db.SaveChanges();

            return RedirectToAction("display");
        }


        public ActionResult display()
        {
            int ? id = HttpContext.Session.GetInt32("user_id");
            List<book> b1 = db.books.Include(n=>n.cat).Where(n => n.user_id == id).OrderByDescending(n=>n.date).ToList();

            return View(b1);
        }

        public ActionResult readmore(int id)
        {
            return View(db.books.Include(n => n.cat).Where(n => n.id == id).FirstOrDefault());
   
        }
       
    }
}
