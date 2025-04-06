using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;
        public IDepartmentRepository departmentRepository { get; }

        public IEmployeeRepository employeeRepository { get; }

        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;

            departmentRepository=new DepartmentRepository(_context);
            employeeRepository=new EmployeeRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
      
    }
}
