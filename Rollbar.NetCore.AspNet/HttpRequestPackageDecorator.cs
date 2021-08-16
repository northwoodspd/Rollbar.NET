﻿using System.Linq;

namespace Rollbar.NetCore.AspNet
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Rollbar.Common;
    using Rollbar.DTOs;
    using Rollbar.Serialization.Json;

    /// <summary>
    /// Class HttpRequestPackageDecorator.
    /// Implements the <see cref="Rollbar.RollbarPackageDecoratorBase" />
    /// </summary>
    /// <seealso cref="Rollbar.RollbarPackageDecoratorBase" />
    public class HttpRequestPackageDecorator
        : RollbarPackageDecoratorBase
    {
        /// <summary>
        /// The HTTP request
        /// </summary>
        private readonly HttpRequest _httpRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestPackageDecorator"/> class.
        /// </summary>
        /// <param name="packageToDecorate">The package to decorate.</param>
        /// <param name="httpRequest">The HTTP request.</param>
        public HttpRequestPackageDecorator(IRollbarPackage packageToDecorate, HttpRequest httpRequest)
            : this(packageToDecorate, httpRequest, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestPackageDecorator"/> class.
        /// </summary>
        /// <param name="packageToDecorate">The package to decorate.</param>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="mustApplySynchronously">if set to <c>true</c> [must apply synchronously].</param>
        public HttpRequestPackageDecorator(IRollbarPackage packageToDecorate, HttpRequest httpRequest, bool mustApplySynchronously)
            : base(packageToDecorate, mustApplySynchronously)
        {
            this._httpRequest = httpRequest;
        }

        /// <summary>
        /// Decorates the specified rollbar data.
        /// </summary>
        /// <param name="rollbarData">The rollbar data.</param>
        protected override void Decorate(Data? rollbarData)
        {
            if(rollbarData == null)
            {
                return;
            }

            if(this._httpRequest == null)
            {
                return; // nothing to decorate with... 
            }

            if (rollbarData.Request == null)
            {
                rollbarData.Request = new Request();
            }

            rollbarData.Request.Url = this._httpRequest.Host.Value + this._httpRequest.Path;
            rollbarData.Request.QueryString = this._httpRequest.QueryString.Value;
            if (!string.IsNullOrWhiteSpace(this._httpRequest.Path))
            {
                rollbarData.Request.Params = new Dictionary<string, object>();
                rollbarData.Request.Params["path"] = this._httpRequest.Path;
            }

            if (this._httpRequest.Headers?.Count > 0)
            {
                rollbarData.Request.Headers = new Dictionary<string, string>(this._httpRequest.Headers.Count);
                foreach (var header in this._httpRequest.Headers)
                {
                    rollbarData.Request.Headers.Add(header.Key, StringUtility.Combine(header.Value, ", "));
                }
            }

            rollbarData.Request.Method = this._httpRequest.Method;

            switch (rollbarData.Request.Method.ToUpper())
            {
                case "POST":
                    AssignRequestBody(rollbarData.Request);
                    break;
                case "GET":
                    if (this._httpRequest.Query?.Count > 0)
                    {
                        rollbarData.Request.GetParams = 
                            new Dictionary<string, object>(this._httpRequest.Query.Count);
                        foreach (var kv in this._httpRequest.Query)
                        {
                            rollbarData.Request.GetParams[kv.Key] = kv.Value;
                        }
                    }
                    break;
                default:
                    // nothing to do...
                    break;
            }
        }

        /// <summary>
        /// Assigns the request body.
        /// </summary>
        /// <param name="request">The request.</param>
        private void AssignRequestBody(Request request)
        {
            if (this._httpRequest == null || this._httpRequest.Body == null)
            {
                return; // nothing to do...
            }

            string? jsonString = StreamUtil.ConvertToString(this._httpRequest.Body);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return;
            }
            request.PostBody = jsonString;

            object? requesBodyObject = JsonUtil.InterpretAsJsonObject(jsonString);
            if (requesBodyObject != null)
            {
                request.PostBody = requesBodyObject;
            }
        }
    }
}
