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
        //[Required]
        //public string LicenseKeyFile { get; set; }

        //[Required]
        //public string PublicKeyFile { get; set; }

        [Required]
        public string ModuleName { get; set; }

        public override bool Execute()
        {
            //Log.LogError($"License file not found: ");
            //return false;
            //if (!File.Exists(LicenseKeyFile))
            //{
            //    Log.LogError($"License file not found: {LicenseKeyFile}");
            //    return false;
            //}

            //if (!File.Exists(PublicKeyFile))
            //{
            //    Log.LogError($"Public key file not found: {PublicKeyFile}");
            //    return false;
            //}

            try
            {
                //var licenseData = File.ReadAllText(LicenseKeyFile);
                //var publicKey = File.ReadAllText(PublicKeyFile);
                Log.LogMessage("VerifyLicense");
                // 这里添加你的实际验证逻辑
                bool isValid = VerifyLicense(this.ModuleName);

                if (!isValid)
                {
                    Log.LogError("Invalid license key");
                    return false;
                }

                Log.LogMessage(MessageImportance.High, "License validation successful");
                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
                return false;
            }
        }

        private bool VerifyLicense(string moduleName)
        {
            LicenseService licenseService = new LicenseService();
            LicenseOption option = licenseService.IsVail(moduleName, out string error);
            if (option == null)
            {
                Log.LogError(option.HostID);
                Log.LogError(option.Module);
                Log.LogError(error);
                return false;
            }
            return true;
        }
    }
}