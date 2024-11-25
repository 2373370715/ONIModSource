using UnityEngine;

public class SealedDoorSideScreen : SideScreenContent {
    [SerializeField]
    private KButton button;

    [SerializeField]
    private LocText label;

    [SerializeField]
    private Door target;

    protected override void OnSpawn() {
        base.OnSpawn();
        button.onClick += delegate { target.OrderUnseal(); };
        Refresh();
    }

    public override bool IsValidForTarget(GameObject target) { return target.GetComponent<Door>() != null; }

    public override void SetTarget(GameObject target) {
        var component = target.GetComponent<Door>();
        if (component == null) {
            Debug.LogError("Target doesn't have a Door associated with it.");
            return;
        }

        this.target = component;
        Refresh();
    }

    private void Refresh() {
        if (!target.isSealed) {
            ContentContainer.SetActive(false);
            return;
        }

        ContentContainer.SetActive(true);
    }
}