using System;
using System.Collections;
using UnityEngine;

public readonly struct Updater : IEnumerator
{
	public readonly Func<float, UpdaterResult> fn;

	object IEnumerator.Current => null;

	public Updater(Func<float, UpdaterResult> fn)
	{
		this.fn = fn;
	}

	public UpdaterResult Internal_Update(float deltaTime)
	{
		return fn(deltaTime);
	}

	bool IEnumerator.MoveNext()
	{
		return fn(GetDeltaTime()) == UpdaterResult.NotComplete;
	}

	void IEnumerator.Reset()
	{
	}

	public static implicit operator Updater(Promise promise)
	{
		return Until(() => promise.IsResolved);
	}

	public static Updater Until(Func<bool> fn)
	{
		return new Updater((float dt) => fn() ? UpdaterResult.Complete : UpdaterResult.NotComplete);
	}

	public static Updater While(Func<bool> isTrueFn)
	{
		return new Updater((float dt) => (!isTrueFn()) ? UpdaterResult.Complete : UpdaterResult.NotComplete);
	}

	public static Updater While(Func<bool> isTrueFn, Func<Updater> getUpdaterWhileNotTrueFn)
	{
		Updater whileNotTrueUpdater = None();
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

	public static Updater None()
	{
		return new Updater((float dt) => UpdaterResult.Complete);
	}

	public static Updater WaitOneFrame()
	{
		return WaitFrames(1);
	}

	public static Updater WaitFrames(int framesToWait)
	{
		int frame = 0;
		return new Updater(delegate
		{
			frame++;
			return (framesToWait <= frame) ? UpdaterResult.Complete : UpdaterResult.NotComplete;
		});
	}

	public static Updater WaitForSeconds(float secondsToWait)
	{
		float currentSeconds = 0f;
		return new Updater(delegate(float dt)
		{
			currentSeconds += dt;
			return (secondsToWait <= currentSeconds) ? UpdaterResult.Complete : UpdaterResult.NotComplete;
		});
	}

	public static Updater Ease(Action<float> fn, float from, float to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return GenericEase(fn, Mathf.LerpUnclamped, easing, from, to, duration, delay);
	}

	public static Updater Ease(Action<Vector2> fn, Vector2 from, Vector2 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return GenericEase(fn, Vector2.LerpUnclamped, easing, from, to, duration, delay);
	}

	public static Updater Ease(Action<Vector3> fn, Vector3 from, Vector3 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return GenericEase(fn, Vector3.LerpUnclamped, easing, from, to, duration, delay);
	}

	public static Updater GenericEase<T>(Action<T> useFn, Func<T, T, float, T> interpolateFn, Easing.EasingFn easingFn, T from, T to, float duration, float delay)
	{
		if (easingFn == null)
		{
			easingFn = Easing.SmoothStep;
		}
		float currentSeconds = 0f;
		UseKeyframeAt(0f);
		Updater updater = new Updater(delegate(float dt)
		{
			currentSeconds += dt;
			if (currentSeconds < duration)
			{
				UseKeyframeAt(currentSeconds / duration);
				return UpdaterResult.NotComplete;
			}
			UseKeyframeAt(1f);
			return UpdaterResult.Complete;
		});
		if (delay > 0f)
		{
			return Series(WaitForSeconds(delay), updater);
		}
		return updater;
		void UseKeyframeAt(float progress01)
		{
			useFn(interpolateFn(from, to, easingFn(progress01)));
		}
	}

	public static Updater Do(System.Action fn)
	{
		return new Updater(delegate
		{
			fn();
			return UpdaterResult.Complete;
		});
	}

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

	public static Updater Loop(params Func<Updater>[] makeUpdaterFns)
	{
		return Internal_Loop(Option.None, makeUpdaterFns);
	}

	public static Updater Loop(int loopCount, params Func<Updater>[] makeUpdaterFns)
	{
		return Internal_Loop(loopCount, makeUpdaterFns);
	}

	public static Updater Internal_Loop(Option<int> limitLoopCount, Func<Updater>[] makeUpdaterFns)
	{
		if (makeUpdaterFns == null || makeUpdaterFns.Length == 0)
		{
			return None();
		}
		int completedLoopCount = 0;
		int currentIndex = 0;
		Updater currentUpdater = makeUpdaterFns[currentIndex]();
		return new Updater(delegate(float dt)
		{
			if (currentUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				currentIndex++;
				if (currentIndex >= makeUpdaterFns.Length)
				{
					currentIndex -= makeUpdaterFns.Length;
					completedLoopCount++;
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
			return (!flag) ? UpdaterResult.Complete : UpdaterResult.NotComplete;
		});
	}

	public static Updater Series(params Updater[] updaters)
	{
		int i = 0;
		return new Updater(delegate(float dt)
		{
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
			{
				i++;
			}
			return (i == updaters.Length) ? UpdaterResult.Complete : UpdaterResult.NotComplete;
		});
	}

	public static Promise RunRoutine(MonoBehaviour monoBehaviour, IEnumerator coroutine)
	{
		Promise willComplete = new Promise();
		monoBehaviour.StartCoroutine(Routine());
		return willComplete;
		IEnumerator Routine()
		{
			yield return coroutine;
			willComplete.Resolve();
		}
	}

	public static Promise Run(MonoBehaviour monoBehaviour, params Updater[] updaters)
	{
		return Run(monoBehaviour, Series(updaters));
	}

	public static Promise Run(MonoBehaviour monoBehaviour, Updater updater)
	{
		Promise willComplete = new Promise();
		monoBehaviour.StartCoroutine(Routine());
		return willComplete;
		IEnumerator Routine()
		{
			while (updater.Internal_Update(GetDeltaTime()) == UpdaterResult.NotComplete)
			{
				yield return null;
			}
			willComplete.Resolve();
		}
	}

	public static float GetDeltaTime()
	{
		return Time.unscaledDeltaTime;
	}
}
