using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.Data.Contexts;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {       
        private readonly CompanyDbContext _context;
        public DepartmentRepository(CompanyDbContext companyDbContext) : base(companyDbContext)
        {
            _context = companyDbContext;
        }   

        public async Task<IEnumerable<Department>> GetByNameAsync(string name)
        {
            return await _context.Departments.Include(D => D.Employees).Where(D => D.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
