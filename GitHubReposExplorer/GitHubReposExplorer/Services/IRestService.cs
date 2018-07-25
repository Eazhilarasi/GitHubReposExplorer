using GitHubReposExplorer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GitHubReposExplorer.Services
{
    public interface IRestService
    {
        Task<IList<Repository>> GetAllRepositories(int page, 
                                                   int page_size, 
                                                   string language = "Javascript"
                                                   );
    }
}
