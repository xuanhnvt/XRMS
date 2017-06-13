using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

using Csla;
using Csla.Rules.CommonRules;
using Csla.Serialization;

using Cinch;
using XRMS.Libraries.BaseObjects;

namespace XRMS.Business.Models
{
    [Serializable]
    public class Product : IdCodeNameBaseObject<Product>
    {
        #region Private Data Members

        //BitmapImage _image;

        ObservableCollection<RecipeItem> _recipes;

        #endregion // Private Data Members

        #region Constructors

        public Product() : base()
        {
            //_recipes = new List<RecipeItem>();
        }

        #endregion

        #region Public Properties

        public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(p => p.Description);
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }

        public static readonly PropertyInfo<int> UnitIdProperty = RegisterProperty<int>(p => p.UnitId);
        /// <summary>
        /// Gets or sets the unit id.
        /// </summary>
        /// <value>
        /// The unit id.
        /// </value>
        public int UnitId
        {
            get { return GetProperty(UnitIdProperty); }
            set { SetProperty(UnitIdProperty, value); }
        }

        public static readonly PropertyInfo<Unit> UnitProperty = RegisterProperty<Unit>(p => p.Unit);
        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        /*public Unit Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }*/
        public Unit Unit
        {
            get { return GetProperty(UnitProperty); }
            set { SetProperty(UnitProperty, value); }
        }

        public static readonly PropertyInfo<decimal> PriceProperty = RegisterProperty<decimal>(p => p.Price);
        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price
        {
            get { return GetProperty(PriceProperty); }
            set { SetProperty(PriceProperty, value); }
        }

        public static readonly PropertyInfo<bool> IsAlwaysReadyProperty = RegisterProperty<bool>(p => p.IsAlwaysReady);
        /// <summary>
        /// Gets or sets the flag "always ready": product is always available or need process in kitchen
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        public bool IsAlwaysReady
        {
            get { return GetProperty(IsAlwaysReadyProperty); }
            set { SetProperty(IsAlwaysReadyProperty, value); }
        }

        public static readonly PropertyInfo<int> GroupIdProperty = RegisterProperty<int>(p => p.GroupId);
        /// <summary>
        /// Gets or sets the group id.
        /// </summary>
        /// <value>
        /// The group id.
        /// </value>
        public int GroupId
        {
            get { return GetProperty(GroupIdProperty); }
            set { SetProperty(GroupIdProperty, value); }
        }

        public static readonly PropertyInfo<ProductGroup> GroupProperty = RegisterProperty<ProductGroup>(p => p.Group);
        /// <summary>
        /// Gets or sets the product group.
        /// </summary>
        /// <value>
        /// The product group.
        /// </value>
        /*public ProductGroup Group
        {
            get { return _group; }
            set { _group = value; }
        }*/
        public ProductGroup Group
        {
            get { return GetProperty(GroupProperty); }
            set { SetProperty(GroupProperty, value); }
        }

        /// <summary>
        /// Gets or sets the list of item in product recipe.
        /// </summary>
        /// <value>
        /// The recipe item list.
        /// </value>
        /*public ObservableCollection<RecipeItem> Recipes
        {
            get { return _recipes; }
            set { _recipes = value; NotifyPropertyChanged(() => Recipes); }
        }*/
        public RecipeItemList Recipes
        {
            get
            {
                if (!FieldManager.FieldExists(RecipesProperty))
                {
                    LoadProperty(RecipesProperty, DataPortal.CreateChild<RecipeItemList>());
                    OnPropertyChanged(RecipesProperty);
                }
                return GetProperty(RecipesProperty);
            }
            //set { SetProperty(RecipesProperty, value); }
        }
        public static readonly PropertyInfo<RecipeItemList> RecipesProperty = RegisterProperty<RecipeItemList>(p => p.Recipes);

        #endregion
    }
}
