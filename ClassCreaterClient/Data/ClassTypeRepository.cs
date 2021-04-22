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
    public class ClassTypeRepository: IClassTypeRepository
    {
        HttpClient client = new HttpClient();
        public ClassTypeRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Get ClassType
        public async Task<ClassType> GetClassType(int ID)
        {
            var response = await client.GetAsync($"api/Types/{ID}");
            if (response.IsSuccessStatusCode)
            {
                ClassType classType = await response.Content.ReadAsAsync<ClassType>();
                return classType;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Get ClassTypes
        public async Task<List<ClassType>> GetClassTypes()
        {
            var response = await client.GetAsync("api/Types");
            if (response.IsSuccessStatusCode)
            {
                List<ClassType> classTypes = await response.Content.ReadAsAsync<List<ClassType>>();
                return classTypes;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Add ClassType
        public async Task AddClassType(ClassType classTypeToAdd)
        {
            var response = await client.PostAsJsonAsync("api/Types", classTypeToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Update ClassType
        public async Task UpdateClassType(ClassType classTypeToUpdate)
        {
            var response = await client.PutAsJsonAsync($"api/Types/{classTypeToUpdate.ID}", classTypeToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        // Delete ClassType
        public async Task DeleteClassType(ClassType classTypeToDelete)
        {
            var response = await client.DeleteAsync($"api/Types/{classTypeToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }        
    }
}
