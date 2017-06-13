using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cinch;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// A simple ViewModel that is the ViewModel for the 
    /// <c>AddImageRatingPopup.xaml</c> popup window.
    /// 
    /// This example shows you you to show popups from
    /// we can use a Validating ViewModel, and also how
    /// to use the control focus from the ViewModel using the
    /// <c>TextBoxFocusBehavior</c>. 
    /// It also shows how to the <c>NumericTextBoxBehaviour</c> for
    /// the Rating TextBox.
    /// </summary>
    public class ImageRatingViewModel : ValidatingViewModelBase
    {
        #region Data
        private DataWrapper<Int32> imageRating;
        private IEnumerable<DataWrapperBase> cachedListOfDataWrappers;
        private static SimpleRule imageRatingRule;
        private IMessageBoxService messageBoxService;
        #endregion

        #region Ctor

        public ImageRatingViewModel(IMessageBoxService messageBoxService)
        {

            //setup services
            this.messageBoxService = messageBoxService;
            
            //Commands
            SaveImageRatingCommand = new SimpleCommand<Object, Object>(ExecuteSaveImageRatingCommand);


            #region Create DataWrappers

            ImageRating = new DataWrapper<Int32>(this, imageRatingChangeArgs);
            ImageRating.IsEditable = true;

            //fetch list of all DataWrappers, so they can be used again later without the
            //need for reflection
            cachedListOfDataWrappers =
                DataWrapperHelper.GetWrapperProperties<ImageRatingViewModel>(this);
            #endregion

            #region Create Validation Rules

            imageRating.AddRule(imageRatingRule);
            #endregion

        }


        static ImageRatingViewModel()
        {

            imageRatingRule = new SimpleRule("DataValue", "ImageRating must be between 1-5",
                      (Object domainObject)=>
                      {
                          DataWrapper<Int32> obj = (DataWrapper<Int32>)domainObject;
                          return obj.DataValue < 0 || obj.DataValue > 5;
                      });
        }
        #endregion

        #region Public Properties


        //commands
        public SimpleCommand<Object, Object> SaveImageRatingCommand { get; private set; }



        /// <summary>
        /// ImageRating
        /// </summary>
        static PropertyChangedEventArgs imageRatingChangeArgs =
            ObservableHelper.CreateArgs<ImageRatingViewModel>(x => x.ImageRating);

        public DataWrapper<Int32> ImageRating
        {
            get { return imageRating; }
            private set
            {
                imageRating = value;
                NotifyPropertyChanged(imageRatingChangeArgs);
            }
        }
        #endregion

        #region Private Methods

        private void ExecuteSaveImageRatingCommand(Object args)
        {
            
            if (IsValid)
            {
                CloseActivePopUpCommand.Execute(true);
            }
            else
            {
                NotifyPropertyChanged(isValidChangeArgs);
                RaiseFocusEvent("ImageRating");
                messageBoxService.ShowError("The Rating entered is invalid it must be between 1-5");
            }
        }

        #endregion

        #region Overrides
        /// <summary>
        /// Is the ViewModel Valid
        /// </summary>

        static PropertyChangedEventArgs isValidChangeArgs =
            ObservableHelper.CreateArgs<ImageRatingViewModel>(x => x.IsValid);

        public override bool IsValid
        {
            get
            {
                //return base.IsValid and use DataWrapperHelper, if you are
                //using DataWrappers
                return base.IsValid &&
                    DataWrapperHelper.AllValid(cachedListOfDataWrappers);
            }
               
        }
        #endregion
    }
}
