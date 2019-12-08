using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Net.Http;

namespace R6SAdapter
{
    static class R6SFile
    {
        static readonly string BackupPath;
        static readonly string SteamBackupPath;
        static readonly string UplayBackupPath;
        static R6SFile()
        {
            BackupPath = Path.Combine(Environment.CurrentDirectory, "Backup");
            SteamBackupPath = Path.Combine(BackupPath, "Steam");
            UplayBackupPath = Path.Combine(BackupPath, "Uplay");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns> true为steam版，false为uplay版</returns>
        public static bool CheckSteamVersion()
        {
            return !File.Exists(Path.Combine(Config.Configuration.R6SPath, "uplay_install.manifest"));
        }
        public static bool CheckSteamBackup()
        {
            if (!Directory.Exists(BackupPath)) Directory.CreateDirectory(BackupPath);
            return Directory.Exists(SteamBackupPath);
        }
        public static bool CheckUplayBackup()
        {
            if (!Directory.Exists(BackupPath)) Directory.CreateDirectory(BackupPath);
            return Directory.Exists(UplayBackupPath);
        }
        public static void BackupSteamFiles()
        {
            if (!Directory.Exists(SteamBackupPath)) Directory.CreateDirectory(SteamBackupPath); 
            File.Copy(Path.Combine(Config.Configuration.R6SPath, "defaultargs.dll"), Path.Combine(SteamBackupPath, "defaultargs.dll"), true);
            File.Copy(Path.Combine(Config.Configuration.R6SPath, "steam_api64.dll"), Path.Combine(SteamBackupPath, "steam_api64.dll"), true);
        }
        public static void BackupUplayFiles()
        {
            if (!Directory.Exists(UplayBackupPath)) Directory.CreateDirectory(UplayBackupPath);
            File.Copy(Path.Combine(Config.Configuration.R6SPath, "defaultargs.dll"), Path.Combine(UplayBackupPath, "defaultargs.dll"), true);
            File.Copy(Path.Combine(Config.Configuration.R6SPath, "uplay_install.manifest"), Path.Combine(UplayBackupPath, "uplay_install.manifest"), true);
            File.Copy(Path.Combine(Config.Configuration.R6SPath, "uplay_install.state"), Path.Combine(UplayBackupPath, "uplay_install.state"), true);
        }
        public static void RecoverySteamFiles()
        {
            File.Copy(Path.Combine(SteamBackupPath, "defaultargs.dll"), Path.Combine(Config.Configuration.R6SPath, "defaultargs.dll"), true);
            File.Delete(Path.Combine(Config.Configuration.R6SPath, "uplay_install.manifest"));
            File.Delete(Path.Combine(Config.Configuration.R6SPath, "uplay_install.state"));
            DeleteTempFiles();
        }
        public static void RecoveryUplayFiles()
        {
            File.Copy(Path.Combine(UplayBackupPath, "defaultargs.dll"), Path.Combine(Config.Configuration.R6SPath, "defaultargs.dll"), true);
            File.Copy(Path.Combine(UplayBackupPath, "uplay_install.manifest"), Path.Combine(Config.Configuration.R6SPath, "uplay_install.manifest"), true);
            File.Copy(Path.Combine(UplayBackupPath, "uplay_install.state"), Path.Combine(Config.Configuration.R6SPath, "uplay_install.state"), true);
            DeleteTempFiles();
        }
        public static DateTime GetSteamBackupTime()
        {
            var fi = new FileInfo(Path.Combine(SteamBackupPath, "defaultargs.dll"));
            return fi.LastAccessTime;
        }
        public static DateTime GetUplayBackupTime()
        {
            var fi = new FileInfo(Path.Combine(UplayBackupPath, "defaultargs.dll"));
            return fi.LastAccessTime;
        }
        static void DeleteTempFiles()
        {
            foreach (string f in Directory.GetFileSystemEntries(Path.Combine(Config.Configuration.R6SPath,"download","cache")))
            {

                if (File.Exists(f))
                {
                    File.Delete(f);
                }
            }
        }
    }
}
