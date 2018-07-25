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
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace GitHubReposExplorer.ViewModels
{
	public class RepositoryListPageViewModel : ViewModelBase
	{
        IRestService restApiService;
        private const int PageSize = 37;

        private InfiniteScrollCollection<Repository> FullList;
        private InfiniteScrollCollection<Repository> _items;
        public InfiniteScrollCollection<Repository> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                RaisePropertyChanged("Items");
            }
        }

        public Command FilterCommand { get; set; }
        public string SearchText { get; set; }

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
                    FullList.AddRange(repositoryList);
                    Debug.WriteLine(Items.Count);

                    IsBusy = false;

                    // return the items that need to be added
                    return repositoryList;
                },
                OnCanLoadMore = () =>
                {
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        return false;
                    }
                    else
                    {
                        return Items.Count < 1000;
                    }
                }
            };

            FilterCommand = new Command(() =>
            {
                executeFilter();
            });
        }

        private void executeFilter()
        {
            try
            {


                string text = SearchText;
                if (text == null)
                {
                    Items.Clear();
                    Items.AddRange(FullList);
                    return;
                }

                text = text.ToString().ToLower().Trim();

                if (text == null || text.Trim().Length == 0)
                {
                    Items.Clear();
                    Items.AddRange(FullList);
                }
                else
                {
                    if (FullList != null && FullList.Count > 0)
                    {
                        IEnumerable<Repository> searchList = FullList
                           .Where(r => (!string.IsNullOrEmpty(r.Name) && r.Name.ToLower().Trim().Contains(text)) ||
                                       (r.Owner != null && !string.IsNullOrEmpty(r.Owner.Login) && r.Owner.Login.ToLower().Trim().Contains(text))
                                       );
                        if (searchList != null)
                        {
                            Items.Clear();
                            Items.AddRange(searchList);
                        }
                    }

                }
            }
            catch(Exception ex)
            {

            }
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
                    FullList = new InfiniteScrollCollection<Repository>(repositoryList);
                }
                IsBusy = false;
            });
            base.OnNavigatedTo(parameters);
        }


    }
}
