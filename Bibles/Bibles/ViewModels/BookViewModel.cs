namespace Bibles.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Bibles.Domain;
    using Bibles.Helpers;
    using Models;
    using Services;
    using Xamarin.Forms;

    public class BookViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private Book book;
        private bool isRefreshing;
        private ContentResponse contentResponse;
        private ObservableCollection<Verse> verses;
        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public ObservableCollection<Verse> Verses
        {
            get { return this.verses; }
            set { SetValue(ref this.verses, value); }
        }
        #endregion

        #region Constructors
        public BookViewModel(Book book)
        {
            this.apiService = new ApiService();
            this.book = book;

            var mainViewModel = MainViewModel.GetInstance();
            this.LoadContent();
        }

        #endregion

        #region Methods
        public async void LoadContent(string search = null)
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var parameters = "";
            Domain.Response response = null;
            if (search == null)
            {
                parameters = string.Format(
                    "?bible={0}&reference={1}",
                    MainViewModel.GetInstance().SelectedModule,
                    this.book.Shortname);
                response = await this.apiService.Get<ContentResponse>(
                    "http://api.biblesupersearch.com",
                    "/api",
                    parameters);
            }
            else
            {
                parameters = string.Format(
                    "?bible={0}&reference={1}&search={2}",
                    MainViewModel.GetInstance().SelectedModule,
                    this.book.Shortname,
                    search);
                response = await this.apiService.Get<SearchResponse>(
                    "http://api.biblesupersearch.com",
                    "/api",
                    parameters);
            }


            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }


            if (search == null)
            {
                HandleBook(response);
            }
            else
            {
                HandleSearch(response);
            }
        }

        private void HandleSearch(Response response)
        {

            var search = (SearchResponse)response.Result;


            this.contentResponse = new ContentResponse();
            contentResponse.Contents = new List<Content>();

            foreach (var item in search.Contents)
            {
                contentResponse.Contents.Add(new Content
                {
                    BookId = item.BookId,
                    BookName = item.BookName,
                    BookShort = item.BookShort,
                    //BookRaw = item.BookRaw,
                    ChapterVerse = item.ChapterVerse,
                    ChapterVerseRaw = item.ChapterVerseRaw,
                    Verses = item.Verses,
                    VersesCount = item.VersesCount,
                    SingleVerse = item.SingleVerse,
                    //Nav = item.Nav
                });
            }

            //this.contentResponse = (SearchResponse)response.Result;
            this.IsRefreshing = false;

            List<Verse> myVerses = new List<Verse>();
            foreach (var content in contentResponse.Contents)
            {

                var contentResult = content;

                var type = typeof(Verses);
                var properties = type.GetRuntimeFields();
                Bible bible = null;

                foreach (var property in properties)
                {
                    bible = (Bible)property.GetValue(contentResult.Verses);
                    if (bible != null)
                    {
                        break;
                    }
                }

                if (bible == null)
                {
                    return;
                }

                type = typeof(Bible);
                properties = type.GetRuntimeFields();
                Dictionary<string, Verse> chapter = null;

                foreach (var property in properties)
                {
                    if (property.Name.StartsWith("<Chapter"))
                    {
                        chapter = (Dictionary<string, Verse>)property.GetValue(bible);

                        if (chapter != null)
                        {
                            break;
                        }
                    }
                }

                myVerses.AddRange(chapter.Select(v => new Verse
                {
                    Book = v.Value.Book,
                    Chapter = v.Value.Chapter,
                    Id = v.Value.Id,
                    Italics = v.Value.Italics,
                    Text = v.Value.Text,
                    VerseNumber = v.Value.VerseNumber,
                }).ToList());

            }
            this.Verses = new ObservableCollection<Verse>(myVerses);

        }

        private void HandleBook(Response response)
        {

            this.contentResponse = (ContentResponse)response.Result;
            this.IsRefreshing = false;

            var contentResult = contentResponse.Contents[0];

            var type = typeof(Verses);
            var properties = type.GetRuntimeFields();
            Bible bible = null;

            foreach (var property in properties)
            {
                bible = (Bible)property.GetValue(contentResult.Verses);
                if (bible != null)
                {
                    break;
                }
            }

            if (bible == null)
            {
                return;
            }

            type = typeof(Bible);
            properties = type.GetRuntimeFields();
            Dictionary<string, Verse> chapter = null;

            foreach (var property in properties)
            {
                if (property.Name.StartsWith("<Chapter"))
                {
                    chapter = (Dictionary<string, Verse>)property.GetValue(bible);

                    if (chapter != null)
                    {
                        break;
                    }
                }
            }

            var myVerses = chapter.Select(v => new Verse
            {
                Book = v.Value.Book,
                Chapter = v.Value.Chapter,
                Id = v.Value.Id,
                Italics = v.Value.Italics,
                Text = v.Value.Text,
                VerseNumber = v.Value.VerseNumber,
            });

            this.Verses = new ObservableCollection<Verse>(myVerses);
        }
        #endregion
    }
}