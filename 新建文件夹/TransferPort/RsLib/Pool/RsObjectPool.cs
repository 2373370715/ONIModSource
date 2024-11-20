using System;
using System.Collections.Generic;

namespace RsLib.Pool;

public class RsObjectPool<T> {
    private readonly Func<T>   createFn;
    private readonly Stack<T>  pool;
    private readonly Action<T> releaseFn;

    public RsObjectPool(Func<T> createFn, Action<T> releaseFn) {
        this.createFn  = createFn;
        this.releaseFn = releaseFn;
        pool           = new Stack<T>();
    }

    public T Get() {
        if (pool.Count == 0) { return createFn(); }

        return pool.Pop();
    }

    public void Release(T t) {
        pool.Push(t);
        releaseFn(t);
    }
}