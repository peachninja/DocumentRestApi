using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentContainerRestService.Models;
using TikaOnDotNet.TextExtraction;

namespace DocumentContainerRestService.Controllers
{
    public class DocumentVersionsController : Controller
    {
        private DocumentContainerRestServiceContext db = new DocumentContainerRestServiceContext();

        // GET: DocumentVersions
        public ActionResult Index()
        {
            return View(db.DocumentVersions.ToList());
        }

        // GET: DocumentVersions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentVersion documentVersion = db.DocumentVersions.Find(id);
            if (documentVersion == null)
            {
                return HttpNotFound();
            }
            return View(documentVersion);
        }

        // GET: DocumentVersions/Create
        public DocumentVersion Create(Document document, TextExtractionResult result)
        {
            int versionNumber = 1;
            if (db.DocumentVersions.Where(a => a.DocumentId == document.Guid) != null)
            {
                var docVer = db.DocumentVersions.Where(a => a.DocumentId == document.Guid)
                    .OrderBy(a => a.VersionNumber).Last();
                versionNumber += docVer.VersionNumber;
            }
           
          
            DocumentVersion docVersion = new DocumentVersion
            {
                DocumentId = document.Guid,
                VersionNumber = versionNumber,
                FileName = Path.GetFileNameWithoutExtension(result.Metadata["FilePath"]),
                FileExtension = Path.GetExtension(result.Metadata["FilePath"]),
                LastUpdated = DateTime.Parse(result.Metadata["Last-Modified"]),
                Index = document.Guid.ToString() + Guid.NewGuid()

            };

            db.DocumentVersions.Add(docVersion);
            db.SaveChanges();
            return docVersion;
        }

        public void UpdateSize(int size, int id)
        {
            DocumentVersion documentVersion = db.DocumentVersions.Find(id);
            documentVersion.Size = size;
            db.DocumentVersions.AddOrUpdate(documentVersion);
            db.SaveChanges();
        }
        // POST: DocumentVersions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DocumentId,VersionNumber,DocumentPath,FileName,FileExtension,LastUpdated,LastUpdatedBy,Size,Index")] DocumentVersion documentVersion)
        {
            if (ModelState.IsValid)
            {
                db.DocumentVersions.Add(documentVersion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(documentVersion);
        }

        // GET: DocumentVersions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentVersion documentVersion = db.DocumentVersions.Find(id);
            if (documentVersion == null)
            {
                return HttpNotFound();
            }
            return View(documentVersion);
        }

        // POST: DocumentVersions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DocumentId,VersionNumber,DocumentPath,FileName,FileExtension,LastUpdated,LastUpdatedBy,Size,Index")] DocumentVersion documentVersion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentVersion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(documentVersion);
        }

        // GET: DocumentVersions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentVersion documentVersion = db.DocumentVersions.Find(id);
            if (documentVersion == null)
            {
                return HttpNotFound();
            }
            return View(documentVersion);
        }

        // POST: DocumentVersions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentVersion documentVersion = db.DocumentVersions.Find(id);
            db.DocumentVersions.Remove(documentVersion);
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
