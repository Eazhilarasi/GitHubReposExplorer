using GitHubReposExplorer.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GitHubReposExplorer.ViewModels
{
	public class RepositoryListPageViewModel : BindableBase
	{
        IRestService restApiService;
        public RepositoryListPageViewModel(IRestService restService)
        {
            this.restApiService = restService;
            restApiService.GetAllRepositories("Javascript");
        }
	}
}
