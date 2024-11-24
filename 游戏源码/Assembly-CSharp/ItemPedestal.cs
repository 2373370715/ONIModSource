using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000E0B RID: 3595
[AddComponentMenu("KMonoBehaviour/scripts/ItemPedestal")]
public class ItemPedestal : KMonoBehaviour
{
	// Token: 0x060046C2 RID: 18114 RVA: 0x0024FEA4 File Offset: 0x0024E0A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ItemPedestal>(-731304873, ItemPedestal.OnOccupantChangedDelegate);
		if (this.receptacle.Occupant)
		{
			KBatchedAnimController component = this.receptacle.Occupant.GetComponent<KBatchedAnimController>();
			if (component)
			{
				component.enabled = true;
				component.sceneLayer = Grid.SceneLayer.Move;
			}
			this.OnOccupantChanged(this.receptacle.Occupant);
		}
	}

	// Token: 0x060046C3 RID: 18115 RVA: 0x0024FF14 File Offset: 0x0024E114
	private void OnOccupantChanged(object data)
	{
		Attributes attributes = this.GetAttributes();
		if (this.decorModifier != null)
		{
			attributes.Remove(this.decorModifier);
			attributes.Remove(this.decorRadiusModifier);
			this.decorModifier = null;
			this.decorRadiusModifier = null;
		}
		if (data != null)
		{
			GameObject gameObject = (GameObject)data;
			UnityEngine.Object component = gameObject.GetComponent<DecorProvider>();
			float value = 5f;
			float value2 = 3f;
			if (component != null)
			{
				value = Mathf.Max(Db.Get().BuildingAttributes.Decor.Lookup(gameObject).GetTotalValue() * 2f, 5f);
				value2 = Db.Get().BuildingAttributes.DecorRadius.Lookup(gameObject).GetTotalValue() + 2f;
			}
			string description = string.Format(BUILDINGS.PREFABS.ITEMPEDESTAL.DISPLAYED_ITEM_FMT, gameObject.GetComponent<KPrefabID>().PrefabTag.ProperName());
			this.decorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, value, description, false, false, true);
			this.decorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, value2, description, false, false, true);
			attributes.Add(this.decorModifier);
			attributes.Add(this.decorRadiusModifier);
		}
	}

	// Token: 0x040030FB RID: 12539
	[MyCmpReq]
	protected SingleEntityReceptacle receptacle;

	// Token: 0x040030FC RID: 12540
	[MyCmpReq]
	private DecorProvider decorProvider;

	// Token: 0x040030FD RID: 12541
	private const float MINIMUM_DECOR = 5f;

	// Token: 0x040030FE RID: 12542
	private const float STORED_DECOR_MODIFIER = 2f;

	// Token: 0x040030FF RID: 12543
	private const int RADIUS_BONUS = 2;

	// Token: 0x04003100 RID: 12544
	private AttributeModifier decorModifier;

	// Token: 0x04003101 RID: 12545
	private AttributeModifier decorRadiusModifier;

	// Token: 0x04003102 RID: 12546
	private static readonly EventSystem.IntraObjectHandler<ItemPedestal> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<ItemPedestal>(delegate(ItemPedestal component, object data)
	{
		component.OnOccupantChanged(data);
	});
}
