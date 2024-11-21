using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetPortfolio.Models;
using aspnetPortfolio.dbContext;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace aspnetPortfolio.Controllers;

public class ProjectController : Controller
{
    private PortofolioDbContext db = new PortofolioDbContext();
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int? tagId, string? message = null)
{
    ViewBag.SuccessMessage = message;
    ViewBag.Tags = db.Tags
        .Select(tag => new SelectListItem
        {
            Value = tag.Id.ToString(),
            Text = tag.Name
        })
        .ToList();

    var projects = db.Projects.Include(p => p.Tags).AsQueryable();

    if (tagId.HasValue)
    {
        projects = projects.Where(p => p.Tags.Any(t => t.Id == tagId.Value));
    }

    return View(projects.ToList());
}

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Tags = db.Tags
            .Select(tag => new SelectListItem
            {
                Value = tag.Id.ToString(),
                Text = tag.Name
            })
            .ToList();

        return View();
    }
   [HttpGet]
    public IActionResult Edit(int id)
    {
    var project = db.Projects
        .Include(p => p.Tags) 
        .FirstOrDefault(p => p.Id == id);

    if (project == null)
    {
        return NotFound();
    }

    var allTags = db.Tags.ToList();

    ViewBag.TagList = new MultiSelectList(allTags, "Id", "Name", project.Tags.Select(t => t.Id));

    return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Project project, int[] selectedTags)
    {
        if (id != project.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                db.Entry(project).State = EntityState.Modified;

                var existingProject = db.Projects
                    .Include(p => p.Tags)
                    .FirstOrDefault(p => p.Id == id);

                if (existingProject != null)
                {
                    existingProject.Tags.Clear(); 

                    foreach (var tagId in selectedTags)
                    {
                        var tag = db.Tags.Find(tagId);
                        if (tag != null)
                        {
                            existingProject.Tags.Add(tag);
                        }
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index", "Project", new { message = "Project edited succesfully" });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
        }

        var allTags = db.Tags.ToList();
        ViewBag.TagList = new MultiSelectList(allTags, "Id", "Name", selectedTags);

        return View(project);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var project = db.Projects.Include(p => p.Tags).FirstOrDefault(p => p.Id == id);
        if (project == null)
        {
            return NotFound();
        }

        db.Projects.Remove(project);
        db.SaveChanges();

        return RedirectToAction("Index", "Project", new { message = "Project deleted successfully!" });

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(Project project, IFormFile file, int[] SelectedTags)
    {
         if (!string.IsNullOrWhiteSpace(project.GithubLink) &&
        !Regex.IsMatch(project.GithubLink, @"^https:\/\/github\.com\/.+"))
        {
            ModelState.AddModelError("GithubLink", "The GitHub link must start with 'https://github.com/'.");
        }
        if (ModelState.IsValid)
        {
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
                Directory.CreateDirectory(uploadsFolder);
                string path = Path.Combine(uploadsFolder, Path.GetFileName(file.FileName));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                project.FilePath = "/Uploads/" + file.FileName;
            }

            if (SelectedTags != null && SelectedTags.Any())
            {
                foreach (var tagId in SelectedTags)
                {
                    var tag = db.Tags.Find(tagId);
                    if (tag != null)
                    {
                        project.Tags.Add(tag);
                    }
                }
            }

            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Project", new { message = "Project created successfully!" });
        }

        ViewBag.Tags = db.Tags
            .Select(tag => new SelectListItem
            {
                Value = tag.Id.ToString(),
                Text = tag.Name
            })
            .ToList();

        return View(project);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
