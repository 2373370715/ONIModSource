using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000CA5 RID: 3237
[AddComponentMenu("KMonoBehaviour/Workable/Bed")]
public class Bed : Workable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06003E8A RID: 16010 RVA: 0x002346C4 File Offset: 0x002328C4
	private bool CanSleepOwnablePrecondition(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = (Db.Get().Amounts.Stamina.Lookup(minionIdentity) != null);
		}
		return result;
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x000C8ADE File Offset: 0x000C6CDE
	protected override void OnPrefabInit()
	{
		this.ownable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanSleepOwnablePrecondition));
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x00234704 File Offset: 0x00232904
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.BasicBuildings.Add(this);
		this.sleepable = base.GetComponent<Sleepable>();
		Sleepable sleepable = this.sleepable;
		sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x000C8B04 File Offset: 0x000C6D04
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			this.AddEffects();
			return;
		}
		if (workable_event == Workable.WorkableEvent.WorkStopped)
		{
			this.RemoveEffects();
		}
	}

	// Token: 0x06003E8E RID: 16014 RVA: 0x00234758 File Offset: 0x00232958
	private void AddEffects()
	{
		this.targetWorker = this.sleepable.worker;
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Add(effect_id, false);
			}
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject == null)
		{
			return;
		}
		RoomType roomType = roomOfGameObject.roomType;
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			if (keyValuePair.Key == roomType.Id)
			{
				this.targetWorker.GetComponent<Effects>().Add(keyValuePair.Value, false);
			}
		}
		roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), this.targetWorker.GetComponent<Effects>());
	}

	// Token: 0x06003E8F RID: 16015 RVA: 0x00234854 File Offset: 0x00232A54
	private void RemoveEffects()
	{
		if (this.targetWorker == null)
		{
			return;
		}
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Remove(effect_id);
			}
		}
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			this.targetWorker.GetComponent<Effects>().Remove(keyValuePair.Value);
		}
		this.targetWorker = null;
	}

	// Token: 0x06003E90 RID: 16016 RVA: 0x00234900 File Offset: 0x00232B00
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.effects != null)
		{
			foreach (string text in this.effects)
			{
				if (text != null && text != "")
				{
					Effect.AddModifierDescriptions(base.gameObject, list, text, false);
				}
			}
		}
		return list;
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x00234954 File Offset: 0x00232B54
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		if (this.sleepable != null)
		{
			Sleepable sleepable = this.sleepable;
			sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		}
	}

	// Token: 0x04002AB4 RID: 10932
	[MyCmpReq]
	private Ownable ownable;

	// Token: 0x04002AB5 RID: 10933
	[MyCmpReq]
	private Sleepable sleepable;

	// Token: 0x04002AB6 RID: 10934
	private WorkerBase targetWorker;

	// Token: 0x04002AB7 RID: 10935
	public string[] effects;

	// Token: 0x04002AB8 RID: 10936
	private static Dictionary<string, string> roomSleepingEffects = new Dictionary<string, string>
	{
		{
			"Barracks",
			"BarracksStamina"
		},
		{
			"Luxury Barracks",
			"BarracksStamina"
		},
		{
			"Private Bedroom",
			"BedroomStamina"
		}
	};
}
