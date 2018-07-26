using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GitHubReposExplorer.Models;
using Newtonsoft.Json;

namespace GitHubReposExplorer.Services
{
    public class RestService : IRestService
    {
        string baseURL = "https://api.github.com";
        HttpClient client;

        public RestService(HttpClient httpClient)
        {
            this.client = httpClient;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IList<Repository>> GetAllRepositories(int page, int page_size, string language = "JavaScript")
        {
            string url = string.Format("/search/repositories?q=language:{0}&sort=stars&page={1}&per_page={2}", language,page,page_size);
            var records = await GetAsync<Contract.RepositoryContract>(url);
       
            if (records != null)
                return records.Items;

            return null;
        }

        private HttpRequestMessage GetRequest(HttpMethod method, string url)
        {
            var req = new HttpRequestMessage(method,url);
            //Mandatory header(User-Agent) to make any successful Git Hub API request
            req.Headers.Add("User-Agent", "Eazhilarasi");
            return req;
        }

        private async Task<T> GetAsync<T>(string url)
        {
            try
            {
                string fullUrl = string.Format("{0}{1}", baseURL, url);
                var req = GetRequest(HttpMethod.Get, fullUrl);

                var result = await client.SendAsync(req);


                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var str = await result.Content.ReadAsStringAsync();
                    var records = JsonConvert.DeserializeObject<T>(str);
                    return records;
                }
            }
            catch(Exception ex)
            {

            }

            return default(T);
        }

        public async Task<IList<PullRequest>> GetAllPullRequestsForRepo(string repo, string username)
        {
            string url = string.Format("/repos/{0}/{1}/pulls?state=all", username, repo);
            var records = await GetAsync<IList<PullRequest>>(url);

            if (records != null)
                return records;

            return null;
        }
    }
}
