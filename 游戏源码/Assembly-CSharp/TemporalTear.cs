using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02001947 RID: 6471
[SerializationConfig(MemberSerialization.OptIn)]
public class TemporalTear : ClusterGridEntity
{
	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x060086F5 RID: 34549 RVA: 0x000F85B6 File Offset: 0x000F67B6
	public override string Name
	{
		get
		{
			return Db.Get().SpaceDestinationTypes.Wormhole.typeName;
		}
	}

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x060086F6 RID: 34550 RVA: 0x000A6603 File Offset: 0x000A4803
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x060086F7 RID: 34551 RVA: 0x0034F384 File Offset: 0x0034D584
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("temporal_tear_kanim"),
					initialAnim = "closed_loop"
				}
			};
		}
	}

	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x060086F8 RID: 34552 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x060086F9 RID: 34553 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060086FA RID: 34554 RVA: 0x000F85CC File Offset: 0x000F67CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ClusterManager.Instance.GetComponent<ClusterPOIManager>().RegisterTemporalTear(this);
		this.UpdateStatus();
	}

	// Token: 0x060086FB RID: 34555 RVA: 0x0034F3C8 File Offset: 0x0034D5C8
	public void UpdateStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		ClusterMapVisualizer clusterMapVisualizer = null;
		if (ClusterMapScreen.Instance != null)
		{
			clusterMapVisualizer = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		}
		if (this.IsOpen())
		{
			if (clusterMapVisualizer != null)
			{
				clusterMapVisualizer.PlayAnim("open_loop", KAnim.PlayMode.Loop);
			}
			component.RemoveStatusItem(Db.Get().MiscStatusItems.TearClosed, false);
			component.AddStatusItem(Db.Get().MiscStatusItems.TearOpen, null);
			return;
		}
		if (clusterMapVisualizer != null)
		{
			clusterMapVisualizer.PlayAnim("closed_loop", KAnim.PlayMode.Loop);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.TearOpen, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.TearClosed, null);
	}

	// Token: 0x060086FC RID: 34556 RVA: 0x000F85EA File Offset: 0x000F67EA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060086FD RID: 34557 RVA: 0x0034F48C File Offset: 0x0034D68C
	public void ConsumeCraft(Clustercraft craft)
	{
		if (this.m_open && craft.Location == base.Location && !craft.IsFlightInProgress())
		{
			for (int i = 0; i < Components.MinionIdentities.Count; i++)
			{
				MinionIdentity minionIdentity = Components.MinionIdentities[i];
				if (minionIdentity.GetMyWorldId() == craft.ModuleInterface.GetInteriorWorld().id)
				{
					Util.KDestroyGameObject(minionIdentity.gameObject);
				}
			}
			craft.DestroyCraftAndModules();
			this.m_hasConsumedCraft = true;
		}
	}

	// Token: 0x060086FE RID: 34558 RVA: 0x000F85F2 File Offset: 0x000F67F2
	public void Open()
	{
		this.m_open = true;
		this.UpdateStatus();
	}

	// Token: 0x060086FF RID: 34559 RVA: 0x000F8601 File Offset: 0x000F6801
	public bool IsOpen()
	{
		return this.m_open;
	}

	// Token: 0x06008700 RID: 34560 RVA: 0x000F8609 File Offset: 0x000F6809
	public bool HasConsumedCraft()
	{
		return this.m_hasConsumedCraft;
	}

	// Token: 0x040065DB RID: 26075
	[Serialize]
	private bool m_open;

	// Token: 0x040065DC RID: 26076
	[Serialize]
	private bool m_hasConsumedCraft;
}
