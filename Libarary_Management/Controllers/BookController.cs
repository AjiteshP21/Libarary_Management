using System;
using System.Linq;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Libarary_Management; 

public class BooksController : Controller
{
    private readonly Lib_ManagementEntities _context;

    public BooksController()
    {
        _context = new Lib_ManagementEntities(); 
    }

    public ActionResult Index(string search)
    {
        var books = _context.tblBooks.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            // Search for books by Title, Author, or ISBN
            books = books.Where(b => b.Title.Contains(search) ||
                                     b.AuthorName.Contains(search) ||
                                     b.ISBN.Contains(search));
        }

        return View(books.ToList());
    }

    public ActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.tblCategories.ToList(), "CategoryName", "CategoryName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(tblBook book)
    {
        if (_context.tblBooks.Any(b => b.ISBN == book.ISBN))
        {
            ModelState.AddModelError("ISBN", "ISBN already exists.");
        }

        var isbnPattern = @"^\d{3}-\d{2}-\d{5}-\d{2}-\d{1}$"; 
        if (!Regex.IsMatch(book.ISBN, isbnPattern))
        {
            ModelState.AddModelError("ISBN", "Invalid ISBN format.");
        }

        if (book.CategoryType == "Horror" && !book.ISBN.StartsWith("978"))
        {
            ModelState.AddModelError("ISBN", "ISBN for Horror category must start with 978.");
        }

        if (ModelState.IsValid)
        {
            book.CreatedBy = User.Identity.Name ?? "Admin";
            book.CreatedDate = DateTime.Now;
            _context.tblBooks.Add(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewBag.Categories = new SelectList(_context.tblCategories.ToList(), "CategoryName", "CategoryName");
        return View(book);
    }

    // Action to handle deleting a book
    public ActionResult Delete(int id)
    {
        var book = _context.tblBooks.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return HttpNotFound();
        }
        return View(book);
    }

    // Action to confirm and delete a book
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        var book = _context.tblBooks.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return HttpNotFound();
        }

        _context.tblBooks.Remove(book);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
