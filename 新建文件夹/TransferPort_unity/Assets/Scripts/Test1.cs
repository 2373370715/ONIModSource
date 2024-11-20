using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Test1 : MonoBehaviour
{

    public SpriteAtlas atlas;
    public Image image;
    void Start()
    {
        Sprite sprite = atlas.GetSprite("overview_highlight_outline_sharp");
        // Debug.Log(sprite);
        image.sprite = sprite;
    }

    void Update()
    {
        
    }
}
