using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x0200194E RID: 6478
public class AsteroidGridEntity : ClusterGridEntity
{
	// Token: 0x0600871A RID: 34586 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x0600871B RID: 34587 RVA: 0x000F86CA File Offset: 0x000F68CA
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x0600871C RID: 34588 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Asteroid;
		}
	}

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x0600871D RID: 34589 RVA: 0x0034F988 File Offset: 0x0034DB88
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			List<ClusterGridEntity.AnimConfig> list = new List<ClusterGridEntity.AnimConfig>();
			ClusterGridEntity.AnimConfig item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim(this.m_asteroidAnim.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : this.m_asteroidAnim),
				initialAnim = "idle_loop"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("orbit_kanim"),
				initialAnim = "orbit"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("shower_asteroid_current_kanim"),
				initialAnim = "off",
				playMode = KAnim.PlayMode.Once
			};
			list.Add(item);
			return list;
		}
	}

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x0600871E RID: 34590 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x0600871F RID: 34591 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x06008720 RID: 34592 RVA: 0x000F86D2 File Offset: 0x000F68D2
	public void Init(string name, AxialI location, string asteroidTypeId)
	{
		this.m_name = name;
		this.m_location = location;
		this.m_asteroidAnim = asteroidTypeId;
	}

	// Token: 0x06008721 RID: 34593 RVA: 0x0034FA4C File Offset: 0x0034DC4C
	protected override void OnSpawn()
	{
		KAnimFile kanimFile;
		if (!Assets.TryGetAnim(this.m_asteroidAnim, out kanimFile))
		{
			this.m_asteroidAnim = AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM;
		}
		Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Subscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Subscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		if (ClusterGrid.Instance.IsCellVisible(this.m_location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.m_location, 1);
		}
		base.OnSpawn();
	}

	// Token: 0x06008722 RID: 34594 RVA: 0x0034FB18 File Offset: 0x0034DD18
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Unsubscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		base.OnCleanUp();
	}

	// Token: 0x06008723 RID: 34595 RVA: 0x0034FB98 File Offset: 0x0034DD98
	public void OnClusterLocationChanged(object data)
	{
		if (this.m_worldContainer.IsDiscovered)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		Clustercraft component = ((ClusterLocationChangedEvent)data).entity.GetComponent<Clustercraft>();
		if (component == null)
		{
			return;
		}
		if (component.GetOrbitAsteroid() == this)
		{
			this.m_worldContainer.SetDiscovered(true);
		}
	}

	// Token: 0x06008724 RID: 34596 RVA: 0x000F86E9 File Offset: 0x000F68E9
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		base.OnClusterMapIconShown(levelUsed);
		if (levelUsed == ClusterRevealLevel.Visible)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x06008725 RID: 34597 RVA: 0x000F86FC File Offset: 0x000F68FC
	private void OnMeteorShowerEventChanged(object _worldID)
	{
		if ((int)_worldID == this.m_worldContainer.id)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x06008726 RID: 34598 RVA: 0x0034FBFC File Offset: 0x0034DDFC
	public void RefreshMeteorShowerEffect()
	{
		if (ClusterMapScreen.Instance == null)
		{
			return;
		}
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim == null)
		{
			return;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null)
		{
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(this.m_worldContainer.id, ref list);
			bool flag = false;
			string s = "off";
			foreach (GameplayEventInstance gameplayEventInstance in list)
			{
				if (gameplayEventInstance != null && gameplayEventInstance.smi is MeteorShowerEvent.StatesInstance)
				{
					MeteorShowerEvent.StatesInstance statesInstance = gameplayEventInstance.smi as MeteorShowerEvent.StatesInstance;
					if (statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
					{
						flag = true;
						s = "idle_loop";
						break;
					}
				}
			}
			animController.Play(s, flag ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06008727 RID: 34599 RVA: 0x0034FD04 File Offset: 0x0034DF04
	public void OnFogOfWarRevealed(object data = null)
	{
		if (data == null)
		{
			return;
		}
		if ((AxialI)data != this.m_location)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			WorldDetectedMessage message = new WorldDetectedMessage(this.m_worldContainer);
			MusicManager.instance.PlaySong("Stinger_WorldDetected", false);
			Messenger.Instance.QueueMessage(message);
			if (!this.m_worldContainer.IsDiscovered)
			{
				using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Clustercraft)enumerator.Current).GetOrbitAsteroid() == this)
						{
							this.m_worldContainer.SetDiscovered(true);
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x040065EC RID: 26092
	public static string DEFAULT_ASTEROID_ICON_ANIM = "asteroid_sandstone_start_kanim";

	// Token: 0x040065ED RID: 26093
	[MyCmpReq]
	private WorldContainer m_worldContainer;

	// Token: 0x040065EE RID: 26094
	[Serialize]
	private string m_name;

	// Token: 0x040065EF RID: 26095
	[Serialize]
	private string m_asteroidAnim;
}
