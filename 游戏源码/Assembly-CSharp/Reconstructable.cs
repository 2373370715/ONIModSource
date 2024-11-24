using KSerialization;
using UnityEngine;

public class Reconstructable : KMonoBehaviour
{
	[MyCmpReq]
	private Deconstructable deconstructable;

	[MyCmpReq]
	private Building building;

	[Serialize]
	private Tag[] selectedElementsTags;

	[Serialize]
	private bool reconstructRequested;

	public bool AllowReconstruct
	{
		get
		{
			if (deconstructable.allowDeconstruction)
			{
				if (!building.Def.ShowInBuildMenu)
				{
					return SelectModuleSideScreen.moduleButtonSortOrder.Contains(building.Def.PrefabID);
				}
				return true;
			}
			return false;
		}
	}

	public Tag PrimarySelectedElementTag => selectedElementsTags[0];

	public bool ReconstructRequested => reconstructRequested;

	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	public void RequestReconstruct(Tag newElement)
	{
		if (deconstructable.allowDeconstruction)
		{
			reconstructRequested = !reconstructRequested;
			if (reconstructRequested)
			{
				deconstructable.QueueDeconstruction(userTriggered: false);
				selectedElementsTags = new Tag[1] { newElement };
			}
			else
			{
				deconstructable.CancelDeconstruction();
			}
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	public void CancelReconstructOrder()
	{
		reconstructRequested = false;
		deconstructable.CancelDeconstruction();
		Trigger(954267658);
	}

	public void TryCommenceReconstruct()
	{
		if (deconstructable.allowDeconstruction && reconstructRequested)
		{
			string facadeID = building.GetComponent<BuildingFacade>().CurrentFacade;
			Vector3 position = building.transform.position;
			Orientation orientation = building.Orientation;
			GameScheduler.Instance.ScheduleNextFrame("Reconstruct", delegate
			{
				building.Def.TryPlace(null, position, orientation, selectedElementsTags, facadeID, restrictToActiveWorld: false);
			});
		}
	}
}
