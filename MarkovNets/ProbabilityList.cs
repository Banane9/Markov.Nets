using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkovNets
{
    /// <summary>
    /// Represents a list of the possible items tied to their probabilitiess.
    /// </summary>
    /// <typeparam name="TItem">The type of the items.</typeparam>
    public class ProbabilityList<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// The value that is treated as a 100% chance.
        /// </summary>
        private const int maxProbability = 1000000;

        private readonly List<ItemProbabilityPair<TItem>> itemProbabilityPairs = new List<ItemProbabilityPair<TItem>>();

        /// <summary>
        /// Gets the number of items in the possibility list.
        /// </summary>
        public int Count
        {
            get { return itemProbabilityPairs.Count; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ProbabilityList&lt;TItem&gt;"/> class with the given items. Each item will have the same probability.
        /// </summary>
        /// <param name="items">The items to start with. Each will have the same probability.</param>
        public ProbabilityList(params TItem[] items)
        {
            AddItems(items);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ProbabilityList&lt;TItem&gt;"/> class with the given items. Each item will have the same probability.
        /// </summary>
        /// <param name="items">The items to start with. Each will have the same probability.</param>
        public ProbabilityList(IEnumerable<TItem> items)
        {
            AddItems(items);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ProbabilityList&lt;TItem&gt;"/> class with the given Item - Probability pairs.
        /// </summary>
        /// <param name="items">The Item - Probability pairs to start with.</param>
        public ProbabilityList(params ItemProbabilityPair<TItem> items)
        {
            AddItems(items);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ProbabilityList&lt;TItem&gt;"/> class with the given Item - Probability pairs.
        /// </summary>
        /// <param name="items">The Item - Probability pairs to start with.</param>
        public ProbabilityList(IEnumerable<TItem> items)
        {
            AddItems(items);
        }

        /// <summary>
        /// Adds the Item - Possiblity pairs to the list.
        /// </summary>
        /// <param name="items">The Item - Possiblity pairs to add.</param>
        public void AddItems(params ItemProbabilityPair<TItem>[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items", "Items can't be null!");

            itemProbabilityPairs.AddRange(items.Where(item => item != null));
        }

        /// <summary>
        /// Adds the Item - Possiblity pairs to the list.
        /// </summary>
        /// <param name="items">The Item - Possiblity pairs to add.</param>
        public void AddItems(IEnumerable<ItemProbabilityPair<TItem>> items)
        {
            if (items == null)
                throw new ArgumentNullException("items", "Items can't be null!");

            AddItems(items.ToArray());
        }

        /// <summary>
        /// Adds the given items to the list with a probability for each as if all the ones afterwards being in the list had the same probability.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public void AddItems(params TItem[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items", "Items can't be null!");

            var itemProbability = maxProbability / (items.Length + itemProbabilityPairs.Count);
            var requiredSpace = itemProbability * items.Length;
            foreach (var itemProbabilityPair in itemProbabilityPairs)
            {
                var shrinkBy = requiredSpace / (maxProbability / itemProbabilityPair.Probability);
                itemProbabilityPair.Probability -= shrinkBy;
            }

            itemProbabilityPairs.AddRange(items.Select(item => new ItemProbabilityPair<TItem>(item, itemProbability)));
        }

        /// <summary>
        /// Adds the given items to the list with a probability for each as if all the ones afterwards being in the list had the same probability.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public void AddItems(IEnumerable<TItem> items)
        {
            if (items == null)
                throw new ArgumentNullException("items", "Items can't be null!");

            AddItems(items.ToArray());
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return ((IEnumerable<TItem>)itemProbabilityPairs).GetEnumerator();
        }

        /// <summary>
        /// Gets a random item in the list.
        /// </summary>
        /// <param name="getRandomNumber">Function that returns a random number given firstly the inclusive lowest possible number and secondly the exclusive highest number.</param>
        /// <returns>A random item in the list.</returns>
        public TItem GetRandomItem(Func<int, int, int> getRandomNumber)
        {
            var randomValue = getRandomNumber(0, maxProbability);

            var counter = 0;
            foreach (var itemProbabilityPair in itemProbabilityPairs)
            {
                counter += itemProbabilityPair.Probability;

                if (counter > randomValue)
                    return itemProbabilityPair.Item;
            }

            if (counter < maxProbability)
                throw new Exception("Something went wrong - random value was larger than possible sum of possibility values.");

            return default(TItem);
        }

        /// <summary>
        /// Removes any occurances of the given item and adjusts all others to fill the probability-gap.
        /// </summary>
        /// <param name="item">The item of which all occurances shall be removed.</param>
        public void Remove(TItem item)
        {
            var freedProbability = itemProbabilityPairs.Where(itemProbabilityPair => itemProbabilityPair.Item.Equals(item))
                .Sum(itemProbabilityPair => itemProbabilityPair.Probability);

            if (0 == itemProbabilityPairs.RemoveAll(itemProbabilityPair => itemProbabilityPair.Item.Equals(item)))
                return;

            adjustForFreeProbability(freedProbability);
        }

        /// <summary>
        /// Removes any occurances of the given <see cref="ItemProbabilityPair&lt;TItem&gt;"/> and adjusts all others to fill the probability-gap.
        /// </summary>
        /// <param name="item">The pair of which all occurences shall be removed.</param>
        public void Remove(ItemProbabilityPair<TItem> item)
        {
            var removed = itemProbabilityPairs.RemoveAll(itemProbabilityPair => itemProbabilityPair == item);

            adjustForFreeProbability(item.Probability * removed);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return itemProbabilityPairs.GetEnumerator();
        }

        private void adjustForFreeProbability(int freedProbability)
        {
            var probabilityPerItem = freedProbability / itemProbabilityPairs.Count;
            foreach (var itemProbabilityPair in itemProbabilityPairs)
                itemProbabilityPair.Probability += probabilityPerItem;
        }
    }
}