using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.IO;

namespace R6SAdapter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BuildSteamLibrary.IsEnabled = false;//暂时隐藏不用
            RunSteamVersion.IsEnabled = false; RunUplayVersion.IsEnabled = false;
            Config.LoadConfiguration();
            R6PathTextBox.Text = Config.Configuration.R6SPath;
            SetR6SState();
            SetR6SSaveState();
        }
        private void SetR6SState()
        {
            if (R6SFile.CheckSteamVersion()) R6SState.Content = Properties.Resources.CurrentVersionSteam;
            else R6SState.Content = Properties.Resources.CurrentVersionUplay;
        }
        private void SetR6SSaveState()
        {
            if (Config.Configuration.R6SPath == String.Empty)
            {
                SaveSteamFile.IsEnabled = false; SaveUplayFile.IsEnabled = false;
            }
            else
            {
                SaveSteamFile.IsEnabled = true; SaveUplayFile.IsEnabled = true;
            }
            var sb = new StringBuilder();
            sb.Append(Properties.Resources.SteamBackup);
            if (R6SFile.CheckSteamBackup())
            {
                sb.Append(Properties.Resources.Exsist);
                RunSteamVersion.IsEnabled = true;
            }
            else sb.Append(Properties.Resources.NoExsist);
            sb.Append("\n");
            sb.Append(Properties.Resources.UplayBackup);
            if (R6SFile.CheckUplayBackup())
            {
                sb.Append(Properties.Resources.Exsist);
                RunUplayVersion.IsEnabled = true;
            }
            else sb.Append(Properties.Resources.NoExsist);
            R6SSaveState.Content = sb.ToString();
        }
        private void R6PathSelectButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = Properties.Resources.PleaseSelectR6SInstallDirectory
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(Path.Combine(dialog.SelectedPath, "RainbowSix.exe")))
                {
                    Config.Configuration.R6SPath = dialog.SelectedPath;
                    Config.SaveConfiguration();
                    R6PathTextBox.Text = dialog.SelectedPath;
                    SetR6SState();
                    SetR6SSaveState();
                }
                else MessageBox.Show(Properties.Resources.FailedtoFindR6S);
            }
        }
        private void AdaptR6SButton_Click(object sender, RoutedEventArgs e)
        {
            if (R6SFile.CheckSteamBackup() && R6SFile.CheckUplayBackup())
            {
                if (R6SFile.CheckSteamVersion())
                {
                    R6SFile.RecoveryUplayFiles();
                    MessageBox.Show(Properties.Resources.SwitchSuccess);
                }
                else
                {
                    R6SFile.RecoverySteamFiles();
                    MessageBox.Show(Properties.Resources.SwitchSuccess);
                }
                SetR6SState();
            }
            else MessageBox.Show(Properties.Resources.PleaseBackupFilesFirst);
        }
        private void FirstUseHelpLink_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ColdThunder11/R6SAdapter/wiki/UseHelp-Steam2Uplay(CN)");
        }
        private void AboutLink_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://space.bilibili.com/11988933");
        }
        private void SaveSteamFile_Click(object sender, RoutedEventArgs e)
        {
            if (R6SFile.CheckSteamVersion())
            {
                R6SFile.BackupSteamFiles();
                MessageBox.Show(Properties.Resources.BackupSuccessfully);
            }
            else MessageBox.Show("请确认目前的彩虹六号为Steam版");
            SetR6SSaveState();
        }

        private void SaveUplayFile_Click(object sender, RoutedEventArgs e)
        {
            if (!R6SFile.CheckSteamVersion())
            {
                R6SFile.BackupUplayFiles();
                MessageBox.Show(Properties.Resources.BackupSuccessfully);
            }
            else MessageBox.Show("请确认目前的彩虹六号为Uplay版");
            SetR6SSaveState();
        }

        private void BuildSteamLibrary_Click(object sender, RoutedEventArgs e)
        {
            if(!Config.Configuration.R6SPath.EndsWith(@"steamapps\common\Tom Clancy's Rainbow Six Siege"))
            {
                MessageBox.Show("请确保R6S安装文件已经拷贝至Steam库文件夹后再执行操作");
                return;
            }
            var R6SLB = new R6SteamLibraryBuilder(Config.Configuration.R6SPath);
            if (R6SLB.CheckSteamLibraryExsist())
            {
                MessageBox.Show("Steam库文件已经存在!");
            }
            else
            {
                if (R6SLB.RebuildSteamLibrary()) MessageBox.Show("操作完成");
            }
        }
        private void RunSteamVersion_Click(object sender, RoutedEventArgs e)
        {
            if (!R6SFile.CheckSteamVersion())
            {
                R6SFile.RecoverySteamFiles();
                SetR6SState();
            }
            System.Diagnostics.Process.Start("steam://run/359550");
        }

        private void RunUplayVersion_Click(object sender, RoutedEventArgs e)
        {
            if (R6SFile.CheckSteamVersion())
            {
                R6SFile.RecoveryUplayFiles();
                SetR6SState();
            }
            System.Diagnostics.Process.Start("uplay://launch/635/0");
        }

        private void ViewGithub_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ColdThunder11/R6SAdapter");
        }
    }
}
