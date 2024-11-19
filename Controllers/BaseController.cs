using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tasker_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public BaseUser Account => (BaseUser)HttpContext.Items["User"];
    }
}
