using System;
using System.Text;

namespace Energy.Base
{
    #region Tree

    /// <summary>
    /// Tree generic class
    /// </summary>
    /// <remarks>
    /// Example
    /// <pre>
    ///     Core.Tree&lt;string&gt; tree = new Core.Tree&lt;string&gt;();
    ///
    ///     tree.Children.Add(new Core.TreeNode&lt;string&gt;("a"));
    ///     tree.Children.Add(new Core.TreeNode&lt;string&gt;("b"));
    ///     tree.Children.Add(new Core.TreeNode&lt;string&gt;("c"));
    ///
    ///     tree.Children[1].Children.Add(new Core.TreeNode&lt;string&gt;("d"));
    ///     tree.Children[1].Children.Add(new Core.TreeNode&lt;string&gt;("e"));
    ///
    ///     tree.Children[2].Children.Add(new Core.TreeNode&lt;string&gt;("f"));
    ///     tree.Children[2].Children.Add(new Core.TreeNode&lt;string&gt;("g"));
    ///     tree.Children[2].Children.Add(new Core.TreeNode&lt;string&gt;("h"));
    ///
    ///     Console.WriteLine(tree.ToString("+ "));
    /// </pre>
    /// Expected result
    /// <pre>
    ///     + a
    ///     + b
    ///     + + d
    ///     + + e
    ///     + c
    ///     + + f
    ///     + + g
    ///     + + h
    /// </pre>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class Tree<T> : TreeNode<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public override string ToString(string indent)
        {
            string result = "";
            for (int i = 0; i < children.Count; i++)
            {
                result += children[i].ToString(indent);
            }
            return result;
        }
    }

    #endregion

    #region TreeNode

    /// <summary>
    /// Tree node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T> : IDisposable
    {
        /// <summary>
        /// Node value
        /// </summary>
        public T Value;

        /// <summary>
        /// Optional object associated with node
        /// </summary>
        public object Object;

        private int depth;

        /// <summary>
        /// Deep index
        /// </summary>
        public int Depth { get { return depth; } }

        /// <summary>
        ///
        /// </summary>
        public TreeNode()
        {
            children = new TreeNodeList<T>(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public TreeNode(T value)
        {
            children = new TreeNodeList<T>(this);
            Value = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        public TreeNode(T value, TreeNode<T> parent)
        {
            children = new TreeNodeList<T>(this);
            this.Value = value;
            this.Parent = parent;
        }

        private TreeNode<T> parent;

        /// <summary>
        ///
        /// </summary>
        protected TreeNodeList<T> children;

        /// <summary>
        /// List of tree nodes associated with this node
        /// </summary>
        public TreeNodeList<T> Children { get { return children; } }

        /// <summary>
        ///
        /// </summary>
        public TreeNode<T> Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (value == parent) return;
                if (parent != null) parent.children.Remove(this);
                depth = value == null ? 0 : value.depth + 1;
                if (value != null && !value.children.Contains(this))
                {
                    value.children.Add(this);
                }
                parent = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public TreeNode<T> Root
        {
            get
            {
                TreeNode<T> node = this;
                while (node.parent != null)
                {
                    node = node.parent;
                }
                return node;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            if (parent != null) parent.Children.Remove(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public virtual string ToString(string indent)
        {
            string result = indent;
            if (Value != null) result += Value.ToString();
            if (!String.IsNullOrEmpty(indent)) result += Environment.NewLine;
            for (int i = 0; i < children.Count; i++)
            {
                result += children[i].ToString(indent + indent);
            }
            return result;
        }
    }

    #endregion

    #region TreeNodeList

    /// <summary>
    /// List of tree nodes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNodeList<T> : System.Collections.Generic.List<TreeNode<T>>
    {
        private TreeNode<T> parent;

        /// <summary>
        /// Parent
        /// </summary>
        public TreeNode<T> Parent { get { return parent; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        public TreeNodeList(TreeNode<T> parent)
        {
            this.parent = parent;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new TreeNode<T> Add(TreeNode<T> item)
        {
            base.Add(item);
            return item.Parent = this.parent;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public TreeNode<T> Add(T value)
        {
            TreeNode<T> item = new TreeNode<T>(value, this.parent);
            base.Add(item);
            return item;
        }
    }

    #endregion
}
