using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        //IEnumerable<Department> GetAll();

        //Department? Get(int id);

        //int Add(Department department);


        //int Update(Department department);


        //int Delete(Department department);

        Task<IEnumerable<Department>> GetByNameAsync(string name);
    }
}
