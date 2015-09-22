using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkovNets
{
    /// <summary>
    /// Represents an Item - Posssibility pairing.
    /// </summary>
    /// <typeparam name="TItem">The type of the Item.</typeparam>
    public class ItemProbabilityPair<TItem>
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        public TItem Item { get; private set; }

        /// <summary>
        /// Gets or sets the possibility.
        /// </summary>
        public int Probability { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="ItemProbabilityPair&lt;TItem&gt;"/> class for the given item and probability.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="probability">The probability of the item.</param>
        public ItemProbabilityPair(TItem item, int probability)
        {
            Item = item;
            Probability = probability;
        }

        #region Equality Stuff

        public static bool operator !=(ItemProbabilityPair<TItem> left, ItemProbabilityPair<TItem> right)
        {
            return !(left == right);
        }

        public static bool operator ==(ItemProbabilityPair<TItem> left, ItemProbabilityPair<TItem> right)
        {
            if (object.ReferenceEquals(left, null) ^ object.ReferenceEquals(right, null))
                return false;

            if (object.ReferenceEquals(left, right))
                return true;

            return left.Probability == right.Probability && left.Item.Equals(right.Item);
        }

        public override bool Equals(object obj)
        {
            var itemProbabilityPairObj = obj as ItemProbabilityPair<TItem>;

            return this == itemProbabilityPairObj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Probability.GetHashCode() * 13 + Item.GetHashCode();
            }
        }

        #endregion Equality Stuff
    }
}