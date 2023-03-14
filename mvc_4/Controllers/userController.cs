using Microsoft.AspNetCore.Mvc;
using mvc_4.Models;
namespace mvc_4.Controllers
{
    public class userController : Controller
    {
        BlogContext db;
        public userController(BlogContext db)
        {
            this.db = db;
        }
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(User u )
        {
            db.Users.Add( u );
            db.SaveChanges();
            return RedirectToAction("login");
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string username , string password)
        {
             User s = db.Users.Where(n=>n.username == username && n.password==password).FirstOrDefault();
            if (s != null)
            {
                HttpContext.Session.SetInt32("userid", s.id);
                return RedirectToAction("profile"); 
            }
            else
            {
                ViewBag.status = "incorrect username or password";
                return View();
            }
            return View();
        }
        public ActionResult profile()
        {
            int ? id = HttpContext.Session.GetInt32("userid");
            if (id == null)
            {
                return RedirectToAction("login");
            }
            User s = db.Users.Where(n => n.id == id).FirstOrDefault();
           
            return View(s);
        }
        public ActionResult logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("login");
        }
    }
}
