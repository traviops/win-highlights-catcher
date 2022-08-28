using System;
using System.IO;
using win_highlights_catcher.Common;

namespace win_highlights_catcher.Util
{
    public static class ExportDirUtil
    {
        public static string ExportDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Constants.APPLICATION_NAME);

        public static void ValidateExportDirectory()
        {
            if (!Directory.Exists(ExportDirectory))
            {
                Directory.CreateDirectory(ExportDirectory);
            }
        }
    }
}
