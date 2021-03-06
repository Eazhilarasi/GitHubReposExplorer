﻿using Prism;
using Prism.Ioc;
using GitHubReposExplorer.ViewModels;
using GitHubReposExplorer.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using System.Net.Http;
using GitHubReposExplorer.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GitHubReposExplorer
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/RepositoryListPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            HttpClient httpClient = new HttpClient();
            containerRegistry.RegisterInstance(httpClient);
            containerRegistry.Register<IRestService, RestService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<RepositoryListPage>();
            containerRegistry.RegisterForNavigation<PullRequestsPage>();
        }
    }
}
