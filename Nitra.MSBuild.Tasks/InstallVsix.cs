using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.ExtensionManager;
using System.IO;
using Microsoft.Win32;
using System.Globalization;
using Microsoft.VisualStudio.Settings;

namespace Nitra.MSBuild.Tasks
{
    public class InstallVsix : Task
    {
        [Required]
        public string VsixPath { get; set; }

        [Required]
        public string RootSuffix { get; set; }

        public string VisualStudioVersion { get; set; }

        public override bool Execute()
        {
            try
            {
                var vsVersion = VisualStudioVersion ?? FindVsVersions().LastOrDefault().ToString();
                if (string.IsNullOrEmpty(vsVersion))
                    throw new Exception("Cannot find any installed copies of Visual Studio.");

                string vsExe = GetVersionExe(vsVersion);
                if (string.IsNullOrEmpty(vsExe) && vsVersion.All(char.IsNumber))
                {
                    vsVersion += ".0";
                    vsExe = GetVersionExe(vsVersion);
                }

                if (string.IsNullOrEmpty(vsExe))
                {
                    throw new Exception(string.Format("Cannot find Visual Studio {0}. Detected versions:\n{1}",
                        vsVersion, string.Join("\n", FindVsVersions().Where(v => !string.IsNullOrEmpty(GetVersionExe(v.ToString()))))));
                }

                if (!File.Exists(VsixPath))
                    throw new Exception("Cannot find VSIX file " + VsixPath);

                var vsix = ExtensionManagerService.CreateInstallableExtension(VsixPath);

                Console.WriteLine("Installing {0} version {1} to Visual Studio {2} /RootSuffix {3}",
                    vsix.Header.Name, vsix.Header.Version, vsVersion, RootSuffix);

                Install(vsExe, vsix, RootSuffix);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
                return false;
            }
        }

        public static IEnumerable<decimal?> FindVsVersions()
        {
            using (var software = Registry.LocalMachine.OpenSubKey("SOFTWARE"))
            using (var ms = software.OpenSubKey("Microsoft"))
            using (var vs = ms.OpenSubKey("VisualStudio"))
                return vs.GetSubKeyNames()
                        .Select(s =>
                {
                    decimal v;
                    if (!decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                        return new decimal?();
                    return v;
                })
                .Where(d => d.HasValue)
                .OrderBy(d => d);
        }

        public static string GetVersionExe(string version)
        {
            return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\" + version + @"\Setup\VS", "EnvironmentPath", null) as string;
        }

        public static void Install(string vsExe, IInstallableExtension vsix, string rootSuffix)
        {
            using (var esm = ExternalSettingsManager.CreateForApplication(vsExe, rootSuffix))
            {
                var ems = new ExtensionManagerService(esm);
                var installed = ems.GetInstalledExtension(vsix.Header.Identifier);
                if (installed != null)
                    ems.Uninstall(installed);
                ems.Install(vsix, perMachine: false);
            }
        }
    }
}
