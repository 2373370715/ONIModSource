using System;
using KSerialization;

// Token: 0x0200168C RID: 5772
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownables : Assignables
{
	// Token: 0x06007731 RID: 30513 RVA: 0x000EE423 File Offset: 0x000EC623
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06007732 RID: 30514 RVA: 0x002B64E0 File Offset: 0x002B46E0
	public void UnassignAll()
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.assignable != null)
			{
				assignableSlotInstance.assignable.Unassign();
			}
		}
	}
}
