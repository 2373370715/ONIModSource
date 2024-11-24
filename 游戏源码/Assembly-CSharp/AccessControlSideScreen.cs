using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001F1C RID: 7964
public class AccessControlSideScreen : SideScreenContent
{
	// Token: 0x0600A7E8 RID: 42984 RVA: 0x0010CF30 File Offset: 0x0010B130
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return string.Format(base.GetTitle(), this.target.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x0600A7E9 RID: 42985 RVA: 0x003FAECC File Offset: 0x003F90CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.sortByNameToggle.onValueChanged.AddListener(delegate(bool reverse_sort)
		{
			this.SortEntries(reverse_sort, new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName));
		});
		this.sortByRoleToggle.onValueChanged.AddListener(delegate(bool reverse_sort)
		{
			this.SortEntries(reverse_sort, new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole));
		});
		this.sortByPermissionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SortByPermission));
	}

	// Token: 0x0600A7EA RID: 42986 RVA: 0x0010CF5D File Offset: 0x0010B15D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AccessControl>() != null && target.GetComponent<AccessControl>().controlEnabled;
	}

	// Token: 0x0600A7EB RID: 42987 RVA: 0x003FAF34 File Offset: 0x003F9134
	public override void SetTarget(GameObject target)
	{
		if (this.target != null)
		{
			this.ClearTarget();
		}
		this.target = target.GetComponent<AccessControl>();
		this.doorTarget = target.GetComponent<Door>();
		if (this.target == null)
		{
			return;
		}
		target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
		target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		if (this.rowPool == null)
		{
			this.rowPool = new UIPool<AccessControlSideScreenRow>(this.rowPrefab);
		}
		base.gameObject.SetActive(true);
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.Refresh(this.identityList, true);
	}

	// Token: 0x0600A7EC RID: 42988 RVA: 0x003FAFF4 File Offset: 0x003F91F4
	public override void ClearTarget()
	{
		base.ClearTarget();
		if (this.target != null)
		{
			this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
			this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		}
	}

	// Token: 0x0600A7ED RID: 42989 RVA: 0x003FB050 File Offset: 0x003F9250
	private void Refresh(List<MinionAssignablesProxy> identities, bool rebuild)
	{
		Rotatable component = this.target.GetComponent<Rotatable>();
		bool rotated = component != null && component.IsRotated;
		this.defaultsRow.SetRotated(rotated);
		this.defaultsRow.SetContent(this.target.DefaultPermission, new Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnDefaultPermissionChanged));
		if (rebuild)
		{
			this.ClearContent();
		}
		foreach (MinionAssignablesProxy minionAssignablesProxy in identities)
		{
			AccessControlSideScreenRow accessControlSideScreenRow;
			if (rebuild)
			{
				accessControlSideScreenRow = this.rowPool.GetFreeElement(this.rowGroup, true);
				this.identityRowMap.Add(minionAssignablesProxy, accessControlSideScreenRow);
			}
			else
			{
				accessControlSideScreenRow = this.identityRowMap[minionAssignablesProxy];
			}
			AccessControl.Permission setPermission = this.target.GetSetPermission(minionAssignablesProxy);
			bool isDefault = this.target.IsDefaultPermission(minionAssignablesProxy);
			accessControlSideScreenRow.SetRotated(rotated);
			accessControlSideScreenRow.SetMinionContent(minionAssignablesProxy, setPermission, isDefault, new Action<MinionAssignablesProxy, AccessControl.Permission>(this.OnPermissionChanged), new Action<MinionAssignablesProxy, bool>(this.OnPermissionDefault));
		}
		this.RefreshOnline();
		this.ContentContainer.SetActive(this.target.controlEnabled);
	}

	// Token: 0x0600A7EE RID: 42990 RVA: 0x003FB18C File Offset: 0x003F938C
	private void RefreshOnline()
	{
		bool flag = this.target.Online && (this.doorTarget == null || this.doorTarget.CurrentState == Door.ControlState.Auto);
		this.disabledOverlay.SetActive(!flag);
		this.headerBG.ColorState = (flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Inactive);
	}

	// Token: 0x0600A7EF RID: 42991 RVA: 0x0010CF7A File Offset: 0x0010B17A
	private void SortByPermission(bool state)
	{
		this.ExecuteSort<int>(this.sortByPermissionToggle, state, delegate(MinionAssignablesProxy identity)
		{
			if (!this.target.IsDefaultPermission(identity))
			{
				return (int)this.target.GetSetPermission(identity);
			}
			return -1;
		}, false);
	}

	// Token: 0x0600A7F0 RID: 42992 RVA: 0x003FB1EC File Offset: 0x003F93EC
	private void ExecuteSort<T>(Toggle toggle, bool state, Func<MinionAssignablesProxy, T> sortFunction, bool refresh = false)
	{
		toggle.GetComponent<ImageToggleState>().SetActiveState(state);
		if (!state)
		{
			return;
		}
		this.identityList = (state ? this.identityList.OrderBy(sortFunction).ToList<MinionAssignablesProxy>() : this.identityList.OrderByDescending(sortFunction).ToList<MinionAssignablesProxy>());
		if (refresh)
		{
			this.Refresh(this.identityList, false);
			return;
		}
		for (int i = 0; i < this.identityList.Count; i++)
		{
			if (this.identityRowMap.ContainsKey(this.identityList[i]))
			{
				this.identityRowMap[this.identityList[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x0600A7F1 RID: 42993 RVA: 0x003FB29C File Offset: 0x003F949C
	private void SortEntries(bool reverse_sort, Comparison<MinionAssignablesProxy> compare)
	{
		this.identityList.Sort(compare);
		if (reverse_sort)
		{
			this.identityList.Reverse();
		}
		for (int i = 0; i < this.identityList.Count; i++)
		{
			if (this.identityRowMap.ContainsKey(this.identityList[i]))
			{
				this.identityRowMap[this.identityList[i]].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x0600A7F2 RID: 42994 RVA: 0x0010CF96 File Offset: 0x0010B196
	private void ClearContent()
	{
		if (this.rowPool != null)
		{
			this.rowPool.ClearAll();
		}
		this.identityRowMap.Clear();
	}

	// Token: 0x0600A7F3 RID: 42995 RVA: 0x003FB314 File Offset: 0x003F9514
	private void OnDefaultPermissionChanged(MinionAssignablesProxy identity, AccessControl.Permission permission)
	{
		this.target.DefaultPermission = permission;
		this.Refresh(this.identityList, false);
		foreach (MinionAssignablesProxy key in this.identityList)
		{
			if (this.target.IsDefaultPermission(key))
			{
				this.target.ClearPermission(key);
			}
		}
	}

	// Token: 0x0600A7F4 RID: 42996 RVA: 0x0010CFB6 File Offset: 0x0010B1B6
	private void OnPermissionChanged(MinionAssignablesProxy identity, AccessControl.Permission permission)
	{
		this.target.SetPermission(identity, permission);
	}

	// Token: 0x0600A7F5 RID: 42997 RVA: 0x0010CFC5 File Offset: 0x0010B1C5
	private void OnPermissionDefault(MinionAssignablesProxy identity, bool isDefault)
	{
		if (isDefault)
		{
			this.target.ClearPermission(identity);
		}
		else
		{
			this.target.SetPermission(identity, this.target.DefaultPermission);
		}
		this.Refresh(this.identityList, false);
	}

	// Token: 0x0600A7F6 RID: 42998 RVA: 0x0010CFFC File Offset: 0x0010B1FC
	private void OnAccessControlChanged(object data)
	{
		this.RefreshOnline();
	}

	// Token: 0x0600A7F7 RID: 42999 RVA: 0x0010CFFC File Offset: 0x0010B1FC
	private void OnDoorStateChanged(object data)
	{
		this.RefreshOnline();
	}

	// Token: 0x0600A7F8 RID: 43000 RVA: 0x003FB394 File Offset: 0x003F9594
	private void OnSelectSortFunc(IListableOption role, object data)
	{
		if (role != null)
		{
			foreach (AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo in AccessControlSideScreen.MinionIdentitySort.SortInfos)
			{
				if (sortInfo.name == role.GetProperName())
				{
					this.sortInfo = sortInfo;
					this.identityList.Sort(this.sortInfo.compare);
					for (int j = 0; j < this.identityList.Count; j++)
					{
						if (this.identityRowMap.ContainsKey(this.identityList[j]))
						{
							this.identityRowMap[this.identityList[j]].transform.SetSiblingIndex(j);
						}
					}
					return;
				}
			}
		}
	}

	// Token: 0x0400840A RID: 33802
	[SerializeField]
	private AccessControlSideScreenRow rowPrefab;

	// Token: 0x0400840B RID: 33803
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x0400840C RID: 33804
	[SerializeField]
	private AccessControlSideScreenDoor defaultsRow;

	// Token: 0x0400840D RID: 33805
	[SerializeField]
	private Toggle sortByNameToggle;

	// Token: 0x0400840E RID: 33806
	[SerializeField]
	private Toggle sortByPermissionToggle;

	// Token: 0x0400840F RID: 33807
	[SerializeField]
	private Toggle sortByRoleToggle;

	// Token: 0x04008410 RID: 33808
	[SerializeField]
	private GameObject disabledOverlay;

	// Token: 0x04008411 RID: 33809
	[SerializeField]
	private KImage headerBG;

	// Token: 0x04008412 RID: 33810
	private AccessControl target;

	// Token: 0x04008413 RID: 33811
	private Door doorTarget;

	// Token: 0x04008414 RID: 33812
	private UIPool<AccessControlSideScreenRow> rowPool;

	// Token: 0x04008415 RID: 33813
	private AccessControlSideScreen.MinionIdentitySort.SortInfo sortInfo = AccessControlSideScreen.MinionIdentitySort.SortInfos[0];

	// Token: 0x04008416 RID: 33814
	private Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow> identityRowMap = new Dictionary<MinionAssignablesProxy, AccessControlSideScreenRow>();

	// Token: 0x04008417 RID: 33815
	private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();

	// Token: 0x02001F1D RID: 7965
	private static class MinionIdentitySort
	{
		// Token: 0x0600A7FD RID: 43005 RVA: 0x0010D077 File Offset: 0x0010B277
		public static int CompareByName(MinionAssignablesProxy a, MinionAssignablesProxy b)
		{
			return a.GetProperName().CompareTo(b.GetProperName());
		}

		// Token: 0x0600A7FE RID: 43006 RVA: 0x003FB44C File Offset: 0x003F964C
		public static int CompareByRole(MinionAssignablesProxy a, MinionAssignablesProxy b)
		{
			global::Debug.Assert(a, "a was null");
			global::Debug.Assert(b, "b was null");
			GameObject targetGameObject = a.GetTargetGameObject();
			GameObject targetGameObject2 = b.GetTargetGameObject();
			MinionResume minionResume = targetGameObject ? targetGameObject.GetComponent<MinionResume>() : null;
			MinionResume minionResume2 = targetGameObject2 ? targetGameObject2.GetComponent<MinionResume>() : null;
			if (minionResume2 == null)
			{
				return 1;
			}
			if (minionResume == null)
			{
				return -1;
			}
			int num = minionResume.CurrentRole.CompareTo(minionResume2.CurrentRole);
			if (num != 0)
			{
				return num;
			}
			return AccessControlSideScreen.MinionIdentitySort.CompareByName(a, b);
		}

		// Token: 0x04008418 RID: 33816
		public static readonly AccessControlSideScreen.MinionIdentitySort.SortInfo[] SortInfos = new AccessControlSideScreen.MinionIdentitySort.SortInfo[]
		{
			new AccessControlSideScreen.MinionIdentitySort.SortInfo
			{
				name = UI.MINION_IDENTITY_SORT.NAME,
				compare = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByName)
			},
			new AccessControlSideScreen.MinionIdentitySort.SortInfo
			{
				name = UI.MINION_IDENTITY_SORT.ROLE,
				compare = new Comparison<MinionAssignablesProxy>(AccessControlSideScreen.MinionIdentitySort.CompareByRole)
			}
		};

		// Token: 0x02001F1E RID: 7966
		public class SortInfo : IListableOption
		{
			// Token: 0x0600A800 RID: 43008 RVA: 0x0010D08A File Offset: 0x0010B28A
			public string GetProperName()
			{
				return this.name;
			}

			// Token: 0x04008419 RID: 33817
			public LocString name;

			// Token: 0x0400841A RID: 33818
			public Comparison<MinionAssignablesProxy> compare;
		}
	}
}
