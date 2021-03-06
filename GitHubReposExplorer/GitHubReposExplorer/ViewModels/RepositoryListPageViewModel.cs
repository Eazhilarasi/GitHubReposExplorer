﻿using GitHubReposExplorer.Models;
using GitHubReposExplorer.Services;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
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
        IPageDialogService dialog;
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
        public Command GoToPullRequestPageCommand { get; set; }
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
                                            INavigationService navigationService,
                                            IPageDialogService dialogService)
                                            :base(navigationService,
                                                  dialogService)
        {
            this.restApiService = restService;
            this.dialog = dialogService;

            Items = new InfiniteScrollCollection<Repository>
            {
                OnLoadMore = async () =>
                {
                    IList<Repository> repositoryList = null;
                    try
                    {
                        if (CrossConnectivity.Current.IsConnected)
                        {

                            IsBusy = true;

                            // load the next page

                            var page = (Items.Count / PageSize) + 1;


                            repositoryList = await restApiService.GetAllRepositories(page, PageSize);
                            if (FullList != null && repositoryList != null)
                                FullList.AddRange(repositoryList);
                            Debug.WriteLine(Items.Count);

                            IsBusy = false;
                        }
                        else
                        {
                            DisplayAlert("Connectivity", "No Internet, try again later");
                        }
                    }
                    catch
                    {
                        DisplayAlert("Apologies", "Something went wrong");
                    }

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

            GoToPullRequestPageCommand = new Command<object>(async (p) =>
            {
                Repository r = (Repository)p;
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("repository", r);
                await navigationService.NavigateAsync("PullRequestsPage", parameters);
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
                    if(FullList!= null)
                        Items.AddRange(FullList);
                    return;
                }

                text = text.ToString().ToLower().Trim();

                if (text == null || text.Trim().Length == 0)
                {
                    Items.Clear();
                    if (FullList != null)
                        Items.AddRange(FullList);
                }
                else
                {
                    if (FullList != null && FullList.Count > 0)
                    {
                        IEnumerable<Repository> searchList = FullList
                           .Where(r => 
                           (!string.IsNullOrEmpty(r.Name) 
                           && r.Name.ToLower().Trim().Contains(text))
                           ||
                           (r.Owner != null 
                           && !string.IsNullOrEmpty(r.Owner.Login) 
                           && r.Owner.Login.ToLower().Trim().Contains(text)));
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
                DisplayAlert("Apologies", "Something went wrong");
            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            //If navigation is not a back navigation from pull requests page.
            if (parameters!= null && parameters.GetNavigationMode().Equals(NavigationMode.New))
            {
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                IsBusy = true;
                                IList<Repository> repositoryList = await restApiService.GetAllRepositories(1, PageSize);
                                if (repositoryList != null && repositoryList.Count > 0)
                                {
                                    Items.AddRange(repositoryList);
                                    FullList = new InfiniteScrollCollection<Repository>(repositoryList);
                                }
                                IsBusy = false;
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("Connectivity", "No Internet, try again later");
                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            DisplayAlert("Apologies", "Something went wrong");
                        }
                    });
                }
            }
            
            base.OnNavigatedTo(parameters);
        }


    }
}
