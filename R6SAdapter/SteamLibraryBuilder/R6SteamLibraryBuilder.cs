using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace R6SAdapter
{
    class R6SteamLibraryBuilder
    {
        public R6SteamLibraryBuilder(string R6SPath)
        {
            SteamLibraryPath = R6SPath.Replace(@"\common\Tom Clancy's Rainbow Six Siege", string.Empty);
        }

        readonly string SteamLibraryPath;
        /// <returns>true->Exsist</returns>
        public bool CheckSteamLibraryExsist()
        {
            return File.Exists(Path.Combine(SteamLibraryPath, "appmanifest_359550.acf"));
        }
        public bool RebuildSteamLibrary()
        {
            try
            {
                CreateSteamAcfFile();
                CreateWorkShopFile();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Source + e.Message);
                return false;
            }
        }
        private void CreateSteamAcfFile()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://coldthunder11.com/R6SAdapter/SteamFiles/appmanifest_359550.acf")
            };
            var requestTask = httpClient.GetStringAsync(new Uri("https://coldthunder11.com/R6SAdapter/SteamFiles/appmanifest_359550.acf"));
            string steamAcfStr = requestTask.Result;
            File.WriteAllText(Path.Combine(SteamLibraryPath, "appmanifest_359550.acf"), steamAcfStr);
        }
        private void CreateWorkShopFile()
        {
            string workShopDir = Path.Combine(SteamLibraryPath, "workshop");
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://coldthunder11.com/R6SAdapter/SteamFiles/appworkshop_359550.acf")
            };
            var requestTask = httpClient.GetStringAsync(new Uri("https://coldthunder11.com/R6SAdapter/SteamFiles/appworkshop_359550.acf"));
            string workshopStr = requestTask.Result;
            File.WriteAllText(Path.Combine(workShopDir, "appworkshop_359550.acf"),workshopStr);
        }
    }
}
