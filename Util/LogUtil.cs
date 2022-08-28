using System;
using System.IO;
using win_highlights_catcher.Common;

namespace win_highlights_catcher.Util
{
    public static class LogUtil
    {
        private static string ExportFileName = Path.Combine(ExportDirUtil.ExportDirectory, Constants.LOG_FILE_NAME);

        public static void WriteLog(string message)
        {
            try
            {
                ExportDirUtil.ValidateExportDirectory();

                string currDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                File.AppendAllText(ExportFileName, $"[{currDate}]: {message}{Environment.NewLine}");
            }
            catch (Exception) { }
        }
    }
}
