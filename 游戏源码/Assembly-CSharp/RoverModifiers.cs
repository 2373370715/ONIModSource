using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200111A RID: 4378
[SerializationConfig(MemberSerialization.OptIn)]
public class RoverModifiers : Modifiers, ISaveLoadable
{
	// Token: 0x060059BC RID: 22972 RVA: 0x00292CE8 File Offset: 0x00290EE8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributes.Add(Db.Get().Attributes.Construction);
		this.attributes.Add(Db.Get().Attributes.Digging);
		this.attributes.Add(Db.Get().Attributes.Strength);
	}

	// Token: 0x060059BD RID: 22973 RVA: 0x00292D4C File Offset: 0x00290F4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<ChoreConsumer>() != null)
		{
			base.Subscribe<RoverModifiers>(-1988963660, RoverModifiers.OnBeginChoreDelegate);
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			base.transform.SetPosition(position);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			this.SetupDependentAttribute(Db.Get().Attributes.CarryAmount, Db.Get().AttributeConverters.CarryAmountFromStrength);
		}
	}

	// Token: 0x060059BE RID: 22974 RVA: 0x00292DE0 File Offset: 0x00290FE0
	private void SetupDependentAttribute(Klei.AI.Attribute targetAttribute, AttributeConverter attributeConverter)
	{
		Klei.AI.Attribute attribute = attributeConverter.attribute;
		AttributeInstance attributeInstance = attribute.Lookup(this);
		AttributeModifier target_modifier = new AttributeModifier(targetAttribute.Id, attributeConverter.Lookup(this).Evaluate(), attribute.Name, false, false, false);
		this.GetAttributes().Add(target_modifier);
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(delegate()
		{
			target_modifier.SetValue(attributeConverter.Lookup(this).Evaluate());
		}));
	}

	// Token: 0x060059BF RID: 22975 RVA: 0x00292E74 File Offset: 0x00291074
	private void OnBeginChore(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x04003F5B RID: 16219
	private static readonly EventSystem.IntraObjectHandler<RoverModifiers> OnBeginChoreDelegate = new EventSystem.IntraObjectHandler<RoverModifiers>(delegate(RoverModifiers component, object data)
	{
		component.OnBeginChore(data);
	});
}
