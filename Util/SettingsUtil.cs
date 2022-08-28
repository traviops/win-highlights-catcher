using System.IO;
using win_highlights_catcher.Common;
using Newtonsoft.Json;
using win_highlights_catcher.Models;
using System;
using Microsoft.Win32;
using System.Reflection;

namespace win_highlights_catcher.Util
{
    public static class SettingsUtil
    {
        private static string SettingsFileName = Path.Combine(ExportDirUtil.ExportDirectory, Constants.SETTINGS_FILE_NAME);

        public static Settings ValidateSettingsFile()
        {
            Settings settings = null;

            try
            {
                if (!File.Exists(SettingsFileName))
                {
                    settings = new Settings { AutoStart = 1 };
                    File.WriteAllText(SettingsFileName, JsonConvert.SerializeObject(settings));
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog("(ERRO) Erro ao criar arquivo de configurações: " + ex.Message);
            }

            return settings;
        }

        public static bool SettingsExists()
        {
            return File.Exists(SettingsFileName);
        }

        public static Settings GetSettings()
        {
            var settings = ValidateSettingsFile();

            if (settings == null)
            {
                try
                {
                    if (File.Exists(SettingsFileName))
                    {
                        var settingsFileContent = File.ReadAllText(SettingsFileName);
                        settings = JsonConvert.DeserializeObject<Settings>(settingsFileContent);

                        if (settings.AutoStart < 0)
                        {
                            settings.AutoStart = 0;
                            UpdateSettings(settings);
                        }
                        else if (settings.AutoStart > 1)
                        {
                            settings.AutoStart = 1;
                            UpdateSettings(settings);
                        }
                    }
                }
                catch (Exception)
                {
                    LogUtil.WriteLog("(AVISO) Erro ao ler arquivo de configurações, recriando arquivo.");
                    settings = ValidateSettingsFile();
                }
            }

            return settings;
        }

        public static bool UpdateSettings(Settings settings)
        {
            if (settings != null)
            {
                try
                {
                    File.WriteAllText(SettingsFileName, JsonConvert.SerializeObject(settings));
                    return true;
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog("(ERRO) Erro ao atualizar arquivo de configurações: " + ex.Message);
                }
            }

            return false;
        }

        public static void SetStartUpRegister()
        {
            var settings = GetSettings();

            if (settings == null)
            {
                LogUtil.WriteLog("(ERRO) Não foi possível configurar a inicialização automática da aplicação.");
            }
            else
            {
                var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (settings.IsAutoStartEnabled)
                {
                    registryKey.SetValue(Constants.APPLICATION_NAME, Assembly.GetEntryAssembly().Location);
                }
                else
                {
                    registryKey.DeleteValue(Constants.APPLICATION_NAME, false);
                }
            }
        }
    }
}
