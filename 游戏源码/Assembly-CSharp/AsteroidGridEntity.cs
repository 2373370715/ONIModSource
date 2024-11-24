using System.Collections.Generic;
using Klei.AI;
using KSerialization;

public class AsteroidGridEntity : ClusterGridEntity
{
	public static string DEFAULT_ASTEROID_ICON_ANIM = "asteroid_sandstone_start_kanim";

	[MyCmpReq]
	private WorldContainer m_worldContainer;

	[Serialize]
	private string m_name;

	[Serialize]
	private string m_asteroidAnim;

	public override string Name => m_name;

	public override EntityLayer Layer => EntityLayer.Asteroid;

	public override List<AnimConfig> AnimConfigs => new List<AnimConfig>
	{
		new AnimConfig
		{
			animFile = Assets.GetAnim(m_asteroidAnim.IsNullOrWhiteSpace() ? DEFAULT_ASTEROID_ICON_ANIM : m_asteroidAnim),
			initialAnim = "idle_loop"
		},
		new AnimConfig
		{
			animFile = Assets.GetAnim("orbit_kanim"),
			initialAnim = "orbit"
		},
		new AnimConfig
		{
			animFile = Assets.GetAnim("shower_asteroid_current_kanim"),
			initialAnim = "off",
			playMode = KAnim.PlayMode.Once
		}
	};

	public override bool IsVisible => true;

	public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Peeked;

	public override bool ShowName()
	{
		return true;
	}

	public void Init(string name, AxialI location, string asteroidTypeId)
	{
		m_name = name;
		m_location = location;
		m_asteroidAnim = asteroidTypeId;
	}

	protected override void OnSpawn()
	{
		if (!Assets.TryGetAnim(m_asteroidAnim, out var _))
		{
			m_asteroidAnim = DEFAULT_ASTEROID_ICON_ANIM;
		}
		Game.Instance.Subscribe(-1298331547, OnClusterLocationChanged);
		Game.Instance.Subscribe(-1991583975, OnFogOfWarRevealed);
		Game.Instance.Subscribe(78366336, OnMeteorShowerEventChanged);
		Game.Instance.Subscribe(1749562766, OnMeteorShowerEventChanged);
		if (ClusterGrid.Instance.IsCellVisible(m_location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(m_location, 1);
		}
		base.OnSpawn();
	}

	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1298331547, OnClusterLocationChanged);
		Game.Instance.Unsubscribe(-1991583975, OnFogOfWarRevealed);
		Game.Instance.Unsubscribe(78366336, OnMeteorShowerEventChanged);
		Game.Instance.Unsubscribe(1749562766, OnMeteorShowerEventChanged);
		base.OnCleanUp();
	}

	public void OnClusterLocationChanged(object data)
	{
		if (!m_worldContainer.IsDiscovered && ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			Clustercraft component = ((ClusterLocationChangedEvent)data).entity.GetComponent<Clustercraft>();
			if (!(component == null) && component.GetOrbitAsteroid() == this)
			{
				m_worldContainer.SetDiscovered(reveal_surface: true);
			}
		}
	}

	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		base.OnClusterMapIconShown(levelUsed);
		if (levelUsed == ClusterRevealLevel.Visible)
		{
			RefreshMeteorShowerEffect();
		}
	}

	private void OnMeteorShowerEventChanged(object _worldID)
	{
		if ((int)_worldID == m_worldContainer.id)
		{
			RefreshMeteorShowerEffect();
		}
	}

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
		if (!(animController != null))
		{
			return;
		}
		List<GameplayEventInstance> results = new List<GameplayEventInstance>();
		GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(m_worldContainer.id, ref results);
		bool flag = false;
		string text = "off";
		foreach (GameplayEventInstance item in results)
		{
			if (item != null && item.smi is MeteorShowerEvent.StatesInstance)
			{
				MeteorShowerEvent.StatesInstance obj = item.smi as MeteorShowerEvent.StatesInstance;
				if (obj.IsInsideState(obj.sm.running.bombarding))
				{
					flag = true;
					text = "idle_loop";
					break;
				}
			}
		}
		animController.Play(text, (!flag) ? KAnim.PlayMode.Once : KAnim.PlayMode.Loop);
	}

	public void OnFogOfWarRevealed(object data = null)
	{
		if (data == null || (AxialI)data != m_location || !ClusterGrid.Instance.IsCellVisible(base.Location) || !DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		WorldDetectedMessage message = new WorldDetectedMessage(m_worldContainer);
		MusicManager.instance.PlaySong("Stinger_WorldDetected");
		Messenger.Instance.QueueMessage(message);
		if (m_worldContainer.IsDiscovered)
		{
			return;
		}
		foreach (Clustercraft clustercraft in Components.Clustercrafts)
		{
			if (clustercraft.GetOrbitAsteroid() == this)
			{
				m_worldContainer.SetDiscovered(reveal_surface: true);
				break;
			}
		}
	}
}
