using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000A95 RID: 2709
[AddComponentMenu("KMonoBehaviour/scripts/MiningSounds")]
public class MiningSounds : KMonoBehaviour
{
	// Token: 0x0600320A RID: 12810 RVA: 0x000C077A File Offset: 0x000BE97A
	protected override void OnPrefabInit()
	{
		base.Subscribe<MiningSounds>(-1762453998, MiningSounds.OnStartMiningSoundDelegate);
		base.Subscribe<MiningSounds>(939543986, MiningSounds.OnStopMiningSoundDelegate);
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x00201C9C File Offset: 0x001FFE9C
	private void OnStartMiningSound(object data)
	{
		if (this.miningSound == null)
		{
			Element element = data as Element;
			if (element != null)
			{
				string text = element.substance.GetMiningSound();
				if (text == null || text == "")
				{
					return;
				}
				text = "Mine_" + text;
				string sound = GlobalAssets.GetSound(text, false);
				this.miningSoundEvent = RuntimeManager.PathToEventReference(sound);
				if (!this.miningSoundEvent.IsNull)
				{
					this.loopingSounds.StartSound(this.miningSoundEvent);
				}
			}
		}
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x000C079E File Offset: 0x000BE99E
	private void OnStopMiningSound(object data)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.StopSound(this.miningSoundEvent);
			this.miningSound = null;
		}
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x000C07C5 File Offset: 0x000BE9C5
	public void SetPercentComplete(float progress)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.SetParameter(this.miningSoundEvent, MiningSounds.HASH_PERCENTCOMPLETE, progress);
		}
	}

	// Token: 0x040021A5 RID: 8613
	private static HashedString HASH_PERCENTCOMPLETE = "percentComplete";

	// Token: 0x040021A6 RID: 8614
	[MyCmpGet]
	private LoopingSounds loopingSounds;

	// Token: 0x040021A7 RID: 8615
	private FMODAsset miningSound;

	// Token: 0x040021A8 RID: 8616
	private EventReference miningSoundEvent;

	// Token: 0x040021A9 RID: 8617
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStartMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStartMiningSound(data);
	});

	// Token: 0x040021AA RID: 8618
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStopMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStopMiningSound(data);
	});
}
