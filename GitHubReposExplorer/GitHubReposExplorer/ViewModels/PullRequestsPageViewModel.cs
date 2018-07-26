using GitHubReposExplorer.Models;
using GitHubReposExplorer.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubReposExplorer.ViewModels
{
	public class PullRequestsPageViewModel : ViewModelBase
	{
        private IRestService restApiService;

        private IList<PullRequest> pullRequests;
        public IList<PullRequest> PullReqList
        {
            get
            {
                return pullRequests;
            }
            set
            {
                pullRequests = value;
                RaisePropertyChanged("PullReqList");
            }
        }
        private string openPullReqText { get; set; }
        public string OpenPullReqText
        {
            get
            {
                return openPullReqText;
            }
            set
            {
                openPullReqText = value;
                RaisePropertyChanged("OpenPullReqText");
            }
        }

        public Repository Repository { get; set; }
        public PullRequestsPageViewModel(IRestService restService,
                                         INavigationService navigationService)
                                         :base(navigationService)
        {
            this.restApiService = restService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters != null && parameters.ContainsKey("repository"))
            {
                Repository r = (Repository)parameters["repository"];
                Repository = r;
                Task.Run(async () =>
                {
                    //IsBusy = true;
                    IList<PullRequest> pullRequests = await restApiService.GetAllPullRequestsForRepo(r.Owner.Login, r.Name);
                    if (pullRequests != null && pullRequests.Count > 0)
                    {
                        PullReqList = pullRequests.Where(p => p.State.Equals("open")).ToList();
                        if (PullReqList != null && PullReqList.Count > 0)
                        {
                            OpenPullReqText = string.Format("{0} opened/{1} closed", PullReqList.Count, pullRequests.Count);
                            Debug.WriteLine(OpenPullReqText);
                        }
                        else
                        {
                            OpenPullReqText = "No open pull requests";
                        }
                    }
                   
                });
            }
            base.OnNavigatedTo(parameters);
        }
    }
}
