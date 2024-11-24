using System;
using UnityEngine;

// Token: 0x02000D36 RID: 3382
public class DevPump : Filterable, ISim1000ms
{
	// Token: 0x06004222 RID: 16930 RVA: 0x0023FE70 File Offset: 0x0023E070
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.elementState == Filterable.ElementState.Liquid)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
			return;
		}
		if (this.elementState == Filterable.ElementState.Gas)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		}
	}

	// Token: 0x06004223 RID: 16931 RVA: 0x000CAC0D File Offset: 0x000C8E0D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filterElementState = this.elementState;
	}

	// Token: 0x06004224 RID: 16932 RVA: 0x0023FEC0 File Offset: 0x0023E0C0
	public void Sim1000ms(float dt)
	{
		if (!base.SelectedTag.IsValid)
		{
			return;
		}
		float num = 10f - this.storage.GetAmountAvailable(base.SelectedTag);
		if (num <= 0f)
		{
			return;
		}
		Element element = ElementLoader.GetElement(base.SelectedTag);
		GameObject gameObject = Assets.TryGetPrefab(base.SelectedTag);
		if (element != null)
		{
			this.storage.AddElement(element.id, num, element.defaultValues.temperature, byte.MaxValue, 0, false, false);
			return;
		}
		if (gameObject != null)
		{
			Grid.SceneLayer sceneLayer = gameObject.GetComponent<KBatchedAnimController>().sceneLayer;
			GameObject gameObject2 = GameUtil.KInstantiate(gameObject, sceneLayer, null, 0);
			gameObject2.GetComponent<PrimaryElement>().Units = num;
			gameObject2.SetActive(true);
			this.storage.Store(gameObject2, true, false, true, false);
		}
	}

	// Token: 0x04002D0B RID: 11531
	public Filterable.ElementState elementState = Filterable.ElementState.Liquid;

	// Token: 0x04002D0C RID: 11532
	[MyCmpReq]
	private Storage storage;
}
