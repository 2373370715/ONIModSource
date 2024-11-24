using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C98 RID: 7320
public class DebugPaintElementScreen : KScreen
{
	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x060098A9 RID: 39081 RVA: 0x00103551 File Offset: 0x00101751
	// (set) Token: 0x060098AA RID: 39082 RVA: 0x00103558 File Offset: 0x00101758
	public static DebugPaintElementScreen Instance { get; private set; }

	// Token: 0x060098AB RID: 39083 RVA: 0x00103560 File Offset: 0x00101760
	public static void DestroyInstance()
	{
		DebugPaintElementScreen.Instance = null;
	}

	// Token: 0x060098AC RID: 39084 RVA: 0x003B0FBC File Offset: 0x003AF1BC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugPaintElementScreen.Instance = this;
		this.SetupLocText();
		this.inputFields.Add(this.massPressureInput);
		this.inputFields.Add(this.temperatureInput);
		this.inputFields.Add(this.diseaseCountInput);
		this.inputFields.Add(this.filterInput);
		foreach (KInputTextField kinputTextField in this.inputFields)
		{
			kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
			{
				base.isEditing = true;
			}));
			kinputTextField.onEndEdit.AddListener(delegate(string value)
			{
				base.isEditing = false;
			});
		}
		this.temperatureInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnChangeTemperature();
		});
		this.massPressureInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnChangeMassPressure();
		});
		this.diseaseCountInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnDiseaseCountChange();
		});
		base.gameObject.SetActive(false);
		this.activateOnSpawn = true;
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060098AD RID: 39085 RVA: 0x003B1104 File Offset: 0x003AF304
	private void SetupLocText()
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("Title").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TITLE;
		component.GetReference<LocText>("ElementLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		component.GetReference<LocText>("MassLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.MASS_KG;
		component.GetReference<LocText>("TemperatureLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TEMPERATURE_KELVIN;
		component.GetReference<LocText>("DiseaseLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		component.GetReference<LocText>("DiseaseCountLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE_COUNT;
		component.GetReference<LocText>("AddFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ADD_FOW_MASK;
		component.GetReference<LocText>("RemoveFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.REMOVE_FOW_MASK;
		this.elementButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		this.diseaseButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		this.paintButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.PAINT;
		this.fillButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.FILL;
		this.spawnButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SPAWN_ALL;
		this.sampleButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SAMPLE;
		this.storeButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.STORE;
		this.affectBuildings.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.BUILDINGS;
		this.affectCells.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.CELLS;
	}

	// Token: 0x060098AE RID: 39086 RVA: 0x003B12F8 File Offset: 0x003AF4F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.element = SimHashes.Ice;
		this.diseaseIdx = byte.MaxValue;
		this.ConfigureElements();
		List<string> list = new List<string>();
		list.Insert(0, "None");
		foreach (Disease disease in Db.Get().Diseases.resources)
		{
			list.Add(disease.Name);
		}
		this.diseasePopup.SetOptions(list.ToArray());
		KPopupMenu kpopupMenu = this.diseasePopup;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelectDisease));
		this.SelectDiseaseOption((int)this.diseaseIdx);
		this.paintButton.onClick += this.OnClickPaint;
		this.fillButton.onClick += this.OnClickFill;
		this.sampleButton.onClick += this.OnClickSample;
		this.storeButton.onClick += this.OnClickStore;
		if (SaveGame.Instance.worldGenSpawner.SpawnsRemain())
		{
			this.spawnButton.onClick += this.OnClickSpawn;
		}
		KPopupMenu kpopupMenu2 = this.elementPopup;
		kpopupMenu2.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu2.OnSelect, new Action<string, int>(this.OnSelectElement));
		this.elementButton.onClick += this.elementPopup.OnClick;
		this.diseaseButton.onClick += this.diseasePopup.OnClick;
	}

	// Token: 0x060098AF RID: 39087 RVA: 0x003B14B4 File Offset: 0x003AF6B4
	private void FilterElements(string filterValue)
	{
		if (string.IsNullOrEmpty(filterValue))
		{
			foreach (KButtonMenu.ButtonInfo buttonInfo in this.elementPopup.GetButtons())
			{
				buttonInfo.uibutton.gameObject.SetActive(true);
			}
			return;
		}
		filterValue = this.filter.ToLower();
		foreach (KButtonMenu.ButtonInfo buttonInfo2 in this.elementPopup.GetButtons())
		{
			buttonInfo2.uibutton.gameObject.SetActive(buttonInfo2.text.ToLower().Contains(filterValue));
		}
	}

	// Token: 0x060098B0 RID: 39088 RVA: 0x003B1580 File Offset: 0x003AF780
	private void ConfigureElements()
	{
		if (this.filter != null)
		{
			this.filter = this.filter.ToLower();
		}
		List<DebugPaintElementScreen.ElemDisplayInfo> list = new List<DebugPaintElementScreen.ElemDisplayInfo>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.name != "Element Not Loaded" && element.substance != null && element.substance.showInEditor && (string.IsNullOrEmpty(this.filter) || element.name.ToLower().Contains(this.filter)))
			{
				list.Add(new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element.id,
					displayStr = element.name + " (" + element.GetStateString() + ")"
				});
			}
		}
		list.Sort((DebugPaintElementScreen.ElemDisplayInfo a, DebugPaintElementScreen.ElemDisplayInfo b) => a.displayStr.CompareTo(b.displayStr));
		if (string.IsNullOrEmpty(this.filter))
		{
			SimHashes[] array = new SimHashes[]
			{
				SimHashes.SlimeMold,
				SimHashes.Vacuum,
				SimHashes.Dirt,
				SimHashes.CarbonDioxide,
				SimHashes.Water,
				SimHashes.Oxygen
			};
			for (int i = 0; i < array.Length; i++)
			{
				Element element2 = ElementLoader.FindElementByHash(array[i]);
				list.Insert(0, new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element2.id,
					displayStr = element2.name + " (" + element2.GetStateString() + ")"
				});
			}
		}
		this.options_list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (DebugPaintElementScreen.ElemDisplayInfo elemDisplayInfo in list)
		{
			list2.Add(elemDisplayInfo.displayStr);
			this.options_list.Add(elemDisplayInfo.id.ToString());
		}
		this.elementPopup.SetOptions(list2);
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].id == this.element)
			{
				this.elementPopup.SelectOption(list2[j], j);
			}
		}
		this.elementPopup.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0f, 1f);
	}

	// Token: 0x060098B1 RID: 39089 RVA: 0x003B1800 File Offset: 0x003AFA00
	private void OnClickSpawn()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.SetDiscovered(true);
		}
		SaveGame.Instance.worldGenSpawner.SpawnEverything();
		this.spawnButton.GetComponent<KButton>().isInteractable = false;
	}

	// Token: 0x060098B2 RID: 39090 RVA: 0x00103568 File Offset: 0x00101768
	private void OnClickPaint()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.ReplaceSubstance);
	}

	// Token: 0x060098B3 RID: 39091 RVA: 0x0010358D File Offset: 0x0010178D
	private void OnClickStore()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.StoreSubstance);
	}

	// Token: 0x060098B4 RID: 39092 RVA: 0x001035B2 File Offset: 0x001017B2
	private void OnClickSample()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.Sample);
	}

	// Token: 0x060098B5 RID: 39093 RVA: 0x001035D7 File Offset: 0x001017D7
	private void OnClickFill()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		DebugTool.Instance.Activate(DebugTool.Type.FillReplaceSubstance);
	}

	// Token: 0x060098B6 RID: 39094 RVA: 0x001035F6 File Offset: 0x001017F6
	private void OnSelectElement(string str, int index)
	{
		this.element = (SimHashes)Enum.Parse(typeof(SimHashes), this.options_list[index]);
		this.elementButton.GetComponentInChildren<LocText>().text = str;
	}

	// Token: 0x060098B7 RID: 39095 RVA: 0x0010362F File Offset: 0x0010182F
	private void OnSelectElement(SimHashes element)
	{
		this.element = element;
		this.elementButton.GetComponentInChildren<LocText>().text = ElementLoader.FindElementByHash(element).name;
	}

	// Token: 0x060098B8 RID: 39096 RVA: 0x003B1870 File Offset: 0x003AFA70
	private void OnSelectDisease(string str, int index)
	{
		this.diseaseIdx = byte.MaxValue;
		for (int i = 0; i < Db.Get().Diseases.Count; i++)
		{
			if (Db.Get().Diseases[i].Name == str)
			{
				this.diseaseIdx = (byte)i;
			}
		}
		this.SelectDiseaseOption((int)this.diseaseIdx);
	}

	// Token: 0x060098B9 RID: 39097 RVA: 0x003B18D4 File Offset: 0x003AFAD4
	private void SelectDiseaseOption(int diseaseIdx)
	{
		if (diseaseIdx == 255)
		{
			this.diseaseButton.GetComponentInChildren<LocText>().text = "None";
			return;
		}
		string name = Db.Get().Diseases[diseaseIdx].Name;
		this.diseaseButton.GetComponentInChildren<LocText>().text = name;
	}

	// Token: 0x060098BA RID: 39098 RVA: 0x003B1928 File Offset: 0x003AFB28
	private void OnChangeFOWReveal()
	{
		if (this.paintPreventFOWReveal.isOn)
		{
			this.paintAllowFOWReveal.isOn = false;
		}
		if (this.paintAllowFOWReveal.isOn)
		{
			this.paintPreventFOWReveal.isOn = false;
		}
		this.set_prevent_fow_reveal = this.paintPreventFOWReveal.isOn;
		this.set_allow_fow_reveal = this.paintAllowFOWReveal.isOn;
	}

	// Token: 0x060098BB RID: 39099 RVA: 0x003B198C File Offset: 0x003AFB8C
	public void OnChangeMassPressure()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.massPressureInput.text);
		}
		catch
		{
			num = -1f;
		}
		if (num <= 0f)
		{
			num = 1f;
			this.massPressureInput.text = "1";
		}
		this.mass = num;
	}

	// Token: 0x060098BC RID: 39100 RVA: 0x003B19EC File Offset: 0x003AFBEC
	public void OnChangeTemperature()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.temperatureInput.text);
		}
		catch
		{
			num = -1f;
		}
		if (num <= 0f)
		{
			num = 1f;
			this.temperatureInput.text = "1";
		}
		this.temperature = num;
	}

	// Token: 0x060098BD RID: 39101 RVA: 0x003B1A4C File Offset: 0x003AFC4C
	public void OnDiseaseCountChange()
	{
		int num;
		int.TryParse(this.diseaseCountInput.text, out num);
		if (num < 0)
		{
			num = 0;
			this.diseaseCountInput.text = "0";
		}
		this.diseaseCount = num;
	}

	// Token: 0x060098BE RID: 39102 RVA: 0x00103653 File Offset: 0x00101853
	public void OnElementsFilterEdited(string new_filter)
	{
		this.filter = (string.IsNullOrEmpty(this.filterInput.text) ? null : this.filterInput.text);
		this.FilterElements(this.filter);
	}

	// Token: 0x060098BF RID: 39103 RVA: 0x003B1A8C File Offset: 0x003AFC8C
	public void SampleCell(int cell)
	{
		this.massPressureInput.text = (Grid.Pressure[cell] * 0.010000001f).ToString();
		this.temperatureInput.text = Grid.Temperature[cell].ToString();
		this.OnSelectElement(ElementLoader.GetElementID(Grid.Element[cell].tag));
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
	}

	// Token: 0x040076E4 RID: 30436
	[Header("Current State")]
	public SimHashes element;

	// Token: 0x040076E5 RID: 30437
	[NonSerialized]
	public float mass = 1000f;

	// Token: 0x040076E6 RID: 30438
	[NonSerialized]
	public float temperature = -1f;

	// Token: 0x040076E7 RID: 30439
	[NonSerialized]
	public bool set_prevent_fow_reveal;

	// Token: 0x040076E8 RID: 30440
	[NonSerialized]
	public bool set_allow_fow_reveal;

	// Token: 0x040076E9 RID: 30441
	[NonSerialized]
	public int diseaseCount;

	// Token: 0x040076EA RID: 30442
	public byte diseaseIdx;

	// Token: 0x040076EB RID: 30443
	[Header("Popup Buttons")]
	[SerializeField]
	private KButton elementButton;

	// Token: 0x040076EC RID: 30444
	[SerializeField]
	private KButton diseaseButton;

	// Token: 0x040076ED RID: 30445
	[Header("Popup Menus")]
	[SerializeField]
	private KPopupMenu elementPopup;

	// Token: 0x040076EE RID: 30446
	[SerializeField]
	private KPopupMenu diseasePopup;

	// Token: 0x040076EF RID: 30447
	[Header("Value Inputs")]
	[SerializeField]
	private KInputTextField massPressureInput;

	// Token: 0x040076F0 RID: 30448
	[SerializeField]
	private KInputTextField temperatureInput;

	// Token: 0x040076F1 RID: 30449
	[SerializeField]
	private KInputTextField diseaseCountInput;

	// Token: 0x040076F2 RID: 30450
	[SerializeField]
	private KInputTextField filterInput;

	// Token: 0x040076F3 RID: 30451
	[Header("Tool Buttons")]
	[SerializeField]
	private KButton paintButton;

	// Token: 0x040076F4 RID: 30452
	[SerializeField]
	private KButton fillButton;

	// Token: 0x040076F5 RID: 30453
	[SerializeField]
	private KButton sampleButton;

	// Token: 0x040076F6 RID: 30454
	[SerializeField]
	private KButton spawnButton;

	// Token: 0x040076F7 RID: 30455
	[SerializeField]
	private KButton storeButton;

	// Token: 0x040076F8 RID: 30456
	[Header("Parameter Toggles")]
	public Toggle paintElement;

	// Token: 0x040076F9 RID: 30457
	public Toggle paintMass;

	// Token: 0x040076FA RID: 30458
	public Toggle paintTemperature;

	// Token: 0x040076FB RID: 30459
	public Toggle paintDisease;

	// Token: 0x040076FC RID: 30460
	public Toggle paintDiseaseCount;

	// Token: 0x040076FD RID: 30461
	public Toggle affectBuildings;

	// Token: 0x040076FE RID: 30462
	public Toggle affectCells;

	// Token: 0x040076FF RID: 30463
	public Toggle paintPreventFOWReveal;

	// Token: 0x04007700 RID: 30464
	public Toggle paintAllowFOWReveal;

	// Token: 0x04007701 RID: 30465
	private List<KInputTextField> inputFields = new List<KInputTextField>();

	// Token: 0x04007702 RID: 30466
	private List<string> options_list = new List<string>();

	// Token: 0x04007703 RID: 30467
	private string filter;

	// Token: 0x02001C99 RID: 7321
	private struct ElemDisplayInfo
	{
		// Token: 0x04007704 RID: 30468
		public SimHashes id;

		// Token: 0x04007705 RID: 30469
		public string displayStr;
	}
}
