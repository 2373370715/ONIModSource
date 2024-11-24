using System;
using KSerialization;

// Token: 0x02000E34 RID: 3636
public class LogicBroadcaster : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x060047B2 RID: 18354 RVA: 0x000CE881 File Offset: 0x000CCA81
	// (set) Token: 0x060047B3 RID: 18355 RVA: 0x000CE889 File Offset: 0x000CCA89
	public int BroadCastChannelID
	{
		get
		{
			return this.broadcastChannelID;
		}
		private set
		{
			this.broadcastChannelID = value;
		}
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x000CE892 File Offset: 0x000CCA92
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.LogicBroadcasters.Add(this);
	}

	// Token: 0x060047B5 RID: 18357 RVA: 0x000CE8A5 File Offset: 0x000CCAA5
	protected override void OnCleanUp()
	{
		Components.LogicBroadcasters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060047B6 RID: 18358 RVA: 0x00253960 File Offset: 0x00251B60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicBroadcaster>(-801688580, LogicBroadcaster.OnLogicValueChangedDelegate);
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, this.IsSpaceVisible());
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x060047B7 RID: 18359 RVA: 0x000CE76B File Offset: 0x000CC96B
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x060047B8 RID: 18360 RVA: 0x000CE8B8 File Offset: 0x000CCAB8
	public int GetCurrentValue()
	{
		return base.GetComponent<LogicPorts>().GetInputValue(this.PORT_ID);
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void OnLogicValueChanged(object data)
	{
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x002539D0 File Offset: 0x00251BD0
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
				return;
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x00253A5C File Offset: 0x00251C5C
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			if (!this.wasOperational)
			{
				this.wasOperational = true;
				this.animController.Queue("on_pre", KAnim.PlayMode.Once, 1f, 0f);
				this.animController.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else if (this.wasOperational)
		{
			this.wasOperational = false;
			this.animController.Queue("on_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.animController.Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x040031D4 RID: 12756
	public static int RANGE = 5;

	// Token: 0x040031D5 RID: 12757
	private static int INVALID_CHANNEL_ID = -1;

	// Token: 0x040031D6 RID: 12758
	public string PORT_ID = "";

	// Token: 0x040031D7 RID: 12759
	private bool wasOperational;

	// Token: 0x040031D8 RID: 12760
	[Serialize]
	private int broadcastChannelID = LogicBroadcaster.INVALID_CHANNEL_ID;

	// Token: 0x040031D9 RID: 12761
	private static readonly EventSystem.IntraObjectHandler<LogicBroadcaster> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicBroadcaster>(delegate(LogicBroadcaster component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040031DA RID: 12762
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x040031DB RID: 12763
	private Guid spaceNotVisibleStatusItem = Guid.Empty;

	// Token: 0x040031DC RID: 12764
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040031DD RID: 12765
	[MyCmpGet]
	private KBatchedAnimController animController;
}
