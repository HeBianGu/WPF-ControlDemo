// LicenseValidatorTask.cs
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Security.Cryptography;

namespace LicenseValidatorTask
{
    public class LicenseValidatorTask : Task
    {
        [Required]
        public string ModuleName { get; set; }
        [Required]
        public string BasePath { get; set; }

        public override bool Execute()
        {
            try
            {
                string publicKey = Path.Combine(this.BasePath, this.ModuleName, "default.pub");
                publicKey = File.ReadAllText(publicKey);
                string license = Path.Combine(this.BasePath, this.ModuleName, "license.lic");
                license = File.ReadAllText(license);
                LicenseService licenseService = new LicenseService();
                LicenseOption option = licenseService.IsVail(publicKey, this.ModuleName, license, out string error);
                if (option == null)
                {
                    Log.LogError(SystemInfo.Instance.HostID);
                    //Log.LogError(this.ModuleName);
                    Log.LogError(error);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError("error");
                Log.LogErrorFromException(ex);
                return false;
            }
        }
    }
}