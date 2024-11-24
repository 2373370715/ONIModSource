using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

public abstract class ClusterGridEntity : KMonoBehaviour
{
	public struct AnimConfig
	{
		public KAnimFile animFile;

		public string initialAnim;

		public KAnim.PlayMode playMode;

		public string symbolSwapTarget;

		public string symbolSwapSymbol;

		public Vector3 animOffset;

		public float animPlaySpeedModifier;
	}

	[Serialize]
	protected AxialI m_location;

	public bool positionDirty;

	[MyCmpGet]
	protected KSelectable m_selectable;

	[MyCmpReq]
	private Transform m_transform;

	public bool isWorldEntity;

	public abstract string Name { get; }

	public abstract EntityLayer Layer { get; }

	public abstract List<AnimConfig> AnimConfigs { get; }

	public abstract bool IsVisible { get; }

	public abstract ClusterRevealLevel IsVisibleInFOW { get; }

	public AxialI Location
	{
		get
		{
			return m_location;
		}
		set
		{
			if (value != m_location)
			{
				AxialI location = m_location;
				m_location = value;
				if (base.gameObject.GetSMI<StateMachine.Instance>() == null)
				{
					positionDirty = true;
				}
				SendClusterLocationChangedEvent(location, m_location);
			}
		}
	}

	public virtual bool ShowName()
	{
		return false;
	}

	public virtual bool ShowProgressBar()
	{
		return false;
	}

	public virtual float GetProgress()
	{
		return 0f;
	}

	public virtual bool SpaceOutInSameHex()
	{
		return false;
	}

	public virtual bool KeepRotationWhenSpacingOutInHex()
	{
		return false;
	}

	public virtual bool ShowPath()
	{
		return true;
	}

	public virtual void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
	}

	protected override void OnSpawn()
	{
		ClusterGrid.Instance.RegisterEntity(this);
		if (m_selectable != null)
		{
			m_selectable.SetName(Name);
		}
		if (!isWorldEntity)
		{
			m_transform.SetLocalPosition(new Vector3(-1f, 0f, 0f));
		}
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.Trigger(1980521255);
		}
	}

	protected override void OnCleanUp()
	{
		ClusterGrid.Instance.UnregisterEntity(this);
	}

	public virtual Sprite GetUISprite()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			List<AnimConfig> animConfigs = AnimConfigs;
			if (animConfigs.Count > 0)
			{
				return Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile);
			}
		}
		else
		{
			WorldContainer component = GetComponent<WorldContainer>();
			if (component != null)
			{
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(component.worldName);
				if (worldData == null)
				{
					return null;
				}
				return Assets.GetSprite(worldData.asteroidIcon);
			}
		}
		return null;
	}

	public void SendClusterLocationChangedEvent(AxialI oldLocation, AxialI newLocation)
	{
		ClusterLocationChangedEvent data = new ClusterLocationChangedEvent
		{
			entity = this,
			oldLocation = oldLocation,
			newLocation = newLocation
		};
		Trigger(-1298331547, data);
		Game.Instance.Trigger(-1298331547, data);
		if (m_selectable != null && m_selectable.IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}
}
