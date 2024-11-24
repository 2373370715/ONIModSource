using System;
using UnityEngine;

// Token: 0x02001F33 RID: 7987
public class AutoPlumberSideScreen : SideScreenContent
{
	// Token: 0x0600A87C RID: 43132 RVA: 0x003FD5AC File Offset: 0x003FB7AC
	protected override void OnSpawn()
	{
		this.activateButton.onClick += delegate()
		{
			DevAutoPlumber.AutoPlumbBuilding(this.building);
		};
		this.powerButton.onClick += delegate()
		{
			DevAutoPlumber.DoElectricalPlumbing(this.building);
		};
		this.pipesButton.onClick += delegate()
		{
			DevAutoPlumber.DoLiquidAndGasPlumbing(this.building);
		};
		this.solidsButton.onClick += delegate()
		{
			DevAutoPlumber.SetupSolidOreDelivery(this.building);
		};
		this.minionButton.onClick += delegate()
		{
			this.SpawnMinion();
		};
	}

	// Token: 0x0600A87D RID: 43133 RVA: 0x003FD62C File Offset: 0x003FB82C
	private void SpawnMinion()
	{
		MinionStartingStats minionStartingStats = new MinionStartingStats(false, null, null, true);
		GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPos(Grid.PosToCell(this.building), CellAlignment.Bottom, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		minionStartingStats.Apply(gameObject);
	}

	// Token: 0x0600A87E RID: 43134 RVA: 0x0010D723 File Offset: 0x0010B923
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x0600A87F RID: 43135 RVA: 0x0010D72A File Offset: 0x0010B92A
	public override bool IsValidForTarget(GameObject target)
	{
		return DebugHandler.InstantBuildMode && target.GetComponent<Building>() != null;
	}

	// Token: 0x0600A880 RID: 43136 RVA: 0x0010D741 File Offset: 0x0010B941
	public override void SetTarget(GameObject target)
	{
		this.building = target.GetComponent<Building>();
	}

	// Token: 0x0600A881 RID: 43137 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void ClearTarget()
	{
	}

	// Token: 0x0400847B RID: 33915
	public KButton activateButton;

	// Token: 0x0400847C RID: 33916
	public KButton powerButton;

	// Token: 0x0400847D RID: 33917
	public KButton pipesButton;

	// Token: 0x0400847E RID: 33918
	public KButton solidsButton;

	// Token: 0x0400847F RID: 33919
	public KButton minionButton;

	// Token: 0x04008480 RID: 33920
	private Building building;
}
