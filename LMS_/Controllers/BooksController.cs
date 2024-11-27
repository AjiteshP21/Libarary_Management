using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Diagnostics;
using LMS_;

namespace LMS_.Controllers
{
    public class BookController : Controller
    {
        private Lib_ManagementEntities _dbcontext = new Lib_ManagementEntities();
        // GET: Book
        public ActionResult Index(string searchTerm)
        {
            var books = _dbcontext.tblBooks.AsQueryable(); 
            if (!string.IsNullOrEmpty(searchTerm))
            {
                books = books.Where(b => b.ISBN.Contains(searchTerm));
            }

            return View(books.ToList());
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(
                new List<string> { "Mystery", "Horror", "Science fiction", "Thriller", "History" });
            return View();
        }
        [HttpPost]
        public ActionResult Create(tblBook book)
        {
            if (_dbcontext.tblBooks.Any(b => b.ISBN == book.ISBN))
            {
                ModelState.AddModelError("ISBN", "A book with this ISBM is already exists.");
            }
            if (ModelState.IsValid)
            {
                _dbcontext.tblBooks.Add(book);
                _dbcontext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(
                _dbcontext.tblBooks.Select(b => b.CategoryType).Distinct().ToList()
            );
            return View(book);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var book = _dbcontext.tblBooks.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.Categories = new SelectList
                (_dbcontext.tblBooks
                .Select(b => b.CategoryType)
                .Distinct()
                .ToList(),
                book.CategoryType);
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(tblBook book)
        {
            if (_dbcontext.tblBooks.Any(b => b.ISBN == book.ISBN && b.Id != book.Id))
            {
                ModelState.AddModelError("ISBN", "A book with this ISBN already exists.");
            }

            var isbnRegex = @"^\d{3}-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$"; // ISBN format like 978-92-95055-02-5
            if (!System.Text.RegularExpressions.Regex.IsMatch(book.ISBN, isbnRegex))
            {
                ModelState.AddModelError("ISBN", "Invalid ISBN format. Example: 978-92-95055-02-5");
            }

            if (ModelState.IsValid)
            {
                _dbcontext.Entry(book).State = EntityState.Modified;
                try
                {
                    _dbcontext.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    throw; // Optionally re-throw the exception after logging
                }

                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(
                _dbcontext.tblBooks.Select(b => b.CategoryType).Distinct().ToList(),
                book.CategoryType
            );
            return View(book);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var book = _dbcontext.tblBooks.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var book = _dbcontext.tblBooks.Find(id);
            _dbcontext.tblBooks.Remove(book);
            _dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbcontext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}