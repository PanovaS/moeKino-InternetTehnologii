using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using moeKino.Models;
namespace moeKino.Controllers
{
    public class FilmsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Films
        public ActionResult Index()
        {

            foreach (var film in db.Films.ToList())
            {
                film.Rating = (int)(((float)film.clients.Count()/(db.Users.Count()-1))*100);
            }
           
            
            return View(db.Films.ToList());
        }
        public ActionResult Soon() {

            return View();
        }

        public ActionResult RatingMovies()
        {

            foreach (var film in db.Films.ToList())
            {
                film.Rating = (int)(((float)film.clients.Count() / (db.Users.Count() - 1)) * 100);
            }
            return View(db.Films.ToList());
        }







        //2 akcii za dodavanje klient na odreden film
        [HttpGet]
         public ActionResult AddClientToMovie(int id) {
            var model = new RatingClient();

            ViewBag.Client = User.Identity.Name;
            model.FilmId = id;
             model.Clients = db.Clients.ToList();
             var film = db.Films.Find(id);
             ViewBag.Name = film.Name;
             return View(model);
         }
         [HttpPost]
         public ActionResult AddClientToMovie(RatingClient model)
         {
             var film = db.Films.Find(model.FilmId);
             var client = db.Clients.Find(model.ClientId);
             film.clients.Add(client);
             client.Points += 10;
             db.SaveChanges();
             return View("Index", db.Films.ToList());
         }


     



        // GET: Films/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        public ActionResult DetailsRatingMovies(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // GET: Films/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Url,Genre,Director,ReleaseDate,ShortDescription,Stars")] Film film)
        {
            if (ModelState.IsValid)
            {
                db.Films.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(film);
        }

        // GET: Films/Edit/5
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Url,Genre,Director,ReleaseDate,ShortDescription,Stars")] Film film)
        {
            if (ModelState.IsValid)
            {
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(film);
        }

        // GET: Films/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Films.Find(id);
            db.Films.Remove(film);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
