using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class DebugPaintElementScreen : KScreen {
    public Toggle affectBuildings;
    public Toggle affectCells;

    [SerializeField]
    private KButton diseaseButton;

    [NonSerialized]
    public int diseaseCount;

    [SerializeField]
    private KInputTextField diseaseCountInput;

    public byte diseaseIdx;

    [SerializeField]
    private KPopupMenu diseasePopup;

    [Header("Current State")]
    public SimHashes element;

    [Header("Popup Buttons"), SerializeField]
    private KButton elementButton;

    [Header("Popup Menus"), SerializeField]
    private KPopupMenu elementPopup;

    [SerializeField]
    private KButton fillButton;

    private string filter;

    [SerializeField]
    private KInputTextField filterInput;

    private readonly List<KInputTextField> inputFields = new List<KInputTextField>();

    [NonSerialized]
    public float mass = 1000f;

    [Header("Value Inputs"), SerializeField]
    private KInputTextField massPressureInput;

    private List<string> options_list = new List<string>();
    public  Toggle       paintAllowFOWReveal;

    [Header("Tool Buttons"), SerializeField]
    private KButton paintButton;

    public Toggle paintDisease;
    public Toggle paintDiseaseCount;

    [Header("Parameter Toggles")]
    public Toggle paintElement;

    public Toggle paintMass;
    public Toggle paintPreventFOWReveal;
    public Toggle paintTemperature;

    [SerializeField]
    private KButton sampleButton;

    [NonSerialized]
    public bool set_allow_fow_reveal;

    [NonSerialized]
    public bool set_prevent_fow_reveal;

    [SerializeField]
    private KButton spawnButton;

    [SerializeField]
    private KButton storeButton;

    [NonSerialized]
    public float temperature = -1f;

    [SerializeField]
    private KInputTextField temperatureInput;

    public static DebugPaintElementScreen Instance          { get; private set; }
    public static void                    DestroyInstance() { Instance = null; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Instance = this;
        SetupLocText();
        inputFields.Add(massPressureInput);
        inputFields.Add(temperatureInput);
        inputFields.Add(diseaseCountInput);
        inputFields.Add(filterInput);
        foreach (var kinputTextField in inputFields) {
            kinputTextField.onFocus
                = (System.Action)Delegate.Combine(kinputTextField.onFocus,
                                                  new System.Action(delegate { isEditing = true; }));

            kinputTextField.onEndEdit.AddListener(delegate { isEditing = false; });
        }

        temperatureInput.onEndEdit.AddListener(delegate { OnChangeTemperature(); });
        massPressureInput.onEndEdit.AddListener(delegate { OnChangeMassPressure(); });
        diseaseCountInput.onEndEdit.AddListener(delegate { OnDiseaseCountChange(); });
        gameObject.SetActive(false);
        activateOnSpawn    = true;
        ConsumeMouseScroll = true;
    }

    private void SetupLocText() {
        var component = GetComponent<HierarchyReferences>();
        component.GetReference<LocText>("Title").text        = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TITLE;
        component.GetReference<LocText>("ElementLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
        component.GetReference<LocText>("MassLabel").text    = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.MASS_KG;
        component.GetReference<LocText>("TemperatureLabel").text
            = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TEMPERATURE_KELVIN;

        component.GetReference<LocText>("DiseaseLabel").text      = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
        component.GetReference<LocText>("DiseaseCountLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE_COUNT;
        component.GetReference<LocText>("AddFoWMaskLabel").text   = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ADD_FOW_MASK;
        component.GetReference<LocText>("RemoveFoWMaskLabel").text
            = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.REMOVE_FOW_MASK;

        elementButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
        diseaseButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
        paintButton.GetComponentsInChildren<LocText>()[0].text   = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.PAINT;
        fillButton.GetComponentsInChildren<LocText>()[0].text    = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.FILL;
        spawnButton.GetComponentsInChildren<LocText>()[0].text   = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SPAWN_ALL;
        sampleButton.GetComponentsInChildren<LocText>()[0].text  = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SAMPLE;
        storeButton.GetComponentsInChildren<LocText>()[0].text   = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.STORE;
        affectBuildings.transform.parent.GetComponentsInChildren<LocText>()[0].text
            = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.BUILDINGS;

        affectCells.transform.parent.GetComponentsInChildren<LocText>()[0].text
            = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.CELLS;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        element    = SimHashes.Ice;
        diseaseIdx = byte.MaxValue;
        ConfigureElements();
        var list = new List<string>();
        list.Insert(0, "None");
        foreach (var disease in Db.Get().Diseases.resources) list.Add(disease.Name);
        diseasePopup.SetOptions(list.ToArray());
        var kpopupMenu = diseasePopup;
        kpopupMenu.OnSelect
            = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(OnSelectDisease));

        SelectDiseaseOption(diseaseIdx);
        paintButton.onClick  += OnClickPaint;
        fillButton.onClick   += OnClickFill;
        sampleButton.onClick += OnClickSample;
        storeButton.onClick  += OnClickStore;
        if (SaveGame.Instance.worldGenSpawner.SpawnsRemain()) spawnButton.onClick += OnClickSpawn;
        var kpopupMenu2                                                           = elementPopup;
        kpopupMenu2.OnSelect
            = (Action<string, int>)Delegate.Combine(kpopupMenu2.OnSelect, new Action<string, int>(OnSelectElement));

        elementButton.onClick += elementPopup.OnClick;
        diseaseButton.onClick += diseasePopup.OnClick;
    }

    private void FilterElements(string filterValue) {
        if (string.IsNullOrEmpty(filterValue)) {
            foreach (var buttonInfo in elementPopup.GetButtons()) buttonInfo.uibutton.gameObject.SetActive(true);
            return;
        }

        filterValue = filter.ToLower();
        foreach (var buttonInfo2 in elementPopup.GetButtons())
            buttonInfo2.uibutton.gameObject.SetActive(buttonInfo2.text.ToLower().Contains(filterValue));
    }

    private void ConfigureElements() {
        if (filter != null) filter = filter.ToLower();
        var list                   = new List<ElemDisplayInfo>();
        foreach (var element in ElementLoader.elements)
            if (element.name      != "Element Not Loaded" &&
                element.substance != null                 &&
                element.substance.showInEditor            &&
                (string.IsNullOrEmpty(filter) || element.name.ToLower().Contains(filter)))
                list.Add(new ElemDisplayInfo {
                    id = element.id, displayStr = element.name + " (" + element.GetStateString() + ")"
                });

        list.Sort((a, b) => a.displayStr.CompareTo(b.displayStr));
        if (string.IsNullOrEmpty(filter)) {
            SimHashes[] array = {
                SimHashes.SlimeMold,
                SimHashes.Vacuum,
                SimHashes.Dirt,
                SimHashes.CarbonDioxide,
                SimHashes.Water,
                SimHashes.Oxygen
            };

            for (var i = 0; i < array.Length; i++) {
                var element2 = ElementLoader.FindElementByHash(array[i]);
                list.Insert(0,
                            new ElemDisplayInfo {
                                id = element2.id, displayStr = element2.name + " (" + element2.GetStateString() + ")"
                            });
            }
        }

        options_list = new List<string>();
        var list2 = new List<string>();
        foreach (var elemDisplayInfo in list) {
            list2.Add(elemDisplayInfo.displayStr);
            options_list.Add(elemDisplayInfo.id.ToString());
        }

        elementPopup.SetOptions(list2);
        for (var j = 0; j < list.Count; j++)
            if (list[j].id == element)
                elementPopup.SelectOption(list2[j], j);

        elementPopup.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0f, 1f);
    }

    private void OnClickSpawn() {
        foreach (var worldContainer in ClusterManager.Instance.WorldContainers) worldContainer.SetDiscovered(true);
        SaveGame.Instance.worldGenSpawner.SpawnEverything();
        spawnButton.GetComponent<KButton>().isInteractable = false;
    }

    private void OnClickPaint() {
        OnChangeMassPressure();
        OnChangeTemperature();
        OnDiseaseCountChange();
        OnChangeFOWReveal();
        DebugTool.Instance.Activate(DebugTool.Type.ReplaceSubstance);
    }

    private void OnClickStore() {
        OnChangeMassPressure();
        OnChangeTemperature();
        OnDiseaseCountChange();
        OnChangeFOWReveal();
        DebugTool.Instance.Activate(DebugTool.Type.StoreSubstance);
    }

    private void OnClickSample() {
        OnChangeMassPressure();
        OnChangeTemperature();
        OnDiseaseCountChange();
        OnChangeFOWReveal();
        DebugTool.Instance.Activate(DebugTool.Type.Sample);
    }

    private void OnClickFill() {
        OnChangeMassPressure();
        OnChangeTemperature();
        OnDiseaseCountChange();
        DebugTool.Instance.Activate(DebugTool.Type.FillReplaceSubstance);
    }

    private void OnSelectElement(string str, int index) {
        element = (SimHashes)Enum.Parse(typeof(SimHashes), options_list[index]);
        elementButton.GetComponentInChildren<LocText>().text = str;
    }

    private void OnSelectElement(SimHashes element) {
        this.element                                         = element;
        elementButton.GetComponentInChildren<LocText>().text = ElementLoader.FindElementByHash(element).name;
    }

    private void OnSelectDisease(string str, int index) {
        diseaseIdx = byte.MaxValue;
        for (var i = 0; i < Db.Get().Diseases.Count; i++)
            if (Db.Get().Diseases[i].Name == str)
                diseaseIdx = (byte)i;

        SelectDiseaseOption(diseaseIdx);
    }

    private void SelectDiseaseOption(int diseaseIdx) {
        if (diseaseIdx == 255) {
            diseaseButton.GetComponentInChildren<LocText>().text = "None";
            return;
        }

        var name = Db.Get().Diseases[diseaseIdx].Name;
        diseaseButton.GetComponentInChildren<LocText>().text = name;
    }

    private void OnChangeFOWReveal() {
        if (paintPreventFOWReveal.isOn) paintAllowFOWReveal.isOn = false;
        if (paintAllowFOWReveal.isOn) paintPreventFOWReveal.isOn = false;
        set_prevent_fow_reveal = paintPreventFOWReveal.isOn;
        set_allow_fow_reveal   = paintAllowFOWReveal.isOn;
    }

    public void OnChangeMassPressure() {
        float num;
        try { num = Convert.ToSingle(massPressureInput.text); } catch { num = -1f; }

        if (num <= 0f) {
            num                    = 1f;
            massPressureInput.text = "1";
        }

        mass = num;
    }

    public void OnChangeTemperature() {
        float num;
        try { num = Convert.ToSingle(temperatureInput.text); } catch { num = -1f; }

        if (num <= 0f) {
            num                   = 1f;
            temperatureInput.text = "1";
        }

        temperature = num;
    }

    public void OnDiseaseCountChange() {
        int num;
        int.TryParse(diseaseCountInput.text, out num);
        if (num < 0) {
            num                    = 0;
            diseaseCountInput.text = "0";
        }

        diseaseCount = num;
    }

    public void OnElementsFilterEdited(string new_filter) {
        filter = string.IsNullOrEmpty(filterInput.text) ? null : filterInput.text;
        FilterElements(filter);
    }

    public void SampleCell(int cell) {
        massPressureInput.text = (Grid.Pressure[cell] * 0.010000001f).ToString();
        temperatureInput.text  = Grid.Temperature[cell].ToString();
        OnSelectElement(ElementLoader.GetElementID(Grid.Element[cell].tag));
        OnChangeMassPressure();
        OnChangeTemperature();
    }

    private struct ElemDisplayInfo {
        public SimHashes id;
        public string    displayStr;
    }
}