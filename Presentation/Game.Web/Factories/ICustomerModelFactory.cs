using Game.Web.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Web.Factories
{
    public interface ICustomerModelFactory
    {

        /// <summary>
        /// Prepare the login model
        /// </summary>
        /// <returns>Login model</returns>
        SignInPostModel PrepareLoginModel();
        /// <summary>
        /// Prepare the customer register model
        /// </summary>
        /// <param name="model">Customer register model</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <param name="overrideCustomCustomerAttributesXml">Overridden customer attributes in XML format; pass null to use CustomCustomerAttributes of customer</param>
        /// <param name="setDefaultValues">Whether to populate model properties by default values</param>
        /// <returns>Customer register model</returns>
        SignUpPostModel PrepareRegisterModel(SignUpPostModel model, bool excludeProperties,
            string overrideCustomCustomerAttributesXml = "", bool setDefaultValues = false);
        /// <summary>
        /// Prepare the register result model
        /// </summary>
        /// <param name="resultId">Value of UserRegistrationType enum</param>
        /// <returns>Register result model</returns>
        RegisterResultModel PrepareRegisterResultModel(int resultId);
    }
}
