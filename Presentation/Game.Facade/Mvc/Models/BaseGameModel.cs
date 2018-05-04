using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Game.Facade.Mvc.Models
{
    /// <summary>
    /// Represents base gameCommerce model
    /// </summary>
    public partial class BaseGameModel
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public BaseGameModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform additional actions for binding the model
        /// </summary>
        /// <param name="bindingContext">Model binding context</param>
        /// <remarks>Developers can override this method in custom partial classes in order to add some custom model binding</remarks>
        public virtual void BindModel(ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// Perform additional actions for the model initialization
        /// </summary>
        /// <remarks>Developers can override this method in custom partial classes in order to add some custom initialization code to constructors</remarks>
        protected virtual void PostInitialize()
        {            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets property to store any custom values for models 
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }

        #endregion

    }

    /// <summary>
    /// Represents base gameCommerce entity model
    /// </summary>
    public partial class BaseGameEntityModel : BaseGameModel
    {
        /// <summary>
        /// Gets or sets model identifier
        /// </summary>
        public virtual int Id { get; set; }
    }
}
