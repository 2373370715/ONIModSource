using System;
using System.Collections.Generic;
using System.Linq;
using KMod;
using STRINGS;
using UnityEngine;

// Token: 0x02001AF0 RID: 6896
public class ModsScreen : KModalScreen
{
	// Token: 0x060090AE RID: 37038 RVA: 0x0037D1D0 File Offset: 0x0037B3D0
	protected override void OnActivate()
	{
		base.OnActivate();
		this.closeButtonTitle.onClick += this.Exit;
		this.closeButton.onClick += this.Exit;
		System.Action value = delegate()
		{
			App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140");
		};
		this.workshopButton.onClick += value;
		this.UpdateToggleAllButton();
		this.toggleAllButton.onClick += this.OnToggleAllClicked;
		Global.Instance.modManager.Sanitize(base.gameObject);
		this.mod_footprint.Clear();
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				this.mod_footprint.Add(mod.label);
				if ((mod.loaded_content & (Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation)) == (mod.available_content & (Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation)))
				{
					mod.Uncrash();
				}
			}
		}
		this.BuildDisplay();
		Manager modManager = Global.Instance.modManager;
		modManager.on_update = (Manager.OnUpdate)Delegate.Combine(modManager.on_update, new Manager.OnUpdate(this.RebuildDisplay));
	}

	// Token: 0x060090AF RID: 37039 RVA: 0x000FEAA2 File Offset: 0x000FCCA2
	protected override void OnDeactivate()
	{
		Manager modManager = Global.Instance.modManager;
		modManager.on_update = (Manager.OnUpdate)Delegate.Remove(modManager.on_update, new Manager.OnUpdate(this.RebuildDisplay));
		base.OnDeactivate();
	}

	// Token: 0x060090B0 RID: 37040 RVA: 0x0037D328 File Offset: 0x0037B528
	private void Exit()
	{
		Global.Instance.modManager.Save();
		if (!Global.Instance.modManager.MatchFootprint(this.mod_footprint, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
		{
			Global.Instance.modManager.RestartDialog(UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.TITLE, UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.MESSAGE, new System.Action(this.Deactivate), true, base.gameObject, null);
		}
		else
		{
			this.Deactivate();
		}
		Global.Instance.modManager.events.Clear();
	}

	// Token: 0x060090B1 RID: 37041 RVA: 0x000FEAD5 File Offset: 0x000FCCD5
	private void RebuildDisplay(object change_source)
	{
		if (change_source != this)
		{
			this.BuildDisplay();
		}
	}

	// Token: 0x060090B2 RID: 37042 RVA: 0x000FEAE1 File Offset: 0x000FCCE1
	private bool ShouldDisplayMod(Mod mod)
	{
		return mod.status != Mod.Status.NotInstalled && mod.status != Mod.Status.UninstallPending && !mod.HasOnlyTranslationContent();
	}

	// Token: 0x060090B3 RID: 37043 RVA: 0x0037D3B4 File Offset: 0x0037B5B4
	private void BuildDisplay()
	{
		foreach (ModsScreen.DisplayedMod displayedMod in this.displayedMods)
		{
			if (displayedMod.rect_transform != null)
			{
				UnityEngine.Object.Destroy(displayedMod.rect_transform.gameObject);
			}
		}
		this.displayedMods.Clear();
		ModsScreen.ModOrderingDragListener listener = new ModsScreen.ModOrderingDragListener(this, this.displayedMods);
		for (int num = 0; num != Global.Instance.modManager.mods.Count; num++)
		{
			Mod mod = Global.Instance.modManager.mods[num];
			if (this.ShouldDisplayMod(mod))
			{
				HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject, false);
				this.displayedMods.Add(new ModsScreen.DisplayedMod
				{
					rect_transform = hierarchyReferences.gameObject.GetComponent<RectTransform>(),
					mod_index = num
				});
				hierarchyReferences.GetComponent<DragMe>().listener = listener;
				LocText reference = hierarchyReferences.GetReference<LocText>("Title");
				string text = mod.title;
				hierarchyReferences.name = mod.title;
				if (mod.available_content == (Content)0)
				{
					switch (mod.contentCompatability)
					{
					case ModContentCompatability.NoContent:
						text += UI.FRONTEND.MODS.CONTENT_FAILURE.NO_CONTENT;
						goto IL_1AD;
					case ModContentCompatability.OldAPI:
						text += UI.FRONTEND.MODS.CONTENT_FAILURE.OLD_API;
						goto IL_1AD;
					}
					text += UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT.Replace("{Content}", ModsScreen.GetDlcName(DlcManager.GetHighestActiveDlcId()));
				}
				IL_1AD:
				reference.text = text;
				LocText reference2 = hierarchyReferences.GetReference<LocText>("Version");
				if (mod.packagedModInfo != null && mod.packagedModInfo.version != null && mod.packagedModInfo.version.Length > 0)
				{
					string text2 = mod.packagedModInfo.version;
					if (text2.StartsWith("V"))
					{
						text2 = "v" + text2.Substring(1, text2.Length - 1);
					}
					else if (!text2.StartsWith("v"))
					{
						text2 = "v" + text2;
					}
					reference2.text = text2;
					reference2.gameObject.SetActive(true);
				}
				else
				{
					reference2.gameObject.SetActive(false);
				}
				hierarchyReferences.GetReference<ToolTip>("Description").toolTip = mod.description;
				if (mod.crash_count != 0)
				{
					reference.color = Color.Lerp(Color.white, Color.red, (float)mod.crash_count / 3f);
				}
				KButton reference3 = hierarchyReferences.GetReference<KButton>("ManageButton");
				reference3.GetComponentInChildren<LocText>().text = (mod.IsLocal ? UI.FRONTEND.MODS.MANAGE_LOCAL : UI.FRONTEND.MODS.MANAGE);
				reference3.isInteractable = mod.is_managed;
				if (reference3.isInteractable)
				{
					reference3.GetComponent<ToolTip>().toolTip = mod.manage_tooltip;
					reference3.onClick += mod.on_managed;
				}
				KImage reference4 = hierarchyReferences.GetReference<KImage>("BG");
				MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
				toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
				if (mod.available_content != (Content)0)
				{
					reference4.defaultState = KImage.ColorSelector.Inactive;
					reference4.ColorState = KImage.ColorSelector.Inactive;
					MultiToggle toggle2 = toggle;
					toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
					{
						this.OnToggleClicked(toggle, mod.label);
					}));
					toggle.GetComponent<ToolTip>().OnToolTip = (() => mod.IsEnabledForActiveDlc() ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED);
				}
				else
				{
					reference4.defaultState = KImage.ColorSelector.Disabled;
					reference4.ColorState = KImage.ColorSelector.Disabled;
				}
				hierarchyReferences.gameObject.SetActive(true);
			}
		}
		foreach (ModsScreen.DisplayedMod displayedMod2 in this.displayedMods)
		{
			displayedMod2.rect_transform.gameObject.SetActive(true);
		}
		int count = this.displayedMods.Count;
	}

	// Token: 0x060090B4 RID: 37044 RVA: 0x000FEAFF File Offset: 0x000FCCFF
	private static string GetDlcName(string dlcId)
	{
		if (!(dlcId == "EXPANSION1_ID"))
		{
			if ((dlcId == null || dlcId.Length != 0) && dlcId != null)
			{
			}
			return UI.VANILLA.NAME_ITAL;
		}
		return UI.DLC1.NAME_ITAL;
	}

	// Token: 0x060090B5 RID: 37045 RVA: 0x0037D868 File Offset: 0x0037BA68
	private void OnToggleClicked(MultiToggle toggle, Label mod)
	{
		Manager modManager = Global.Instance.modManager;
		bool flag = modManager.IsModEnabled(mod);
		flag = !flag;
		toggle.ChangeState(flag ? 1 : 0);
		modManager.EnableMod(mod, flag, this);
		this.UpdateToggleAllButton();
	}

	// Token: 0x060090B6 RID: 37046 RVA: 0x000FEB33 File Offset: 0x000FCD33
	private bool AreAnyModsDisabled()
	{
		return Global.Instance.modManager.mods.Any((Mod mod) => !mod.IsEmpty() && !mod.IsEnabledForActiveDlc() && this.ShouldDisplayMod(mod));
	}

	// Token: 0x060090B7 RID: 37047 RVA: 0x000FEB55 File Offset: 0x000FCD55
	private void UpdateToggleAllButton()
	{
		this.toggleAllButton.GetComponentInChildren<LocText>().text = (this.AreAnyModsDisabled() ? UI.FRONTEND.MODS.ENABLE_ALL : UI.FRONTEND.MODS.DISABLE_ALL);
	}

	// Token: 0x060090B8 RID: 37048 RVA: 0x0037D8A8 File Offset: 0x0037BAA8
	private void OnToggleAllClicked()
	{
		bool enabled = this.AreAnyModsDisabled();
		Manager modManager = Global.Instance.modManager;
		foreach (Mod mod in modManager.mods)
		{
			if (this.ShouldDisplayMod(mod))
			{
				modManager.EnableMod(mod.label, enabled, this);
			}
		}
		this.BuildDisplay();
		this.UpdateToggleAllButton();
	}

	// Token: 0x04006D55 RID: 27989
	[SerializeField]
	private KButton closeButtonTitle;

	// Token: 0x04006D56 RID: 27990
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04006D57 RID: 27991
	[SerializeField]
	private KButton toggleAllButton;

	// Token: 0x04006D58 RID: 27992
	[SerializeField]
	private KButton workshopButton;

	// Token: 0x04006D59 RID: 27993
	[SerializeField]
	private GameObject entryPrefab;

	// Token: 0x04006D5A RID: 27994
	[SerializeField]
	private Transform entryParent;

	// Token: 0x04006D5B RID: 27995
	private List<ModsScreen.DisplayedMod> displayedMods = new List<ModsScreen.DisplayedMod>();

	// Token: 0x04006D5C RID: 27996
	private List<Label> mod_footprint = new List<Label>();

	// Token: 0x02001AF1 RID: 6897
	private struct DisplayedMod
	{
		// Token: 0x04006D5D RID: 27997
		public RectTransform rect_transform;

		// Token: 0x04006D5E RID: 27998
		public int mod_index;
	}

	// Token: 0x02001AF2 RID: 6898
	private class ModOrderingDragListener : DragMe.IDragListener
	{
		// Token: 0x060090BB RID: 37051 RVA: 0x000FEBB9 File Offset: 0x000FCDB9
		public ModOrderingDragListener(ModsScreen screen, List<ModsScreen.DisplayedMod> mods)
		{
			this.screen = screen;
			this.mods = mods;
		}

		// Token: 0x060090BC RID: 37052 RVA: 0x000FEBD6 File Offset: 0x000FCDD6
		public void OnBeginDrag(Vector2 pos)
		{
			this.startDragIdx = this.GetDragIdx(pos, false);
		}

		// Token: 0x060090BD RID: 37053 RVA: 0x0037D92C File Offset: 0x0037BB2C
		public void OnEndDrag(Vector2 pos)
		{
			if (this.startDragIdx < 0)
			{
				return;
			}
			int dragIdx = this.GetDragIdx(pos, true);
			if (dragIdx != this.startDragIdx)
			{
				int mod_index = this.mods[this.startDragIdx].mod_index;
				int target_index = (0 <= dragIdx && dragIdx < this.mods.Count) ? this.mods[dragIdx].mod_index : -1;
				Global.Instance.modManager.Reinsert(mod_index, target_index, dragIdx >= this.mods.Count, this);
				this.screen.BuildDisplay();
			}
		}

		// Token: 0x060090BE RID: 37054 RVA: 0x0037D9C4 File Offset: 0x0037BBC4
		private int GetDragIdx(Vector2 pos, bool halfPosition)
		{
			int result = -1;
			for (int i = 0; i < this.mods.Count; i++)
			{
				Vector2 vector;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.mods[i].rect_transform, pos, null, out vector);
				if (!halfPosition)
				{
					vector += this.mods[i].rect_transform.rect.min;
				}
				if (vector.y >= 0f)
				{
					break;
				}
				result = i;
			}
			return result;
		}

		// Token: 0x04006D5F RID: 27999
		private List<ModsScreen.DisplayedMod> mods;

		// Token: 0x04006D60 RID: 28000
		private ModsScreen screen;

		// Token: 0x04006D61 RID: 28001
		private int startDragIdx = -1;
	}
}
