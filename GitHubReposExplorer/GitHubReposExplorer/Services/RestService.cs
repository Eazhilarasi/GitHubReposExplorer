using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GitHubReposExplorer.Models;

namespace GitHubReposExplorer.Services
{
    public class RestService : IRestService
    {
        string baseURL = "";
        HttpClient client;

        public RestService(HttpClient httpClient)
        {
            this.client = httpClient;
        }

        public Task<IList<Repository>> GetAllRepositories(string language)
        {
            return null;
        }
    }
}
