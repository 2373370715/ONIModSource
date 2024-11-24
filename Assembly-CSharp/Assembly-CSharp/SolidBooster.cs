﻿using System;
using UnityEngine;

public class SolidBooster : RocketEngine
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SolidBooster>(-887025858, SolidBooster.OnRocketLandedDelegate);
	}

	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		Element element = ElementLoader.GetElement(this.fuelTag);
		GameObject go = element.substance.SpawnResource(base.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		this.fuelStorage.Store(go, false, false, true, false);
		element = ElementLoader.GetElement(GameTags.OxyRock);
		go = element.substance.SpawnResource(base.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		this.fuelStorage.Store(go, false, false, true, false);
	}

	private void OnRocketLanded(object data)
	{
		if (this.fuelStorage != null && this.fuelStorage.items != null)
		{
			for (int i = this.fuelStorage.items.Count - 1; i >= 0; i--)
			{
				Util.KDestroyGameObject(this.fuelStorage.items[i]);
			}
			this.fuelStorage.items.Clear();
		}
	}

	public Storage fuelStorage;

	private static readonly EventSystem.IntraObjectHandler<SolidBooster> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SolidBooster>(delegate(SolidBooster component, object data)
	{
		component.OnRocketLanded(data);
	});
}
