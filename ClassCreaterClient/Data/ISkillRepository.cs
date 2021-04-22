using ClassCreaterClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClassCreaterClient.Data
{
    public interface ISkillRepository
    {
        Task<List<Skill>> GetSkills();
        Task<Skill> GetSkill(int ID);
        Task<List<Skill>> GetSkillByClass(int ClassID);
        Task AddSkill(Skill skillToAdd);
        Task UpdateSkill(Skill skillToUpdate);
        Task DeleteSkill(Skill skillToDelete);
    }
}
