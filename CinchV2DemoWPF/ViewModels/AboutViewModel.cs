using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Composition;

using MEFedMVVM.ViewModelLocator;
using Cinch;

namespace CinchV2DemoWPF
{
    /// This is a workspace type ViewModel, which is created by some code in the <c>MainWindowViewModel</c>
    /// and the DataTemplate in the <c>MainWindow.xaml</c>
    /// 
    /// This ViewModel is also able to operate in design mode by using this ViewModel at runtime or by using
    /// data provided by a Design time ViewModel that inherits from this one.
    /// 
    /// This ViewModel also demonstrates EventToCommand where the XAML fires an Command in this ViewModel
    /// based on some RoutedEvent in the XAML.
    [ExportViewModel("AboutViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AboutViewModel : ViewModelBase
    {
        #region Data
        private string titleContents;
        private string bodyContents;
        public IUIVisualizerService uiVisualizer;
        #endregion

        #region Ctor
        [ImportingConstructor]
        public AboutViewModel(IUIVisualizerService uiVisualizer)
        {
            TitleContents = "A Brief Description Of Cinch V2";

            BodyContents = "This demo is a WPF demo of the Cinch V2 codebase\r\n\r\n" +
                        "This demo app is aimed at introducing the new Cinch V2 features and how to work with them. " +
                        "It does not go over old ground that was covered by Cinch V1 articles at codeproject.\r\n\r\n" +
                        "This was a deliberate decision, and I think it makes sense since a large part of the " +
                        "original Cinch V1 codebase remains unchanged. As such I really just wanted to focus on the new features.\r\n\r\n" +
                        "Fear not....most of the old features will work as they did before with minimal code changes, with the exception of UI services " +
                        "which have received a radical overhaul to reduce dependencies on any 3rd party. In fact Cinch V2 does not rely on any non framework Dlls at all...Neato\r\n\r\n" +
                        "This demo app covers the following:\r\n\r\n" +
                        ">> View to ViewModel resolution\r\n" +
                        ">> Design time data support\r\n" +
                        ">> UI Services and MEF injection\r\n" +
                        ">> Popups from the ViewModel\r\n" +
                        ">> Improved Workspaces\r\n" +
                        ">> MenuItems from the ViewModel\r\n" +
                        ">> EventToCommand\r\n" +
                        ">> VisualStateManger (2 flavours of this, VisualStateManager/ExtendedVisualStateManager)\r\n" +
                        ">> Focus from the ViewModel\r\n" +
                        ">> Attached behaviours\r\n" +
                        ">> Improved ICommand implementation\r\n\r\n" +
                        "As I say this demo focuses on the new Cinch V2 features, but lots of the old stuff will still work such as IEditableObject/Validating ViewModels/DataWrapper<T> and some of the core services are the same as they were in Cinch V1, so hopefully it should not be too bad for people to pick up.";

            this.uiVisualizer = uiVisualizer;
            AboutViewEventToVMFiredCommand = new SimpleCommand<Object, EventToCommandArgs>(ExecuteAboutViewEventToVMFiredCommand);

        }
        #endregion

        #region Public Properties

        /// <summary>
        /// An event to command fired command, have a look at the AboutView, and look for
        /// where this command is used to see how the View can fire Commands in the ViewModel
        /// passing in Parameters 
        /// </summary>
        public SimpleCommand<Object, EventToCommandArgs> AboutViewEventToVMFiredCommand { get; private set; }


        /// <summary>
        /// TitleContents
        /// </summary>
        static PropertyChangedEventArgs titleContentsArgs =
            ObservableHelper.CreateArgs<AboutViewModel>(x => x.TitleContents);

        public string TitleContents
        {
            get { return titleContents; }
            set
            {
                titleContents = value;
                NotifyPropertyChanged(titleContentsArgs);
            }
        }

        /// <summary>
        /// BodyContents
        /// </summary>
        static PropertyChangedEventArgs bodyContentsArgs =
            ObservableHelper.CreateArgs<AboutViewModel>(x => x.BodyContents);

        public string BodyContents
        {
            get { return bodyContents; }
            set
            {
                bodyContents = value;
                NotifyPropertyChanged(bodyContentsArgs);
            }
        }
        #endregion

        #region Private Methods
        private void ExecuteAboutViewEventToVMFiredCommand(EventToCommandArgs args)
        {
            AboutViewLinkRequestedPopupViewModel aboutViewLinkRequestedPopupViewModel =
                new AboutViewLinkRequestedPopupViewModel();
            switch ((String)args.CommandParameter)
            {
                case "Home":
                    aboutViewLinkRequestedPopupViewModel.NavigateTo = 
                        @"http://cinch.codeplex.com/";
                    break;
                case "Source":
                    aboutViewLinkRequestedPopupViewModel.NavigateTo =
                        @"http://cinch.codeplex.com/SourceControl/list/changesets";
                
                    break;
            }
            uiVisualizer.ShowDialog("AboutViewLinkRequestedPopup", aboutViewLinkRequestedPopupViewModel);
        }
        #endregion
    }
}
