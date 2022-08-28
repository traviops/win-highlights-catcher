using System;
using System.IO;
using System.Windows.Forms;
using win_highlights_catcher.Common;
using win_highlights_catcher.Util;

namespace win_highlights_catcher
{
    class Program
    {
        static void Main(string[] args)
        {
            ExportDirUtil.ValidateExportDirectory();

            if (!SettingsUtil.SettingsExists())
            {
                MessageBox.Show(text:
                    "INFORMAÇÕES IMPORTANTES:\n\n" +
                    "A aplicação será executada em segundo plano, sendo iniciada automaticamente com o windows.\n\n" +
                    $"Todos destaques capturados serão exportados no diretório {ExportDirUtil.ExportDirectory}.\n\n" +
                    $"Para encerrar a execução automatica da aplicação edite o arquivo {Constants.SETTINGS_FILE_NAME} localizado no diretório {ExportDirUtil.ExportDirectory} " +
                    $"e altere o valor de AUTO_START para 0. Automaticamente a aplicação deixará de ser executada em segundo plano.\n\n" +
                    $"Para que reativar a inicialização automatica basta voltar o valor de AUTO_START para 1.\n\n\n" +
                    $"\t\tBruno Travi Teixeira @ 2022", caption: "Windows Highlights Catcher");
            }

            SettingsUtil.SetStartUpRegister();

            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = ExplorerUtil.GetWindowsHighlightsDirectory(),
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.*"
            };
            watcher.Created += new FileSystemEventHandler(OnChanged);

            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            ExplorerUtil.CopyHighlight(e.FullPath);
        }
    }
}
