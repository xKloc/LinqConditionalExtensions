﻿using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class SwitchableQueryableExtensions
    {
        public static ISwitchableQueryable<TSwitch, TSource> Switch<TSwitch, TSource>(this IQueryable<TSource> source, TSwitch @switch)
            => new SwitchableQueryable<TSwitch, TSource>(source, @switch);

        public static ISwitchableQueryable<TSwitch, TSource, TResult> Switch<TSwitch, TSource, TResult>(this IQueryable<TSource> source, TSwitch @switch)
            => new SwitchableQueryable<TSwitch, TSource, TResult>(source, @switch);

        public static ISwitchableQueryable<TSwitch, TSource> Case<TSwitch, TSource>(this ISwitchableQueryable<TSwitch, TSource> switchable, TSwitch match, Func<IQueryable<TSource>, IQueryable<TSource>> expression)
            => switchable.IsMet ? switchable : new SwitchableQueryable<TSwitch, TSource>(switchable.Source, switchable.Switch, expression, match.Equals(switchable.Switch));

        public static ISwitchableQueryable<TSwitch, TSource, TResult> Case<TSwitch, TSource, TResult>(this ISwitchableQueryable<TSwitch, TSource, TResult> switchable, TSwitch match, Func<IQueryable<TSource>, TResult> expression)
            => switchable.IsMet ? switchable : new SwitchableQueryable<TSwitch, TSource, TResult>(switchable.Source, switchable.Switch, expression, match.Equals(switchable.Switch));

        public static IQueryable<TSource> Default<TSwitch, TSource>(this ISwitchableQueryable<TSwitch, TSource> switchable)
            => switchable.IsMet ? switchable.Expression.Invoke(switchable.Source) : switchable.Source;

        public static IQueryable<TSource> Default<TSwitch, TSource>(this ISwitchableQueryable<TSwitch, TSource> switchable, Func<IQueryable<TSource>, IQueryable<TSource>> expression)
            => switchable.IsMet ? switchable.Expression.Invoke(switchable.Source) : expression.Invoke(switchable.Source);

        public static TResult Default<TSwitch, TSource, TResult>(this ISwitchableQueryable<TSwitch, TSource, TResult> switchable, Func<IQueryable<TSource>, TResult> expression)
            => switchable.IsMet ? switchable.Expression.Invoke(switchable.Source) : expression.Invoke(switchable.Source);

        public static ISwitchableQueryable<TSwitch, TSource> OrderByCase<TSwitch, TSource, TKey>(this ISwitchableQueryable<TSwitch, TSource> switchable, TSwitch match, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => switchable.IsMet ? switchable : new SwitchableQueryable<TSwitch, TSource>(switchable.Source, switchable.Switch, e => e.OrderBy(keySelector, comparer), match.Equals(switchable.Switch));

        public static ISwitchableQueryable<TSwitch, TSource> OrderByCase<TSwitch, TSource, TKey>(this ISwitchableQueryable<TSwitch, TSource> switchable, TSwitch match, Expression<Func<TSource, TKey>> keySelector)
            => switchable.IsMet ? switchable : new SwitchableQueryable<TSwitch, TSource>(switchable.Source, switchable.Switch, e => e.OrderBy(keySelector), match.Equals(switchable.Switch));

        public static ISwitchableQueryable<TSwitch, TSource> OrderByDescendingCase<TSwitch, TSource, TKey>(this ISwitchableQueryable<TSwitch, TSource> switchable, TSwitch match, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
            => switchable.IsMet ? switchable : new SwitchableQueryable<TSwitch, TSource>(switchable.Source, switchable.Switch, e => e.OrderByDescending(keySelector, comparer), match.Equals(switchable.Switch));

        public static ISwitchableQueryable<TSwitch, TSource> OrderByDescendingCase<TSwitch, TSource, TKey>(this ISwitchableQueryable<TSwitch, TSource> switchable, TSwitch match, Expression<Func<TSource, TKey>> keySelector)
            => switchable.IsMet ? switchable : new SwitchableQueryable<TSwitch, TSource>(switchable.Source, switchable.Switch, e => e.OrderByDescending(keySelector), match.Equals(switchable.Switch));

        public static ISwitchableQueryable<TSwitch, TSource> WhereCase<TSwitch, TSource, TKey>(this ISwitchableQueryable<TSwitch, TSource> switchable, TSwitch match, Expression<Func<TSource, int, bool>> predicate)
            => switchable.IsMet ? switchable : new SwitchableQueryable<TSwitch, TSource>(switchable.Source, switchable.Switch, e => e.Where(predicate), match.Equals(switchable.Switch));

        public static ISwitchableQueryable<TSwitch, TSource> WhereCase<TSwitch, TSource, TKey>(this ISwitchableQueryable<TSwitch, TSource> switchable, TSwitch match, Expression<Func<TSource, bool>> predicate)
            => switchable.IsMet ? switchable : new SwitchableQueryable<TSwitch, TSource>(switchable.Source, switchable.Switch, e => e.Where(predicate), match.Equals(switchable.Switch));
    }
}