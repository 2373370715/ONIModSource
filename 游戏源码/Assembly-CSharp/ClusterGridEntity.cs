using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02001958 RID: 6488
public abstract class ClusterGridEntity : KMonoBehaviour
{
	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x0600875F RID: 34655
	public abstract string Name { get; }

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x06008760 RID: 34656
	public abstract EntityLayer Layer { get; }

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x06008761 RID: 34657
	public abstract List<ClusterGridEntity.AnimConfig> AnimConfigs { get; }

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x06008762 RID: 34658
	public abstract bool IsVisible { get; }

	// Token: 0x06008763 RID: 34659 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual bool ShowName()
	{
		return false;
	}

	// Token: 0x06008764 RID: 34660 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual bool ShowProgressBar()
	{
		return false;
	}

	// Token: 0x06008765 RID: 34661 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public virtual float GetProgress()
	{
		return 0f;
	}

	// Token: 0x06008766 RID: 34662 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual bool SpaceOutInSameHex()
	{
		return false;
	}

	// Token: 0x06008767 RID: 34663 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual bool KeepRotationWhenSpacingOutInHex()
	{
		return false;
	}

	// Token: 0x06008768 RID: 34664 RVA: 0x000A65EC File Offset: 0x000A47EC
	public virtual bool ShowPath()
	{
		return true;
	}

	// Token: 0x06008769 RID: 34665 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
	}

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x0600876A RID: 34666
	public abstract ClusterRevealLevel IsVisibleInFOW { get; }

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x0600876B RID: 34667 RVA: 0x000F88F5 File Offset: 0x000F6AF5
	// (set) Token: 0x0600876C RID: 34668 RVA: 0x00350AA8 File Offset: 0x0034ECA8
	public AxialI Location
	{
		get
		{
			return this.m_location;
		}
		set
		{
			if (value != this.m_location)
			{
				AxialI location = this.m_location;
				this.m_location = value;
				if (base.gameObject.GetSMI<StateMachine.Instance>() == null)
				{
					this.positionDirty = true;
				}
				this.SendClusterLocationChangedEvent(location, this.m_location);
			}
		}
	}

	// Token: 0x0600876D RID: 34669 RVA: 0x00350AF4 File Offset: 0x0034ECF4
	protected override void OnSpawn()
	{
		ClusterGrid.Instance.RegisterEntity(this);
		if (this.m_selectable != null)
		{
			this.m_selectable.SetName(this.Name);
		}
		if (!this.isWorldEntity)
		{
			this.m_transform.SetLocalPosition(new Vector3(-1f, 0f, 0f));
		}
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.Trigger(1980521255, null);
		}
	}

	// Token: 0x0600876E RID: 34670 RVA: 0x000F88FD File Offset: 0x000F6AFD
	protected override void OnCleanUp()
	{
		ClusterGrid.Instance.UnregisterEntity(this);
	}

	// Token: 0x0600876F RID: 34671 RVA: 0x00350B70 File Offset: 0x0034ED70
	public virtual Sprite GetUISprite()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			List<ClusterGridEntity.AnimConfig> animConfigs = this.AnimConfigs;
			if (animConfigs.Count > 0)
			{
				return Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile, "ui", false, "");
			}
		}
		else
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
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

	// Token: 0x06008770 RID: 34672 RVA: 0x00350BEC File Offset: 0x0034EDEC
	public void SendClusterLocationChangedEvent(AxialI oldLocation, AxialI newLocation)
	{
		ClusterLocationChangedEvent data = new ClusterLocationChangedEvent
		{
			entity = this,
			oldLocation = oldLocation,
			newLocation = newLocation
		};
		base.Trigger(-1298331547, data);
		Game.Instance.Trigger(-1298331547, data);
		if (this.m_selectable != null && this.m_selectable.IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x04006611 RID: 26129
	[Serialize]
	protected AxialI m_location;

	// Token: 0x04006612 RID: 26130
	public bool positionDirty;

	// Token: 0x04006613 RID: 26131
	[MyCmpGet]
	protected KSelectable m_selectable;

	// Token: 0x04006614 RID: 26132
	[MyCmpReq]
	private Transform m_transform;

	// Token: 0x04006615 RID: 26133
	public bool isWorldEntity;

	// Token: 0x02001959 RID: 6489
	public struct AnimConfig
	{
		// Token: 0x04006616 RID: 26134
		public KAnimFile animFile;

		// Token: 0x04006617 RID: 26135
		public string initialAnim;

		// Token: 0x04006618 RID: 26136
		public KAnim.PlayMode playMode;

		// Token: 0x04006619 RID: 26137
		public string symbolSwapTarget;

		// Token: 0x0400661A RID: 26138
		public string symbolSwapSymbol;

		// Token: 0x0400661B RID: 26139
		public Vector3 animOffset;

		// Token: 0x0400661C RID: 26140
		public float animPlaySpeedModifier;
	}
}
