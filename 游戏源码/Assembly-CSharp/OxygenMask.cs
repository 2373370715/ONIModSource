using System;
using UnityEngine;

// Token: 0x02001690 RID: 5776
public class OxygenMask : KMonoBehaviour, ISim200ms
{
	// Token: 0x06007754 RID: 30548 RVA: 0x000EE5E2 File Offset: 0x000EC7E2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxygenMask>(608245985, OxygenMask.OnSuitTankDeltaDelegate);
	}

	// Token: 0x06007755 RID: 30549 RVA: 0x0030DA90 File Offset: 0x0030BC90
	private void CheckOxygenLevels(object data)
	{
		if (this.suitTank.IsEmpty())
		{
			Equippable component = base.GetComponent<Equippable>();
			if (component.assignee != null)
			{
				Ownables soleOwner = component.assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					soleOwner.GetComponent<Equipment>().Unequip(component);
				}
			}
		}
	}

	// Token: 0x06007756 RID: 30550 RVA: 0x0030DADC File Offset: 0x0030BCDC
	public void Sim200ms(float dt)
	{
		if (base.GetComponent<Equippable>().assignee == null)
		{
			float num = this.leakRate * dt;
			float massAvailable = this.storage.GetMassAvailable(this.suitTank.elementTag);
			num = Mathf.Min(num, massAvailable);
			this.storage.DropSome(this.suitTank.elementTag, num, true, true, default(Vector3), true, false);
		}
		if (this.suitTank.IsEmpty())
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x04005931 RID: 22833
	private static readonly EventSystem.IntraObjectHandler<OxygenMask> OnSuitTankDeltaDelegate = new EventSystem.IntraObjectHandler<OxygenMask>(delegate(OxygenMask component, object data)
	{
		component.CheckOxygenLevels(data);
	});

	// Token: 0x04005932 RID: 22834
	[MyCmpGet]
	private SuitTank suitTank;

	// Token: 0x04005933 RID: 22835
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005934 RID: 22836
	private float leakRate = 0.1f;
}
