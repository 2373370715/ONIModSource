using UnityEngine;

public class DevPump : Filterable, ISim1000ms
{
	public ElementState elementState = ElementState.Liquid;

	[MyCmpReq]
	private Storage storage;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (elementState == ElementState.Liquid)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		}
		else if (elementState == ElementState.Gas)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		filterElementState = elementState;
	}

	public void Sim1000ms(float dt)
	{
		if (!base.SelectedTag.IsValid)
		{
			return;
		}
		float num = 10f - storage.GetAmountAvailable(base.SelectedTag);
		if (!(num <= 0f))
		{
			Element element = ElementLoader.GetElement(base.SelectedTag);
			GameObject gameObject = Assets.TryGetPrefab(base.SelectedTag);
			if (element != null)
			{
				storage.AddElement(element.id, num, element.defaultValues.temperature, byte.MaxValue, 0, keep_zero_mass: false, do_disease_transfer: false);
			}
			else if (gameObject != null)
			{
				Grid.SceneLayer sceneLayer = gameObject.GetComponent<KBatchedAnimController>().sceneLayer;
				GameObject gameObject2 = GameUtil.KInstantiate(gameObject, sceneLayer);
				gameObject2.GetComponent<PrimaryElement>().Units = num;
				gameObject2.SetActive(value: true);
				storage.Store(gameObject2, hide_popups: true);
			}
		}
	}
}
