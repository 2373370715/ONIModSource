using UnityEngine;
using UnityEngine.UI;



public class LineCenterImage : MonoBehaviour {
    public LineCenterAsset asset;
    public Image image;
    public void SetImage(BuildingType buildingType) { image.sprite = asset.GetSpriteByBuildingType(buildingType); }
    public void SetColor(Color color) { image.color = color; }
}