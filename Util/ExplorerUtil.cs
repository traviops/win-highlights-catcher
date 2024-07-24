using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using win_highlights_catcher.Common;

namespace win_highlights_catcher.Util
{
    public static class ExplorerUtil
    {
        private const string HIGHLIGHT_PATH = @"Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";

        public static string GetWindowsHighlightsDirectory()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), HIGHLIGHT_PATH);
            return path;
        }

        public static void CopyHighlight(string highlightFilePath)
        {
            TryCopyFile(highlightFilePath);
        }

        private static void TryCopyFile(string file, int attempt = 0)
        {
            if (attempt >= 3)
            {
                LogUtil.WriteLog($"(ERRO) Máximo de {attempt + 1} tentativas, erro desconhecido.");
                return;
            }

            Task.Factory.StartNew(() =>
            {
                var fileInfo = new FileInfo(file);

                if (fileInfo.Exists)
                {
                    if (IsFileLocked(fileInfo))
                    {
                        Thread.Sleep(3000);
                        TryCopyFile(file, attempt + 1);
                    }
                    else
                    {
                        try
                        {
                            var currTime = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-ffff");

                            ExportDirUtil.ValidateExportDirectory();

                            var destFileName = Path.Combine(ExportDirUtil.ExportDirectory, string.Format(Constants.EXPORT_FILE_NAME, currTime));

                            File.Copy(file, destFileName, true);
                        }
                        catch (Exception ex)
                        {
                            LogUtil.WriteLog("(ERRO) Erro ao tentar salvar destaque: " + ex.Message);
                        }
                    }
                }
            });
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                // file is being written or being processed by another thread or or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }
}
