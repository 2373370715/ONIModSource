using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02001AB6 RID: 6838
public class CharacterSelectionController : KModalScreen
{
	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x06008F40 RID: 36672 RVA: 0x000FD9B6 File Offset: 0x000FBBB6
	// (set) Token: 0x06008F41 RID: 36673 RVA: 0x000FD9BE File Offset: 0x000FBBBE
	public bool IsStarterMinion { get; set; }

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x06008F42 RID: 36674 RVA: 0x000FD9C7 File Offset: 0x000FBBC7
	public bool AllowsReplacing
	{
		get
		{
			return this.allowsReplacing;
		}
	}

	// Token: 0x06008F43 RID: 36675 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnProceed()
	{
	}

	// Token: 0x06008F44 RID: 36676 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnDeliverableAdded()
	{
	}

	// Token: 0x06008F45 RID: 36677 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnDeliverableRemoved()
	{
	}

	// Token: 0x06008F46 RID: 36678 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnLimitReached()
	{
	}

	// Token: 0x06008F47 RID: 36679 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void OnLimitUnreached()
	{
	}

	// Token: 0x06008F48 RID: 36680 RVA: 0x00375614 File Offset: 0x00373814
	protected virtual void InitializeContainers()
	{
		this.DisableProceedButton();
		if (this.containers != null && this.containers.Count > 0)
		{
			return;
		}
		this.OnReplacedEvent = null;
		this.containers = new List<ITelepadDeliverableContainer>();
		if (this.IsStarterMinion || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CarePackages).id != "Enabled")
		{
			this.numberOfDuplicantOptions = 3;
			this.numberOfCarePackageOptions = 0;
		}
		else
		{
			this.numberOfCarePackageOptions = ((UnityEngine.Random.Range(0, 101) > 70) ? 2 : 1);
			this.numberOfDuplicantOptions = 4 - this.numberOfCarePackageOptions;
		}
		for (int i = 0; i < this.numberOfDuplicantOptions; i++)
		{
			CharacterContainer characterContainer = Util.KInstantiateUI<CharacterContainer>(this.containerPrefab.gameObject, this.containerParent, false);
			characterContainer.SetController(this);
			characterContainer.SetReshufflingState(true);
			this.containers.Add(characterContainer);
		}
		for (int j = 0; j < this.numberOfCarePackageOptions; j++)
		{
			CarePackageContainer carePackageContainer = Util.KInstantiateUI<CarePackageContainer>(this.carePackageContainerPrefab.gameObject, this.containerParent, false);
			carePackageContainer.SetController(this);
			this.containers.Add(carePackageContainer);
			carePackageContainer.gameObject.transform.SetSiblingIndex(UnityEngine.Random.Range(0, carePackageContainer.transform.parent.childCount));
		}
		this.selectedDeliverables = new List<ITelepadDeliverable>();
	}

	// Token: 0x06008F49 RID: 36681 RVA: 0x0037575C File Offset: 0x0037395C
	public virtual void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
		this.Show(false);
	}

	// Token: 0x06008F4A RID: 36682 RVA: 0x003757C4 File Offset: 0x003739C4
	public void RemoveLast()
	{
		if (this.selectedDeliverables == null || this.selectedDeliverables.Count == 0)
		{
			return;
		}
		ITelepadDeliverable obj = this.selectedDeliverables[this.selectedDeliverables.Count - 1];
		if (this.OnReplacedEvent != null)
		{
			this.OnReplacedEvent(obj);
		}
	}

	// Token: 0x06008F4B RID: 36683 RVA: 0x00375814 File Offset: 0x00373A14
	public void AddDeliverable(ITelepadDeliverable deliverable)
	{
		if (this.selectedDeliverables.Contains(deliverable))
		{
			global::Debug.Log("Tried to add the same minion twice.");
			return;
		}
		if (this.selectedDeliverables.Count >= this.selectableCount)
		{
			global::Debug.LogError("Tried to add minions beyond the allowed limit");
			return;
		}
		this.selectedDeliverables.Add(deliverable);
		this.OnDeliverableAdded();
		if (this.selectedDeliverables.Count == this.selectableCount)
		{
			this.EnableProceedButton();
			if (this.OnLimitReachedEvent != null)
			{
				this.OnLimitReachedEvent();
			}
			this.OnLimitReached();
		}
	}

	// Token: 0x06008F4C RID: 36684 RVA: 0x0037589C File Offset: 0x00373A9C
	public void RemoveDeliverable(ITelepadDeliverable deliverable)
	{
		bool flag = this.selectedDeliverables.Count >= this.selectableCount;
		this.selectedDeliverables.Remove(deliverable);
		this.OnDeliverableRemoved();
		if (flag && this.selectedDeliverables.Count < this.selectableCount)
		{
			this.DisableProceedButton();
			if (this.OnLimitUnreachedEvent != null)
			{
				this.OnLimitUnreachedEvent();
			}
			this.OnLimitUnreached();
		}
	}

	// Token: 0x06008F4D RID: 36685 RVA: 0x000FD9CF File Offset: 0x000FBBCF
	public bool IsSelected(ITelepadDeliverable deliverable)
	{
		return this.selectedDeliverables.Contains(deliverable);
	}

	// Token: 0x06008F4E RID: 36686 RVA: 0x000FD9DD File Offset: 0x000FBBDD
	protected void EnableProceedButton()
	{
		this.proceedButton.isInteractable = true;
		this.proceedButton.ClearOnClick();
		this.proceedButton.onClick += delegate()
		{
			this.OnProceed();
		};
	}

	// Token: 0x06008F4F RID: 36687 RVA: 0x00375908 File Offset: 0x00373B08
	protected void DisableProceedButton()
	{
		this.proceedButton.ClearOnClick();
		this.proceedButton.isInteractable = false;
		this.proceedButton.onClick += delegate()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		};
	}

	// Token: 0x04006BFA RID: 27642
	[SerializeField]
	private CharacterContainer containerPrefab;

	// Token: 0x04006BFB RID: 27643
	[SerializeField]
	private CarePackageContainer carePackageContainerPrefab;

	// Token: 0x04006BFC RID: 27644
	[SerializeField]
	private GameObject containerParent;

	// Token: 0x04006BFD RID: 27645
	[SerializeField]
	protected KButton proceedButton;

	// Token: 0x04006BFE RID: 27646
	protected int numberOfDuplicantOptions = 3;

	// Token: 0x04006BFF RID: 27647
	protected int numberOfCarePackageOptions;

	// Token: 0x04006C00 RID: 27648
	[SerializeField]
	protected int selectableCount;

	// Token: 0x04006C01 RID: 27649
	[SerializeField]
	private bool allowsReplacing;

	// Token: 0x04006C03 RID: 27651
	protected List<ITelepadDeliverable> selectedDeliverables;

	// Token: 0x04006C04 RID: 27652
	protected List<ITelepadDeliverableContainer> containers;

	// Token: 0x04006C05 RID: 27653
	public System.Action OnLimitReachedEvent;

	// Token: 0x04006C06 RID: 27654
	public System.Action OnLimitUnreachedEvent;

	// Token: 0x04006C07 RID: 27655
	public Action<bool> OnReshuffleEvent;

	// Token: 0x04006C08 RID: 27656
	public Action<ITelepadDeliverable> OnReplacedEvent;

	// Token: 0x04006C09 RID: 27657
	public System.Action OnProceedEvent;
}
