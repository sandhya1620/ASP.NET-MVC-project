using System;
using System.Linq;
using System.Web.Mvc;
using YourNamespace.Models; // Update this to match your actual namespace

public class InsureeController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext(); // Update to match your context

    // GET: Insuree/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Insuree/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Insuree insuree)
    {
        if (ModelState.IsValid)
        {
            // Base rate
            decimal quote = 50;

            // Age calculation
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (age <= 18)
            {
                quote += 100;
            }
            else if (age >= 19 && age <= 25)
            {
                quote += 50;
            }
            else if (age >= 26)
            {
                quote += 25;
            }

            // Car year calculation
            if (insuree.CarYear < 2000)
            {
                quote += 25;
            }
            else if (insuree.CarYear > 2015)
            {
                quote += 25;
            }

            // Car make calculation
            if (insuree.CarMake == "Porsche")
            {
                quote += 25;
                if (insuree.CarModel == "911 Carrera")
                {
                    quote += 25;
                }
            }

            // Speeding tickets calculation
            quote += insuree.SpeedingTickets * 10;

            // DUI calculation
            if (insuree.DUI)
            {
                quote *= 1.25M; // 25% increase
            }

            // Coverage type calculation
            if (insuree.CoverageType == "Full")
            {
                quote *= 1.50M; // 50% increase
            }

            // Save quote
            insuree.Quote = quote;
            db.Insurees.Add(insuree);
            db.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the Index action to display all insurees
        }

        return View(insuree);
    }

    // GET: Insuree/Index
    public ActionResult Index()
    {
        var insurees = db.Insurees.ToList();
        return View(insurees);
    }

    // GET: Insuree/Admin
    public ActionResult Admin()
    {
        var insurees = db.Insurees.ToList();
        return View(insurees);
    }
}