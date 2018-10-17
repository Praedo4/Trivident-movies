using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using trivident_movies.Models;
using trivident_movies.App_Start;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace trivident_movies.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        MongoContext _dbContext;
        MongoCollection<MovieModel> _dbCollection;
        public MoviesController()
        {
            _dbContext = new MongoContext();
            _dbCollection = _dbContext._database.GetCollection<MovieModel>("movie");
        }

        // GET: MovieModels
        public async Task<ActionResult> Index()
        {
            var allMovies = _dbCollection.FindAll().ToList();

            return View(allMovies); //  await db.MovieModels.ToListAsync()
        }

        // GET: MovieModels/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieModel movieModel = _dbCollection.FindOneById(ObjectId.Parse(id));
            if (movieModel == null)
            {
                return HttpNotFound();
            }
            return View(movieModel);
        }

        // GET: MovieModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MovieModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,Director,ImageLink,Year,Actors")] MovieModel movieModel)
        {
            if (ModelState.IsValid)
            {
                //db.MovieModels.Add(movieModel);
                var result = _dbCollection.Insert(movieModel);
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(movieModel);
        }

        // GET: MovieModels/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //MovieModel movieModel = await db.MovieModels.FindAsync(id);
            MovieModel movieModel = _dbCollection.FindOneById(ObjectId.Parse(id));
            if (movieModel == null)
            {
                return HttpNotFound();
            }
            return View(movieModel);
        }

        // POST: MovieModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind(Include = "Title,Director,ImageLink,Year,Actors")] MovieModel movieModel)
        { 
            if (ModelState.IsValid)
            {
                //db.Entry(movieModel).State = EntityState.Modified;
                var query = Query<MovieModel>.EQ(p => p.Id, ObjectId.Parse(id));
                movieModel.Id = new ObjectId(id);
                var result = _dbCollection.Update(query, Update.Replace(movieModel), UpdateFlags.None);
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movieModel);
        }

        // GET: MovieModels/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //MovieModel movieModel = await db.MovieModels.FindAsync(id);
            MovieModel movieModel = _dbCollection.FindOneById(ObjectId.Parse(id));
            if (movieModel == null)
            {
                return HttpNotFound();
            }
            return View(movieModel);
        }

        // POST: MovieModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            //MovieModel movieModel = await db.MovieModels.FindAsync(id);
            //MovieModel movieModel = _dbCollection.FindOneById(ObjectId.Parse(id));
            //db.MovieModels.Remove(movieModel);
            var query = Query<MovieModel>.EQ(p => p.Id, new ObjectId(id));
            _dbCollection.Remove(query, RemoveFlags.Single);
            //await db.SaveChangesAsync();
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
