using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B27 RID: 2855
[AddComponentMenu("KMonoBehaviour/Workable/Studyable")]
public class Studyable : Workable, ISidescreenButtonControl
{
	// Token: 0x17000258 RID: 600
	// (get) Token: 0x0600363D RID: 13885 RVA: 0x000C348B File Offset: 0x000C168B
	public bool Studied
	{
		get
		{
			return this.studied;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x0600363E RID: 13886 RVA: 0x000C3493 File Offset: 0x000C1693
	public bool Studying
	{
		get
		{
			return this.chore != null && this.chore.InProgress();
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x0600363F RID: 13887 RVA: 0x000C34AA File Offset: 0x000C16AA
	public string SidescreenTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06003640 RID: 13888 RVA: 0x000C34B1 File Offset: 0x000C16B1
	public string SidescreenStatusMessage
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06003641 RID: 13889 RVA: 0x000C34E3 File Offset: 0x000C16E3
	public string SidescreenButtonText
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_BUTTON;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_BUTTON;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_BUTTON;
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06003642 RID: 13890 RVA: 0x000C34B1 File Offset: 0x000C16B1
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x000ABC75 File Offset: 0x000A9E75
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06003644 RID: 13892 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003645 RID: 13893 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06003646 RID: 13894 RVA: 0x000C3515 File Offset: 0x000C1715
	public bool SidescreenButtonInteractable()
	{
		return !this.studied;
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x00213074 File Offset: 0x00211274
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Studying;
		this.resetProgressOnStop = false;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		base.SetWorkTime(3600f);
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x0021313C File Offset: 0x0021133C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.studiedIndicator = new MeterController(base.GetComponent<KBatchedAnimController>(), this.meterTrackerSymbol, this.meterAnim, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			this.meterTrackerSymbol
		});
		this.studiedIndicator.meterController.gameObject.AddComponent<LoopingSounds>();
		this.Refresh();
	}

	// Token: 0x06003649 RID: 13897 RVA: 0x000C3520 File Offset: 0x000C1720
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Studyable.CancelChore");
			this.chore = null;
			base.Trigger(1488501379, null);
		}
	}

	// Token: 0x0600364A RID: 13898 RVA: 0x0021319C File Offset: 0x0021139C
	public void Refresh()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.studied)
		{
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.Studied, null);
			this.studiedIndicator.gameObject.SetActive(true);
			this.studiedIndicator.meterController.Play(this.meterAnim, KAnim.PlayMode.Loop, 1f, 0f);
			this.requiredSkillPerk = null;
			this.UpdateStatusItem(null);
			return;
		}
		if (this.markedForStudy)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<Studyable>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy, null);
		}
		else
		{
			this.CancelChore();
			this.statusItemGuid = component.RemoveStatusItem(this.statusItemGuid, false);
		}
		this.studiedIndicator.gameObject.SetActive(false);
	}

	// Token: 0x0600364B RID: 13899 RVA: 0x002132B4 File Offset: 0x002114B4
	private void ToggleStudyChore()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.studied = true;
			if (this.chore != null)
			{
				this.chore.Cancel("debug");
				this.chore = null;
			}
			base.Trigger(-1436775550, null);
		}
		else
		{
			this.markedForStudy = !this.markedForStudy;
		}
		this.Refresh();
	}

	// Token: 0x0600364C RID: 13900 RVA: 0x000C354D File Offset: 0x000C174D
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.studied = true;
		this.chore = null;
		this.Refresh();
		base.Trigger(-1436775550, null);
		if (DlcManager.IsExpansion1Active())
		{
			this.DropDatabanks();
		}
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x00213314 File Offset: 0x00211514
	private void DropDatabanks()
	{
		int num = UnityEngine.Random.Range(7, 13);
		for (int i = 0; i <= num; i++)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), base.transform.position + new Vector3(0f, 1f, 0f), Grid.SceneLayer.Ore, null, 0);
			gameObject.GetComponent<PrimaryElement>().Temperature = 298.15f;
			gameObject.SetActive(true);
		}
	}

	// Token: 0x0600364E RID: 13902 RVA: 0x000C3583 File Offset: 0x000C1783
	public void OnSidescreenButtonPressed()
	{
		this.ToggleStudyChore();
	}

	// Token: 0x0600364F RID: 13903 RVA: 0x000ABCBD File Offset: 0x000A9EBD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x040024EF RID: 9455
	public string meterTrackerSymbol;

	// Token: 0x040024F0 RID: 9456
	public string meterAnim;

	// Token: 0x040024F1 RID: 9457
	private Chore chore;

	// Token: 0x040024F2 RID: 9458
	private const float STUDY_WORK_TIME = 3600f;

	// Token: 0x040024F3 RID: 9459
	[Serialize]
	private bool studied;

	// Token: 0x040024F4 RID: 9460
	[Serialize]
	private bool markedForStudy;

	// Token: 0x040024F5 RID: 9461
	private Guid statusItemGuid;

	// Token: 0x040024F6 RID: 9462
	private Guid additionalStatusItemGuid;

	// Token: 0x040024F7 RID: 9463
	public MeterController studiedIndicator;
}
