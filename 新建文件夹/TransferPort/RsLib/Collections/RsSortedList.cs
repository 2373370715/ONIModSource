using System;
using System.Collections;
using System.Collections.Generic;

namespace RsLib.Collections;

public class RsSortedList<T> : ICollection<T> where T : IComparable<T> {
    private readonly List<T> _list = new();

    /// <summary>
    ///     是否反序排序
    /// </summary>

    // private bool reverseOrder = false;
    public RsSortedList() { }

    // public RsSortedList(bool reverseOrder)
    // {
    //     this.reverseOrder = reverseOrder;
    // }
    public T this[int index] => _list[index];
    public IEnumerator<T>   GetEnumerator() { return _list.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    public void Add(T item) {
        if (item == null) throw new ArgumentNullException(nameof(item));

        //先跟最后一个比较
        if (_list.Count > 0 && item.CompareTo(_list[_list.Count - 1]) >= 0) {
            _list.Add(item);
            return;
        }

        //一个一个比较然后插入
        for (var i = 0; i < _list.Count; i++) {
            var compareTo = item.CompareTo(_list[i]);
            if (compareTo <= 0) {
                _list.Insert(i, item);
                return;
            }
        }

        //以防万一
        _list.Add(item);
    }

    public void Clear()                           { _list.Clear(); }
    public bool Contains(T item)                  { return _list.Contains(item); }
    public void CopyTo(T[] array, int arrayIndex) { _list.CopyTo(array, arrayIndex); }
    public bool Remove(T   item) { return _list.Remove(item); }
    public int  Count            => _list.Count;
    public bool IsReadOnly       => false;
}