using ClassCreaterClient.Moldes;
using ClassCreaterClient.Ultilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClassCreaterClient.Data
{
    public class CharacterRepository: ICharacterRepository
    {
        HttpClient client = new HttpClient();
        public CharacterRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Get Character
        public async Task<Character> GetCharacter(int ID)
        {
            var response = await client.GetAsync($"api/Characters/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Character character = await response.Content.ReadAsAsync<Character>();
                character.Race.ToString();
                return character;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Get Characters
        public async Task<List<Character>> GetCharacters()
        {
            var response = await client.GetAsync("api/Characters");
            if (response.IsSuccessStatusCode)
            {
                List<Character> characters = await response.Content.ReadAsAsync<List<Character>>();
                return characters;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Get CharacterByClass
        public async Task<List<Character>> GetCharactersByClass(int ClassID)
        {
            var response = await client.GetAsync($"api/Characters/ByClass/{ClassID}");
            if (response.IsSuccessStatusCode)
            {
                List<Character> character = await response.Content.ReadAsAsync<List<Character>>();
                return character;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Add Character
        public async Task AddCharacter(Character characterToAdd)
        {
            var response = await client.PostAsJsonAsync("api/Characters", characterToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Update Character
        public async Task UpdateCharacter(Character characterToUpdate)
        {
            var response = await client.PutAsJsonAsync($"api/Characters/{characterToUpdate.ID}", characterToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Delete Character
        public async Task DeleteCharacter(Character characterToDelete)
        {
            var response = await client.DeleteAsync($"api/Characters/{characterToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
        
    }
}
