using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace H.Test.Demo5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    [LicenseProvider(typeof(LicFileLicenseProvider))]
    public class MyButton : Button
    {
        public MyButton()
        {
            using (License license = LicenseManager.Validate(typeof(MyButton), this))
            {
                System.Diagnostics.Debug.WriteLine("许可验证成功");
            }
        }
    }

    /// <summary>
    /// 自定义LicFileLicenseProvider
    /// </summary>
    public class MyLicFileLicenseProvider : LicFileLicenseProvider
    {
        /// <summary>
        /// 检查密钥是否有效    
        /// </summary>
        protected override bool IsKeyValid(string key, Type type)
        {
            return base.IsKeyValid(key, type);
        }

        protected override string GetKey(Type type)
        {
            return base.GetKey(type);
        }

        /// <summary>
        /// 获取组件实例的许可证并确定其是否有效
        /// </summary>
        /// <returns></returns>
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            return base.GetLicense(context, type, instance, allowExceptions);
        }
    }

    //public class MyLicenseProvider : LicenseProvider
    //{
    //    public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
    //    {
    //        if (context.UsageMode == LicenseUsageMode.Runtime)
    //        {
    //            // 在运行时验证许可
    //            if (IsValidLicense())
    //            {
    //                return new MyLicense(this);
    //            }
    //            else if (allowExceptions)
    //            {
    //                throw new LicenseException(type, instance, "无效的许可");
    //            }
    //        }
    //        return null;
    //    }

    //    private bool IsValidLicense()
    //    {
    //        try
    //        {
    //            // 假设许可文件位于应用程序目录下
    //            string licenseFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "license.txt");

    //            if (File.Exists(licenseFilePath))
    //            {
    //                // 读取许可文件内容
    //                string licenseKey = File.ReadAllText(licenseFilePath).Trim();

    //                // 验证许可密钥
    //                return ValidateLicenseKey(licenseKey);
    //            }
    //            else
    //            {
    //                Console.WriteLine("许可文件不存在。");
    //                return false;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("读取许可文件时发生错误: " + ex.Message);
    //            return false;
    //        }
    //    }

    //    private bool ValidateLicenseKey(string licenseKey)
    //    {
    //        // 在这里实现你的许可密钥验证逻辑
    //        // 例如，检查密钥是否等于预期的有效密钥
    //        return licenseKey == "MyValidLicenseKey";
    //    }

    //}

    //public class MyLicense : License
    //{
    //    private LicenseProvider provider;

    //    public MyLicense(LicenseProvider provider)
    //    {
    //        this.provider = provider;
    //    }

    //    public override string LicenseKey => "MyLicenseKey";

    //    public override void Dispose()
    //    {
    //        // 释放资源
    //    }
    //}
}