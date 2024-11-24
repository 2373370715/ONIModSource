using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02002027 RID: 8231
[AddComponentMenu("KMonoBehaviour/scripts/TemplateSelectionInfoPanel")]
public class TemplateSelectionInfoPanel : KMonoBehaviour, IRender1000ms
{
	// Token: 0x0600AF33 RID: 44851 RVA: 0x0041ECD4 File Offset: 0x0041CED4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.details.Length; i++)
		{
			Util.KInstantiateUI(this.prefab_detail_label, this.current_detail_container, true);
		}
		this.RefreshDetails();
		this.save_button.onClick += this.SaveCurrentDetails;
	}

	// Token: 0x0600AF34 RID: 44852 RVA: 0x0041ED2C File Offset: 0x0041CF2C
	public void SaveCurrentDetails()
	{
		string text = "";
		for (int i = 0; i < this.details.Length; i++)
		{
			text = text + this.details[i](DebugBaseTemplateButton.Instance.SelectedCells) + "\n";
		}
		text += "\n\n";
		text += this.saved_detail_label.text;
		this.saved_detail_label.text = text;
	}

	// Token: 0x0600AF35 RID: 44853 RVA: 0x00111E66 File Offset: 0x00110066
	public void Render1000ms(float dt)
	{
		this.RefreshDetails();
	}

	// Token: 0x0600AF36 RID: 44854 RVA: 0x0041EDA0 File Offset: 0x0041CFA0
	public void RefreshDetails()
	{
		for (int i = 0; i < this.details.Length; i++)
		{
			this.current_detail_container.transform.GetChild(i).GetComponent<LocText>().text = this.details[i](DebugBaseTemplateButton.Instance.SelectedCells);
		}
	}

	// Token: 0x0600AF37 RID: 44855 RVA: 0x0041EDF4 File Offset: 0x0041CFF4
	private static string TotalMass(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Mass[i];
		}
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_MASS, GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x0600AF38 RID: 44856 RVA: 0x0041EE6C File Offset: 0x0041D06C
	private static string AverageMass(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Mass[i];
		}
		num /= (float)cells.Count;
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_MASS, GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x0600AF39 RID: 44857 RVA: 0x0041EEF0 File Offset: 0x0041D0F0
	private static string AverageTemperature(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Temperature[i];
		}
		num /= (float)cells.Count;
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_TEMPERATURE, GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x0600AF3A RID: 44858 RVA: 0x0041EF70 File Offset: 0x0041D170
	private static string TotalJoules(List<int> cells)
	{
		List<GameObject> list = new List<GameObject>();
		float num = 0f;
		foreach (int num2 in cells)
		{
			num += Grid.Element[num2].specificHeatCapacity * Grid.Temperature[num2] * (Grid.Mass[num2] * 1000f);
			num += TemplateSelectionInfoPanel.GetCellEntityEnergy(num2, ref list);
		}
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_JOULES, GameUtil.GetFormattedJoules(num, "F5", GameUtil.TimeSlice.None));
	}

	// Token: 0x0600AF3B RID: 44859 RVA: 0x0041F018 File Offset: 0x0041D218
	private static float GetCellEntityEnergy(int cell, ref List<GameObject> ignoreObjects)
	{
		float num = 0f;
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (!(gameObject == null) && !ignoreObjects.Contains(gameObject))
			{
				ignoreObjects.Add(gameObject);
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Element != null)
				{
					float num2 = component.Mass;
					Building component2 = gameObject.GetComponent<Building>();
					if (component2 != null)
					{
						num2 = component2.Def.MassForTemperatureModification;
					}
					float num3 = num2 * 1000f * component.Element.specificHeatCapacity * component.Temperature;
					num += num3;
					Storage[] components = gameObject.GetComponents<Storage>();
					if (components != null)
					{
						float num4 = 0f;
						Storage[] array = components;
						for (int j = 0; j < array.Length; j++)
						{
							foreach (GameObject gameObject2 in array[j].items)
							{
								PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
								if (!(component3 == null))
								{
									num4 += component3.Mass * 1000f * component3.Element.specificHeatCapacity * component3.Temperature;
								}
							}
						}
						num += num4;
					}
					Conduit component4 = gameObject.GetComponent<Conduit>();
					if (component4 != null)
					{
						ConduitFlow.ConduitContents contents = component4.GetFlowManager().GetContents(cell);
						if (contents.mass > 0f)
						{
							Element element = ElementLoader.FindElementByHash(contents.element);
							float num5 = contents.mass * 1000f * element.specificHeatCapacity * contents.temperature;
							num += num5;
						}
					}
					if (gameObject.GetComponent<SolidConduit>() != null)
					{
						SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
						SolidConduitFlow.ConduitContents contents2 = solidConduitFlow.GetContents(cell);
						if (contents2.pickupableHandle.IsValid())
						{
							Pickupable pickupable = solidConduitFlow.GetPickupable(contents2.pickupableHandle);
							if (pickupable)
							{
								PrimaryElement component5 = pickupable.GetComponent<PrimaryElement>();
								if (component5.Mass > 0f)
								{
									float num6 = component5.Mass * 1000f * component5.Element.specificHeatCapacity * component5.Temperature;
									num += num6;
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x0600AF3C RID: 44860 RVA: 0x0041F270 File Offset: 0x0041D470
	private static string JoulesPerKilogram(List<int> cells)
	{
		float num = 0f;
		float num2 = 0f;
		foreach (int num3 in cells)
		{
			num += Grid.Element[num3].specificHeatCapacity * Grid.Temperature[num3] * (Grid.Mass[num3] * 1000f);
			num2 += Grid.Mass[num3];
		}
		num /= num2;
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.JOULES_PER_KILOGRAM, GameUtil.GetFormattedJoules(num, "F1", GameUtil.TimeSlice.None));
	}

	// Token: 0x0600AF3D RID: 44861 RVA: 0x0041F320 File Offset: 0x0041D520
	private static string TotalRadiation(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Radiation[i];
		}
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_RADS, GameUtil.GetFormattedRads(num, GameUtil.TimeSlice.None));
	}

	// Token: 0x0600AF3E RID: 44862 RVA: 0x0041F394 File Offset: 0x0041D594
	private static string AverageRadiation(List<int> cells)
	{
		float num = 0f;
		foreach (int i in cells)
		{
			num += Grid.Radiation[i];
		}
		num /= (float)cells.Count;
		return string.Format(UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_RADS, GameUtil.GetFormattedRads(num, GameUtil.TimeSlice.None));
	}

	// Token: 0x0600AF3F RID: 44863 RVA: 0x0041F410 File Offset: 0x0041D610
	private static string MassPerElement(List<int> cells)
	{
		TemplateSelectionInfoPanel.mass_per_element.Clear();
		foreach (int num in cells)
		{
			bool flag = false;
			for (int i = 0; i < TemplateSelectionInfoPanel.mass_per_element.Count; i++)
			{
				if (TemplateSelectionInfoPanel.mass_per_element[i].first == Grid.Element[num])
				{
					TemplateSelectionInfoPanel.mass_per_element[i].second += Grid.Mass[num];
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TemplateSelectionInfoPanel.mass_per_element.Add(new global::Tuple<Element, float>(Grid.Element[num], Grid.Mass[num]));
			}
		}
		TemplateSelectionInfoPanel.mass_per_element.Sort(delegate(global::Tuple<Element, float> a, global::Tuple<Element, float> b)
		{
			if (a.second > b.second)
			{
				return -1;
			}
			if (b.second > a.second)
			{
				return 1;
			}
			return 0;
		});
		string text = "";
		foreach (global::Tuple<Element, float> tuple in TemplateSelectionInfoPanel.mass_per_element)
		{
			text = string.Concat(new string[]
			{
				text,
				tuple.first.name,
				": ",
				GameUtil.GetFormattedMass(tuple.second, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
				"\n"
			});
		}
		return text;
	}

	// Token: 0x040089F0 RID: 35312
	[SerializeField]
	private GameObject prefab_detail_label;

	// Token: 0x040089F1 RID: 35313
	[SerializeField]
	private GameObject current_detail_container;

	// Token: 0x040089F2 RID: 35314
	[SerializeField]
	private LocText saved_detail_label;

	// Token: 0x040089F3 RID: 35315
	[SerializeField]
	private KButton save_button;

	// Token: 0x040089F4 RID: 35316
	private Func<List<int>, string>[] details = new Func<List<int>, string>[]
	{
		new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalMass),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageMass),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageTemperature),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalJoules),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.JoulesPerKilogram),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.MassPerElement),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalRadiation),
		new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageRadiation)
	};

	// Token: 0x040089F5 RID: 35317
	private static List<global::Tuple<Element, float>> mass_per_element = new List<global::Tuple<Element, float>>();
}
