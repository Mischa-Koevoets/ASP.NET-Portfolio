using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

using Microsoft.AspNetCore.Mvc;
using aspnetPortfolio.Models;
using aspnetPortfolio.dbContext;

namespace aspnetPortfolio.Controllers;

public class ProjectController : Controller
{
    private PortofolioDbContext db = new PortofolioDbContext();
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger)
    {
        _logger = logger;
    }

    public ActionResult Index()
    {
        return View(db.Projects.ToList());
    }
    public IActionResult Create(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}