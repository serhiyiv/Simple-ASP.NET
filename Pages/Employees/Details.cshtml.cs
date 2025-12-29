using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using System.Security.Cryptography.X509Certificates;

namespace assignment4.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly assignment4.Data.NorthwindContext _context;
       
        
        [BindProperty(SupportsGet = true)]
        public String? ReportsToName { get; set; }



        public DetailsModel(assignment4.Data.NorthwindContext context)
        {
            _context = context;
        }

      public Employee Employee { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {

   
            

   



            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            else 
            {
                Employee = employee;


           


                var reportToEmployee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == employee.ReportsTo);
                if (reportToEmployee != null)
                    ReportsToName = reportToEmployee.FullName;
    

            }
            return Page();
        }
    }
}
