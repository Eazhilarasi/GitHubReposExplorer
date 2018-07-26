using GitHubReposExplorer.Models;
using GitHubReposExplorer.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubReposExplorer.ViewModels
{
	public class PullRequestsPageViewModel : ViewModelBase
	{
        private IRestService restApiService;
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
                Task.Run(async () =>
                {
                //IsBusy = true;
                    IList<PullRequest> pullRequestList = await restApiService.GetAllPullRequestsForRepo(r.Owner.Login, r.Name);
                    if (pullRequestList != null && pullRequestList.Count > 0)
                    {
                        
                    }
                   
                });
            }
            base.OnNavigatedTo(parameters);
        }
    }
}
