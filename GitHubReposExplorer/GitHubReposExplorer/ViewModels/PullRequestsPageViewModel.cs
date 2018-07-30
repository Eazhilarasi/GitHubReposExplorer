using GitHubReposExplorer.Models;
using GitHubReposExplorer.Services;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitHubReposExplorer.ViewModels
{
	public class PullRequestsPageViewModel : ViewModelBase
	{
        private IRestService restApiService;
        private IPageDialogService dialog;

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

        public Command OpenBrowserCommand { get; set; }

        public PullRequestsPageViewModel(IRestService restService,
                                         INavigationService navigationService,
                                         IPageDialogService dialogService)
                                         :base(navigationService,
                                               dialogService)
        {
            this.restApiService = restService;
            this.dialog = dialogService;

            OpenBrowserCommand = new Command<object>((p) =>
            {
                PullRequest pullReq = (PullRequest)p;
                if(pullReq!= null && !string.IsNullOrEmpty(pullReq.Html_Url))
                    Device.OpenUri(new Uri(pullReq.Html_Url));
            });
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (parameters != null && parameters.ContainsKey("repository"))
                {
                    Repository r = (Repository)parameters["repository"];
                    if (r != null)
                    {
                        Title = r.Name;
                        Task.Run(async () =>
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                IsBusy = true;
                                IList<PullRequest> pullRequests = await restApiService.GetAllPullRequestsForRepo(r.Owner.Login, r.Name);
                                IsBusy = false;
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
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("Connectivity", "No Internet, try again later");
                                });
                            }
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("Apologies", "Something went wrong");
            }
            base.OnNavigatedTo(parameters);
        }
    }
}
