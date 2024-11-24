﻿using RsLib;
using UnityEngine;
using UnityEngine.UI;



public class PriorityImage : MonoBehaviour {
    [SerializeField]
    private Image image;

    [SerializeField]
    private int m_priority;

    public int priority {
        get => m_priority;
        set {
            if (m_priority != value) {
                m_priority = value;
                UpdateSprite();
            }
        }
    }

    private void Awake() {
        if (image == null) image = GetComponent<Image>();
    }

    private void Start() { UpdateSprite(); }

    private void UpdateSprite() {
        var clamp = Mathf.Clamp(m_priority, 1, 9);
        image.sprite = RsUITuning.Images.GetSpriteByName("priority_" + clamp);
        image.SetNativeSize();
    }
}