namespace Bibles.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Models;
    using Helpers;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Bibles.Views;

    public class MainViewModel : BaseViewModel
    {
        #region Attibrutes
        private UserLocal user;
        #endregion

        #region Properties
        public List<string> BiblesList
        {
            get;
            set;
        }

        private string search;
        public string Search
        {
            get { return this.search; }
            set { SetValue(ref this.search, value); }
        }

        public string Token { get; set; }

        public string TokenType { get; set; }

        public ObservableCollection<MenuItemViewModel> Menus
        {
            get;
            set;
        }

        public UserLocal User
        {
            get { return this.user; }
            set { SetValue(ref this.user, value); }
        }

        #endregion

        #region ViewModels
        public LoginViewModel Login
        {
            get;
            set;
        }

        public BiblesViewModel Bibles
        {
            get;
            set;
        }

        public BibleViewModel Bible
        {
            get;
            set;
        }

        public BookViewModel Book
        {
            get;
            set;
        }
        #endregion

        #region Properties
        public string SelectedModule
        {
            get;
            set;
        }

        public RegisterViewModel Register
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
            this.LoadMenu();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            this.Menus = new ObservableCollection<MenuItemViewModel>();

            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "BiblesPage",
                Title = Languages.Bibles,
            });
            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.LogOut,
            });

        }
        #endregion

        #region Commands

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(ExecuteSearch);
            }
        }

        private async void ExecuteSearch()
        {
            var mainvm = MainViewModel.GetInstance();
            if (App.Navigator.CurrentPage.Title.Equals(Languages.Bibles))
            {
                mainvm.Book = new BookViewModel();
                mainvm.Book.LoadContent(false, mainvm.Search);
            }
            else
            {
                mainvm.Book.LoadContent(true, mainvm.Search);
            }
            await App.Navigator.PushAsync(new BookPage());
            App.Master.IsPresented = false;
        }
        #endregion
    }
}