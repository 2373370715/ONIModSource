using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F99 RID: 8089
public class OwnablesSidescreen : SideScreenContent
{
	// Token: 0x0600AAD1 RID: 43729 RVA: 0x004074C0 File Offset: 0x004056C0
	private void DefineCategories()
	{
		if (this.categories == null)
		{
			OwnablesSidescreen.Category[] array = new OwnablesSidescreen.Category[2];
			array[0] = new OwnablesSidescreen.Category((IAssignableIdentity assignableIdentity) => (assignableIdentity as MinionIdentity).GetEquipment(), new OwnablesSidescreenCategoryRow.Data(UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.SUITS, new OwnablesSidescreenCategoryRow.AssignableSlotData[]
			{
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Suit, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Outfit, new Func<IAssignableIdentity, bool>(this.Always))
			}));
			array[1] = new OwnablesSidescreen.Category((IAssignableIdentity assignableIdentity) => assignableIdentity.GetSoleOwner(), new OwnablesSidescreenCategoryRow.Data(UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.AMENITIES, new OwnablesSidescreenCategoryRow.AssignableSlotData[]
			{
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Bed, this.HasAmount("Stamina")),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Toilet, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.MessStation, this.HasAmount("Calories"))
			}));
			this.categories = array;
		}
	}

	// Token: 0x0600AAD2 RID: 43730 RVA: 0x000A65EC File Offset: 0x000A47EC
	private bool Always(IAssignableIdentity identity)
	{
		return true;
	}

	// Token: 0x0600AAD3 RID: 43731 RVA: 0x0010EF53 File Offset: 0x0010D153
	private Func<IAssignableIdentity, bool> HasAmount(string amountID)
	{
		return delegate(IAssignableIdentity identity)
		{
			if (identity == null)
			{
				return false;
			}
			GameObject targetGameObject = identity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			return Db.Get().Amounts.Get(amountID).Lookup(targetGameObject) != null;
		};
	}

	// Token: 0x0600AAD4 RID: 43732 RVA: 0x0010197B File Offset: 0x000FFB7B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600AAD5 RID: 43733 RVA: 0x0010EF6C File Offset: 0x0010D16C
	private void ActivateSecondSidescreen(AssignableSlotInstance slot)
	{
		((OwnablesSecondSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.selectedSlotScreenPrefab, slot.slot.Name)).SetSlot(slot);
		if (slot != null && this.OnSlotInstanceSelected != null)
		{
			this.OnSlotInstanceSelected(slot);
		}
	}

	// Token: 0x0600AAD6 RID: 43734 RVA: 0x0010EFAB File Offset: 0x0010D1AB
	private void DeactivateSecondScreen()
	{
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x0600AAD7 RID: 43735 RVA: 0x00407628 File Offset: 0x00405828
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.UnsubscribeFromLastTarget();
		this.lastSelectedSlot = null;
		this.DefineCategories();
		this.CreateCategoryRows();
		this.DeactivateSecondScreen();
		this.RefreshSelectedStatusOnRows();
		IAssignableIdentity component = target.GetComponent<IAssignableIdentity>();
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			Assignables owner = this.categories[i].getAssignablesFn(component);
			this.categoryRows[i].SetOwner(owner);
		}
		this.titleSection.SetActive(target.GetComponent<MinionIdentity>().model == BionicMinionConfig.MODEL);
		MinionIdentity minionIdentity = component as MinionIdentity;
		if (minionIdentity != null)
		{
			this.lastTarget = minionIdentity;
			this.minionDestroyedCallbackIDX = minionIdentity.gameObject.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
		}
	}

	// Token: 0x0600AAD8 RID: 43736 RVA: 0x0010EFB7 File Offset: 0x0010D1B7
	private void OnTargetDestroyed(object o)
	{
		this.ClearTarget();
	}

	// Token: 0x0600AAD9 RID: 43737 RVA: 0x004076FC File Offset: 0x004058FC
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.lastSelectedSlot = null;
		this.RefreshSelectedStatusOnRows();
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			this.categoryRows[i].SetOwner(null);
		}
		this.DeactivateSecondScreen();
		this.UnsubscribeFromLastTarget();
	}

	// Token: 0x0600AADA RID: 43738 RVA: 0x0040774C File Offset: 0x0040594C
	private void CreateCategoryRows()
	{
		if (this.categoryRows == null)
		{
			this.originalCategoryRow.gameObject.SetActive(false);
			this.categoryRows = new OwnablesSidescreenCategoryRow[this.categories.Length];
			for (int i = 0; i < this.categories.Length; i++)
			{
				OwnablesSidescreenCategoryRow.Data data = this.categories[i].data;
				OwnablesSidescreenCategoryRow component = Util.KInstantiateUI(this.originalCategoryRow.gameObject, this.originalCategoryRow.transform.parent.gameObject, false).GetComponent<OwnablesSidescreenCategoryRow>();
				OwnablesSidescreenCategoryRow ownablesSidescreenCategoryRow = component;
				ownablesSidescreenCategoryRow.OnSlotRowClicked = (Action<OwnablesSidescreenItemRow>)Delegate.Combine(ownablesSidescreenCategoryRow.OnSlotRowClicked, new Action<OwnablesSidescreenItemRow>(this.OnSlotRowClicked));
				component.gameObject.SetActive(true);
				component.SetCategoryData(data);
				this.categoryRows[i] = component;
			}
			this.RefreshSelectedStatusOnRows();
		}
	}

	// Token: 0x0600AADB RID: 43739 RVA: 0x0010EFBF File Offset: 0x0010D1BF
	private void OnSlotRowClicked(OwnablesSidescreenItemRow slotRow)
	{
		if (slotRow.IsLocked || slotRow.SlotInstance == this.lastSelectedSlot)
		{
			this.SetSelectedSlot(null);
			return;
		}
		this.SetSelectedSlot(slotRow.SlotInstance);
	}

	// Token: 0x0600AADC RID: 43740 RVA: 0x00407824 File Offset: 0x00405A24
	public void RefreshSelectedStatusOnRows()
	{
		if (this.categoryRows == null)
		{
			return;
		}
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			this.categoryRows[i].SetSelectedRow_VisualsOnly(this.lastSelectedSlot);
		}
	}

	// Token: 0x0600AADD RID: 43741 RVA: 0x0010EFEB File Offset: 0x0010D1EB
	public void SetSelectedSlot(AssignableSlotInstance slot)
	{
		this.lastSelectedSlot = slot;
		if (slot != null)
		{
			this.ActivateSecondSidescreen(slot);
		}
		else
		{
			this.DeactivateSecondScreen();
		}
		this.RefreshSelectedStatusOnRows();
	}

	// Token: 0x0600AADE RID: 43742 RVA: 0x00407860 File Offset: 0x00405A60
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.categoryRows != null)
		{
			for (int i = 0; i < this.categoryRows.Length; i++)
			{
				if (this.categoryRows[i] != null)
				{
					this.categoryRows[i].SetOwner(null);
				}
			}
		}
		this.UnsubscribeFromLastTarget();
	}

	// Token: 0x0600AADF RID: 43743 RVA: 0x0010F00C File Offset: 0x0010D20C
	private void UnsubscribeFromLastTarget()
	{
		if (this.lastTarget != null && this.minionDestroyedCallbackIDX != -1)
		{
			this.lastTarget.Unsubscribe(this.minionDestroyedCallbackIDX);
		}
		this.minionDestroyedCallbackIDX = -1;
		this.lastTarget = null;
	}

	// Token: 0x0600AAE0 RID: 43744 RVA: 0x0010F044 File Offset: 0x0010D244
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IAssignableIdentity>() != null;
	}

	// Token: 0x0600AAE1 RID: 43745 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnValidate()
	{
	}

	// Token: 0x0600AAE2 RID: 43746 RVA: 0x0010F04F File Offset: 0x0010D24F
	private void SetScrollBarVisibility(bool isVisible)
	{
		this.scrollbarSection.gameObject.SetActive(isVisible);
		this.mainLayoutGroup.padding.right = (isVisible ? 20 : 0);
		this.scrollRect.enabled = isVisible;
	}

	// Token: 0x0400863F RID: 34367
	public OwnablesSecondSideScreen selectedSlotScreenPrefab;

	// Token: 0x04008640 RID: 34368
	public OwnablesSidescreenCategoryRow originalCategoryRow;

	// Token: 0x04008641 RID: 34369
	[Header("Editor Settings")]
	public bool usingSlider = true;

	// Token: 0x04008642 RID: 34370
	public GameObject titleSection;

	// Token: 0x04008643 RID: 34371
	public GameObject scrollbarSection;

	// Token: 0x04008644 RID: 34372
	public VerticalLayoutGroup mainLayoutGroup;

	// Token: 0x04008645 RID: 34373
	public KScrollRect scrollRect;

	// Token: 0x04008646 RID: 34374
	private OwnablesSidescreenCategoryRow[] categoryRows;

	// Token: 0x04008647 RID: 34375
	private AssignableSlotInstance lastSelectedSlot;

	// Token: 0x04008648 RID: 34376
	private OwnablesSidescreen.Category[] categories;

	// Token: 0x04008649 RID: 34377
	public Action<AssignableSlotInstance> OnSlotInstanceSelected;

	// Token: 0x0400864A RID: 34378
	private MinionIdentity lastTarget;

	// Token: 0x0400864B RID: 34379
	private int minionDestroyedCallbackIDX = -1;

	// Token: 0x02001F9A RID: 8090
	public struct Category
	{
		// Token: 0x0600AAE4 RID: 43748 RVA: 0x0010F09C File Offset: 0x0010D29C
		public Category(Func<IAssignableIdentity, Assignables> getAssignablesFn, OwnablesSidescreenCategoryRow.Data categoryData)
		{
			this.getAssignablesFn = getAssignablesFn;
			this.data = categoryData;
		}

		// Token: 0x0400864C RID: 34380
		public Func<IAssignableIdentity, Assignables> getAssignablesFn;

		// Token: 0x0400864D RID: 34381
		public OwnablesSidescreenCategoryRow.Data data;
	}
}
