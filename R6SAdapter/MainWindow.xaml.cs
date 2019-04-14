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
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Config.LoadConfiguration();
            R6PathTextBox.Text = Config.Configuration.R6SPath;
            SetR6SState();
            SetR6SSaveState();
        }
        private void SetR6SState()
        {
            if (R6SFile.CheckSteamVersion()) R6SState.Content = "当前的版本：Steam";
            else R6SState.Content = "当前的版本：Uplay";
        }
        private void SetR6SSaveState()
        {
            var sb = new StringBuilder();
            sb.Append("Steam版备份：");
            if (R6SFile.CheckSteamBackup())
            {
                sb.Append("存在 ");
                sb.Append(R6SFile.GetSteamBackupTime().ToShortDateString());
            }
            else sb.Append("不存在");
            sb.Append("\n");
            sb.Append("Uplay版备份：");
            if (R6SFile.CheckUplayBackup())
            {
                sb.Append(" 存在 ");
                sb.Append(R6SFile.GetUplayBackupTime().ToShortDateString());
            }
            else sb.Append("不存在");
            R6SSaveState.Content = sb.ToString();
        }
        private void R6PathSelectButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "请选择彩虹六号：围攻所在文件夹"
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
                else MessageBox.Show("未找到R6安装文件，请重新选择目录");
            }
        }
        private void AdaptR6SButton_Click(object sender, RoutedEventArgs e)
        {
            if (R6SFile.CheckSteamBackup() && R6SFile.CheckUplayBackup())
            {
                if (R6SFile.CheckSteamVersion())
                {
                    R6SFile.RecoveryUplayFiles();
                    MessageBox.Show("成功转换为Uplay版");
                }
                else
                {
                    R6SFile.RecoverySteamFiles();
                    MessageBox.Show("成功转换为Steam版");
                }
                SetR6SState();
            }
            else MessageBox.Show("请先完成文件的备份");
        }
        private void FirstUseHelpLink_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://coldthunder11.com/R6SAdapter/R6SAdapterUseHelp.html");
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
                MessageBox.Show("备份Steam版文件成功");
            }
            else MessageBox.Show("请确认目前的彩虹六号为Steam版");
            SetR6SSaveState();
        }

        private void SaveUplayFile_Click(object sender, RoutedEventArgs e)
        {
            if (!R6SFile.CheckSteamVersion())
            {
                R6SFile.BackupUplayFiles();
                MessageBox.Show("备份Uplay版文件成功");
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

        private void AdvancedUseHelpLink_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
