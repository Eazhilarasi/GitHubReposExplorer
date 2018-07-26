using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubReposExplorer.Models
{
    public class PullRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Number { get; set; }
        public string State { get; set; }

        public User User { get; set; }
        public string Body { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}
