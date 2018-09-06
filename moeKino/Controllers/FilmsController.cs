using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using moeKino.Models;
using Microsoft.AspNet.Identity;
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
        public ActionResult ArchivedMovies()
        {
            return View(db.ArchivedFilms.ToList());
        }
        public ActionResult BestMovies()
        {

            foreach (var film in db.Films.ToList())
            {
                film.Rating = (int)(((float)film.clients.Count() / (db.Users.Count() - 1)) * 100);
            }
            return View(db.Films.ToList());
        }
        public ActionResult AcceptGift()
        {
            int id = 0;
            foreach (var item in db.Clients)
            {

                if (item.Email == User.Identity.GetUserName())
                {
                    id = item.ClientId;
                    break;
                }
            }
            var najdiKlient = db.Clients.Find(id);
            najdiKlient.Points = 0;
            db.SaveChanges();

            return RedirectToAction("Index", "Films");
        }
        public ActionResult Gift1() {
            return View();
        }
        public ActionResult Gift2()
        {
            return View();
        }
        public ActionResult Gift3()
        {
            return View();
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
             ViewBag.Time = film.Time;
             return View(model);
         }
         [HttpPost]
         public ActionResult AddClientToMovie(RatingClient model)
         {
             var film = db.Films.Find(model.FilmId);
             var client = db.Clients.Find(model.ClientId);
             film.clients.Add(client);
             client.Points += (10*model.NumberTickets);
             film.Audience += model.NumberTickets;

            Ticket ticket = new Ticket(model.ClientId,model.Date,model.Time,model.NumberTickets,film.Name);
            db.Tickets.Add(ticket);
            db.SaveChanges();
            if (client.Points >= 50 && client.Points < 100)
            {
                return RedirectToAction("Gift1", "Films");
            }
            else if (client.Points == 100)
            {
                return RedirectToAction("Gift2", "Films");

            }
            else if (client.Points > 100)
            {
                return RedirectToAction("Gift3", "Films");

            }
            return RedirectToAction("Index", "Films"); 
            //return View("Index", db.Films.ToList());
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
