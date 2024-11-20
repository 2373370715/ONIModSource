using System;
using System.Collections;
using System.Collections.Generic;
using RsLib.Collections;



public class PriorityChannelItemList : IList<PriorityChannelItemInfo> {
    private readonly RsSortedList<PriorityChannelItemInfo> priorityList = new();
    public void Add(PriorityChannelItemInfo item) { throw new NotImplementedException(); }
    public void Clear() { priorityList.Clear(); }
    public bool Contains(PriorityChannelItemInfo item) { throw new NotImplementedException(); }
    public void CopyTo(PriorityChannelItemInfo[] array, int arrayIndex) { throw new NotImplementedException(); }
    public bool Remove(PriorityChannelItemInfo item) { throw new NotImplementedException(); }
    public int Count => priorityList.Count;
    public bool IsReadOnly => true;
    public IEnumerator<PriorityChannelItemInfo> GetEnumerator() { return priorityList.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    public int IndexOf(PriorityChannelItemInfo item) { throw new NotImplementedException(); }
    public void Insert(int index, PriorityChannelItemInfo item) { throw new NotImplementedException(); }
    public void RemoveAt(int index) { throw new NotImplementedException(); }

    public PriorityChannelItemInfo this[int index] {
        get => priorityList[index];
        set => throw new NotImplementedException();
    }

    public void AddChannelItem(TransferPortChannel item) { AddChannelItem(item, true); }

    private void AddChannelItem(TransferPortChannel item, bool addEvent) {
        var itemInfo = GetOrAddPriorityInfo(item.Priority);
        itemInfo.items.Add(item);
        if (addEvent) item.OnPriorityChange += ItemOnOnPriorityChange;
    }

    private void ItemOnOnPriorityChange(TransferPortChannel channel, int newPriority, int oldPriority) {
        RemoveChannelItem(channel, false);
        AddChannelItem(channel, false);
    }

    public void RemoveChannelItem(TransferPortChannel item) { RemoveChannelItem(item, true); }

    public void RemoveChannelItem(TransferPortChannel item, bool removeEvent) {
        foreach (var info in priorityList)
            if (info.items.Remove(item)) {
                if (removeEvent) item.OnPriorityChange -= ItemOnOnPriorityChange;
                if (info.items.Count == 0)

                    //移除
                    priorityList.Remove(info);

                return;
            }
    }

    private PriorityChannelItemInfo GetOrAddPriorityInfo(int priority) {
        foreach (var itemInfo in priorityList)
            if (itemInfo.priority == priority)
                return itemInfo;

        var info = new PriorityChannelItemInfo();
        info.priority = priority;
        priorityList.Add(info);
        return info;
    }

    public PriorityChannelItemInfo GetByPriority(int priority) {
        foreach (var itemInfo in priorityList)
            if (itemInfo.priority == priority)
                return itemInfo;

        return null;
    }

    public int[] AllPriority() {
        var priorities                                             = new int[priorityList.Count];
        for (var i = 0; i < priorityList.Count; i++) priorities[i] = priorityList[i].priority;

        return priorities;
    }
}