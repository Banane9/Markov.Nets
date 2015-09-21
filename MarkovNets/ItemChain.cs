using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkovNets
{
    /// <summary>
    /// Represents a chain of items.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class ItemChain<TItem> : IEnumerable<TItem>
    {
        private readonly TItem[] items;

        /// <summary>
        /// Gets the length of the Item Chain.
        /// </summary>
        public int Length
        {
            get { return items.Length; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ItemChain&lt;TItem&gt;"/> class with the given items.
        /// </summary>
        /// <param name="items">The items in the chain.</param>
        public ItemChain(params TItem[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items", "Items can't be null!");

            this.items = items;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ItemChain&lt;TItem&gt;"/> class with the given items.
        /// </summary>
        /// <param name="items">The items in the chain.</param>
        public ItemChain(IEnumerable<TItem> items)
        {
            if (items == null)
                throw new ArgumentNullException("items", "Items can't be null!");

            this.items = items.ToArray();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return ((IEnumerable<TItem>)items).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public override string ToString()
        {
            return items.ToString();
        }

        #region Equality Stuff

        public static bool operator !=(ItemChain<TItem> left, ItemChain<TItem> right)
        {
            return !(left == right);
        }

        public static bool operator ==(ItemChain<TItem> left, ItemChain<TItem> right)
        {
            if (object.ReferenceEquals(left, null) ^ object.ReferenceEquals(right, null))
                return false;

            if (object.ReferenceEquals(left, right))
                return true;

            if (left.Length != right.Length)
                return false;

            for (var i = 0; i < left.Length; ++i)
                if (!left.items[i].Equals(right.items[i]))
                    return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            var itemChainObj = obj as ItemChain<TItem>;

            return this == itemChainObj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 0;
                foreach (var item in items)
                    hash = hash * 13 + item.GetHashCode();

                return hash;
            }
        }

        #endregion Equality Stuff
    }
}