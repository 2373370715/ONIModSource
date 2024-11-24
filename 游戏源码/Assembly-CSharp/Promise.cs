using System;
using System.Collections;

public class Promise : IEnumerator
{
	private System.Action on_complete;

	private bool m_is_resolved;

	private static Promise m_instant;

	public bool IsResolved => m_is_resolved;

	object IEnumerator.Current => null;

	public static Promise Instant => m_instant;

	public static Promise Fail => new Promise();

	public Promise(Action<System.Action> fn)
	{
		fn(delegate
		{
			Resolve();
		});
	}

	public Promise()
	{
	}

	public void EnsureResolved()
	{
		if (!IsResolved)
		{
			Resolve();
		}
	}

	public void Resolve()
	{
		DebugUtil.Assert(!m_is_resolved, "Can only resolve a promise once");
		m_is_resolved = true;
		if (on_complete != null)
		{
			on_complete();
			on_complete = null;
		}
	}

	public Promise Then(System.Action callback)
	{
		if (m_is_resolved)
		{
			callback();
		}
		else
		{
			on_complete = (System.Action)Delegate.Combine(on_complete, callback);
		}
		return this;
	}

	public Promise ThenWait(Func<Promise> callback)
	{
		if (m_is_resolved)
		{
			return callback();
		}
		return new Promise(delegate(System.Action resolve)
		{
			on_complete = (System.Action)Delegate.Combine(on_complete, (System.Action)delegate
			{
				callback().Then(resolve);
			});
		});
	}

	public Promise<T> ThenWait<T>(Func<Promise<T>> callback)
	{
		if (m_is_resolved)
		{
			return callback();
		}
		return new Promise<T>(delegate(Action<T> resolve)
		{
			on_complete = (System.Action)Delegate.Combine(on_complete, (System.Action)delegate
			{
				callback().Then(resolve);
			});
		});
	}

	bool IEnumerator.MoveNext()
	{
		return !IsResolved;
	}

	void IEnumerator.Reset()
	{
	}

	static Promise()
	{
		m_instant = new Promise();
		m_instant.Resolve();
	}

	public static Promise All(params Promise[] promises)
	{
		if (promises == null || promises.Length == 0)
		{
			return Instant;
		}
		Promise all_resolved_promise = new Promise();
		Promise[] array = promises;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Then(TryResolve);
		}
		return all_resolved_promise;
		void TryResolve()
		{
			if (!all_resolved_promise.IsResolved)
			{
				Promise[] array2 = promises;
				for (int j = 0; j < array2.Length; j++)
				{
					if (!array2[j].IsResolved)
					{
						return;
					}
				}
				all_resolved_promise.Resolve();
			}
		}
	}

	public static Promise Chain(params Func<Promise>[] make_promise_fns)
	{
		Promise all_resolve_promise = new Promise();
		int current_promise_fn_index = 0;
		TryNext();
		return all_resolve_promise;
		bool DidAll()
		{
			if (make_promise_fns == null || make_promise_fns.Length == 0)
			{
				return true;
			}
			if (make_promise_fns.Length <= current_promise_fn_index)
			{
				return true;
			}
			return false;
		}
		void TryNext()
		{
			if (DidAll())
			{
				all_resolve_promise.Resolve();
			}
			else
			{
				Promise promise = make_promise_fns[current_promise_fn_index]();
				current_promise_fn_index++;
				promise.Then(TryNext);
			}
		}
	}
}
public class Promise<T> : IEnumerator
{
	private Promise promise = new Promise();

	private T result;

	public bool IsResolved => promise.IsResolved;

	object IEnumerator.Current => null;

	public Promise(Action<Action<T>> fn)
	{
		fn(delegate(T value)
		{
			Resolve(value);
		});
	}

	public Promise()
	{
	}

	public void EnsureResolved(T value)
	{
		result = value;
		promise.EnsureResolved();
	}

	public void Resolve(T value)
	{
		result = value;
		promise.Resolve();
	}

	public Promise<T> Then(Action<T> fn)
	{
		promise.Then(delegate
		{
			fn(result);
		});
		return this;
	}

	public Promise ThenWait(Func<Promise> fn)
	{
		return promise.ThenWait(fn);
	}

	public Promise<T> ThenWait(Func<Promise<T>> fn)
	{
		return promise.ThenWait(fn);
	}

	bool IEnumerator.MoveNext()
	{
		return !promise.IsResolved;
	}

	void IEnumerator.Reset()
	{
	}
}
