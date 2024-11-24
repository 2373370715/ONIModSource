using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200076A RID: 1898
public class ChoreConsumerState
{
	// Token: 0x0600220F RID: 8719 RVA: 0x001C11D8 File Offset: 0x001BF3D8
	public ChoreConsumerState(ChoreConsumer consumer)
	{
		this.consumer = consumer;
		this.navigator = consumer.GetComponent<Navigator>();
		this.prefabid = consumer.GetComponent<KPrefabID>();
		this.ownable = consumer.GetComponent<Ownable>();
		this.gameObject = consumer.gameObject;
		this.solidTransferArm = consumer.GetComponent<SolidTransferArm>();
		this.hasSolidTransferArm = (this.solidTransferArm != null);
		this.resume = consumer.GetComponent<MinionResume>();
		this.choreDriver = consumer.GetComponent<ChoreDriver>();
		this.schedulable = consumer.GetComponent<Schedulable>();
		this.traits = consumer.GetComponent<Traits>();
		this.choreProvider = consumer.GetComponent<ChoreProvider>();
		MinionIdentity component = consumer.GetComponent<MinionIdentity>();
		if (component != null)
		{
			if (component.assignableProxy == null)
			{
				component.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(component.assignableProxy, component);
			}
			this.assignables = component.GetSoleOwner();
			this.equipment = component.GetEquipment();
		}
		else
		{
			this.assignables = consumer.GetComponent<Assignables>();
			this.equipment = consumer.GetComponent<Equipment>();
		}
		this.storage = consumer.GetComponent<Storage>();
		this.consumableConsumer = consumer.GetComponent<ConsumableConsumer>();
		this.worker = consumer.GetComponent<WorkerBase>();
		this.selectable = consumer.GetComponent<KSelectable>();
		if (this.schedulable != null)
		{
			this.scheduleBlock = this.schedulable.GetSchedule().GetCurrentScheduleBlock();
		}
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x001C132C File Offset: 0x001BF52C
	public void Refresh()
	{
		if (this.schedulable != null)
		{
			Schedule schedule = this.schedulable.GetSchedule();
			if (schedule != null)
			{
				this.scheduleBlock = schedule.GetCurrentScheduleBlock();
			}
		}
	}

	// Token: 0x0400165B RID: 5723
	public KPrefabID prefabid;

	// Token: 0x0400165C RID: 5724
	public GameObject gameObject;

	// Token: 0x0400165D RID: 5725
	public ChoreConsumer consumer;

	// Token: 0x0400165E RID: 5726
	public ChoreProvider choreProvider;

	// Token: 0x0400165F RID: 5727
	public Navigator navigator;

	// Token: 0x04001660 RID: 5728
	public Ownable ownable;

	// Token: 0x04001661 RID: 5729
	public Assignables assignables;

	// Token: 0x04001662 RID: 5730
	public MinionResume resume;

	// Token: 0x04001663 RID: 5731
	public ChoreDriver choreDriver;

	// Token: 0x04001664 RID: 5732
	public Schedulable schedulable;

	// Token: 0x04001665 RID: 5733
	public Traits traits;

	// Token: 0x04001666 RID: 5734
	public Equipment equipment;

	// Token: 0x04001667 RID: 5735
	public Storage storage;

	// Token: 0x04001668 RID: 5736
	public ConsumableConsumer consumableConsumer;

	// Token: 0x04001669 RID: 5737
	public KSelectable selectable;

	// Token: 0x0400166A RID: 5738
	public WorkerBase worker;

	// Token: 0x0400166B RID: 5739
	public SolidTransferArm solidTransferArm;

	// Token: 0x0400166C RID: 5740
	public bool hasSolidTransferArm;

	// Token: 0x0400166D RID: 5741
	public ScheduleBlock scheduleBlock;
}
