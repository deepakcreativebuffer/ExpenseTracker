using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
using HtmlAgilityPack;
using System.Linq;
using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
        }


        // GET: Category/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            //var list = _context.Categories.ToList();
            //list = list.Where(m => m.CategoryId == id).ToList();

            //foreach(var item in list)
            //{
            //    ViewBag.list = list.ToList();
            //}

            if (id == 0)
                return View(new Category());
            else
         

                return View(_context.Categories.Find(id));

        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type,Description")] Category category)
        {
            if (category.Description != "")
            {
                category.Description = category.Description.Replace("<p>", "").Replace("</p>", "").Replace("&nbsp;", " ").Replace("&amp;", "&");
            }

            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                    
                    
                _context.Add(category);
                else
                    _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
