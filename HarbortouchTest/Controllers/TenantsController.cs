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
using HarbortouchTest.Helpers;

namespace HarbortouchTest.Controllers
{
    public class TenantsController : Controller
    {
        private HarbortouchContext db = new HarbortouchContext();
        private static TenantItems viewItems = new TenantItems();

        // GET: Tenants
        public ActionResult Index()
        {
            viewItems.Items = db.Tenants.ToList();
            return View(viewItems);
        }

        // GET: Tenants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,StartDate,EndDate,Cost")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Tenants.Add(tenant);
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
                    return View(tenant);
                }

                return RedirectToAction("Index");
            }

            return View(tenant);
        }

        // GET: Tenants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartDate,EndDate,Cost")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();

                viewItems.Messages.Add(new ViewMessage()
                {
                    MessageType = MessageTypes.Success,
                    BoldMessage = "Success!",
                    Message = "Items successfully updated!."
                });

                return RedirectToAction("Index");
            }
            return View(tenant);
        }

        // GET: Tenants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tenant tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
            db.SaveChanges();

            viewItems.Messages.Add(new ViewMessage()
            {
                MessageType = MessageTypes.Success,
                BoldMessage = "Success!",
                Message = "Items successfully deleted!."
            });

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
