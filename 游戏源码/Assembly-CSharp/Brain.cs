using System;
using UnityEngine;

// Token: 0x02000633 RID: 1587
[AddComponentMenu("KMonoBehaviour/scripts/Brain")]
public class Brain : KMonoBehaviour
{
	// Token: 0x06001CC1 RID: 7361 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x000B2F62 File Offset: 0x000B1162
	protected override void OnSpawn()
	{
		this.prefabId = base.GetComponent<KPrefabID>();
		this.choreConsumer = base.GetComponent<ChoreConsumer>();
		this.running = true;
		Components.Brains.Add(this);
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06001CC3 RID: 7363 RVA: 0x001AD648 File Offset: 0x001AB848
	// (remove) Token: 0x06001CC4 RID: 7364 RVA: 0x001AD680 File Offset: 0x001AB880
	public event System.Action onPreUpdate;

	// Token: 0x06001CC5 RID: 7365 RVA: 0x000B2F8E File Offset: 0x000B118E
	public virtual void UpdateBrain()
	{
		if (this.onPreUpdate != null)
		{
			this.onPreUpdate();
		}
		if (this.IsRunning())
		{
			this.UpdateChores();
		}
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x000B2FB1 File Offset: 0x000B11B1
	private bool FindBetterChore(ref Chore.Precondition.Context context)
	{
		return this.choreConsumer.FindNextChore(ref context);
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x001AD6B8 File Offset: 0x001AB8B8
	private void UpdateChores()
	{
		if (this.prefabId.HasTag(GameTags.PreventChoreInterruption))
		{
			return;
		}
		Chore.Precondition.Context chore = default(Chore.Precondition.Context);
		if (this.FindBetterChore(ref chore))
		{
			if (this.prefabId.HasTag(GameTags.PerformingWorkRequest))
			{
				base.Trigger(1485595942, null);
				return;
			}
			this.choreConsumer.choreDriver.SetChore(chore);
		}
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x000B2FBF File Offset: 0x000B11BF
	public bool IsRunning()
	{
		return this.running && !this.suspend;
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x000B2FD4 File Offset: 0x000B11D4
	public void Reset(string reason)
	{
		this.Stop("Reset");
		this.running = true;
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x000B2FE8 File Offset: 0x000B11E8
	public void Stop(string reason)
	{
		base.GetComponent<ChoreDriver>().StopChore();
		this.running = false;
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x000B2FFC File Offset: 0x000B11FC
	public void Resume(string caller)
	{
		this.suspend = false;
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x000B3005 File Offset: 0x000B1205
	public void Suspend(string caller)
	{
		this.suspend = true;
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x000B300E File Offset: 0x000B120E
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.Stop("OnCmpDisable");
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x000B3021 File Offset: 0x000B1221
	protected override void OnCleanUp()
	{
		this.Stop("OnCleanUp");
		Components.Brains.Remove(this);
	}

	// Token: 0x040011F1 RID: 4593
	private bool running;

	// Token: 0x040011F2 RID: 4594
	private bool suspend;

	// Token: 0x040011F3 RID: 4595
	protected KPrefabID prefabId;

	// Token: 0x040011F4 RID: 4596
	protected ChoreConsumer choreConsumer;
}
