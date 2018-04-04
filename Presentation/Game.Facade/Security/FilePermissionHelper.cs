using Game.Base;
using Game.Base.Data;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Game.Facade.Security
{
    /// <summary>
    /// File permission helper
    /// </summary>
    public static class FilePermissionHelper
    {
        /// <summary>
        /// Gets a list of directories (physical paths) which require write permission
        /// </summary>
        /// <returns>Result</returns>
        public static IEnumerable<string> GetDirectoriesWrite()
        {
            var rootDir = CommonHelper.MapPath("~/");
            var dirsToCheck = new List<string>();
            //dirsToCheck.Add(rootDir);
            dirsToCheck.Add(Path.Combine(rootDir, "App_Data"));
            dirsToCheck.Add(Path.Combine(rootDir, "bin"));
            dirsToCheck.Add(Path.Combine(rootDir, "plugins"));
            dirsToCheck.Add(Path.Combine(rootDir, "plugins\\bin"));
            dirsToCheck.Add(Path.Combine(rootDir, "wwwroot\\bundles"));
            dirsToCheck.Add(Path.Combine(rootDir, "wwwroot\\db_backups"));
            dirsToCheck.Add(Path.Combine(rootDir, "wwwroot\\files\\exportimport"));
            dirsToCheck.Add(Path.Combine(rootDir, "wwwroot\\images"));
            dirsToCheck.Add(Path.Combine(rootDir, "wwwroot\\images\\thumbs"));
            dirsToCheck.Add(Path.Combine(rootDir, "wwwroot\\images\\uploaded"));
            return dirsToCheck;
        }

        /// <summary>
        /// Gets a list of files (physical paths) which require write permission
        /// </summary>
        /// <returns>Result</returns>
        public static IEnumerable<string> GetFilesWrite()
        {
            return new List<string>
            {
                CommonHelper.MapPath(DataSettingsManager.DataSettingsFilePath)
            };
        }
    }
}
