using System;
using System.Collections.Generic;
using UnityEngine;

public static class SequenceUtil
{
			public static YieldInstruction WaitForNextFrame
	{
		get
		{
			return null;
		}
	}

			public static YieldInstruction WaitForEndOfFrame
	{
		get
		{
			if (SequenceUtil.waitForEndOfFrame == null)
			{
				SequenceUtil.waitForEndOfFrame = new WaitForEndOfFrame();
			}
			return SequenceUtil.waitForEndOfFrame;
		}
	}

			public static YieldInstruction WaitForFixedUpdate
	{
		get
		{
			if (SequenceUtil.waitForFixedUpdate == null)
			{
				SequenceUtil.waitForFixedUpdate = new WaitForFixedUpdate();
			}
			return SequenceUtil.waitForFixedUpdate;
		}
	}

		public static YieldInstruction WaitForSeconds(float duration)
	{
		WaitForSeconds result;
		if (!SequenceUtil.scaledTimeCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.scaledTimeCache[duration] = new WaitForSeconds(duration));
		}
		return result;
	}

		public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
	{
		WaitForSecondsRealtime result;
		if (!SequenceUtil.reailTimeWaitCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.reailTimeWaitCache[duration] = new WaitForSecondsRealtime(duration));
		}
		return result;
	}

		private static WaitForEndOfFrame waitForEndOfFrame = null;

		private static WaitForFixedUpdate waitForFixedUpdate = null;

		private static Dictionary<float, WaitForSeconds> scaledTimeCache = new Dictionary<float, WaitForSeconds>();

		private static Dictionary<float, WaitForSecondsRealtime> reailTimeWaitCache = new Dictionary<float, WaitForSecondsRealtime>();
}
