using System;
using System.IO;
using Klei.AI;
using KSerialization;
using UnityEngine;
using Attribute = Klei.AI.Attribute;

[SerializationConfig(MemberSerialization.OptIn)]
public class MinionModifiers : Modifiers, ISaveLoadable {
    private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDeathDelegate
        = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data) {
                                                                  component.OnDeath(data);
                                                              });

    private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnAttachFollowCamDelegate
        = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data) {
                                                                  component.OnAttachFollowCam(data);
                                                              });

    private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDetachFollowCamDelegate
        = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data) {
                                                                  component.OnDetachFollowCam(data);
                                                              });

    private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnBeginChoreDelegate
        = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data) {
                                                                  component.OnBeginChore(data);
                                                              });

    public bool addBaseTraits = true;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        if (addBaseTraits) {
            foreach (var attribute in Db.Get().Attributes.resources)
                if (attributes.Get(attribute) == null)
                    attributes.Add(attribute);

            foreach (var disease in Db.Get().Diseases.resources) {
                var amountInstance = AddAmount(disease.amount);
                attributes.Add(disease.cureSpeedBase);
                amountInstance.SetValue(0f);
            }

            var component = GetComponent<ChoreConsumer>();
            if (component != null) {
                component.AddProvider(GlobalChoreProvider.Instance);
                gameObject.AddComponent<QualityOfLifeNeed>();
            }
        }
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        if (GetComponent<ChoreConsumer>() != null) {
            Subscribe(1623392196,  OnDeathDelegate);
            Subscribe(-1506069671, OnAttachFollowCamDelegate);
            Subscribe(-485480405,  OnDetachFollowCamDelegate);
            Subscribe(-1988963660, OnBeginChoreDelegate);
            var amountInstance = this.GetAmounts().Get("Calories");
            if (amountInstance != null) {
                var amountInstance2 = amountInstance;
                amountInstance2.OnMaxValueReached
                    = (System.Action)Delegate.Combine(amountInstance2.OnMaxValueReached,
                                                      new System.Action(OnMaxCaloriesReached));
            }

            var position = transform.GetPosition();
            position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
            transform.SetPosition(position);
            gameObject.layer = LayerMask.NameToLayer("Default");
            SetupDependentAttribute(Db.Get().Attributes.CarryAmount,
                                    Db.Get().AttributeConverters.CarryAmountFromStrength);
        }
    }

    private AmountInstance AddAmount(Amount amount) {
        var instance = new AmountInstance(amount, gameObject);
        return amounts.Add(instance);
    }

    private void SetupDependentAttribute(Attribute targetAttribute, AttributeConverter attributeConverter) {
        var attribute         = attributeConverter.attribute;
        var attributeInstance = attribute.Lookup(this);
        var target_modifier = new AttributeModifier(targetAttribute.Id,
                                                    attributeConverter.Lookup(this).Evaluate(),
                                                    attribute.Name,
                                                    false,
                                                    false,
                                                    false);

        this.GetAttributes().Add(target_modifier);
        attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty,
                                                                    new System.Action(delegate {
                                                                        target_modifier
                                                                            .SetValue(attributeConverter
                                                                                .Lookup(this)
                                                                                .Evaluate());
                                                                    }));
    }

    private void OnDeath(object data) {
        Debug.LogFormat("OnDeath {0} -- {1} has died!", data, name);
        foreach (var minionIdentity in Components.LiveMinionIdentities.Items)
            minionIdentity.GetComponent<Effects>().Add("Mourning", true);
    }

    private void OnMaxCaloriesReached() { GetComponent<Effects>().Add("WellFed", true); }

    private void OnBeginChore(object data) {
        var component = GetComponent<Storage>();
        if (component != null) component.DropAll();
    }

    public override void OnSerialize(BinaryWriter writer) { base.OnSerialize(writer); }
    public override void OnDeserialize(IReader    reader) { base.OnDeserialize(reader); }
    private         void OnAttachFollowCam(object data)   { GetComponent<Effects>().Add("CenterOfAttention", false); }
    private         void OnDetachFollowCam(object data)   { GetComponent<Effects>().Remove("CenterOfAttention"); }
}