using System;
using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
	public Promise Run(IEnumerator routine)
	{
		return new Promise(delegate(System.Action resolve)
		{
			StartCoroutine(RunRoutine(routine, resolve));
		});
	}

	public (Promise, System.Action) RunCancellable(IEnumerator routine)
	{
		Promise promise = new Promise();
		Coroutine coroutine = StartCoroutine(RunRoutine(routine, promise.Resolve));
		System.Action item = delegate
		{
			StopCoroutine(coroutine);
		};
		return (promise, item);
	}

	private IEnumerator RunRoutine(IEnumerator routine, System.Action completedCallback)
	{
		yield return routine;
		completedCallback();
	}

	public static CoroutineRunner Create()
	{
		return new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
	}

	public static Promise RunOne(IEnumerator routine)
	{
		CoroutineRunner runner = Create();
		return runner.Run(routine).Then(delegate
		{
			UnityEngine.Object.Destroy(runner.gameObject);
		});
	}
}
