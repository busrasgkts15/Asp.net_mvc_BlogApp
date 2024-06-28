using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdminBlock.Models;

namespace AdminBlock.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogContext _context;

    public HomeController(ILogger<HomeController> logger, BlogContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> AddCategory(Category category)
    {
        if (category.Id == 0)
        {
            await _context.AddAsync(category);
        }
        else
        {
            _context.Update(category);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Category));

    }


    public IActionResult Category()
    {
        // buraya bir liste göndermem gerektiği için liste oluşturuyoruz.
        List<Category> list = _context.Category.ToList();
        // Ardında bu listeyi sayfamıza gönderiyoruz.
        return View(list);
    }


    // Buraya bir yazar nesnesi oluşturuyoruz.
    public IActionResult Author()
    {
        // buraya bir liste göndermem gerektiği için liste oluşturuyoruz.
        List<Author> list = _context.Author.ToList();
        // Ardında bu listeyi sayfamıza gönderiyoruz.
        return View(list);
    }

    public async Task<IActionResult> AddAuthor(Author author)
    {
        if (author.Id == 0)
        {
            await _context.AddAsync(author);
        }
        else
        {
            _context.Update(author);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Author));

    }

    public async Task<IActionResult> AuthorDetails(int Id)
    {
        var author = await _context.Author.FindAsync(Id);
        return Json(author);
    }


    public async Task<IActionResult> DeleteAuthor(int? Id)
    {
        Author author = await _context.Author.FindAsync(Id);
        _context.Remove(author);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Author));
    }


    public async Task<IActionResult> DeleteCategory(int? Id)
    {
        Category category = await _context.Category.FindAsync(Id);
        _context.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Category));
    }

    public async Task<IActionResult> CategoryDetails(int Id)
    {
        var category = await _context.Category.FindAsync(Id);
        return Json(category);
    }



    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
