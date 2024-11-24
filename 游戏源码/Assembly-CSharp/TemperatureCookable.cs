using System;
using UnityEngine;

// Token: 0x020011E4 RID: 4580
[AddComponentMenu("KMonoBehaviour/scripts/TemperatureCookable")]
public class TemperatureCookable : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06005D34 RID: 23860 RVA: 0x000DCDF4 File Offset: 0x000DAFF4
	public void Sim1000ms(float dt)
	{
		if (this.element.Temperature > this.cookTemperature && this.cookedID != null)
		{
			this.Cook();
		}
	}

	// Token: 0x06005D35 RID: 23861 RVA: 0x0029D888 File Offset: 0x0029BA88
	private void Cook()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(this.cookedID), position);
		gameObject.SetActive(true);
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
		component2.Temperature = this.element.Temperature;
		component2.Mass = this.element.Mass;
		base.gameObject.DeleteObject();
	}

	// Token: 0x040041F3 RID: 16883
	[MyCmpReq]
	private PrimaryElement element;

	// Token: 0x040041F4 RID: 16884
	public float cookTemperature = 273150f;

	// Token: 0x040041F5 RID: 16885
	public string cookedID;
}
