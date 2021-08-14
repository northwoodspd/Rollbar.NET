﻿namespace Rollbar.DTOs
{
    using System.Collections.Generic;
    using Rollbar.Common;

    /// <summary>
    /// Class Notifier.
    /// Implements the <see cref="Rollbar.DTOs.ExtendableDtoBase" />
    /// </summary>
    /// <seealso cref="Rollbar.DTOs.ExtendableDtoBase" />
    public class Notifier
        : ExtendableDtoBase
    {
        /// <summary>
        /// The DTO reserved properties.
        /// </summary>
        public static class ReservedProperties
        {
            /// <summary>
            /// The name
            /// </summary>
            public static readonly string Name = "name";
            /// <summary>
            /// The version
            /// </summary>
            public static readonly string Version = "version";
            /// <summary>
            /// The configuration
            /// </summary>
            public static readonly string Configuration = "configured_options";
        }

        /// <summary>
        /// The notifier name
        /// </summary>
        private static readonly string notifierName = "Rollbar.NET";
        /// <summary>
        /// The notifier assembly version
        /// </summary>
        private static readonly string notifierAssemblyVersion;

        /// <summary>
        /// Detects the notifier assembly version.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string DetectNotifierAssemblyVersion()
        {
            return RuntimeEnvironmentUtility.GetTypeAssemblyVersion(typeof(Data));
        }

        /// <summary>
        /// Detects the name of the notifier.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string DetectNotifierProduct()
        {
            return RuntimeEnvironmentUtility.GetTypeAssemblyProduct(typeof(Data));
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string? Name
        {
            get { return this[ReservedProperties.Name] as string; }
            private set { this[ReservedProperties.Name] = value; }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public string? Version
        {
            get { return this[ReservedProperties.Version] as string; }
            private set { this[ReservedProperties.Version] = value; }
        }
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IRollbarLoggerConfig? Configuration
        {
            get { return this[ReservedProperties.Configuration] as IRollbarLoggerConfig; }
            set { this[ReservedProperties.Configuration] = value; }
        }

        /// <summary>
        /// Initializes static members of the <see cref="Notifier"/> class.
        /// </summary>
        static Notifier()
        {
            string product = Notifier.DetectNotifierProduct();
            if (!string.IsNullOrWhiteSpace(product))
            {
                Notifier.notifierName = $"{Notifier.notifierName} ({product})";
            }
            Notifier.notifierAssemblyVersion = Notifier.DetectNotifierAssemblyVersion();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notifier"/> class.
        /// </summary>
        public Notifier()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notifier"/> class.
        /// </summary>
        /// <param name="arbitraryKeyValuePairs">The arbitrary key value pairs.</param>
        public Notifier(IDictionary<string, object>? arbitraryKeyValuePairs) 
            : base(arbitraryKeyValuePairs)
        {
            this.Name = Notifier.notifierName;
            this.Version = Notifier.notifierAssemblyVersion;
        }
    }
}
