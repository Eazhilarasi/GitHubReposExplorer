using GitHubReposExplorer.Models;
using GitHubReposExplorer.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubReposExplorer.ViewModels
{
	public class RepositoryListPageViewModel : ViewModelBase
	{
        IRestService restApiService;
        
        private ObservableCollection<Repository> repositoryCollection;
        public ObservableCollection<Repository> RepositoryCollection
        {
            get { return repositoryCollection; }
            set
            {
                repositoryCollection = value;
                RaisePropertyChanged("RepositoryCollection");
            }
        }
        public RepositoryListPageViewModel(IRestService restService,
                                            INavigationService navigationService)
                                            :base(navigationService)
        {
            this.restApiService = restService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Task.Run(async () =>
            {
                IList<Repository> repositoryList = await restApiService.GetAllRepositories();
                if (repositoryList != null && repositoryList.Count > 0)
                {
                    RepositoryCollection = new ObservableCollection<Repository>(repositoryList.ToList());
                }
            });
            base.OnNavigatedTo(parameters);
        }


    }
}
