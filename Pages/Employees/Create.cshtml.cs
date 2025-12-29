using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using assignment4.Data;
using assignment4.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Hosting;

namespace assignment4.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly assignment4.Data.NorthwindContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public IFormFile? Upload { get; set; }


        public CreateModel(assignment4.Data.NorthwindContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {

            var employees = _context.Employees.Select(e => new
            {
                e.EmployeeId, FullName = e.FirstName + " " + e.LastName
            }).ToList();

            ViewData["ReportsTo"] = new SelectList(employees, "EmployeeId", "FullName");

            return Page();
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Employees == null || Employee == null)
            {
                return Page();
            }


            if (Upload != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Upload.FileName);
                var file = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);

                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }

                Employee.PhotoPath = fileName;
            }

            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
