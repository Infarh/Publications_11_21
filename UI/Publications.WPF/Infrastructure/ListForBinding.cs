using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publications.WPF.Infrastructure;

public class ListForBinding<T> : IList<T>, INotifyCollectionChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => 
        CollectionChanged?.Invoke(this, e);

    protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, object? item = null) =>
        OnCollectionChanged(item is null 
            ? new NotifyCollectionChangedEventArgs(action) 
            : new NotifyCollectionChangedEventArgs(action, item));

    private readonly IList<T> _Items;

    public int Count => _Items.Count;

    public bool IsReadOnly => _Items.IsReadOnly;

    public T this[int index]
    {
        get => _Items[index];
        set => _Items[index] = value;
    }

    public ListForBinding(IList<T> Items) => _Items = Items;

    public void Add(T item)
    {
        _Items.Add(item);
        OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
    }

    public void Clear()
    {
        _Items.Clear();
        OnCollectionChanged(NotifyCollectionChangedAction.Reset);
    }

    public bool Contains(T item) => _Items.Contains(item);

    public void CopyTo(T[] array, int Index) => _Items.CopyTo(array, Index);

    public bool Remove(T item)
    {
        var removed = _Items.Remove(item);
        if(removed)
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
        return removed;
    }

    public int IndexOf(T item) => _Items.IndexOf(item);

    public void Insert(int index, T item)
    {
        _Items.Insert(index, item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
    }

    public void RemoveAt(int index)
    {
        var removed_item = this[index];
        _Items.RemoveAt(index);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed_item, index));
    }

    public IEnumerator<T> GetEnumerator() => _Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_Items).GetEnumerator();
}