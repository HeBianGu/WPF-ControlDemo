
using H.Extensions.ValueConverter;

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
}