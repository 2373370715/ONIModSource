using UnityEngine;
using UnityEngine.UI;

public class AlternateSiblingColor : KMonoBehaviour {
    public  Color evenColor;
    public  Image image;
    private int   mySiblingIndex;
    public  Color oddColor;

    protected override void OnSpawn() {
        base.OnSpawn();
        var siblingIndex = transform.GetSiblingIndex();
        RefreshColor(siblingIndex % 2 == 0);
    }

    private void RefreshColor(bool evenIndex) {
        if (image == null) return;

        image.color = evenIndex ? evenColor : oddColor;
    }

    private void Update() {
        if (mySiblingIndex != transform.GetSiblingIndex()) {
            mySiblingIndex = transform.GetSiblingIndex();
            RefreshColor(mySiblingIndex % 2 == 0);
        }
    }
}