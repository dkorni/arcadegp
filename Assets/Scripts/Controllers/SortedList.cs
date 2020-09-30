using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Sorted List
/// </summary>
/// <typeparam name="T"></typeparam>
public class SortedList<T> where T : IComparable
{
    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>
    /// The count.
    /// </value>
    public int Count { get; private set; }

    private List<T> _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="SortedList{T}"/> class.
    /// </summary>
    public SortedList()
    {
        // allocate list
        _items = new List<T>(4);
    }

    /// <summary>
    /// Adds the specified item i ASC order.
    /// </summary>
    /// <param name="item">The item.</param>
    public void Add(T item)
    {
        // extend capacity if list is full
        if (Count == _items.Capacity)
            _items.Capacity = _items.Capacity * _items.Capacity;

        for (int i = 0; i < _items.Count; i++)
        {
            // add lesser object before bigger one
            if (item.CompareTo(_items[i]) < 0)
            {
                _items.Insert(i, item);
                Count++;
                return;
            }
        }


        // add to back
        _items.Add(item);
        Count++;
    }

    /// <summary>
    /// Gets the first or default.
    /// </summary>
    /// <returns></returns>
    public T GetFirstOrDefault()
    {
        return _items.FirstOrDefault();
    }

    /// <summary>
    /// Removes the specified item.
    /// </summary>
    /// <param name="item">The item.</param>
    public void Remove(T item)
    {
        _items.Remove(item);
        Count--;
    }

    /// <summary>
    /// Sorts this instance.
    /// </summary>
    public void Sort()
    {
        _items.Sort();
    }
}
