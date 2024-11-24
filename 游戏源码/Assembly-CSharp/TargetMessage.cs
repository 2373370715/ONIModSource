using System;
using KSerialization;

// Token: 0x02001E03 RID: 7683
public abstract class TargetMessage : Message
{
	// Token: 0x0600A0DC RID: 41180 RVA: 0x001082E7 File Offset: 0x001064E7
	protected TargetMessage()
	{
	}

	// Token: 0x0600A0DD RID: 41181 RVA: 0x00108973 File Offset: 0x00106B73
	public TargetMessage(KPrefabID prefab_id)
	{
		this.target = new MessageTarget(prefab_id);
	}

	// Token: 0x0600A0DE RID: 41182 RVA: 0x00108987 File Offset: 0x00106B87
	public MessageTarget GetTarget()
	{
		return this.target;
	}

	// Token: 0x0600A0DF RID: 41183 RVA: 0x0010898F File Offset: 0x00106B8F
	public override void OnCleanUp()
	{
		this.target.OnCleanUp();
	}

	// Token: 0x04007D99 RID: 32153
	[Serialize]
	private MessageTarget target;
}
