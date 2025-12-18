namespace Vben.Common.Sqlsugar.Mvc.Pojo;

public class TreeUtils
{
    public static List<Stree> BuildStree(List<Stree> nodes)
    {
        List<Stree> list = new List<Stree>();
        foreach (var node in nodes)
        {
            if (node.pid == null)
            {
                list.Add(findStreeChildrenByTier(node, nodes));
            }
            else
            {
                bool flag = false;
                foreach (var node2 in nodes)
                {
                    if (node.pid == (node2.id))
                    {
                        flag = true;
                        break;
                    }
                }
    
                if (!flag)
                {
                    list.Add(findStreeChildrenByTier(node, nodes));
                }
            }
        }
    
        return list;
    }
    
    private static Stree findStreeChildrenByTier(Stree node, List<Stree> nodes)
    {
        foreach (var item in nodes)
        {
            if (node.id == item.pid)
            {
                if (node.children == null)
                {
                    node.children = new List<Stree>();
                }
    
                node.children.Add(findStreeChildrenByTier(item, nodes));
            }
        }
    
        return node;
    }

    //递归查找子节点
   
    
    public static List<Ltree> BuildLtree(List<Ltree> nodes)
    {
        List<Ltree> list = new List<Ltree>();
        foreach (var node in nodes)
        {
            if (node.pid == 0)
            {
                list.Add(findLtreeChildrenByTier(node, nodes));
            }
            else
            {
                bool flag = false;
                foreach (var node2 in nodes)
                {
                    if (node.pid == (node2.id))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    list.Add(findLtreeChildrenByTier(node, nodes));
                }
            }
        }

        return list;
    }
    
    private static Ltree findLtreeChildrenByTier(Ltree node, List<Ltree> nodes)
    {
        foreach (var item in nodes)
        {
            if (node.id == item.pid)
            {
                if (node.children == null)
                {
                    node.children = new List<Ltree>();
                }

                node.children.Add(findLtreeChildrenByTier(item, nodes));
            }
        }

        return node;
    }

    //递归查找子节点


    //.net的有bug（子对象缺失属性），java的可以
    // public static List<T> BuildEtree<T>(List<T> nodes) where T : Ztree
    // {
    //     List<T> list = new List<T>();
    //     foreach (var node in nodes)
    //     {
    //         if (node.pid == null)
    //         {
    //             list.Add(findEtreeChildrenByTier(node, nodes));
    //         }
    //         else
    //         {
    //             bool flag = false;
    //             foreach (var node2 in nodes)
    //             {
    //                 if (node.pid == (node2.id))
    //                 {
    //                     flag = true;
    //                     break;
    //                 }
    //             }
    //             if (!flag)
    //             {
    //                 list.Add(findEtreeChildrenByTier(node, nodes));
    //             }
    //         }
    //     }
    //
    //     return list;
    // }

    //递归查找子节点
    // private static T findEtreeChildrenByTier<T>(T node, List<T> nodes) where T : Ztree
    // {
    //     foreach (var item in nodes)
    //     {
    //         if (node.id == item.pid)
    //         {
    //             if (node.children == null)
    //             {
    //                 node.children = new List<Ztree>();
    //             }
    //             node.children.Add(findEtreeChildrenByTier(item, nodes));
    //         }
    //     }
    //     return node;
    // }
}