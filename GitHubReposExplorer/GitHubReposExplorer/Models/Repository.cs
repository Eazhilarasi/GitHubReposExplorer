using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubReposExplorer.Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Full_Name { get; set; }
        public string Description { get; set; }
        public Author Owner { get; set; }
        public int Size { get; set; }
        public int Stargazers_Count { get; set; }
        public int Forks_Count { get; set; }
        public int Forks { get; set; }
    }
}
