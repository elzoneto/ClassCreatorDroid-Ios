using ClassCreaterClient.Models;
using ClassCreaterClient.Ultilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClassCreaterClient.Data
{
    public class SkillRepository: ISkillRepository
    {
        HttpClient client = new HttpClient();
        public SkillRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Get Skill
        public async Task<Skill> GetSkill(int ID)
        {
            var response = await client.GetAsync($"api/Skills/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Skill skill = await response.Content.ReadAsAsync<Skill>();
                return skill;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Get Skills
        public async Task<List<Skill>> GetSkills()
        {
            var response = await client.GetAsync("api/Skills");
            if (response.IsSuccessStatusCode)
            {
                List<Skill> skills = await response.Content.ReadAsAsync<List<Skill>>();
                return skills;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Get CharacterByClass
        public async Task<List<Skill>> GetSkillByClass(int ClassID)
        {
            var response = await client.GetAsync($"api/Skills/ByClass/{ClassID}");
            if (response.IsSuccessStatusCode)
            {
                List<Skill> skill = await response.Content.ReadAsAsync<List<Skill>>();
                return skill;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Add Skill
        public async Task AddSkill(Skill skillToAdd)
        {
            var response = await client.PostAsJsonAsync("api/Skills", skillToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Update Character
        public async Task UpdateSkill(Skill skillToUpdate)
        {
            var response = await client.PutAsJsonAsync($"api/Skills/{skillToUpdate.ID}", skillToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Delete Character
        public async Task DeleteSkill(Skill skillToDelete)
        {
            var response = await client.DeleteAsync($"api/Skills/{skillToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
}
