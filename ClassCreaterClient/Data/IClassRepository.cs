using ClassCreaterClient.Moldes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClassCreaterClient.Data
{
    public interface IClassRepository
    {
        Task<List<Class>> GetClasses();
        Task<Class> GetClass(int ID);
        Task<List<Class>> GetClassesByTypes(int ClassTypeID);
        Task AddClass(Class classToAdd);
        Task UpdateClass(Class classToUpdate);
        Task DeleteClass(Class classToDelete);
    }
}
