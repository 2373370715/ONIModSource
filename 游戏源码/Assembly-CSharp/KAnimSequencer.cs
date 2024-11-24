using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200091D RID: 2333
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimSequencer")]
public class KAnimSequencer : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060029D8 RID: 10712 RVA: 0x000BB37B File Offset: 0x000B957B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.mb = base.GetComponent<MinionBrain>();
		if (this.autoRun)
		{
			this.PlaySequence();
		}
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x000BB3A9 File Offset: 0x000B95A9
	public void Reset()
	{
		this.currentIndex = 0;
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x001D728C File Offset: 0x001D548C
	public void PlaySequence()
	{
		if (this.sequence != null && this.sequence.Length != 0)
		{
			if (this.mb != null)
			{
				this.mb.Suspend("AnimSequencer");
			}
			this.kbac.onAnimComplete += this.PlayNext;
			this.PlayNext(null);
		}
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x001D72EC File Offset: 0x001D54EC
	private void PlayNext(HashedString name)
	{
		if (this.sequence.Length > this.currentIndex)
		{
			this.kbac.Play(new HashedString(this.sequence[this.currentIndex].anim), this.sequence[this.currentIndex].mode, this.sequence[this.currentIndex].speed, 0f);
			this.currentIndex++;
			return;
		}
		this.kbac.onAnimComplete -= this.PlayNext;
		if (this.mb != null)
		{
			this.mb.Resume("AnimSequencer");
		}
	}

	// Token: 0x04001BDC RID: 7132
	[Serialize]
	public bool autoRun;

	// Token: 0x04001BDD RID: 7133
	[Serialize]
	public KAnimSequencer.KAnimSequence[] sequence = new KAnimSequencer.KAnimSequence[0];

	// Token: 0x04001BDE RID: 7134
	private int currentIndex;

	// Token: 0x04001BDF RID: 7135
	private KBatchedAnimController kbac;

	// Token: 0x04001BE0 RID: 7136
	private MinionBrain mb;

	// Token: 0x0200091E RID: 2334
	[SerializationConfig(MemberSerialization.OptOut)]
	[Serializable]
	public class KAnimSequence
	{
		// Token: 0x04001BE1 RID: 7137
		public string anim;

		// Token: 0x04001BE2 RID: 7138
		public float speed = 1f;

		// Token: 0x04001BE3 RID: 7139
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}
}
