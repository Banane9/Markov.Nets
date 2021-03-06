﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkovNets
{
    /// <summary>
    /// Represents a Markov Chain Generator with a certain possibility network.
    /// </summary>
    /// <typeparam name="TItem">The type of the items.</typeparam>
    public class MarkovGenerator<TItem>
    {
        private readonly Dictionary<ItemChain<TItem>, ProbabilityList<TItem>> itemPossibilities = new Dictionary<ItemChain<TItem>, ProbabilityList<TItem>>();

        private readonly Random random;

        public MarkovGenerator()
        {
            random.Next(,)
        }
    }
}