using System;
using System.Configuration;

namespace BGuest.Azure.Configuration
{
    /// <summary>
    /// Utility methods to get AppSettings and Connection Strings on azure web jobs. 
    /// </summary>
    /// <remarks>Methods in this class use the environment variables set by the cloud environment, and fallback to the app.config.</remarks>
    public static class WebJobConfigurationManager
    {
        /// <summary>
        /// Get an AppSetting by key
        /// </summary>
        /// <param name="key">The setting key</param>
        /// <returns>The setting value or null if not found.</returns>
        public static string GetAppSetting(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var value = Environment.GetEnvironmentVariable($"APPSETTING_{key}");
            if (value != null)
            {
                Console.WriteLine($"AppSetting: [ENV] {key} -> {value}");
                return value;
            }
            value = ConfigurationManager.AppSettings[key];
            Console.WriteLine($"AppSetting: [CONFIG] {key} -> {value}");
            return value;
        }

        /// <summary>
        /// Get a connection string.
        /// </summary>
        /// <param name="name">Connection string name.</param>
        /// <remarks>Throws ApplicationException if not found.</remarks>
        /// <returns>The connection string.</returns>
        public static string GetConnectionString(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var value = Environment.GetEnvironmentVariable($"SQLAZURECONNSTR_{name}");
            if (value != null)
            {
                Console.WriteLine($"ConnectionString: [ENV-SQLAZURECONNSTR] {name} -> {value}");
                return value;
            }
            value = Environment.GetEnvironmentVariable($"CUSTOMCONNSTR_{name}");
            if (value != null)
            {
                Console.WriteLine($"ConnectionString: [ENV-CUSTOMCONNSTR] {name} -> {value}");
                return value;
            }
            try
            {
                value = ConfigurationManager.ConnectionStrings[name].ConnectionString;
                Console.WriteLine($"ConnectionString: [CONFIG] {name} -> {value}");
                return value;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception thrown while getting connection string '{name}'. See inner exception.", e);
            }
        }
    }
}
