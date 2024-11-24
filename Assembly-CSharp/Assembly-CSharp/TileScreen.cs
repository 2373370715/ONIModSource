﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileScreen : KScreen
{
	private bool SetSliderColour(float temperature, float transition_temperature)
	{
		if (Mathf.Abs(temperature - transition_temperature) < 5f)
		{
			this.temperatureSliderText.color = this.temperatureTransitionColour;
			this.temperatureSliderIcon.color = this.temperatureTransitionColour;
			return true;
		}
		this.temperatureSliderText.color = this.temperatureDefaultColour;
		this.temperatureSliderIcon.color = this.temperatureDefaultColour;
		return false;
	}

	private void DisplayTileInfo()
	{
		Vector3 mousePos = KInputManager.GetMousePos();
		mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(mousePos));
		if (Grid.IsValidCell(num) && Grid.IsVisible(num))
		{
			Element element = Grid.Element[num];
			this.nameLabel.text = element.name;
			float num2 = Grid.Mass[num];
			string arg = "kg";
			if (num2 < 5f)
			{
				num2 *= 1000f;
				arg = "g";
			}
			if (num2 < 5f)
			{
				num2 *= 1000f;
				arg = "mg";
			}
			if (num2 < 5f)
			{
				num2 *= 1000f;
				arg = "mcg";
				num2 = Mathf.Floor(num2);
			}
			this.massAmtLabel.text = string.Format("{0:0.0} {1}", num2, arg);
			this.massTitleLabel.text = "mass";
			float num3 = Grid.Temperature[num];
			if (element.IsSolid)
			{
				this.solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
				this.gasIcon.gameObject.transform.parent.gameObject.SetActive(false);
				this.massIcon.sprite = this.solidIcon.sprite;
				this.solidText.text = ((int)element.highTemp).ToString();
				this.gasText.text = "";
				this.liquidIcon.rectTransform.SetParent(this.solidIcon.transform.parent, true);
				this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(0f, 64f));
				this.SetSliderColour(num3, element.highTemp);
				this.temperatureSlider.SetMinMaxValue(element.highTemp, Mathf.Min(element.highTemp + 100f, 4000f), Mathf.Max(element.highTemp - 100f, 0f), Mathf.Min(element.highTemp + 100f, 4000f));
			}
			else if (element.IsLiquid)
			{
				this.solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
				this.gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
				this.massIcon.sprite = this.liquidIcon.sprite;
				this.solidText.text = ((int)element.lowTemp).ToString();
				this.gasText.text = ((int)element.highTemp).ToString();
				this.liquidIcon.rectTransform.SetParent(this.temperatureSlider.transform.parent, true);
				this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(-80f, 0f));
				if (!this.SetSliderColour(num3, element.lowTemp))
				{
					this.SetSliderColour(num3, element.highTemp);
				}
				this.temperatureSlider.SetMinMaxValue(element.lowTemp, element.highTemp, Mathf.Max(element.lowTemp - 100f, 0f), Mathf.Min(element.highTemp + 100f, 5200f));
			}
			else if (element.IsGas)
			{
				this.solidText.text = "";
				this.gasText.text = ((int)element.lowTemp).ToString();
				this.solidIcon.gameObject.transform.parent.gameObject.SetActive(false);
				this.gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
				this.massIcon.sprite = this.gasIcon.sprite;
				this.SetSliderColour(num3, element.lowTemp);
				this.liquidIcon.rectTransform.SetParent(this.gasIcon.transform.parent, true);
				this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(0f, -64f));
				this.temperatureSlider.SetMinMaxValue(0f, Mathf.Max(element.lowTemp - 100f, 0f), 0f, element.lowTemp + 100f);
			}
			this.temperatureSlider.SetExtraValue(num3);
			this.temperatureSliderText.text = GameUtil.GetFormattedTemperature((float)((int)num3), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			Dictionary<int, float> info = FallingWater.instance.GetInfo(num);
			if (info.Count <= 0)
			{
				return;
			}
			List<Element> elements = ElementLoader.elements;
			using (Dictionary<int, float>.Enumerator enumerator = info.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, float> keyValuePair = enumerator.Current;
					Element element2 = elements[keyValuePair.Key];
					Text text = this.nameLabel;
					text.text = text.text + "\n" + element2.name + string.Format(" {0:0.00} kg", keyValuePair.Value);
				}
				return;
			}
		}
		this.nameLabel.text = "Unknown";
	}

	private void DisplayConduitFlowInfo()
	{
		HashedString mode = OverlayScreen.Instance.GetMode();
		UtilityNetworkManager<FlowUtilityNetwork, Vent> utilityNetworkManager = (mode == OverlayModes.GasConduits.ID) ? Game.Instance.gasConduitSystem : Game.Instance.liquidConduitSystem;
		ConduitFlow conduitFlow = (mode == OverlayModes.LiquidConduits.ID) ? Game.Instance.gasConduitFlow : Game.Instance.liquidConduitFlow;
		Vector3 mousePos = KInputManager.GetMousePos();
		mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
		int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(mousePos));
		if (Grid.IsValidCell(cell) && utilityNetworkManager.GetConnections(cell, true) != (UtilityConnections)0)
		{
			ConduitFlow.ConduitContents contents = conduitFlow.GetContents(cell);
			Element element = ElementLoader.FindElementByHash(contents.element);
			float num = contents.mass;
			float temperature = contents.temperature;
			this.nameLabel.text = element.name;
			string arg = "kg";
			if (num < 5f)
			{
				num *= 1000f;
				arg = "g";
			}
			this.massAmtLabel.text = string.Format("{0:0.0} {1}", num, arg);
			this.massTitleLabel.text = "mass";
			if (element.IsLiquid)
			{
				this.solidIcon.gameObject.transform.parent.gameObject.SetActive(true);
				this.gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
				this.massIcon.sprite = this.liquidIcon.sprite;
				this.solidText.text = ((int)element.lowTemp).ToString();
				this.gasText.text = ((int)element.highTemp).ToString();
				this.liquidIcon.rectTransform.SetParent(this.temperatureSlider.transform.parent, true);
				this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(-80f, 0f));
				if (!this.SetSliderColour(temperature, element.lowTemp))
				{
					this.SetSliderColour(temperature, element.highTemp);
				}
				this.temperatureSlider.SetMinMaxValue(element.lowTemp, element.highTemp, Mathf.Max(element.lowTemp - 100f, 0f), Mathf.Min(element.highTemp + 100f, 5200f));
			}
			else if (element.IsGas)
			{
				this.solidText.text = "";
				this.gasText.text = ((int)element.lowTemp).ToString();
				this.solidIcon.gameObject.transform.parent.gameObject.SetActive(false);
				this.gasIcon.gameObject.transform.parent.gameObject.SetActive(true);
				this.massIcon.sprite = this.gasIcon.sprite;
				this.SetSliderColour(temperature, element.lowTemp);
				this.liquidIcon.rectTransform.SetParent(this.gasIcon.transform.parent, true);
				this.liquidIcon.rectTransform.SetLocalPosition(new Vector3(0f, -64f));
				this.temperatureSlider.SetMinMaxValue(0f, Mathf.Max(element.lowTemp - 100f, 0f), 0f, element.lowTemp + 100f);
			}
			this.temperatureSlider.SetExtraValue(temperature);
			this.temperatureSliderText.text = GameUtil.GetFormattedTemperature((float)((int)temperature), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			return;
		}
		this.nameLabel.text = "No Conduit";
		this.symbolLabel.text = "";
		this.massAmtLabel.text = "";
		this.massTitleLabel.text = "";
	}

	private void Update()
	{
		base.transform.SetPosition(KInputManager.GetMousePos());
		HashedString mode = OverlayScreen.Instance.GetMode();
		if (mode == OverlayModes.GasConduits.ID || mode == OverlayModes.LiquidConduits.ID)
		{
			this.DisplayConduitFlowInfo();
			return;
		}
		this.DisplayTileInfo();
	}

	public Text nameLabel;

	public Text symbolLabel;

	public Text massTitleLabel;

	public Text massAmtLabel;

	public Image massIcon;

	public MinMaxSlider temperatureSlider;

	public Text temperatureSliderText;

	public Image temperatureSliderIcon;

	public Image solidIcon;

	public Image liquidIcon;

	public Image gasIcon;

	public Text solidText;

	public Text gasText;

	[SerializeField]
	private Color temperatureDefaultColour;

	[SerializeField]
	private Color temperatureTransitionColour;
}
