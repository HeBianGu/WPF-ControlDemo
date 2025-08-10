// See https://aka.ms/new-console-template for more information
using LicenseValidatorTask;

Console.WriteLine("Hello, World!");
try
{

    LicenseService licenseService = new LicenseService();
    LicenseOption option = licenseService.IsVail("VisionMaster", out string error);
    if (option == null)
    {
        //Console.WriteLine(SystemInfo.Instance.HostID);
        Console.WriteLine(error);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
Console.WriteLine("Success");
Console.ReadKey();
