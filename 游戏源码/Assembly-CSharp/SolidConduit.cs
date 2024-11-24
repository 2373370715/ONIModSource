using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000F88 RID: 3976
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduit")]
public class SolidConduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr
{
	// Token: 0x06005086 RID: 20614 RVA: 0x000D4871 File Offset: 0x000D2A71
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06005087 RID: 20615 RVA: 0x000D4887 File Offset: 0x000D2A87
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06005088 RID: 20616 RVA: 0x000D4896 File Offset: 0x000D2A96
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.solidConduitSystem;
	}

	// Token: 0x06005089 RID: 20617 RVA: 0x000D48A2 File Offset: 0x000D2AA2
	public UtilityNetwork GetNetwork()
	{
		return this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
	}

	// Token: 0x0600508A RID: 20618 RVA: 0x000D48B5 File Offset: 0x000D2AB5
	public static SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x0600508B RID: 20619 RVA: 0x000C19AF File Offset: 0x000BFBAF
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x0600508C RID: 20620 RVA: 0x000D48C1 File Offset: 0x000D2AC1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Conveyor, this);
	}

	// Token: 0x0600508D RID: 20621 RVA: 0x0026F014 File Offset: 0x0026D214
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(this);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			SolidConduit.GetFlowManager().EmptyConduit(cell);
		}
		base.OnCleanUp();
	}

	// Token: 0x04003824 RID: 14372
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04003825 RID: 14373
	private System.Action firstFrameCallback;
}
