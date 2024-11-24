using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CD5 RID: 7381
public class FrontEndBackground : UIDupeRandomizer
{
	// Token: 0x06009A19 RID: 39449 RVA: 0x003B7EC0 File Offset: 0x003B60C0
	protected override void Start()
	{
		this.tuning = TuningData<FrontEndBackground.Tuning>.Get();
		base.Start();
		for (int i = 0; i < this.anims.Length; i++)
		{
			int minionIndex = i;
			KBatchedAnimController kbatchedAnimController = this.anims[i].minions[0];
			if (kbatchedAnimController.gameObject.activeInHierarchy)
			{
				kbatchedAnimController.onAnimComplete += delegate(HashedString name)
				{
					this.WaitForABit(minionIndex, name);
				};
				this.WaitForABit(i, HashedString.Invalid);
			}
		}
		this.dreckoController = base.transform.GetChild(0).Find("startmenu_drecko").GetComponent<KBatchedAnimController>();
		if (this.dreckoController.gameObject.activeInHierarchy)
		{
			this.dreckoController.enabled = false;
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minFirstDreckoInterval, this.tuning.maxFirstDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x06009A1A RID: 39450 RVA: 0x001044CF File Offset: 0x001026CF
	protected override void Update()
	{
		base.Update();
		this.UpdateDrecko();
	}

	// Token: 0x06009A1B RID: 39451 RVA: 0x003B7FB0 File Offset: 0x003B61B0
	private void UpdateDrecko()
	{
		if (this.dreckoController.gameObject.activeInHierarchy && Time.unscaledTime > this.nextDreckoTime)
		{
			this.dreckoController.enabled = true;
			this.dreckoController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minDreckoInterval, this.tuning.maxDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x06009A1C RID: 39452 RVA: 0x001044DD File Offset: 0x001026DD
	private void WaitForABit(int minion_idx, HashedString name)
	{
		base.StartCoroutine(this.WaitForTime(minion_idx));
	}

	// Token: 0x06009A1D RID: 39453 RVA: 0x001044ED File Offset: 0x001026ED
	private IEnumerator WaitForTime(int minion_idx)
	{
		this.anims[minion_idx].lastWaitTime = UnityEngine.Random.Range(this.anims[minion_idx].minSecondsBetweenAction, this.anims[minion_idx].maxSecondsBetweenAction);
		yield return new WaitForSecondsRealtime(this.anims[minion_idx].lastWaitTime);
		base.GetNewBody(minion_idx);
		using (List<KBatchedAnimController>.Enumerator enumerator = this.anims[minion_idx].minions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KBatchedAnimController kbatchedAnimController = enumerator.Current;
				kbatchedAnimController.ClearQueue();
				kbatchedAnimController.Play(this.anims[minion_idx].anim_name, KAnim.PlayMode.Once, 1f, 0f);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x0400784C RID: 30796
	private KBatchedAnimController dreckoController;

	// Token: 0x0400784D RID: 30797
	private float nextDreckoTime;

	// Token: 0x0400784E RID: 30798
	private FrontEndBackground.Tuning tuning;

	// Token: 0x02001CD6 RID: 7382
	public class Tuning : TuningData<FrontEndBackground.Tuning>
	{
		// Token: 0x0400784F RID: 30799
		public float minDreckoInterval;

		// Token: 0x04007850 RID: 30800
		public float maxDreckoInterval;

		// Token: 0x04007851 RID: 30801
		public float minFirstDreckoInterval;

		// Token: 0x04007852 RID: 30802
		public float maxFirstDreckoInterval;
	}
}
