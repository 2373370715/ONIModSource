using System;
using Klei;
using UnityEngine;

// Token: 0x0200127C RID: 4732
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ElementChunk")]
public class ElementChunk : KMonoBehaviour
{
	// Token: 0x06006108 RID: 24840 RVA: 0x000DF4F3 File Offset: 0x000DD6F3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameComps.OreSizeVisualizers.Add(base.gameObject);
		GameComps.ElementSplitters.Add(base.gameObject);
		base.Subscribe<ElementChunk>(-2064133523, ElementChunk.OnAbsorbDelegate);
	}

	// Token: 0x06006109 RID: 24841 RVA: 0x002B16CC File Offset: 0x002AF8CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		base.transform.SetPosition(position);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Element element = component.Element;
		KSelectable component2 = base.GetComponent<KSelectable>();
		Func<Element> data = () => element;
		component2.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, data);
		component2.AddStatusItem(Db.Get().MiscStatusItems.OreMass, base.gameObject);
		component2.AddStatusItem(Db.Get().MiscStatusItems.OreTemp, base.gameObject);
	}

	// Token: 0x0600610A RID: 24842 RVA: 0x000DF52E File Offset: 0x000DD72E
	protected override void OnCleanUp()
	{
		GameComps.ElementSplitters.Remove(base.gameObject);
		GameComps.OreSizeVisualizers.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x0600610B RID: 24843 RVA: 0x002B1780 File Offset: 0x002AF980
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			PrimaryElement primaryElement = pickupable.PrimaryElement;
			if (primaryElement != null)
			{
				float mass = primaryElement.Mass;
				if (mass > 0f)
				{
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					float mass2 = component.Mass;
					float temperature = (mass2 > 0f) ? SimUtil.CalculateFinalTemperature(mass2, component.Temperature, mass, primaryElement.Temperature) : primaryElement.Temperature;
					component.SetMassTemperature(mass2 + mass, temperature);
				}
				if (CameraController.Instance != null)
				{
					string sound = GlobalAssets.GetSound("Ore_absorb", false);
					Vector3 position = pickupable.transform.GetPosition();
					position.z = 0f;
					if (sound != null && CameraController.Instance.IsAudibleSound(position, sound))
					{
						KFMOD.PlayOneShot(sound, position, 1f);
					}
				}
			}
		}
	}

	// Token: 0x04004516 RID: 17686
	private static readonly EventSystem.IntraObjectHandler<ElementChunk> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<ElementChunk>(delegate(ElementChunk component, object data)
	{
		component.OnAbsorb(data);
	});
}
