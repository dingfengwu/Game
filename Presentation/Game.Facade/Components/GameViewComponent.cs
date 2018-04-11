using Game.Base.Infrastructure;
using Game.Facade.Events;
using Game.Facade.Mvc.Models;
using Game.Services.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Collections.Generic;

namespace Game.Facade.Components
{
    /// <summary>
    /// Base class for ViewComponent in gameCommerce
    /// </summary>
    public abstract class GameViewComponent : ViewComponent
    {
        private void PublishModelPrepared<TModel>(TModel model)
        {
            //Components are not part of the controller life cycle.
            //Hence, we could no longer use Action Filters to intercept the Models being returned
            //as we do in the /Game.Facade/Mvc/Filters/PublishModelEventsAttribute.cs for controllers

            //model prepared event
            if (model is BaseGameModel)
            {
                var eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();

                //we publish the ModelPrepared event for all models as the BaseGameModel, 
                //so you need to implement IConsumer<ModelPrepared<BaseGameModel>> interface to handle this event
                eventPublisher.ModelPrepared(model as BaseGameModel);
            }

            if (model is IEnumerable<BaseGameModel> modelCollection)
            {
                var eventPublisher = EngineContext.Current.Resolve<IEventPublisher>();

                //we publish the ModelPrepared event for collection as the IEnumerable<BaseGameModel>, 
                //so you need to implement IConsumer<ModelPrepared<IEnumerable<BaseGameModel>>> interface to handle this event
                eventPublisher.ModelPrepared(modelCollection);
            }
        }
        /// <summary>
        /// Returns a result which will render the partial view with name <paramref name="viewName"/>.
        /// </summary>
        /// <param name="viewName">The name of the partial view to render.</param>
        /// <param name="model">The model object for the view.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public new ViewViewComponentResult View<TModel>(string viewName, TModel model)
        {
            PublishModelPrepared(model);

            //invoke the base method
            return base.View<TModel>(viewName, model);
        }

        /// <summary>
        /// Returns a result which will render the partial view
        /// </summary>
        /// <param name="model">The model object for the view.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public new ViewViewComponentResult View<TModel>(TModel model)
        {
            PublishModelPrepared(model);

            //invoke the base method
            return base.View<TModel>(model);
        }

        /// <summary>
        ///  Returns a result which will render the partial view with name viewName
        /// </summary>
        /// <param name="viewName">The name of the partial view to render.</param>
        /// <returns>A <see cref="ViewViewComponentResult"/>.</returns>
        public new ViewViewComponentResult View(string viewName)
        {
            //invoke the base method
            return base.View(viewName);
        }
    }
}