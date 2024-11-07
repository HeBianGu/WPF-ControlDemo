using System.Runtime.Serialization;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace H.Test.Demo4
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

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test test = new Test();
            test.Name = "HeBianGu";

            {
                var txt = JsonConvert.SerializeObject(test);
                System.Diagnostics.Debug.WriteLine(txt);
            }
            {
                var txt = JsonConvert.SerializeObject(test, Formatting.Indented);
                System.Diagnostics.Debug.WriteLine(txt);
            }
            {
                // 序列化接口 1 TypeNameHandling.Auto 2 序列化反序列化都用jsonSerializerSettings
                List<ITest> tests = new List<ITest>();
                tests.Add(new Test());
                tests.Add(new MyTest());

                Data data = new Data();
                data.Tests = tests;

                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.Formatting = Formatting.Indented;
                jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                jsonSerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy=new DefaultNamingStrategy()
                };
                jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                jsonSerializerSettings.StringEscapeHandling=StringEscapeHandling.Default;
                var txt = JsonConvert.SerializeObject(data, jsonSerializerSettings); 
                System.Diagnostics.Debug.WriteLine(txt);
                var datas = JsonConvert.DeserializeObject<Data>(txt, jsonSerializerSettings);
             
            }
        }
    }

    public interface ITest
    {

    }

    public class Data
    {
        public List<ITest> Tests { get; set; } = new List<ITest>();
    }

    public abstract class TestBase : ITest
    {
        public string ID { get; set; }
    }
    public class Test : TestBase
    {
        public string Name { get; set; }
        //public HorizontalAlignment HorizontalAlignment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }

    public class MyTest : TestBase
    {
        public string Member2 { get; set; }
        public string Member3 { get; set; }
        public string Member4 { get; set; }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            Member2 = "This value went into the data file during serialization.";
        }

        [OnSerialized]
        internal void OnSerializedMethod(StreamingContext context)
        {
            Member2 = "This value was reset after serialization.";
        }

        [OnDeserializing]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            Member3 = "This value was set during deserialization";
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Member4 = "This value was set after deserialization.";
        }
    }

}