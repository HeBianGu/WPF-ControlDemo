using H.Extensions.Tree;
using System.Collections;
using System.Windows;
using System.Windows.Markup;
using H.Mvvm;

namespace H.Test.Demo
{
    //public class ShowDialogCommand : MarkupCommandBase
    //{
    //    public override void Execute(object parameter)
    //    {
    //        MessageBox.Show(parameter?.ToString());
    //    }
    //}

    //public class ClassTypeTreeDataProviderExtension : MarkupExtension
    //{
    //    public Type Type { get; set; }
    //    public bool IsRecursion { get; set; } = false;
    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        ClassTypeTree tree = new ClassTypeTree(this.Type);
    //        System.Collections.Generic.IEnumerable<ITreeNode> result = GetTreeNodes(tree, this.IsRecursion);
    //        return result;
    //    }

    //    public IEnumerable<ITreeNode> GetTreeNodes(ITree tree, bool isRecursion = true)
    //    {
    //        IEnumerable children = tree.GetChildren(null);
    //        foreach (object item in children)
    //        {
    //            TreeNodeBase<object> treeNodeBase = new TreeNodeBase<object>(item);
    //            IEnumerable<TreeNodeBase<object>> treeNodes = GetTreeNodes(tree, item, isRecursion);
    //            foreach (var treeNode in treeNodes)
    //            {
    //                treeNodeBase.AddNode(treeNode);
    //            }
    //            yield return treeNodeBase;
    //        }
    //    }

    //    public IEnumerable<TreeNodeBase<object>> GetTreeNodes(ITree tree, object parent, bool isRecursion = true)
    //    {
    //        foreach (object child in tree.GetChildren(parent))
    //        {
    //            TreeNodeBase<object> treeNodeBase = new TreeNodeBase<object>(child);
    //            if (isRecursion)
    //            {
    //                IEnumerable<TreeNodeBase<object>> treeNodes = tree.GetTreeNodes(child);
    //                foreach (var treeNode in treeNodes)
    //                {
    //                    treeNodeBase.AddNode(treeNode);
    //                }
    //            }

    //            yield return treeNodeBase;
    //        }
    //    }
    //}
}