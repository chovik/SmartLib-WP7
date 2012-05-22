using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SmartLib.ViewModels;
using SmartLib.Helpers;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Coding4Fun.Phone.Controls;
using SmartLib.DataManagers;
using SmartLib.Models;
using System.Collections;

namespace SmartLib
{
    public partial class App : Application
    {
        public TimeSpan TimeDifference { get; set; }

        public DateTime CurrentTime
        {
            get
            {
                return DateTime.Now + TimeDifference;
            }
        }

        private bool loggedIn = false;

        public bool LoggedIn
        {
            get { return loggedIn; }
            set 
            { 
                loggedIn = value; 
                OnLoginChanged(value);
            }
        }


        private string loggedUco = null;

        public string LoggedUco
        {
            get { return loggedUco; }
            set { loggedUco = value; }
        }

        public event EventHandler<LoginChangedEventArgs> LoginChanged;

        protected virtual void OnLoginChanged(bool isLoggedIn)
        {
            var errorEvent = LoginChanged;
            if (errorEvent != null)
                errorEvent(this, new LoginChangedEventArgs(isLoggedIn));
        }

        public static App CurrentApplication 
        {
            get
            {
                return App.Current as App;
            }
        }

        public const string SERVER_ADDRESS = "web-smartlib.rhcloud.com";

        private IMessageService messageService = new ToastMessageService();

        public IMessageService MessageService
        {
            get { return messageService; }
        }

        private RequestManager requestManager = new RequestManager();

        public RequestManager RequestManager
        {
            get { return requestManager; }
        }

        private BookRequestManager bookRequestManager;

        public BookRequestManager BookRequestManager
        {
            get { return bookRequestManager; }
        }

        private ReviewRequestManager reviewRequestManager;

        public ReviewRequestManager ReviewRequestManager
        {
            get { return reviewRequestManager; }
        }

        private UserReuqestManager userRequestManager;

        public UserReuqestManager UserRequestManager
        {
            get { return userRequestManager; }
        }
        

#region persistent data

        private ObservableCollection<BookViewModel> history = new ObservableCollection<BookViewModel>();
        public ObservableCollection<BookViewModel> History
        {
            get
            {
                return history;
            }
        }

        private ObservableCollection<BookViewModel> favouriteBooks = new ObservableCollection<BookViewModel>();
        public ObservableCollection<BookViewModel> FavouriteBooks
        {
            get
            {
                return favouriteBooks;
            }
        }

        public const string TITLE_KEY = "title";
        public const string AUTHOR_KEY = "author";
        public const string ISBN_KEY = "isbn";
        private Dictionary<string, ObservableCollection<string>> searchedStrings = new Dictionary<string, ObservableCollection<string>>()
        {
            { TITLE_KEY, new ObservableCollection<string>() },
            { AUTHOR_KEY, new ObservableCollection<string>() },
            { ISBN_KEY, new ObservableCollection<string>() },
        };

        public Dictionary<string, ObservableCollection<string>> SearchedStrings
        {
            get
            {
                return searchedStrings;
            }
        }

        private int resultsCount;

        public int ResultsCount
        {
            get { return resultsCount; }
            set { resultsCount = value; }
        }

        private string uco;

        public string UCO
        {
            get { return uco; }
            set { uco = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        
#endregion

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            TimeDifference = TimeSpan.FromMilliseconds(0);
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            ThemeManager.ToDarkTheme();

            //InitializeStyles();

            bookRequestManager = new BookRequestManager(requestManager, SERVER_ADDRESS);
            reviewRequestManager = new ReviewRequestManager(requestManager, SERVER_ADDRESS);
            userRequestManager = new UserReuqestManager(requestManager, SERVER_ADDRESS);

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private async void Application_Launching(object sender, LaunchingEventArgs e)
        {
            this.LoadPersistenData();
            this.LoggedIn = false;
            if (ValidateLoginInputs(UCO, Password, false))
                Login(UCO, Password, null);

            //string json = "[{\"sysno\":\"000057620\",\"title\":\"Vlastivěda moravská.\",\"coverUrl\":null,\"authors\":[{\"type\":\"aut\",\"name\":\"Malý, Josef\"},{\"type\":\"oth\",\"name\":\"Kratochvíl, Augustin\"}],\"averageRating\":4,\"ratingCount\":4},{\"sysno\":\"000360302\",\"title\":\"JavaScript :kompletní průvodce\",\"coverUrl\":\"http://www.obalkyknih.cz/file/cover/11377/medium\",\"authors\":[{\"type\":\"aut\",\"name\":\"Flanagan, David\"}],\"averageRating\":4,\"ratingCount\":2},{\"sysno\":\"000056063\",\"title\":\"Latinsko-český slovníček římského práva :(vybrané pojmy a termíny)\",\"coverUrl\":null,\"authors\":[{\"type\":\"aut\",\"name\":\"Skřejpek, Michal\"}],\"averageRating\":3,\"ratingCount\":3},{\"sysno\":\"000360300\",\"title\":\"Java :servlety a stránky JSP\",\"coverUrl\":\"http://www.obalkyknih.cz/file/cover/623132/medium\",\"authors\":[{\"type\":\"aut\",\"name\":\"Hall, Marty\"}],\"averageRating\":4,\"ratingCount\":2},{\"sysno\":\"000355404\",\"title\":\"Geologické mapování na části listu území M-33-107-A-a (Ivanovice na Hané)\",\"coverUrl\":null,\"authors\":[{\"type\":\"dis\",\"name\":\"Wolf, Igor\"},{\"type\":\"ths\",\"name\":\"Krystek, Ivan\"}],\"averageRating\":1,\"ratingCount\":2},{\"sysno\":\"000596458\",\"title\":\"Festschrift Harry Rosenbusch :gewidmet von seinen Schülern zum 70. Geburtstag 24. Juni 1906\",\"coverUrl\":null,\"authors\":[{\"type\":\"hnr\",\"name\":\"Rosenbusch, H\"},{\"type\":\"aui\",\"name\":\"Wülfing, E. A\"}],\"averageRating\":2,\"ratingCount\":2},{\"sysno\":\"000060941\",\"title\":\"Magnae Moraviae fontes historici.\",\"coverUrl\":null,\"authors\":[{\"type\":\"edt\",\"name\":\"Bartoňková, Dagmar\"}],\"averageRating\":3,\"ratingCount\":4},{\"sysno\":\"000049621\",\"title\":\"Kámen a hlína jako ekofakt a artefakt ve vývoji životního prostředí\",\"coverUrl\":null,\"authors\":[{\"type\":\"aut\",\"name\":\"Malina, Jaroslav\"}],\"averageRating\":3,\"ratingCount\":2},{\"sysno\":\"000022658\",\"title\":\"A dictionary of foreign words and phrases in current English\",\"coverUrl\":null,\"authors\":[{\"type\":\"aut\",\"name\":\"Bliss, A. J\"}],\"averageRating\":4,\"ratingCount\":1}]";
            //var c = RequestManager.ParseDataAsync<IEnumerable<Book>>(json);
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if(!e.IsApplicationInstancePreserved)
                this.LoadPersistenData();
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            this.SavePersistenData();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
            this.SavePersistenData();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            Debug.WriteLine(e.Exception);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.ExceptionObject);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();

            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        protected static Color GetColorFromHexString(string s)
        {
            // remove artifacts
            s = s.Trim().TrimStart('#');

            // only 8 (with alpha channel) or 6 symbols are allowed
            if (s.Length != 8 && s.Length != 6)
                throw new ArgumentException("Unknown string format!");

            int startParseIndex = 0;
            bool alphaChannelExists = s.Length == 8; // check if alpha canal exists           

            // read alpha channel value
            byte a = 255;
            if (alphaChannelExists)
            {
                a = System.Convert.ToByte(s.Substring(0, 2), 16);
                startParseIndex += 2;
            }

            // read r value
            byte r = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);
            startParseIndex += 2;
            // read g value
            byte g = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);
            startParseIndex += 2;
            // read b value
            byte b = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }

        private void InitializeStyles()
        {
            //var dictionaries = new ResourceDictionary();
            //string source = String.Format("/Styles/Colors.xaml");
            //var themeStyles = new ResourceDictionary { Source = new Uri(source, UriKind.Relative) };
            //dictionaries.MergedDictionaries.Add(themeStyles);


            //ResourceDictionary appResources = App.Current.Resources;
            //foreach (DictionaryEntry entry in dictionaries.MergedDictionaries[0])
            //{
            //    SolidColorBrush colorBrush = entry.Value as SolidColorBrush;
            //    SolidColorBrush existingBrush = appResources[entry.Key] as SolidColorBrush;
            //    if (existingBrush != null && colorBrush != null)
            //    {
            //        existingBrush.Color = colorBrush.Color;
            //    }
            //}

            (App.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush).Color = Colors.Blue;
            //87 percent Black - #DE000000
            (App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush).Color = (Color)GetColorFromHexString("#1e1f1c");
            //100 percent White - #FFFFFFFF
            // (Color)GetColorFromHexString("#f5fce6");
        } 

        private IDataStorage persistentDataStorage = new PersistentDataStorage();

        public IDataStorage PersistentDataStorage
        {
            get
            {
                return persistentDataStorage;
            }
        }

        private void SavePersistenData()
        {
            persistentDataStorage.Backup("history", this.History); //strings dat do constant
            persistentDataStorage.Backup("favourites", this.FavouriteBooks);
            persistentDataStorage.Backup("searched", this.SearchedStrings);
            persistentDataStorage.Backup("uco", this.UCO);
            persistentDataStorage.Backup("password", this.Password);
            persistentDataStorage.Backup("resultsCount", this.ResultsCount);
        }

        private void LoadPersistenData()
        {
            this.history = persistentDataStorage.Restore<ObservableCollection<BookViewModel>>("history")
                ?? new ObservableCollection<BookViewModel>();

            this.favouriteBooks = persistentDataStorage.Restore<ObservableCollection<BookViewModel>>("favourites")
                ?? new ObservableCollection<BookViewModel>();

            this.searchedStrings = persistentDataStorage.Restore<Dictionary<string, ObservableCollection<string>>>("searched")
                ?? searchedStrings;

            this.UCO = persistentDataStorage.Restore<string>("uco")
                ?? this.UCO;

            this.Password = persistentDataStorage.Restore<string>("password")
                ?? this.Password;

            var resultsCountTemp = persistentDataStorage.Restore<int>("resultsCount");
            this.ResultsCount = resultsCountTemp > 0 ? resultsCountTemp : 10;
        }

        public static bool ValidateLoginInputs(string uco, string password, bool showMessages = true)
        {
            bool isValid = true;

            List<string> invalidInputs = new List<string>();

            if (string.IsNullOrWhiteSpace(uco))
            {
                isValid = false;
                invalidInputs.Add("uco");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
                invalidInputs.Add("password");
            }

            if (!isValid
                && showMessages)
            {
                var msg = string.Format("Following fields are required:\n{0}\nPlease fill them.",
                            string.Join("\n", invalidInputs));

                App.CurrentApplication.MessageService.ShowWarningMessage(msg, "Login");
            }

            return isValid;
        }

        public async void Login(string uco, string password, string nextPageURI)
        {
            if (uco == null)
                throw new ArgumentNullException("uco");
            if (password == null)
                throw new ArgumentNullException("password");
            try
            {
                HttpStatusCode statusCode = await App.CurrentApplication.UserRequestManager.Login(uco, password);

                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        this.MessageService.ShowSuccessMessage("You were successfuly log on.", "Login");
                        App.CurrentApplication.LoggedIn = true;
                        this.LoggedUco = uco;
                        var root = this.RootVisual as Frame;

                        if (nextPageURI == null)
                        {
                            if (root.CanGoBack)
                                root.GoBack();
                        }
                        else
                        {
                            root.Navigate(new Uri(nextPageURI, UriKind.Relative));
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        this.MessageService.ShowErrorMessage("Wrong password.", "Login");
                        break;
                    default:
                        Debug.WriteLine("Unxpected Status Code '{0}'.", statusCode);
                        break;
                }
            }
            catch(WebException webEx)
            {
                MessageService.ShowErrorMessage("No connection.", "Login Error");
            }
        }
    }
}