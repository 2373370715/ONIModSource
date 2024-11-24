using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200202E RID: 8238
[AddComponentMenu("KMonoBehaviour/scripts/ToolParameterMenu")]
public class ToolParameterMenu : KMonoBehaviour
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x0600AF64 RID: 44900 RVA: 0x0042054C File Offset: 0x0041E74C
	// (remove) Token: 0x0600AF65 RID: 44901 RVA: 0x00420584 File Offset: 0x0041E784
	public event System.Action onParametersChanged;

	// Token: 0x0600AF66 RID: 44902 RVA: 0x00111FF5 File Offset: 0x001101F5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ClearMenu();
	}

	// Token: 0x0600AF67 RID: 44903 RVA: 0x004205BC File Offset: 0x0041E7BC
	public void PopulateMenu(Dictionary<string, ToolParameterMenu.ToggleState> parameters)
	{
		this.ClearMenu();
		this.currentParameters = parameters;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in parameters)
		{
			GameObject gameObject = Util.KInstantiateUI(this.widgetPrefab, this.widgetContainer, true);
			gameObject.GetComponentInChildren<LocText>().text = Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key + ".NAME");
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (componentInChildren != null)
			{
				componentInChildren.SetSimpleTooltip(Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key + ".TOOLTIP"));
			}
			this.widgets.Add(keyValuePair.Key, gameObject);
			MultiToggle toggle = gameObject.GetComponentInChildren<MultiToggle>();
			ToolParameterMenu.ToggleState value = keyValuePair.Value;
			if (value == ToolParameterMenu.ToggleState.Disabled)
			{
				toggle.ChangeState(2);
			}
			else if (value == ToolParameterMenu.ToggleState.On)
			{
				toggle.ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
			}
			else
			{
				toggle.ChangeState(0);
			}
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.widgets)
				{
					if (keyValuePair2.Value == toggle.transform.parent.gameObject)
					{
						if (this.currentParameters[keyValuePair2.Key] == ToolParameterMenu.ToggleState.Disabled)
						{
							break;
						}
						this.ChangeToSetting(keyValuePair2.Key);
						this.OnChange();
						break;
					}
				}
			}));
		}
		this.content.SetActive(true);
	}

	// Token: 0x0600AF68 RID: 44904 RVA: 0x00420740 File Offset: 0x0041E940
	public void ClearMenu()
	{
		this.content.SetActive(false);
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.widgets.Clear();
	}

	// Token: 0x0600AF69 RID: 44905 RVA: 0x004207B0 File Offset: 0x0041E9B0
	private void ChangeToSetting(string key)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			if (this.currentParameters[keyValuePair.Key] != ToolParameterMenu.ToggleState.Disabled)
			{
				this.currentParameters[keyValuePair.Key] = ToolParameterMenu.ToggleState.Off;
			}
		}
		this.currentParameters[key] = ToolParameterMenu.ToggleState.On;
	}

	// Token: 0x0600AF6A RID: 44906 RVA: 0x00420834 File Offset: 0x0041EA34
	private void OnChange()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			switch (this.currentParameters[keyValuePair.Key])
			{
			case ToolParameterMenu.ToggleState.On:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
				break;
			case ToolParameterMenu.ToggleState.Off:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(0);
				break;
			case ToolParameterMenu.ToggleState.Disabled:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(2);
				break;
			}
		}
		if (this.onParametersChanged != null)
		{
			this.onParametersChanged();
		}
	}

	// Token: 0x0600AF6B RID: 44907 RVA: 0x00112003 File Offset: 0x00110203
	public string GetLastEnabledFilter()
	{
		return this.lastEnabledFilter;
	}

	// Token: 0x04008A1C RID: 35356
	public GameObject content;

	// Token: 0x04008A1D RID: 35357
	public GameObject widgetContainer;

	// Token: 0x04008A1E RID: 35358
	public GameObject widgetPrefab;

	// Token: 0x04008A20 RID: 35360
	private Dictionary<string, GameObject> widgets = new Dictionary<string, GameObject>();

	// Token: 0x04008A21 RID: 35361
	private Dictionary<string, ToolParameterMenu.ToggleState> currentParameters;

	// Token: 0x04008A22 RID: 35362
	private string lastEnabledFilter;

	// Token: 0x0200202F RID: 8239
	public class FILTERLAYERS
	{
		// Token: 0x04008A23 RID: 35363
		public static string BUILDINGS = "BUILDINGS";

		// Token: 0x04008A24 RID: 35364
		public static string TILES = "TILES";

		// Token: 0x04008A25 RID: 35365
		public static string WIRES = "WIRES";

		// Token: 0x04008A26 RID: 35366
		public static string LIQUIDCONDUIT = "LIQUIDPIPES";

		// Token: 0x04008A27 RID: 35367
		public static string GASCONDUIT = "GASPIPES";

		// Token: 0x04008A28 RID: 35368
		public static string SOLIDCONDUIT = "SOLIDCONDUITS";

		// Token: 0x04008A29 RID: 35369
		public static string CLEANANDCLEAR = "CLEANANDCLEAR";

		// Token: 0x04008A2A RID: 35370
		public static string DIGPLACER = "DIGPLACER";

		// Token: 0x04008A2B RID: 35371
		public static string LOGIC = "LOGIC";

		// Token: 0x04008A2C RID: 35372
		public static string BACKWALL = "BACKWALL";

		// Token: 0x04008A2D RID: 35373
		public static string CONSTRUCTION = "CONSTRUCTION";

		// Token: 0x04008A2E RID: 35374
		public static string DIG = "DIG";

		// Token: 0x04008A2F RID: 35375
		public static string CLEAN = "CLEAN";

		// Token: 0x04008A30 RID: 35376
		public static string OPERATE = "OPERATE";

		// Token: 0x04008A31 RID: 35377
		public static string METAL = "METAL";

		// Token: 0x04008A32 RID: 35378
		public static string BUILDABLE = "BUILDABLE";

		// Token: 0x04008A33 RID: 35379
		public static string FILTER = "FILTER";

		// Token: 0x04008A34 RID: 35380
		public static string LIQUIFIABLE = "LIQUIFIABLE";

		// Token: 0x04008A35 RID: 35381
		public static string LIQUID = "LIQUID";

		// Token: 0x04008A36 RID: 35382
		public static string CONSUMABLEORE = "CONSUMABLEORE";

		// Token: 0x04008A37 RID: 35383
		public static string ORGANICS = "ORGANICS";

		// Token: 0x04008A38 RID: 35384
		public static string FARMABLE = "FARMABLE";

		// Token: 0x04008A39 RID: 35385
		public static string GAS = "GAS";

		// Token: 0x04008A3A RID: 35386
		public static string MISC = "MISC";

		// Token: 0x04008A3B RID: 35387
		public static string HEATFLOW = "HEATFLOW";

		// Token: 0x04008A3C RID: 35388
		public static string ABSOLUTETEMPERATURE = "ABSOLUTETEMPERATURE";

		// Token: 0x04008A3D RID: 35389
		public static string RELATIVETEMPERATURE = "RELATIVETEMPERATURE";

		// Token: 0x04008A3E RID: 35390
		public static string ADAPTIVETEMPERATURE = "ADAPTIVETEMPERATURE";

		// Token: 0x04008A3F RID: 35391
		public static string STATECHANGE = "STATECHANGE";

		// Token: 0x04008A40 RID: 35392
		public static string ALL = "ALL";
	}

	// Token: 0x02002030 RID: 8240
	public enum ToggleState
	{
		// Token: 0x04008A42 RID: 35394
		On,
		// Token: 0x04008A43 RID: 35395
		Off,
		// Token: 0x04008A44 RID: 35396
		Disabled
	}
}
