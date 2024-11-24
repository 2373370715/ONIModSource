using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public readonly struct Updater : IEnumerator
{
	// Token: 0x06001C79 RID: 7289 RVA: 0x000B2B76 File Offset: 0x000B0D76
	public Updater(Func<float, UpdaterResult> fn)
	{
		this.fn = fn;
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x000B2B7F File Offset: 0x000B0D7F
	public UpdaterResult Internal_Update(float deltaTime)
	{
		return this.fn(deltaTime);
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06001C7B RID: 7291 RVA: 0x000AD332 File Offset: 0x000AB532
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x000B2B8D File Offset: 0x000B0D8D
	bool IEnumerator.MoveNext()
	{
		return this.fn(Updater.GetDeltaTime()) == UpdaterResult.NotComplete;
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x000A5E40 File Offset: 0x000A4040
	void IEnumerator.Reset()
	{
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x000B2BA2 File Offset: 0x000B0DA2
	public static implicit operator Updater(Promise promise)
	{
		return Updater.Until(() => promise.IsResolved);
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x000B2BC0 File Offset: 0x000B0DC0
	public static Updater Until(Func<bool> fn)
	{
		return new Updater(delegate(float dt)
		{
			if (!fn())
			{
				return UpdaterResult.NotComplete;
			}
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x000B2BDE File Offset: 0x000B0DDE
	public static Updater While(Func<bool> isTrueFn)
	{
		return new Updater(delegate(float dt)
		{
			if (!isTrueFn())
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000B2BFC File Offset: 0x000B0DFC
	public static Updater While(Func<bool> isTrueFn, Func<Updater> getUpdaterWhileNotTrueFn)
	{
		Updater whileNotTrueUpdater = Updater.None();
		return new Updater(delegate(float dt)
		{
			if (whileNotTrueUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				if (!isTrueFn())
				{
					return UpdaterResult.Complete;
				}
				whileNotTrueUpdater = getUpdaterWhileNotTrueFn();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x000B2C2C File Offset: 0x000B0E2C
	public static Updater None()
	{
		return new Updater((float dt) => UpdaterResult.Complete);
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x000B2C52 File Offset: 0x000B0E52
	public static Updater WaitOneFrame()
	{
		return Updater.WaitFrames(1);
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x000B2C5A File Offset: 0x000B0E5A
	public static Updater WaitFrames(int framesToWait)
	{
		int frame = 0;
		return new Updater(delegate(float dt)
		{
			int frame = frame;
			frame++;
			if (framesToWait <= frame)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x000B2C7F File Offset: 0x000B0E7F
	public static Updater WaitForSeconds(float secondsToWait)
	{
		float currentSeconds = 0f;
		return new Updater(delegate(float dt)
		{
			currentSeconds += dt;
			if (secondsToWait <= currentSeconds)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x000B2CA8 File Offset: 0x000B0EA8
	public static Updater Ease(Action<float> fn, float from, float to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<float>(fn, new Func<float, float, float, float>(Mathf.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000B2CC3 File Offset: 0x000B0EC3
	public static Updater Ease(Action<Vector2> fn, Vector2 from, Vector2 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<Vector2>(fn, new Func<Vector2, Vector2, float, Vector2>(Vector2.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x000B2CDE File Offset: 0x000B0EDE
	public static Updater Ease(Action<Vector3> fn, Vector3 from, Vector3 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<Vector3>(fn, new Func<Vector3, Vector3, float, Vector3>(Vector3.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x001AD28C File Offset: 0x001AB48C
	public static Updater GenericEase<T>(Action<T> useFn, Func<T, T, float, T> interpolateFn, Easing.EasingFn easingFn, T from, T to, float duration, float delay)
	{
		Updater.<>c__DisplayClass18_0<T> CS$<>8__locals1 = new Updater.<>c__DisplayClass18_0<T>();
		CS$<>8__locals1.useFn = useFn;
		CS$<>8__locals1.interpolateFn = interpolateFn;
		CS$<>8__locals1.from = from;
		CS$<>8__locals1.to = to;
		CS$<>8__locals1.easingFn = easingFn;
		CS$<>8__locals1.duration = duration;
		if (CS$<>8__locals1.easingFn == null)
		{
			CS$<>8__locals1.easingFn = Easing.SmoothStep;
		}
		CS$<>8__locals1.currentSeconds = 0f;
		CS$<>8__locals1.<GenericEase>g__UseKeyframeAt|0(0f);
		Updater updater = new Updater(delegate(float dt)
		{
			CS$<>8__locals1.currentSeconds += dt;
			if (CS$<>8__locals1.currentSeconds < CS$<>8__locals1.duration)
			{
				base.<GenericEase>g__UseKeyframeAt|0(CS$<>8__locals1.currentSeconds / CS$<>8__locals1.duration);
				return UpdaterResult.NotComplete;
			}
			base.<GenericEase>g__UseKeyframeAt|0(1f);
			return UpdaterResult.Complete;
		});
		if (delay > 0f)
		{
			return Updater.Series(new Updater[]
			{
				Updater.WaitForSeconds(delay),
				updater
			});
		}
		return updater;
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x000B2CF9 File Offset: 0x000B0EF9
	public static Updater Do(System.Action fn)
	{
		return new Updater(delegate(float dt)
		{
			fn();
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x000B2D17 File Offset: 0x000B0F17
	public static Updater Do(Func<Updater> fn)
	{
		bool didInitalize = false;
		Updater target = default(Updater);
		return new Updater(delegate(float dt)
		{
			if (!didInitalize)
			{
				target = fn();
				didInitalize = true;
			}
			return target.Internal_Update(dt);
		});
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x000B2D48 File Offset: 0x000B0F48
	public static Updater Loop(params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(Option.None, makeUpdaterFns);
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000B2D5A File Offset: 0x000B0F5A
	public static Updater Loop(int loopCount, params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(loopCount, makeUpdaterFns);
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x001AD334 File Offset: 0x001AB534
	public static Updater Internal_Loop(Option<int> limitLoopCount, Func<Updater>[] makeUpdaterFns)
	{
		if (makeUpdaterFns == null || makeUpdaterFns.Length == 0)
		{
			return Updater.None();
		}
		int completedLoopCount = 0;
		int currentIndex = 0;
		Updater currentUpdater = makeUpdaterFns[currentIndex]();
		return new Updater(delegate(float dt)
		{
			if (currentUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				int num = currentIndex;
				currentIndex = num + 1;
				if (currentIndex >= makeUpdaterFns.Length)
				{
					currentIndex -= makeUpdaterFns.Length;
					num = completedLoopCount;
					completedLoopCount = num + 1;
					if (limitLoopCount.IsSome() && completedLoopCount >= limitLoopCount.Unwrap())
					{
						return UpdaterResult.Complete;
					}
				}
				currentUpdater = makeUpdaterFns[currentIndex]();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C8F RID: 7311 RVA: 0x000B2D68 File Offset: 0x000B0F68
	public static Updater Parallel(params Updater[] updaters)
	{
		bool[] isCompleted = new bool[updaters.Length];
		return new Updater(delegate(float dt)
		{
			bool flag = false;
			for (int i = 0; i < updaters.Length; i++)
			{
				if (!isCompleted[i])
				{
					if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
					{
						isCompleted[i] = true;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x000B2D99 File Offset: 0x000B0F99
	public static Updater Series(params Updater[] updaters)
	{
		int i = 0;
		return new Updater(delegate(float dt)
		{
			int i;
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
			{
				i = i;
				i++;
			}
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x001AD3A4 File Offset: 0x001AB5A4
	public static Promise RunRoutine(MonoBehaviour monoBehaviour, IEnumerator coroutine)
	{
		Updater.<>c__DisplayClass26_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass26_0();
		CS$<>8__locals1.coroutine = coroutine;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<RunRoutine>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x000B2DBE File Offset: 0x000B0FBE
	public static Promise Run(MonoBehaviour monoBehaviour, params Updater[] updaters)
	{
		return Updater.Run(monoBehaviour, Updater.Series(updaters));
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x001AD3DC File Offset: 0x001AB5DC
	public static Promise Run(MonoBehaviour monoBehaviour, Updater updater)
	{
		Updater.<>c__DisplayClass28_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass28_0();
		CS$<>8__locals1.updater = updater;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<Run>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x000B2DCC File Offset: 0x000B0FCC
	public static float GetDeltaTime()
	{
		return Time.unscaledDeltaTime;
	}

	// Token: 0x040011C6 RID: 4550
	public readonly Func<float, UpdaterResult> fn;
}
