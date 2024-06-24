using H.Extensions.ValueConverter;
using H.Styles.Default;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace H.Test.Demo
{
    public class GetAgeConverter : MarkupValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                DateTime now = DateTime.Now;
                int age = now.Year - dateTime.Year;
                if (now.Month < dateTime.Month || (now.Month == dateTime.Month && now.Day < dateTime.Day))
                {
                    age--;
                }
                return age > 0 ? age : 0;
            }
            return this.DefaultValue;
        }
    }

    public class MyGenericClass<T>
    {
        public T Value { get; set; }
    }

    public class MyGenericTypeExtension : MarkupExtension
    {
        public Type MyType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return typeof(MyGenericClass<>).MakeGenericType(this.MyType);
        }
    }


    public class MyDataTemplate : DataTemplate
    {
        public MyDataTemplate()
        {

        }
        public MyDataTemplate(object dataType) : base(dataType)
        {

        }
        public Type BaseType { get; set; }
    }

    public class GenericType : MarkupExtension
    {
        public GenericType()
        {

        }

        public GenericType(Type baseType, params Type[] innerTypes)
        {
            BaseType = baseType;
            InnerTypes = innerTypes;
        }

        public Type BaseType { get; set; }

        public Type[] InnerTypes { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Type result = BaseType.MakeGenericType(InnerTypes);
            return result;
        }
    }

    public class MyDataTemplateExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            DataTemplate dataTemplate = new DataTemplate(typeof(MyGenericClass<string>));
            return dataTemplate;
        }
    }
}