using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lab3.Models;

namespace lab3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FruitService _fruitService;

    public HomeController(ILogger<HomeController> logger, FruitService fruitService)
    {
        _logger = logger;
        _fruitService = fruitService;
    }

    public IActionResult Index()
    {
        ViewData["random"] = Random.Shared.NextDouble();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Fruits([FromQuery] string? sort = null)
    {
        IEnumerable<Fruit> fruits = _fruitService.GetFruits();

        if (sort == "name")
        {
            fruits = fruits.OrderBy(f => f.Name);
        }
        else if (sort == "price")
        {
            fruits = fruits.OrderBy(f => f.Price);
        }
        else if (sort == "sweet")
        {
            fruits = fruits.OrderBy(f => f.IsSweet);
        }

        return View(fruits);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Fruit fruit)
    {
        if (ModelState.IsValid)
        {
            _fruitService.Add(fruit);
            return RedirectToAction(nameof(Fruits));
        }

        return View(fruit);
    }

    public IActionResult Delete(int id)
    {
        if (!_fruitService.Remove(id))
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Fruits));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
