using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GitHubReposExplorer.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService DialogService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService,
                             IPageDialogService dialogService)
        {
            NavigationService = navigationService;
            DialogService = dialogService;
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            
        }

        public virtual void Destroy()
        {
            
        }

        protected void DisplayAlert(string title, string message, string button = "OK")
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DialogService.DisplayAlertAsync(title, message, button);
            });
        }
    }
}
