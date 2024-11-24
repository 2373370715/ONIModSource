using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
public static class SequenceUtil
{
	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06001B8D RID: 7053 RVA: 0x000AD332 File Offset: 0x000AB532
	public static YieldInstruction WaitForNextFrame
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06001B8E RID: 7054 RVA: 0x000B1EA2 File Offset: 0x000B00A2
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

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06001B8F RID: 7055 RVA: 0x000B1EBA File Offset: 0x000B00BA
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

	// Token: 0x06001B90 RID: 7056 RVA: 0x001ABF64 File Offset: 0x001AA164
	public static YieldInstruction WaitForSeconds(float duration)
	{
		WaitForSeconds result;
		if (!SequenceUtil.scaledTimeCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.scaledTimeCache[duration] = new WaitForSeconds(duration));
		}
		return result;
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x001ABF98 File Offset: 0x001AA198
	public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
	{
		WaitForSecondsRealtime result;
		if (!SequenceUtil.reailTimeWaitCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.reailTimeWaitCache[duration] = new WaitForSecondsRealtime(duration));
		}
		return result;
	}

	// Token: 0x0400115D RID: 4445
	private static WaitForEndOfFrame waitForEndOfFrame = null;

	// Token: 0x0400115E RID: 4446
	private static WaitForFixedUpdate waitForFixedUpdate = null;

	// Token: 0x0400115F RID: 4447
	private static Dictionary<float, WaitForSeconds> scaledTimeCache = new Dictionary<float, WaitForSeconds>();

	// Token: 0x04001160 RID: 4448
	private static Dictionary<float, WaitForSecondsRealtime> reailTimeWaitCache = new Dictionary<float, WaitForSecondsRealtime>();
}
