﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BasicAuthentication.Filters
{
    public class BasicAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return;
            }
            var authHeader = actionContext.Request.Headers.Authorization;
            if (authHeader!=null)
            {
                if (authHeader.Scheme.Equals("basic",StringComparison.OrdinalIgnoreCase)&& !string.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var rawCredentials = authHeader.Parameter;
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var credentials = encoding.GetString(Convert.FromBase64String(rawCredentials));
                    var split = credentials.Split(':');
                    var username = split[0];
                    var password = split[1];
                    if (username=="ahasan"&& password=="123456")
                    {
                        var principal = new GenericPrincipal(new GenericIdentity(username), null);
                        Thread.CurrentPrincipal = principal;
                        return;
                    }
                }
            }
            HandleUnauthorized(actionContext);
        }

        void HandleUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate",
        "Basic Scheme='CountingKs' location='http://localhost:8901/account/login'");
        }
    }
}