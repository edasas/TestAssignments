using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HarbortouchTest.DataContext;
using HarbortouchTest.Models;
using System.Data.Entity.Validation;
using HarbortouchTest.Helpers;

namespace HarbortouchTest.Controllers
{
    public class RentalsController : Controller
    {
        private HarbortouchContext db = new HarbortouchContext();
        private static RentalItems viewItems = new RentalItems();

        // GET: Rentals
        public ActionResult Index()
        {
            viewItems.Items = db.Rentals.ToList();
            return View(viewItems);
        }

        // GET: Rentals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rental rental = db.Rentals.Find(id);
            //Rental rental = db.Rentals.Include("Tenants").FirstOrDefault(rntl => rntl.Id == id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            var rentalView = ViewModelHelper.RentalToViewModel(rental);
            return View(rentalView);
        }

        // GET: Rentals/Create
        public ActionResult Create()
        {
            var rental = db.Rentals.Add(new Rental());
            //var rentalView = ViewModelHelper.ToViewModel(rental, db.Tenants.ToList());
            var rentalView = new RentalView() { Tenants = ViewModelHelper.GetAllTenantData(db) };
            
            return View(rentalView);
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,City,State,ZipCode,OwnerName,Tenants")] RentalView rentalView)
        {
            if (ModelState.IsValid)
            {
                var rental = ViewModelHelper.RentalViewToRental(rentalView);                
                AddOrUpdateTenants(rental, rentalView.Tenants);

                db.Rentals.Attach(rental);
                db.Rentals.Add(rental);

                try
                {
                    db.SaveChanges();
                    viewItems.Messages.Add(new ViewMessage()
                    {
                        MessageType = MessageTypes.Success,
                        BoldMessage = "Success!",
                        Message = "New rental created."
                    });
                }
                catch
                {                    
                    return View(rentalView);
                }
                
                return RedirectToAction("Index");
            }

            return View(rentalView);
        }

        // GET: Rentals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rental rental = db.Rentals.FirstOrDefault(rentl => rentl.Id == id);
            if (rental == null)
            {
                return HttpNotFound();
            }

            var allTenants = db.Tenants.ToList();
            var rentalView = ViewModelHelper.RentalToViewModel(rental, allTenants);

            return View(rentalView);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,City,State,ZipCode,OwnerName,Tenants")] RentalView rentalView)
        {
            if (ModelState.IsValid)
            {
                Rental rental = db.Rentals.FirstOrDefault(rntl => rntl.Id == rentalView.Id);
                AddOrUpdateTenants(rental, rentalView.Tenants);


                db.Entry(rental).CurrentValues.SetValues(rentalView);
                db.SaveChanges();

                viewItems.Messages.Add(new ViewMessage()
                {
                    MessageType = MessageTypes.Success,
                    BoldMessage = "Success!",
                    Message = "Items successfully updated!."
                });

                return RedirectToAction("Index");
            }
            return View(rentalView);
        }

        // GET: Rentals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            var rentalView = ViewModelHelper.RentalToViewModel(rental);
            return View(rentalView);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rental rental = db.Rentals.Find(id);
            db.Rentals.Remove(rental);
            db.SaveChanges();

            viewItems.Messages.Add(new ViewMessage()
            {
                MessageType = MessageTypes.Success,
                BoldMessage = "Success!",
                Message = "Items successfully deleted!."
            });

            return RedirectToAction("Index");
        }

        private void AddOrUpdateTenants(Rental rental, IEnumerable<TenantRentingData> rentingTenants)
        {
            var newRentalTenantIds = rentingTenants.Where(tenant => tenant.IsRenting).Select(tent => tent.Id).ToList();
            var currentRentalTenantIds = rental.Tenants.Select(tenant => tenant.Id).ToList();
            var rentalTenantIdsToDelete = currentRentalTenantIds.Where(tenantId => !newRentalTenantIds.Contains(tenantId)).ToList();

            foreach (var tenantId in rentalTenantIdsToDelete)
            {
                var tenant = db.Tenants.FirstOrDefault(tennt => tennt.Id == tenantId);
                if (tenant != null)
                {
                    rental.Tenants.Remove(tenant);
                }
            }

            foreach (var tenantId in newRentalTenantIds)
            {
                if (!currentRentalTenantIds.Contains(tenantId))
                {
                    var tenant = db.Tenants.FirstOrDefault(tennt => tennt.Id == tenantId);
                    if (tenant != null)
                    {
                        rental.Tenants.Add(tenant);
                    }
                }
            }
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
