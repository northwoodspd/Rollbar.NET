﻿namespace Rollbar
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using Rollbar.Common;

    public class RollbarInfrastructureConfig
        : ReconfigurableBase<RollbarInfrastructureConfig, IRollbarInfrastructureConfig>
        , IRollbarInfrastructureConfig

    {
        private readonly RollbarLoggerConfig _rollbarLoggerConfig = 
            null;
        private readonly RollbarInfrastructureOptions _rollbarInfrastructureOptions = 
            new RollbarInfrastructureOptions();
        private readonly RollbarTelemetryOptions _rollbarTelemetryOptions = 
            new RollbarTelemetryOptions();
        private readonly RollbarOfflineStoreOptions _rollbarOfflineStoreOptions = 
            new RollbarOfflineStoreOptions();

        public RollbarInfrastructureConfig()
            : this("seedToken")
        {
        }

        public RollbarInfrastructureConfig(string rollbarAccessToken, string? rollbarEnvironment = null)
        {
            this._rollbarLoggerConfig = new RollbarLoggerConfig(rollbarAccessToken, rollbarEnvironment);
        }

        public IRollbarLoggerConfig RollbarLoggerConfig
        {
            get
            {
                return this._rollbarLoggerConfig;
            }
        }
        public IRollbarInfrastructureOptions RollbarInfrastructureOptions
        {
            get
            {
                return this._rollbarInfrastructureOptions;
            }
        }

        public IRollbarTelemetryOptions RollbarTelemetryOptions
        {
            get
            {
                return this._rollbarTelemetryOptions;
            }
        }

        public IRollbarOfflineStoreOptions RollbarOfflineStoreOptions
        {
            get
            {
                return this._rollbarOfflineStoreOptions;
            }
        }

        public override RollbarInfrastructureConfig Reconfigure(IRollbarInfrastructureConfig likeMe)
        {
            return base.Reconfigure(likeMe);
        }

        public override Validator GetValidator()
        {
            var validator =
                new Validator<RollbarInfrastructureConfig, RollbarInfrastructureConfig.RollbarInfrastructureConfigValidationRule>()
                    .AddValidation(
                        RollbarInfrastructureConfig.RollbarInfrastructureConfigValidationRule.LoggerConfigRequired,
                        (config) => { return this.RollbarLoggerConfig != null; }
                        )
                    .AddValidation(
                        RollbarInfrastructureConfig.RollbarInfrastructureConfigValidationRule.InfrastructureOptionsRequred,
                        (config) => { return this.RollbarInfrastructureOptions != null; }
                        )
                    .AddValidation(
                        RollbarInfrastructureConfig.RollbarInfrastructureConfigValidationRule.TelemetryOptionsRequired,
                        (config) => { return this.RollbarTelemetryOptions != null; }
                        )
                    .AddValidation(
                        RollbarInfrastructureConfig.RollbarInfrastructureConfigValidationRule.OfflineStoreOptionsRequired,
                        (config) => { return this.RollbarOfflineStoreOptions != null; }
                        )
               ;

            IValidatable[] validatableComponents =
            {
                this._rollbarLoggerConfig,
                this._rollbarInfrastructureOptions,
                this._rollbarTelemetryOptions,
                this._rollbarOfflineStoreOptions,
            };

            Debug.Assert(validator.TotalValidationRules == (new HashSet<IValidatable>(validatableComponents)).Count);
            CompositeValidator compositeValidator =
                new CompositeValidator(
                    new Validator[] { validator },
                    validatableComponents
                    );

            return compositeValidator;
        }

        public enum RollbarInfrastructureConfigValidationRule
        {
            LoggerConfigRequired,
            InfrastructureOptionsRequred,
            TelemetryOptionsRequired,
            OfflineStoreOptionsRequired,
        }

        IRollbarInfrastructureConfig IReconfigurable<IRollbarInfrastructureConfig, IRollbarInfrastructureConfig>.Reconfigure(IRollbarInfrastructureConfig likeMe)
        {
            return this.Reconfigure(likeMe);
        }
    }
}
