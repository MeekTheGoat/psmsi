﻿// Copyright (C) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

using System;
using System.Collections.Generic;

namespace Microsoft.Tools.WindowsInstaller
{
    /// <summary>
    /// A function that takes a single argument of type <typeparamref name="T"/> and returns an object of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of the input argument.</typeparam>
    /// <typeparam name="TResult">The type of the return object.</typeparam>
    /// <param name="arg">The input argument.</param>
    /// <returns>An object of type <typeparamref name="TResult"/>.</returns>
    internal delegate TResult Func<T, TResult>(T arg);

    /// <summary>
    /// Extension methods.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Returns the first item of an enumeration; otherwise, if not found, the default value for type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type being enumerated.</typeparam>
        /// <param name="source">An enumeration of type <typeparamref name="T"/>.</param>
        /// <returns>the first item of an enumeration; otherwise, if not found, the default value for type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> argument is null.</exception>
        internal static T FirstOrDefault<T>(this IEnumerable<T> source)
        {
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }

            var list = source as IList<T>;
            if (null != list && 0 < list.Count)
            {
                return list[0];
            }
            else
            {
                using (var e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        return e.Current;
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An <see cref="IEnumerable&lt;T&gt;"/> whose elements are the result of invoking the transform function on each element of source..</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> or <paramref name="selector"/> argument is null.</exception>
        internal static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }
            else if (null == selector)
            {
                throw new ArgumentNullException("selector");
            }

            // Project each item in a separate method or null checks above are compiled away.
            return ForEach(source, selector);
        }

        /// <summary>
        /// Reeturns the sum of the field selected from each item in the source enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values from which fields are selected to sum.</param>
        /// <param name="selector">A transform function to apply to each element to get the field to sum.</param>
        /// <returns>The sum of the field selected from each item in the source enumerable.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> or <paramref name="selector"/> argument is null.</exception>
        internal static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }
            else if (null == selector)
            {
                throw new ArgumentNullException("selector");
            }

            int sum = 0;
            foreach (int i in ForEach(source, selector))
            {
                sum += i;
            }

            return sum;
        }

        /// <summary>
        /// Creates an array from the enumerable <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An enumerable from which the array is created.</param>
        /// <returns>An array that contains the elements from the input <paramref name="source"/>.</returns>
        internal static T[] ToArray<T>(this IEnumerable<T> source)
        {
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }

            var list = new List<T>(source);
            return list.ToArray();
        }

        /// <summary>
        /// Createsa new list from the enumerable <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An enumerable from which the array is created.</param>
        /// <returns>A list that contains the elements from the input <paramref name="source"/>.</returns>
        internal static IList<T> ToList<T>(this IEnumerable<T> source)
        {
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }

            return new List<T>(source);
        }

        private static IEnumerable<TResult> ForEach<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }
    }
}
