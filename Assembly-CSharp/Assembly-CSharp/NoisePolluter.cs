using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/NoisePolluter")]
public class NoisePolluter : KMonoBehaviour, IPolluter {
    public const string ID = "NoisePolluter";

    private static readonly EventSystem.IntraObjectHandler<NoisePolluter> OnActiveChangedDelegate
        = new EventSystem.IntraObjectHandler<NoisePolluter>(delegate(NoisePolluter component, object data) {
                                                                component.OnActiveChanged(data);
                                                            });

    public AttributeInstance dB;
    public AttributeInstance dBRadius;
    public bool              isMovable;
    public int               noise;

    [MyCmpReq]
    public OccupyArea occupyArea;

    public  Action<object> onCollectNoisePollutersCallback;
    public  int            radius;
    public  System.Action  refreshCallback;
    public  Action<object> refreshPartionerCallback;
    private NoiseSplat     splat;
    public  string         sourceName { get; private set; }
    public  bool           active     { get; private set; }

    public void SetAttributes(Vector2 pos, int dB, GameObject go, string name) {
        sourceName = name;
        noise      = dB;
    }

    public int        GetRadius()                    { return radius; }
    public int        GetNoise()                     { return noise; }
    public GameObject GetGameObject()                { return gameObject; }
    public void       SetSplat(NoiseSplat new_splat) { splat = new_splat; }

    public void Clear() {
        if (splat != null) {
            splat.Clear();
            splat = null;
        }
    }

    public Vector2 GetPosition() { return transform.GetPosition(); }

    public string GetName() {
        if (string.IsNullOrEmpty(sourceName)) sourceName = GetComponent<KSelectable>().GetName();
        return sourceName;
    }

    public static bool IsNoiseableCell(int cell) {
        return Grid.IsValidCell(cell) && (Grid.IsGas(cell) || !Grid.IsSubstantialLiquid(cell));
    }

    public void ResetCells() {
        if (radius == 0) Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", GetName());
    }

    public void SetActive(bool active = true) {
        if (!active && splat != null) {
            AudioEventManager.Get().ClearNoiseSplat(splat);
            splat.Clear();
        }

        this.active = active;
    }

    public void Refresh() {
        if (active) {
            if (splat != null) {
                AudioEventManager.Get().ClearNoiseSplat(splat);
                splat.Clear();
            }

            var component  = GetComponent<KSelectable>();
            var name       = component != null ? component.GetName() : this.name;
            var gameObject = GetComponent<KMonoBehaviour>().gameObject;
            splat = AudioEventManager.Get().CreateNoiseSplat(GetPosition(), noise, radius, name, gameObject);
        }
    }

    private void OnActiveChanged(object data) {
        var isActive = ((Operational)data).IsActive;
        SetActive(isActive);
        Refresh();
    }

    public void SetValues(EffectorValues values) {
        noise  = values.amount;
        radius = values.radius;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        if (radius == 0 || noise == 0) {
            Debug.LogWarning(string.Concat("Noisepollutor::OnSpawn [",
                                           GetName(),
                                           "] noise: [",
                                           noise.ToString(),
                                           "] radius: [",
                                           radius.ToString(),
                                           "]"));

            Destroy(this);
            return;
        }

        ResetCells();
        var component = GetComponent<Operational>();
        if (component != null) Subscribe(824508782, OnActiveChangedDelegate);
        refreshCallback                 = Refresh;
        refreshPartionerCallback        = delegate { Refresh(); };
        onCollectNoisePollutersCallback = OnCollectNoisePolluters;
        var attributes = this.GetAttributes();
        var db         = Db.Get();
        dB       = attributes.Add(db.BuildingAttributes.NoisePollution);
        dBRadius = attributes.Add(db.BuildingAttributes.NoisePollutionRadius);
        if (noise != 0 && radius != 0) {
            var modifier
                = new AttributeModifier(db.BuildingAttributes.NoisePollution.Id, noise, UI.TOOLTIPS.BASE_VALUE);

            var modifier2 = new AttributeModifier(db.BuildingAttributes.NoisePollutionRadius.Id,
                                                  radius,
                                                  UI.TOOLTIPS.BASE_VALUE);

            attributes.Add(modifier);
            attributes.Add(modifier2);
        } else
            Debug.LogWarning(string.Concat("Noisepollutor::OnSpawn [",
                                           GetName(),
                                           "] radius: [",
                                           radius.ToString(),
                                           "] noise: [",
                                           noise.ToString(),
                                           "]"));

        var component2 = GetComponent<KBatchedAnimController>();
        isMovable = component2 != null && component2.isMovable;
        Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(transform,
                                                                         OnCellChange,
                                                                         "NoisePolluter.OnSpawn");

        var attributeInstance = dB;
        attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, refreshCallback);
        var attributeInstance2 = dBRadius;
        attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, refreshCallback);
        if (component != null) OnActiveChanged(component.IsActive);
    }

    private void OnCellChange()                       { Refresh(); }
    private void OnCollectNoisePolluters(object data) { ((List<NoisePolluter>)data).Add(this); }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        if (isSpawned) {
            if (dB != null) {
                var attributeInstance = dB;
                attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, refreshCallback);
                var attributeInstance2 = dBRadius;
                attributeInstance2.OnDirty
                    = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, refreshCallback);
            }

            if (isMovable) Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(transform, OnCellChange);
        }

        if (splat != null) {
            AudioEventManager.Get().ClearNoiseSplat(splat);
            splat.Clear();
        }
    }

    public float GetNoiseForCell(int cell) { return splat.GetDBForCell(cell); }

    public List<Descriptor> GetEffectDescriptions() {
        var list = new List<Descriptor>();
        if (dB != null && dBRadius != null) {
            var totalValue  = dB.GetTotalValue();
            var totalValue2 = dBRadius.GetTotalValue();
            string text = noise > 0
                              ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE
                              : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;

            text = text + "\n\n" + dB.GetAttributeValueTooltip();
            var arg = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
            var item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg, totalValue2),
                                      string.Format(text,                             arg, totalValue2));

            list.Add(item);
        } else if (noise != 0) {
            string format = noise >= 0
                                ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE
                                : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;

            var arg2 = GameUtil.AddPositiveSign(noise.ToString(), noise > 0);
            var item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg2, radius),
                                       string.Format(format,                           arg2, radius));

            list.Add(item2);
        }

        return list;
    }

    public List<Descriptor> GetDescriptors(GameObject go) { return GetEffectDescriptions(); }
}