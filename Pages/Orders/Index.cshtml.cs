using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace assignment4.Pages.Orders
{
    public class IndexModel : PageModel
    {

        public SelectList Employees { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? EmployeeId { get; set; }


        private readonly assignment4.Data.NorthwindContext _context;

        public IndexModel(assignment4.Data.NorthwindContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {


            var dropList = _context.Employees.OrderBy(x => x.LastName).ToList();

            Employees = new SelectList(dropList, nameof(Employee.EmployeeId), nameof(Employee.FullName));


            var orders = from m in _context.Orders.Where(x => x.Freight >= 250)
                         select m;

            if (EmployeeId != null)
            {
                orders = orders.Where(e => e.EmployeeId == EmployeeId);
            }




            Order = await orders
       .Include(o => o.Employee)
       .Include(o => o.ShipViaNavigation).ToListAsync();




        }
    }
}
