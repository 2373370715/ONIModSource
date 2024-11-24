using System.Collections.Generic;
using UnityEngine;

public static class SequenceUtil
{
	private static WaitForEndOfFrame waitForEndOfFrame = null;

	private static WaitForFixedUpdate waitForFixedUpdate = null;

	private static Dictionary<float, WaitForSeconds> scaledTimeCache = new Dictionary<float, WaitForSeconds>();

	private static Dictionary<float, WaitForSecondsRealtime> reailTimeWaitCache = new Dictionary<float, WaitForSecondsRealtime>();

	public static YieldInstruction WaitForNextFrame => null;

	public static YieldInstruction WaitForEndOfFrame
	{
		get
		{
			if (waitForEndOfFrame == null)
			{
				waitForEndOfFrame = new WaitForEndOfFrame();
			}
			return waitForEndOfFrame;
		}
	}

	public static YieldInstruction WaitForFixedUpdate
	{
		get
		{
			if (waitForFixedUpdate == null)
			{
				waitForFixedUpdate = new WaitForFixedUpdate();
			}
			return waitForFixedUpdate;
		}
	}

	public static YieldInstruction WaitForSeconds(float duration)
	{
		if (!scaledTimeCache.TryGetValue(duration, out var value))
		{
			return scaledTimeCache[duration] = new WaitForSeconds(duration);
		}
		return value;
	}

	public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
	{
		if (!reailTimeWaitCache.TryGetValue(duration, out var value))
		{
			return reailTimeWaitCache[duration] = new WaitForSecondsRealtime(duration);
		}
		return value;
	}
}
