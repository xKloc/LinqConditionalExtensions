﻿using System.Collections.Generic;

namespace System.Linq
{
    internal class ConditionalEnumerable<TSource> : IConditionalEnumerable<TSource>
    {
        public ConditionalEnumerable(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TSource>> expression, bool isMet)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            IsMet = isMet;
        }

        public IEnumerable<TSource> Source { get; }

        public Func<IEnumerable<TSource>, IEnumerable<TSource>> Expression { get; }

        public bool IsMet { get; }
    }
}
