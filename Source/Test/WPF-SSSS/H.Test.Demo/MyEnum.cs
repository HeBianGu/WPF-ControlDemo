using H.Extensions.TypeConverter;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace H.Test.Demo
{
    [TypeConverter(typeof(DisplayEnumConverter))]
    public enum MyEnum
    {
        [Display(Name = "无")]
        None,
        [Display(Name = "第一个")]
        First,
        [Display(Name = "第二个")]
        Second,
        [Display(Name = "第三个")]
        Third
    }
}