using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
public class CoroutineRunner : MonoBehaviour
{
	// Token: 0x06001B93 RID: 7059 RVA: 0x000B1EF4 File Offset: 0x000B00F4
	public Promise Run(IEnumerator routine)
	{
		return new Promise(delegate(System.Action resolve)
		{
			this.StartCoroutine(this.RunRoutine(routine, resolve));
		});
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x001ABFCC File Offset: 0x001AA1CC
	public ValueTuple<Promise, System.Action> RunCancellable(IEnumerator routine)
	{
		Promise promise = new Promise();
		Coroutine coroutine = base.StartCoroutine(this.RunRoutine(routine, new System.Action(promise.Resolve)));
		System.Action item = delegate()
		{
			this.StopCoroutine(coroutine);
		};
		return new ValueTuple<Promise, System.Action>(promise, item);
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x000B1F19 File Offset: 0x000B0119
	private IEnumerator RunRoutine(IEnumerator routine, System.Action completedCallback)
	{
		yield return routine;
		completedCallback();
		yield break;
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x000B1F2F File Offset: 0x000B012F
	public static CoroutineRunner Create()
	{
		return new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x001AC020 File Offset: 0x001AA220
	public static Promise RunOne(IEnumerator routine)
	{
		CoroutineRunner runner = CoroutineRunner.Create();
		return runner.Run(routine).Then(delegate
		{
			UnityEngine.Object.Destroy(runner.gameObject);
		});
	}
}
