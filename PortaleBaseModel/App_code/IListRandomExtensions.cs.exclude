using System;
using System.Collections.Generic;

/// <summary>
/// Adds extension methods for System.Collections.Generic.IList&lt;T&gt;.
/// </summary>
public static class IListRandomExtensions
{
    private static Random random = new Random();

    /// <summary>
    /// Randomly shuffles the elements within the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">Represents a collection of objects that can be individually accessed by index.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        if (list.Count <= 1)
        {
            return; // nothing to do
        }

        for (int i = 0; i < list.Count; i++)
        {
            int newIndex = random.Next(0, list.Count);

            // swap the two elements over
            T x = list[i];
            list[i] = list[newIndex];
            list[newIndex] = x;
        }
    }

    /// <summary>
    /// Returns a single element from the list, selected at random.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">Represents a collection of objects that can be individually accessed by index.</param>
    /// <returns>An element from the list or the default value if no elements exist.</returns>
    public static T GetRandom<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            return default(T);
        }

        return list[random.Next(0, list.Count)];
    }
}

