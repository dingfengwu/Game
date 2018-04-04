using FluentValidation;
using Game.Base.Infrastructure;
using Game.Data;
using Game.Services.Localization;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Game.Facade.Validators
{
    /// <summary>
    /// Base class for validators
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseGameValidator<T> : AbstractValidator<T> where T : class
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected BaseGameValidator()
        {
            PostInitialize();
        }

        /// <summary>
        /// Developers can override this method in custom partial classes
        /// in order to add some custom initialization code to constructors
        /// </summary>
        protected virtual void PostInitialize()
        {

        }
    }
}