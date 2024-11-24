using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000AD2 RID: 2770
public class Reconstructable : KMonoBehaviour
{
	// Token: 0x17000230 RID: 560
	// (get) Token: 0x060033DF RID: 13279 RVA: 0x000C1D5D File Offset: 0x000BFF5D
	public bool AllowReconstruct
	{
		get
		{
			return this.deconstructable.allowDeconstruction && (this.building.Def.ShowInBuildMenu || SelectModuleSideScreen.moduleButtonSortOrder.Contains(this.building.Def.PrefabID));
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x060033E0 RID: 13280 RVA: 0x000C1D9C File Offset: 0x000BFF9C
	public Tag PrimarySelectedElementTag
	{
		get
		{
			return this.selectedElementsTags[0];
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x060033E1 RID: 13281 RVA: 0x000C1DAA File Offset: 0x000BFFAA
	public bool ReconstructRequested
	{
		get
		{
			return this.reconstructRequested;
		}
	}

	// Token: 0x060033E2 RID: 13282 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060033E3 RID: 13283 RVA: 0x00208240 File Offset: 0x00206440
	public void RequestReconstruct(Tag newElement)
	{
		if (!this.deconstructable.allowDeconstruction)
		{
			return;
		}
		this.reconstructRequested = !this.reconstructRequested;
		if (this.reconstructRequested)
		{
			this.deconstructable.QueueDeconstruction(false);
			this.selectedElementsTags = new Tag[]
			{
				newElement
			};
		}
		else
		{
			this.deconstructable.CancelDeconstruction();
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x000C1DB2 File Offset: 0x000BFFB2
	public void CancelReconstructOrder()
	{
		this.reconstructRequested = false;
		this.deconstructable.CancelDeconstruction();
		base.Trigger(954267658, null);
	}

	// Token: 0x060033E5 RID: 13285 RVA: 0x002082B4 File Offset: 0x002064B4
	public void TryCommenceReconstruct()
	{
		if (!this.deconstructable.allowDeconstruction)
		{
			return;
		}
		if (!this.reconstructRequested)
		{
			return;
		}
		string facadeID = this.building.GetComponent<BuildingFacade>().CurrentFacade;
		Vector3 position = this.building.transform.position;
		Orientation orientation = this.building.Orientation;
		GameScheduler.Instance.ScheduleNextFrame("Reconstruct", delegate(object data)
		{
			this.building.Def.TryPlace(null, position, orientation, this.selectedElementsTags, facadeID, false, 0);
		}, null, null);
	}

	// Token: 0x040022ED RID: 8941
	[MyCmpReq]
	private Deconstructable deconstructable;

	// Token: 0x040022EE RID: 8942
	[MyCmpReq]
	private Building building;

	// Token: 0x040022EF RID: 8943
	[Serialize]
	private Tag[] selectedElementsTags;

	// Token: 0x040022F0 RID: 8944
	[Serialize]
	private bool reconstructRequested;
}
