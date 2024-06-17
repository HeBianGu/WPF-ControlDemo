using H.Extensions.Behvaiors;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace H.Test.Demo
{
    public class DislayDataGrid : DataGrid
    {
        protected override void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
        {
            base.OnAutoGeneratingColumn(e);
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.Attributes.OfType<DisplayAttribute>()?.FirstOrDefault().Name;
                DataGridColumnAttribute columnAttribute = descriptor.Attributes.OfType<DataGridColumnAttribute>().FirstOrDefault();
                if(columnAttribute!=null)
                    e.Column.Width = columnAttribute.Width;
            }

        }
    }
}