using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237assignment6;

namespace cis237assignment6.Controllers
{
    public class BeveragesController : Controller
    {
        private BeverageJKoehlerEntities db = new BeverageJKoehlerEntities();

        // GET: Beverages
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DbSet<Beverage> BevSearch = db.Beverages;

            string bevName = "";
            string bevPack = "";
            string bevMinPrice = "";
            string bevMaxPrice = "";

            decimal minPrice = 0;
            decimal maxPrice = 1000;

            if (Session["name"] != null && !String.IsNullOrWhiteSpace((string)Session["name"]))
            {
                bevName = (string)Session["name"];
            }
            if (Session["pack"] != null && !String.IsNullOrWhiteSpace((string)Session["pack"]))
            {
                bevPack = (string)Session["pack"];
            }

            if (Session["min"] != null && !String.IsNullOrWhiteSpace((string)Session["min"]))
            {
                bevMinPrice = (string)Session["min"];
                minPrice = Decimal.Parse(bevMinPrice);
            }

            if (Session["max"] != null && !String.IsNullOrWhiteSpace((string)Session["max"]))
            {
                bevMaxPrice = (string)Session["max"];
                maxPrice = Decimal.Parse(bevMaxPrice);
            }

            IEnumerable<Beverage> filteredBevs = BevSearch.Where(Beverage => Beverage.name.Contains(bevName) &&
                                                                             Beverage.pack.Contains(bevPack) &&
                                                                             Beverage.price >= minPrice      &&
                                                                             Beverage.price <= maxPrice);

            ViewBag.bevName = bevName;
            ViewBag.bevPack = bevPack;
            ViewBag.bevMinPrice = bevMinPrice;
            ViewBag.bevMaxPrice = bevMaxPrice;



            return View(filteredBevs);
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter()
        {
       
            String name = Request.Form.Get("name");
            String pack = Request.Form.Get("pack");
            String min = Request.Form.Get("min");
            String max = Request.Form.Get("max");

            
            Session["name"] = name;
            Session["pack"] = pack;
            Session["min"] = min;
            Session["max"] = max;

          
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
