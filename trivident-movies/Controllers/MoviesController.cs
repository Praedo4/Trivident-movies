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
        IMongoCollection<MovieModel> _dbCollection;
        public MoviesController()
        {
            _dbContext = new MongoContext();
            _dbCollection = _dbContext._database.GetCollection<MovieModel>("movie");
        }

        // GET: Movies
        public async Task<ActionResult> Index()
        {
            var allMovies = (await _dbCollection.FindAsync(FilterDefinition<MovieModel>.Empty)).ToList();

            return View(allMovies); //  await db.MovieModels.ToListAsync()
        }

        // GET: Movies/Details/id
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            FilterDefinition<MovieModel> filter = Builders<MovieModel>.Filter.Eq(m => m.Id,ObjectId.Parse(id));
            MovieModel movieModel = (await (await _dbCollection.FindAsync<MovieModel>(filter)).FirstOrDefaultAsync()) ;
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
                FilterDefinition<MovieModel> filter = Builders<MovieModel>.Filter.And(Builders<MovieModel>.Filter.Eq(m => m.Title, movieModel.Title), Builders<MovieModel>.Filter.Eq(m => m.Year, movieModel.Year));
                var queryResults = (await _dbCollection.FindAsync<MovieModel>(filter)).ToList();
                if (queryResults.Count() == 0)
                {
                    await _dbCollection.InsertOneAsync(movieModel);
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
            FilterDefinition<MovieModel> filter = Builders<MovieModel>.Filter.Eq(m => m.Id, ObjectId.Parse(id));
            MovieModel movieModel = (await (await _dbCollection.FindAsync<MovieModel>(filter)).SingleAsync());
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
                FilterDefinition<MovieModel> filter = Builders<MovieModel>.Filter.And(Builders<MovieModel>.Filter.Eq(m => m.Title, movieModel.Title), Builders<MovieModel>.Filter.Eq(m => m.Year, movieModel.Year));
                var queryResults = (await _dbCollection.FindAsync<MovieModel>(filter)).ToList();
                string foundID = queryResults.First<MovieModel>().Id.ToString();
                if (queryResults.Count() == 0 || foundID.Equals(id))
                {
                    FilterDefinition<MovieModel> IDfilter = Builders<MovieModel>.Filter.Eq(m => m.Id, ObjectId.Parse(id));
                    movieModel.Id = new ObjectId(id);
                    await _dbCollection.ReplaceOneAsync(IDfilter, movieModel);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "This movie already exists in the database!";
                    ViewBag.Year = movieModel.Year;
                    ViewBag.ExistingId = foundID;
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
            FilterDefinition<MovieModel> filter = Builders<MovieModel>.Filter.Eq(m => m.Id, ObjectId.Parse(id));
            MovieModel movieModel = (await (await _dbCollection.FindAsync(filter)).FirstOrDefaultAsync());
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
            FilterDefinition<MovieModel> filter = Builders<MovieModel>.Filter.Eq(m => m.Id, ObjectId.Parse(id));
            await _dbCollection.FindOneAndDeleteAsync(filter);
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
