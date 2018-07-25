using GitHubReposExplorer.Models;
using GitHubReposExplorer.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Extended;

namespace GitHubReposExplorer.ViewModels
{
	public class RepositoryListPageViewModel : ViewModelBase
	{
        IRestService restApiService;
        private const int PageSize = 37;

        public InfiniteScrollCollection<Repository> Items { get; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
        public RepositoryListPageViewModel(IRestService restService,
                                            INavigationService navigationService)
                                            :base(navigationService)
        {
            this.restApiService = restService;

            Items = new InfiniteScrollCollection<Repository>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    // load the next page
                    var page = (Items.Count / PageSize) + 1;
                   

                    IList<Repository> repositoryList = await restApiService.GetAllRepositories(page, PageSize);
                    Debug.WriteLine(Items.Count);

                    IsBusy = false;

                    // return the items that need to be added
                    return repositoryList;
                },
                OnCanLoadMore = () =>
                {
                    return Items.Count < 1000;
                }
            };
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Task.Run(async () =>
            {
                IsBusy = true;
                IList<Repository> repositoryList = await restApiService.GetAllRepositories(1,PageSize);
                if (repositoryList != null && repositoryList.Count > 0)
                {
                    Items.AddRange(repositoryList);
                }
                IsBusy = false;
            });
            base.OnNavigatedTo(parameters);
        }


    }
}
