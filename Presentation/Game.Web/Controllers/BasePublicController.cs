using Game.Facade.Controllers;
using Game.Facade.Mvc.Filters;
using Game.Facade.Security;
using Microsoft.AspNetCore.Mvc;

namespace Game.Web.Controllers
{
    [HttpsRequirement(SslRequirement.NoMatter)]
    [WwwRequirement]
    [CheckLanguageSeoCode]
    public abstract partial class BasePublicController : BaseController
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}