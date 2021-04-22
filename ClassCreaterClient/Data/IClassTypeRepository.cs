using ClassCreaterClient.Moldes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClassCreaterClient.Data
{
    public interface IClassTypeRepository
    {
        Task<List<ClassType>> GetClassTypes();
        Task<ClassType> GetClassType(int ID);
        Task AddClassType(ClassType classTypeToAdd);
        Task UpdateClassType(ClassType classTypeToUpdate);
        Task DeleteClassType(ClassType classTypeToDelete);
    }
}
