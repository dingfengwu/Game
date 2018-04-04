using Game.Facade.Mvc.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Game.Facade.Mvc.ModelBinding
{
    /// <summary>
    /// Represents model binder provider for the creating GameModelBinder
    /// </summary>
    public class GameModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Creates a game model binder based on passed context
        /// </summary>
        /// <param name="context">Model binder provider context</param>
        /// <returns>Model binder</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            var modelType = context.Metadata.ModelType;
            if (!typeof(BaseGameModel).IsAssignableFrom(modelType))
                return null;

            //use GameModelBinder as a ComplexTypeModelBinder for BaseGameModel
            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                //create binders for all model properties
                var propertyBinders = context.Metadata.Properties
                    .ToDictionary(modelProperty => modelProperty, modelProperty => context.CreateBinder(modelProperty));

                return new GameModelBinder(propertyBinders);
            }

            //or return null to further search for a suitable binder
            return null;
        }
    }
}
