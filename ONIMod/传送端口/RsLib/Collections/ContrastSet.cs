using System;
using System.Collections.Generic;

namespace RsLib.Collections;

public class ContrastSet<T> {
    private readonly Action<T>  onAdd;
    private readonly Action<T>  onRemove;
    private readonly HashSet<T> source = new();
    private readonly HashSet<T> target = new();

    public ContrastSet(Action<T> onAdd, Action<T> onRemove) {
        this.onAdd    = onAdd;
        this.onRemove = onRemove;
    }

    public void Add(T    obj) { source.Add(obj); }
    public void Remove(T obj) { source.Remove(obj); }
    public void StartRecord() { source.Clear(); }

    public void EndAndContrast() {
        RsUtil.ContrastSet(source, target, onAdd, onRemove);
        source.Clear();
    }

    public void Clear() {
        StartRecord();
        EndAndContrast();
    }
}