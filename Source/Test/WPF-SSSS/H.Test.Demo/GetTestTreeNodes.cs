
using H.Data.Test;
using H.Providers.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Xml.Linq;

namespace H.Test.Demo
{

    public class GetTestTreeNodes : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var roots = this.GetTreeNodeBases().ToList();
            foreach (var item in roots)
            {
                var n1 = this.GetTreeNodeBases();
                foreach (var item1 in n1)
                {
                    var n2 = this.GetTreeNodeBases();
                    item1.Nodes = n2.ToObservable();
                }
                item.Nodes = n1.ToObservable();
            }
            return roots;
        }
        private IEnumerable<TreeNodeBase<Student>> GetTreeNodeBases()
        {
            int c = Random.Shared.Next(3, 15);
            for (int i = 0; i < c; i++)
            {
                yield return new TreeNodeBase<Student>(Student.Random());
            }

        }
    }
}