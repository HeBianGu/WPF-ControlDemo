// LicenseValidatorTask.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace LicenseValidatorTask
{
    public class LicenseService
    {
        private DateTime _trialEndTime = DateTime.Now.AddDays(30);
        public DateTime TrialEndTime
        {
            get { return _trialEndTime; }
            set
            {
                _trialEndTime = value;
            }
        }

        private bool _UseTrial;
        public bool UseTrial
        {
            get { return _UseTrial; }
            set
            {
                _UseTrial = value;
            }
        }


        //public LicenseOption TryActive(string module, string lic, out string error)
        //{
        //    LicenseOption result = this.IsVail(module, lic, out error);
        //    if (result == null)
        //        return null;
        //    this.OnActiveLic(module, lic);
        //    return result;
        //}

        protected virtual void OnActiveLic(string module, string lic)
        {
            string file = this.GetLicFile(module);
            string folder = Path.GetDirectoryName(file);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            File.WriteAllText(file, lic);
        }

        ///// <summary>
        ///// 检查本地许可是否合法
        ///// </summary>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public virtual LicenseOption IsVail(out string error)
        //{
        //    string module = Assembly.GetEntryAssembly().GetName().Name;
        //    return this.IsVail(module, out error);
        //}

        ///// <summary>
        ///// 检查本地许可是否合法
        ///// </summary>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public LicenseOption IsVail(string module, out string error)
        //{
        //    module = module ?? Assembly.GetEntryAssembly().GetName().Name;
        //    string file = this.GetLicFile(module);
        //    if (!File.Exists(file))
        //    {
        //        error = "许可文件不存在";
        //        if (this.UseTrial)
        //        {
        //            if (this.TrialEndTime.Date < DateTime.Now.Date)
        //            {
        //                error = "试用许可已到期，请申请正式许可";
        //                return null;
        //            }
        //            LicenseOption licenseOption = new LicenseOption();
        //            licenseOption.EndTime = this.TrialEndTime;
        //            licenseOption.HostID = this.GetHostID();
        //            licenseOption.Module = module;
        //            licenseOption.Level = -1;
        //            this.OnTrial();
        //            return licenseOption;
        //        }

        //        return null;
        //    }
        //    string lic = File.ReadAllText(file);
        //    return this.IsVail(module, lic, out error);
        //}

        protected virtual void OnTrial()
        {

        }

        /// <summary>
        /// 检验许可文本是否可用
        /// </summary>
        /// <param name="lic"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public LicenseOption IsVail(string publicKey, string module, string lic, out string error)
        {
            string value = RSAHelper.DecryptString(lic, publicKey);
            LicenseOption licenseOption = new LicenseOption();
            licenseOption.HostID = this.GetHostID();
            licenseOption.Module = module;
            if (!licenseOption.IsMatch(value, out error))
            {
                licenseOption.License = lic;
                return null;
            }
            licenseOption.License = lic;
            return licenseOption;
        }

        protected virtual string GetLicFile(string module)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(path, module, "license.lic");
        }

        //private string GetPublicKey(string filePath)
        //{
        //    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "default.pub");
        //    return File.ReadAllText(path);
        //}

        public string GetHostID()
        {
            return SystemInfo.Instance.HostID;
        }
    }

    internal static class RSAHelper
    {
        public const int DWKEYSIZE = 1024;
        public struct RSAKey
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        public static RSAKey GetRASKey()
        {
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(DWKEYSIZE);
            RSAParameters p = rsaProvider.ExportParameters(true);

            return new RSAKey()
            {
                PublicKey = ComponentKey(p.Exponent, p.Modulus),
                PrivateKey = ComponentKey(p.D, p.Modulus)
            };
        }
        public static bool CheckSourceValidate(string source)
        {
            return DWKEYSIZE / 8 - 11 >= source.Length;
        }

        private static string ComponentKey(byte[] b1, byte[] b2)
        {
            List<byte> list = new List<byte>();
            list.Add((byte)b1.Length);
            list.AddRange(b1);
            list.AddRange(b2);
            byte[] b = list.ToArray<byte>();
            return Convert.ToBase64String(b);
        }

        private static void ResolveKey(string key, out byte[] b1, out byte[] b2)
        {
            byte[] b = Convert.FromBase64String(key);
            b1 = new byte[b[0]];
            b2 = new byte[b.Length - b[0] - 1];
            for (int n = 1, i = 0, j = 0; n < b.Length; n++)
            {
                if (n <= b[0])
                {
                    b1[i++] = b[n];
                }
                else
                {
                    b2[j++] = b[n];
                }
            }
        }

        public static string EncryptString(string source, string key)
        {
            string encryptString = string.Empty;
            byte[] d;
            byte[] n;
            try
            {
                if (!CheckSourceValidate(source))
                {
                    throw new Exception("source string too long");
                }
                ResolveKey(key, out d, out n);
                BigInteger biN = new BigInteger(n);
                BigInteger biD = new BigInteger(d);
                encryptString = EncryptString(source, biD, biN);
            }
            catch
            {
                encryptString = source;
            }
            return encryptString;
        }

        public static string DecryptString(string encryptString, string key)
        {
            string source = string.Empty;
            byte[] e;
            byte[] n;
            try
            {
                ResolveKey(key, out e, out n);
                BigInteger biE = new BigInteger(e);
                BigInteger biN = new BigInteger(n);
                source = DecryptString(encryptString, biE, biN);
            }
            catch
            {
                source = encryptString;
            }
            return source;
        }

        private static string EncryptString(string source, BigInteger d, BigInteger n)
        {
            int len = source.Length;
            int len1 = 0;
            int blockLen = 0;
            if (len % 128 == 0)
                len1 = len / 128;
            else
                len1 = len / 128 + 1;
            string block = "";
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < len1; i++)
            {
                if (len >= 128)
                    blockLen = 128;
                else
                    blockLen = len;
                block = source.Substring(i * 128, blockLen);
                byte[] oText = Encoding.Default.GetBytes(block);
                BigInteger biText = new BigInteger(oText);
                BigInteger biEnText = biText.modPow(d, n);
                string temp = biEnText.ToHexString();
                result.Append(temp).Append("@");
                len -= blockLen;
            }
            return result.ToString().TrimEnd('@');
        }

        private static string DecryptString(string encryptString, BigInteger e, BigInteger n)
        {
            StringBuilder result = new StringBuilder();
            string[] strarr1 = encryptString.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strarr1.Length; i++)
            {
                string block = strarr1[i];
                BigInteger biText = new BigInteger(block, 16);
                BigInteger biEnText = biText.modPow(e, n);
                string temp = Encoding.Default.GetString(biEnText.getBytes());
                result.Append(temp);
            }
            return result.ToString();
        }
    }


}