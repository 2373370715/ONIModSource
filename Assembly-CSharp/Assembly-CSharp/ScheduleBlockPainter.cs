﻿using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockPainter")]
public class ScheduleBlockPainter : KMonoBehaviour
{
	public void Setup(Action<float> blockPaintHandler)
	{
		this.blockPaintHandler = blockPaintHandler;
		this.button.onPointerDown += this.OnPointerDown;
		this.button.onDrag += this.OnDrag;
	}

	private void OnPointerDown()
	{
		this.Transmit();
	}

	private void OnDrag()
	{
		this.Transmit();
	}

	private void Transmit()
	{
		float obj = (base.transform.InverseTransformPoint(KInputManager.GetMousePos()).x - this.rectTransform.rect.x) / this.rectTransform.rect.width;
		this.blockPaintHandler(obj);
	}

	[SerializeField]
	private KButtonDrag button;

	private Action<float> blockPaintHandler;

	[MyCmpGet]
	private RectTransform rectTransform;
}
