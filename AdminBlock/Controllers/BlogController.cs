using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdminBlock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminBlock.Controllers;

public class BlogController : Controller
{
    private readonly ILogger<BlogController> _logger;
    private readonly BlogContext _context;

    public BlogController(ILogger<BlogController> logger, BlogContext context)
    {
        _logger = logger;
        _context = context;
    }


    public IActionResult Index()
    {
        ViewBag.Categories = _context.Category.Select(w =>
        new SelectListItem
        {
            Text = w.Name,
            Value = w.Id.ToString()
        }).ToList();
        return View();
    }

    public async Task<IActionResult> Save(Blog model)
    {
        if (model != null)
        {
            var file = Request.Form.Files.First();
            //C:\Users\User\Desktop\Asp.net mvc Proje\Asp.net Blog\wwwroot\img
            string savePath = Path.Combine("C:", "Users", "User", "Desktop", "Asp.net mvc Proje", "Asp.net Blog", "wwwroot", "img");

            var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
            var fileUrl = Path.Combine(savePath, fileName);
            using (var fileStream = new FileStream(fileUrl, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
                // kopyası oluşturuldu. 
            }

            model.Imagepath = fileName;
            model.AuthorId = (int) HttpContext.Session.GetInt32("id");
            // veritabanımıza ekleme işlemi
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        return Json(false);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
