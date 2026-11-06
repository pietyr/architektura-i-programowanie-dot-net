using System.Diagnostics;
using lab5.Data;
using lab5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab5.Controllers;

public class HomeController : Controller
{
    private readonly MoviesDbContext _context;

    public HomeController(MoviesDbContext context)
    {
        _context = context;
    }

    // GET: Home
    public async Task<IActionResult> Index()
    {
        return View(await _context.Movies.ToListAsync());
    }

    // GET: Home/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: Home/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Home/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,Rating,TrailerLink")] Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: Home/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    // POST: Home/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Rating,TrailerLink")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }

                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: Home/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: Home/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Home/Top
    public async Task<IActionResult> Top()
    {
        var topMovie = await _context.Movies
            .OrderByDescending(m => m.Rating)
            .FirstOrDefaultAsync();

        if (topMovie == null)
        {
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Details), new { id = topMovie.Id });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}
