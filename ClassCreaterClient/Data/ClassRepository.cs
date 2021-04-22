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
    public class ClassRepository: IClassRepository
    {
        HttpClient client = new HttpClient();
        public ClassRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Get Class
        public async Task<Class> GetClass(int ID)
        {
            var response = await client.GetAsync($"api/Classes/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Class Getclass = await response.Content.ReadAsAsync<Class>();
                return Getclass;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Get Classes
        public async Task<List<Class>> GetClasses()
        {
            var response = await client.GetAsync("api/Classes");
            if (response.IsSuccessStatusCode)
            {
                List<Class> @class = await response.Content.ReadAsAsync<List<Class>>();
                return @class;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Get ClassByClassType
        public async Task<List<Class>> GetClassesByTypes(int ClassTypeID)
        {
            var response = await client.GetAsync($"api/Classes/ByType/{ClassTypeID}");
            if (response.IsSuccessStatusCode)
            {
                List<Class> classes = await response.Content.ReadAsAsync<List<Class>>();
                return classes;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Add Class
        public async Task AddClass(Class classToAdd)
        {
            var response = await client.PostAsJsonAsync("api/Classes", classToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Update Class
        public async Task UpdateClass(Class classToUpdate)
        {
            var response = await client.PutAsJsonAsync($"api/Classes/{classToUpdate.ID}", classToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Delete Class
        public async Task DeleteClass(Class classToDelete)
        {
            var response = await client.DeleteAsync($"api/Classes/{classToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
}
