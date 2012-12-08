using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PunchlineDetector.Common;
using PunchlineDetector.Views;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace PunchlineDetector
{
    public class MainPageViewModel : BindableBase
    {
        private readonly List<StorageFile> _positiveFiles;
        private readonly List<StorageFile> _negativeFiles;
        private bool _happy;
        private bool _unhappy;
         
        private static readonly Random _random = new Random();
        private Popup settingsPopup;

        public bool Happy
        {
            get { return _happy; }
            set { SetProperty(ref _happy, value);
                Description = null;
            }
        }

        public bool Unhappy
        {
            get { return _unhappy; }
            set { SetProperty(ref _unhappy, value);
                Description = null;
            }
        }


        public MainPageViewModel()
        {
            _positiveFiles = new List<StorageFile>();
            _negativeFiles = new List<StorageFile>();
            Happy = Unhappy = true;
        }
        
        public async Task LoadFiles()
        {
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Sounds");

            var positiveFolder = await folder.GetFolderAsync("Positive");

            await AddFiles(positiveFolder, _positiveFiles);

            var negativefolder = await folder.GetFolderAsync("Negative");
            await AddFiles(negativefolder, _negativeFiles);

        }

        private async Task AddFiles(StorageFolder folder, List<StorageFile> files)
        {
            var storagefiles = await folder.GetFilesAsync();
            files.AddRange(storagefiles);
        }

        public async Task GetMediaElement()
        {
            var element = new MediaElement();
            var elm = GetRandomElement();
            var stream = await elm.OpenAsync(FileAccessMode.Read);
            element.SetSource(stream, elm.ContentType);
            await PlayElement(element);
        }

        private StorageFile GetRandomElement()
        {
            
            var list = GetRandomList();

            try
            {
                return list[_random.Next(0, list.Count)];
            }
            catch (Exception)
            {
                return list[0];
            }
        }

        private List<StorageFile> GetRandomList()
        {
            if (Happy != Unhappy)
            {
                if (Happy)
                    return _positiveFiles;
                
                if (Unhappy)
                    return _negativeFiles;
            }

            var list = new List<StorageFile>();

            while (list.Count <= 0)
            {
                list = _random.Next(0, 2) == 1 ? _positiveFiles : _negativeFiles;
            }
            return list;
        }

        private async Task PlayElement(MediaElement element)
        {
            await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, element.Play);
        }

        /// <summary>
        /// The setter of this property is only a facade to enable triggering notifyPropertychanged
        /// </summary>
        public string Description
        {
            get
            {
                if (Happy == Unhappy)
                    return "A random sound effect that is either amused or not amused is generated";
                if (Happy)
                    return "A random sound effect that is always amused (of course. It's you telling a funny thing)";
                if (Unhappy)
                    return "A random effect thas is always unamused (probably one of your friends trying to be funny)";

                return string.Empty;
            }
            
            private set {OnPropertyChanged(); }
        }

        public void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var handler = new UICommandInvokedHandler(onSettingsCommand);

            var generalCommand = new SettingsCommand("Feedback","Feedback", handler);
            args.Request.ApplicationCommands.Add(generalCommand);
        }

        private void onSettingsCommand(IUICommand command)
        {
            // Create a Popup window which will contain our flyout.
            settingsPopup = new Popup();
            settingsPopup.Closed += OnPopupClosed;
            Window.Current.Activated += OnWindowActivated;
            settingsPopup.IsLightDismissEnabled = true;
            settingsPopup.Width = 646;
            settingsPopup.Height = Window.Current.Bounds.Height;

            // Add the proper animation for the panel.
            settingsPopup.ChildTransitions = new TransitionCollection();
            settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                       EdgeTransitionLocation.Right :
                       EdgeTransitionLocation.Left
            });

            // Create a SettingsFlyout the same dimenssions as the Popup.
            Feedback mypane = new Feedback();
            mypane.Width = 646;
            mypane.Height = Window.Current.Bounds.Height;

            // Place the SettingsFlyout inside our Popup window.
            settingsPopup.Child = mypane;

            // Let's define the location of our Popup.
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (Window.Current.Bounds.Width - 646) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;
        }

        private void OnWindowActivated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                settingsPopup.IsOpen = false;
            }
        }

        private void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }
    }
}