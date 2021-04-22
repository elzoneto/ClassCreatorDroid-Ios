using ClassCreaterClient.Moldes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClassCreaterClient.Data
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetCharacters();
        Task<Character> GetCharacter(int ID);
        Task<List<Character>> GetCharactersByClass(int ClassID);
        Task AddCharacter(Character characterToAdd);
        Task UpdateCharacter(Character characterToUpdate);
        Task DeleteCharacter(Character characterToDelete);
    }
}
