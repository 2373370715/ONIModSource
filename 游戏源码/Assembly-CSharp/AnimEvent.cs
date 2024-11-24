using System;
using UnityEngine;

// Token: 0x02000906 RID: 2310
[Serializable]
public class AnimEvent
{
	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06002901 RID: 10497 RVA: 0x000BAA88 File Offset: 0x000B8C88
	// (set) Token: 0x06002902 RID: 10498 RVA: 0x000BAA90 File Offset: 0x000B8C90
	[SerializeField]
	public string name { get; private set; }

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06002903 RID: 10499 RVA: 0x000BAA99 File Offset: 0x000B8C99
	// (set) Token: 0x06002904 RID: 10500 RVA: 0x000BAAA1 File Offset: 0x000B8CA1
	[SerializeField]
	public string file { get; private set; }

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06002905 RID: 10501 RVA: 0x000BAAAA File Offset: 0x000B8CAA
	// (set) Token: 0x06002906 RID: 10502 RVA: 0x000BAAB2 File Offset: 0x000B8CB2
	[SerializeField]
	public int frame { get; private set; }

	// Token: 0x06002907 RID: 10503 RVA: 0x000A5E2C File Offset: 0x000A402C
	public AnimEvent()
	{
	}

	// Token: 0x06002908 RID: 10504 RVA: 0x001D4548 File Offset: 0x001D2748
	public AnimEvent(string file, string name, int frame)
	{
		this.file = ((file == "") ? null : file);
		if (this.file != null)
		{
			this.fileHash = new KAnimHashedString(this.file);
		}
		this.name = name;
		this.frame = frame;
	}

	// Token: 0x06002909 RID: 10505 RVA: 0x001D459C File Offset: 0x001D279C
	public void Play(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.IsFilteredOut(behaviour))
		{
			return;
		}
		if (behaviour.previousFrame < behaviour.currentFrame)
		{
			if (behaviour.previousFrame < this.frame && behaviour.currentFrame >= this.frame)
			{
				this.OnPlay(behaviour);
				return;
			}
		}
		else if (behaviour.previousFrame > behaviour.currentFrame && (behaviour.previousFrame < this.frame || this.frame <= behaviour.currentFrame))
		{
			this.OnPlay(behaviour);
		}
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void DebugAnimEvent(string ev_name, AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void OnUpdate(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Stop(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x000BAABB File Offset: 0x000B8CBB
	protected bool IsFilteredOut(AnimEventManager.EventPlayerData behaviour)
	{
		return this.file != null && !behaviour.controller.HasAnimationFile(this.fileHash);
	}

	// Token: 0x04001B5A RID: 7002
	[SerializeField]
	private KAnimHashedString fileHash;

	// Token: 0x04001B5C RID: 7004
	public bool OnExit;
}
