using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200098F RID: 2447
[AddComponentMenu("KMonoBehaviour/Workable/AttackableBase")]
public class AttackableBase : Workable, IApproachable
{
	// Token: 0x06002C61 RID: 11361 RVA: 0x001EBBDC File Offset: 0x001E9DDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.attributeConverter = Db.Get().AttributeConverters.AttackDamage;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
		this.SetupScenePartitioner(null);
		base.Subscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate);
		GameUtil.SubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate, true);
		base.Subscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
		base.Subscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x001EBC7C File Offset: 0x001E9E7C
	public float GetDamageMultiplier()
	{
		if (this.attributeConverter != null && base.worker != null)
		{
			AttributeConverterInstance attributeConverter = base.worker.GetAttributeConverter(this.attributeConverter.Id);
			return Mathf.Max(1f + attributeConverter.Evaluate(), 0.1f);
		}
		return 1f;
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x001EBCD4 File Offset: 0x001E9ED4
	private void SetupScenePartitioner(object data = null)
	{
		Extents extents = new Extents(Grid.PosToXY(base.transform.GetPosition()).x, Grid.PosToXY(base.transform.GetPosition()).y, 1, 1);
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.GetComponent<FactionAlignment>(), extents, GameScenePartitioner.Instance.attackableEntitiesLayer, null);
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x000BCB7F File Offset: 0x000BAD7F
	private void OnDefeated(object data = null)
	{
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
	}

	// Token: 0x06002C65 RID: 11365 RVA: 0x000B4E11 File Offset: 0x000B3011
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return 1f;
	}

	// Token: 0x06002C66 RID: 11366 RVA: 0x001EBD44 File Offset: 0x001E9F44
	protected override void OnCleanUp()
	{
		base.Unsubscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate, false);
		GameUtil.UnsubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate);
		base.Unsubscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate, false);
		base.Unsubscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate, false);
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x04001DC9 RID: 7625
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x04001DCA RID: 7626
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<AttackableBase>(GameTags.Dead, delegate(AttackableBase component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04001DCB RID: 7627
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04001DCC RID: 7628
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> SetupScenePartitionerDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		component.SetupScenePartitioner(data);
	});

	// Token: 0x04001DCD RID: 7629
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnCellChangedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		GameScenePartitioner.Instance.UpdatePosition(component.scenePartitionerEntry, Grid.PosToCell(component.gameObject));
	});
}
