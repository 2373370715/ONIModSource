﻿using System;
using UnityEngine;

public class AutoPlumberSideScreen : SideScreenContent
{
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

		public override int GetSideScreenSortOrder()
	{
		return -150;
	}

		public override bool IsValidForTarget(GameObject target)
	{
		return DebugHandler.InstantBuildMode && target.GetComponent<Building>() != null;
	}

		public override void SetTarget(GameObject target)
	{
		this.building = target.GetComponent<Building>();
	}

		public override void ClearTarget()
	{
	}

		public KButton activateButton;

		public KButton powerButton;

		public KButton pipesButton;

		public KButton solidsButton;

		public KButton minionButton;

		private Building building;
}
