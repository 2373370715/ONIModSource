using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D9C RID: 7580
[AddComponentMenu("KMonoBehaviour/scripts/CrewPortrait")]
[Serializable]
public class CrewPortrait : KMonoBehaviour
{
	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x06009E70 RID: 40560 RVA: 0x001072C4 File Offset: 0x001054C4
	// (set) Token: 0x06009E71 RID: 40561 RVA: 0x001072CC File Offset: 0x001054CC
	public IAssignableIdentity identityObject { get; private set; }

	// Token: 0x06009E72 RID: 40562 RVA: 0x001072D5 File Offset: 0x001054D5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.startTransparent)
		{
			base.StartCoroutine(this.AlphaIn());
		}
		this.requiresRefresh = true;
	}

	// Token: 0x06009E73 RID: 40563 RVA: 0x001072F9 File Offset: 0x001054F9
	private IEnumerator AlphaIn()
	{
		this.SetAlpha(0f);
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime * 4f)
		{
			this.SetAlpha(i);
			yield return 0;
		}
		this.SetAlpha(1f);
		yield break;
	}

	// Token: 0x06009E74 RID: 40564 RVA: 0x00107308 File Offset: 0x00105508
	private void OnRoleChanged(object data)
	{
		if (this.controller == null)
		{
			return;
		}
		CrewPortrait.RefreshHat(this.identityObject, this.controller);
	}

	// Token: 0x06009E75 RID: 40565 RVA: 0x003CBAC8 File Offset: 0x003C9CC8
	private void RegisterEvents()
	{
		if (this.areEventsRegistered)
		{
			return;
		}
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Subscribe(540773776, new Action<object>(this.OnRoleChanged));
		this.areEventsRegistered = true;
	}

	// Token: 0x06009E76 RID: 40566 RVA: 0x003CBB14 File Offset: 0x003C9D14
	private void UnregisterEvents()
	{
		if (!this.areEventsRegistered)
		{
			return;
		}
		this.areEventsRegistered = false;
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Unsubscribe(540773776, new Action<object>(this.OnRoleChanged));
	}

	// Token: 0x06009E77 RID: 40567 RVA: 0x0010732A File Offset: 0x0010552A
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RegisterEvents();
		this.ForceRefresh();
	}

	// Token: 0x06009E78 RID: 40568 RVA: 0x0010733E File Offset: 0x0010553E
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.UnregisterEvents();
	}

	// Token: 0x06009E79 RID: 40569 RVA: 0x0010734C File Offset: 0x0010554C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.UnregisterEvents();
	}

	// Token: 0x06009E7A RID: 40570 RVA: 0x003CBB60 File Offset: 0x003C9D60
	public void SetIdentityObject(IAssignableIdentity identity, bool jobEnabled = true)
	{
		this.UnregisterEvents();
		this.identityObject = identity;
		this.RegisterEvents();
		this.targetImage.enabled = true;
		if (this.identityObject != null)
		{
			this.targetImage.enabled = false;
		}
		if (this.useLabels && (identity is MinionIdentity || identity is MinionAssignablesProxy))
		{
			this.SetDuplicantJobTitleActive(jobEnabled);
		}
		this.requiresRefresh = true;
	}

	// Token: 0x06009E7B RID: 40571 RVA: 0x003CBBC8 File Offset: 0x003C9DC8
	public void SetSubTitle(string newTitle)
	{
		if (this.subTitle != null)
		{
			if (string.IsNullOrEmpty(newTitle))
			{
				this.subTitle.gameObject.SetActive(false);
				return;
			}
			this.subTitle.gameObject.SetActive(true);
			this.subTitle.SetText(newTitle);
		}
	}

	// Token: 0x06009E7C RID: 40572 RVA: 0x0010735A File Offset: 0x0010555A
	public void SetDuplicantJobTitleActive(bool state)
	{
		if (this.duplicantJob != null && this.duplicantJob.gameObject.activeInHierarchy != state)
		{
			this.duplicantJob.gameObject.SetActive(state);
		}
	}

	// Token: 0x06009E7D RID: 40573 RVA: 0x0010738E File Offset: 0x0010558E
	public void ForceRefresh()
	{
		this.requiresRefresh = true;
	}

	// Token: 0x06009E7E RID: 40574 RVA: 0x00107397 File Offset: 0x00105597
	public void Update()
	{
		if (this.requiresRefresh && (this.controller == null || this.controller.enabled))
		{
			this.requiresRefresh = false;
			this.Rebuild();
		}
	}

	// Token: 0x06009E7F RID: 40575 RVA: 0x003CBC1C File Offset: 0x003C9E1C
	private void Rebuild()
	{
		if (this.controller == null)
		{
			this.controller = base.GetComponentInChildren<KBatchedAnimController>();
			if (this.controller == null)
			{
				if (this.targetImage != null)
				{
					this.targetImage.enabled = true;
				}
				global::Debug.LogWarning("Controller for [" + base.name + "] null");
				return;
			}
		}
		CrewPortrait.SetPortraitData(this.identityObject, this.controller, this.useDefaultExpression);
		if (this.useLabels && this.duplicantName != null)
		{
			this.duplicantName.SetText((!this.identityObject.IsNullOrDestroyed()) ? this.identityObject.GetProperName() : "");
			if (this.identityObject is MinionIdentity && this.duplicantJob != null)
			{
				this.duplicantJob.SetText((this.identityObject != null) ? (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle() : "");
				this.duplicantJob.GetComponent<ToolTip>().toolTip = (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle();
			}
		}
	}

	// Token: 0x06009E80 RID: 40576 RVA: 0x003CBD54 File Offset: 0x003C9F54
	private static void RefreshHat(IAssignableIdentity identityObject, KBatchedAnimController controller)
	{
		string hat_id = "";
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity != null)
		{
			hat_id = minionIdentity.GetComponent<MinionResume>().CurrentHat;
		}
		else if (identityObject as StoredMinionIdentity != null)
		{
			hat_id = (identityObject as StoredMinionIdentity).currentHat;
		}
		MinionResume.ApplyHat(hat_id, controller);
	}

	// Token: 0x06009E81 RID: 40577 RVA: 0x003CBDA8 File Offset: 0x003C9FA8
	public static void SetPortraitData(IAssignableIdentity identityObject, KBatchedAnimController controller, bool useDefaultExpression = true)
	{
		if (identityObject == null)
		{
			controller.gameObject.SetActive(false);
			return;
		}
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity == null)
		{
			MinionAssignablesProxy minionAssignablesProxy = identityObject as MinionAssignablesProxy;
			if (minionAssignablesProxy != null && minionAssignablesProxy.target != null)
			{
				minionIdentity = (minionAssignablesProxy.target as MinionIdentity);
			}
		}
		controller.gameObject.SetActive(true);
		controller.Play("ui_idle", KAnim.PlayMode.Once, 1f, 0f);
		SymbolOverrideController component = controller.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		if (minionIdentity != null)
		{
			HashSet<KAnimHashedString> hashSet = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet2 = new HashSet<KAnimHashedString>();
			Accessorizer component2 = minionIdentity.GetComponent<Accessorizer>();
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory = component2.GetAccessory(accessorySlot);
				if (accessory != null)
				{
					component.AddSymbolOverride(accessorySlot.targetSymbolId, accessory.symbol, 0);
					hashSet.Add(accessorySlot.targetSymbolId);
				}
				else
				{
					hashSet2.Add(accessorySlot.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet, true);
			controller.BatchSetSymbolsVisiblity(hashSet2, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component2.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(minionIdentity, controller);
		}
		else
		{
			HashSet<KAnimHashedString> hashSet3 = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet4 = new HashSet<KAnimHashedString>();
			StoredMinionIdentity storedMinionIdentity = identityObject as StoredMinionIdentity;
			if (storedMinionIdentity == null)
			{
				MinionAssignablesProxy minionAssignablesProxy2 = identityObject as MinionAssignablesProxy;
				if (minionAssignablesProxy2 != null && minionAssignablesProxy2.target != null)
				{
					storedMinionIdentity = (minionAssignablesProxy2.target as StoredMinionIdentity);
				}
			}
			if (!(storedMinionIdentity != null))
			{
				controller.gameObject.SetActive(false);
				return;
			}
			foreach (AccessorySlot accessorySlot2 in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory2 = storedMinionIdentity.GetAccessory(accessorySlot2);
				if (accessory2 != null)
				{
					component.AddSymbolOverride(accessorySlot2.targetSymbolId, accessory2.symbol, 0);
					hashSet3.Add(accessorySlot2.targetSymbolId);
				}
				else
				{
					hashSet4.Add(accessorySlot2.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet3, true);
			controller.BatchSetSymbolsVisiblity(hashSet4, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(storedMinionIdentity, controller);
		}
		float animScale = 0.25f;
		controller.animScale = animScale;
		string s = "ui_idle";
		controller.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		controller.SetSymbolVisiblity("snapTo_neck", false);
		controller.SetSymbolVisiblity("snapTo_goggles", false);
	}

	// Token: 0x06009E82 RID: 40578 RVA: 0x003CC140 File Offset: 0x003CA340
	public void SetAlpha(float value)
	{
		if (this.controller == null)
		{
			return;
		}
		if ((float)this.controller.TintColour.a != value)
		{
			this.controller.TintColour = new Color(1f, 1f, 1f, value);
		}
	}

	// Token: 0x04007C3A RID: 31802
	public Image targetImage;

	// Token: 0x04007C3B RID: 31803
	public bool startTransparent;

	// Token: 0x04007C3C RID: 31804
	public bool useLabels = true;

	// Token: 0x04007C3D RID: 31805
	[SerializeField]
	public KBatchedAnimController controller;

	// Token: 0x04007C3E RID: 31806
	public float animScaleBase = 0.2f;

	// Token: 0x04007C3F RID: 31807
	public LocText duplicantName;

	// Token: 0x04007C40 RID: 31808
	public LocText duplicantJob;

	// Token: 0x04007C41 RID: 31809
	public LocText subTitle;

	// Token: 0x04007C42 RID: 31810
	public bool useDefaultExpression = true;

	// Token: 0x04007C43 RID: 31811
	private bool requiresRefresh;

	// Token: 0x04007C44 RID: 31812
	private bool areEventsRegistered;
}
