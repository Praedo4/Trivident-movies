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

        // GET: Movies
        public async Task<ActionResult> Index()
        {
            var allMovies = _dbCollection.FindAll().ToList();

            return View(allMovies); //  await db.MovieModels.ToListAsync()
        }

        // GET: Movies/Details/id
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            MovieModel movieModel = _dbCollection.FindOneById(ObjectId.Parse(id));
            if (movieModel == null)
            {
                return HttpNotFound();
            }
            return View(movieModel);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.Year = DateTime.Now.Year;
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,Director,ImageLink,Year,Actors")] MovieModel movieModel)
        {
            if (ModelState.IsValid)
            {
                var query = Query.And(Query.EQ("title", movieModel.Title), Query.EQ("year", movieModel.Year));
                var queryResults = _dbCollection.FindAs<MovieModel>(query);
                if (queryResults.Count() == 0)
                {
                    var result = _dbCollection.Insert(movieModel);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "This movie already exists in the database!";
                    ViewBag.Year = movieModel.Year;
                    ViewBag.ExistingId = queryResults.First<MovieModel>().Id.ToString();
                    return View("Create", movieModel);
                }
            }

            return View(movieModel);
        }

        // GET: Movies/Edit/id
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            MovieModel movieModel = _dbCollection.FindOneById(ObjectId.Parse(id));
            if (movieModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieTitle = movieModel.Title;
            ViewBag.ImageLink = movieModel.ImageLink;
            return View(movieModel);
        }

        // POST: Movies/Edit/id
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind(Include = "Title,Director,ImageLink,Year,Actors")] MovieModel movieModel)
        { 
            if (ModelState.IsValid)
            {
                var copyQuery = Query.And(Query.EQ("title", movieModel.Title), Query.EQ("year", movieModel.Year));
                var queryResults = _dbCollection.FindAs<MovieModel>(copyQuery);
                if (queryResults.Count() == 0)
                {
                    var query = Query<MovieModel>.EQ(p => p.Id, ObjectId.Parse(id));
                    movieModel.Id = new ObjectId(id);
                    var result = _dbCollection.Update(query, Update.Replace(movieModel), UpdateFlags.None);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "This movie already exists in the database!";
                    ViewBag.Year = movieModel.Year;
                    ViewBag.ExistingId = queryResults.First<MovieModel>().Id.ToString();
                    return View("Edit", movieModel);
                }
            }
            return View(movieModel);
        }

        // GET: Movies/Delete/id
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            MovieModel movieModel = _dbCollection.FindOneById(ObjectId.Parse(id));
            if (movieModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieTitle = movieModel.Title;
            ViewBag.ImageLink = movieModel.ImageLink;
            return View(movieModel);
        }

        // POST: Movies/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var query = Query<MovieModel>.EQ(p => p.Id, new ObjectId(id));
            _dbCollection.Remove(query, RemoveFlags.Single);
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
