using GitHubReposExplorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubReposExplorer.Contract
{
    public class RepositoryContract
    {
        public int total_count { get; set; }
        public bool incomplete_results { get; set; }
        public List<Repository> Items { get; set; }

    }
}
