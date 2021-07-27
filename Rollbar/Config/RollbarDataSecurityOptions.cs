﻿namespace Rollbar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using Rollbar.Common;

    public class RollbarDataSecurityOptions
        : ReconfigurableBase<RollbarDataSecurityOptions, IRollbarDataSecurityOptions>
        , IRollbarDataSecurityOptions
    {
        public RollbarDataSecurityOptions()
            : this(
                  PersonDataCollectionPolicies.None, 
                  IpAddressCollectionPolicy.Collect,
                  RollbarDataScrubbingHelper.Instance.GetDefaultFields().ToArray(),
                  null)
        {
        }

        public RollbarDataSecurityOptions(
            PersonDataCollectionPolicies personDataCollectionPolicies, 
            IpAddressCollectionPolicy ipAddressCollectionPolicy,
            string[] scrubFields = null,
            string[] scrubSafeFields = null)
        {
            this.PersonDataCollectionPolicies = personDataCollectionPolicies;
            this.IpAddressCollectionPolicy = ipAddressCollectionPolicy;
            this.ScrubFields = scrubFields;
            this.ScrubSafelistFields = scrubSafeFields;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public PersonDataCollectionPolicies PersonDataCollectionPolicies
        {
            get;
            set;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public IpAddressCollectionPolicy IpAddressCollectionPolicy
        {
            get;
            set;
        }

        public string[] ScrubFields
        {
            get; 
            set;
        }

        public string[] ScrubSafelistFields
        {
            get;
            set;
        }

        public IReadOnlyCollection<string> GetFieldsToScrub()
        {
            if(this.ScrubFields == null || this.ScrubFields.Length == 0)
            {
                return new string[0];
            }

            if(this.ScrubSafelistFields == null || this.ScrubSafelistFields.Length == 0)
            {
                return this.ScrubFields.ToArray();
            }

            var scrubSafeList = this.ScrubSafelistFields.ToArray();
            return this.ScrubFields.Where(i => !scrubSafeList.Contains(i)).ToArray();
        }

        public override RollbarDataSecurityOptions Reconfigure(IRollbarDataSecurityOptions likeMe)
        {
            return base.Reconfigure(likeMe);
        }

        public override Validator GetValidator()
        {
            return null;
        }

        IRollbarDataSecurityOptions IReconfigurable<IRollbarDataSecurityOptions, IRollbarDataSecurityOptions>.Reconfigure(IRollbarDataSecurityOptions likeMe)
        {
            return this.Reconfigure(likeMe);
        }
    }
}
