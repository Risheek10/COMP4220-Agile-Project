using System;
using System.Configuration;

namespace ywBookStoreLIB
{
    public static class SettingsManager
    {
        public static bool EnableUserRegistration
        {
            get => GetSetting("EnableUserRegistration", true);
            set => SetSetting("EnableUserRegistration", value);
        }

        public static bool RequireEmailVerification
        {
            get => GetSetting("RequireEmailVerification", false);
            set => SetSetting("RequireEmailVerification", value);
        }

        public static bool EnableMaintenanceMode
        {
            get => GetSetting("EnableMaintenanceMode", false);
            set => SetSetting("EnableMaintenanceMode", value);
        }

        public static bool SendDailyReports
        {
            get => GetSetting("SendDailyReports", false);
            set => SetSetting("SendDailyReports", value);
        }

        public static string SmtpServer
        {
            get => GetSetting("SmtpServer", "smtp.example.com");
            set => SetSetting("SmtpServer", value);
        }

        public static int SmtpPort
        {
            get => GetSetting("SmtpPort", 587);
            set => SetSetting("SmtpPort", value);
        }

        public static string AdminEmail
        {
            get => GetSetting("AdminEmail", "admin@bookstore.com");
            set => SetSetting("AdminEmail", value);
        }

        private static T GetSetting<T>(string key, T defaultValue)
        {
            try
            {
                if (ConfigurationManager.AppSettings[key] != null)
                {
                    return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
                }
            }
            catch (Exception)
            {
                // Log or handle error
            }
            return defaultValue;
        }

        private static void SetSetting<T>(string key, T value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] == null)
                {
                    config.AppSettings.Settings.Add(key, value.ToString());
                }
                else
                {
                    config.AppSettings.Settings[key].Value = value.ToString();
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception)
            {
                // Log or handle error
            }
        }
    }
}
