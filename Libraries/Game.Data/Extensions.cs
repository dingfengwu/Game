using Game.Base;
using Game.Base.Caching;
using System;

namespace Game.Data
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class DataEntityExtensions
    {
        /// <summary>
        /// Get unproxied entity type
        /// </summary>
        /// <remarks> If your Entity Framework context is proxy-enabled, 
        /// the runtime will create a proxy instance of your entities, 
        /// i.e. a dynamically generated class which inherits from your entity class 
        /// and overrides its virtual properties by inserting specific code useful for example 
        /// for tracking changes and lazy loading.
        /// </remarks>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Type GetUnproxiedEntityType(this BaseEntity entity)
        {
            var type = entity is IEntityForCaching ? 
               ((IEntityForCaching) entity).GetType().BaseType :
               entity.GetType();//待处理...
            if (type == null)
                throw new Exception("Original entity type cannot be loaded");

            return type;
        }
    }
}
