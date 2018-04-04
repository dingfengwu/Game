using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

using Game.Facade.Mvc.Filters;

namespace Game.Facade.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    [SaveIpAddress]
    public abstract class BaseController : Controller
    {
        
    }
}
