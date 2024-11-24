using System;
using KSerialization;
using STRINGS;

// Token: 0x02000337 RID: 823
public class FossilBits : FossilExcavationWorkable, ISidescreenButtonControl
{
	// Token: 0x06000D31 RID: 3377 RVA: 0x000ABBDD File Offset: 0x000A9DDD
	protected override bool IsMarkedForExcavation()
	{
		return this.MarkedForDig;
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x000ABBE5 File Offset: 0x000A9DE5
	public void SetEntombStatusItemVisibility(bool visible)
	{
		this.entombComponent.SetShowStatusItemOnEntombed(visible);
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x00172CB0 File Offset: 0x00170EB0
	public void CreateWorkableChore()
	{
		if (this.chore == null && this.operational.IsOperational)
		{
			this.chore = new WorkChore<FossilBits>(Db.Get().ChoreTypes.ExcavateFossil, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x000ABBF3 File Offset: 0x000A9DF3
	public void CancelWorkChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("FossilBits.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x00172D00 File Offset: 0x00170F00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		base.SetWorkTime(30f);
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x000ABC14 File Offset: 0x000A9E14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		base.SetShouldShowSkillPerkStatusItem(this.IsMarkedForExcavation());
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x000ABC34 File Offset: 0x000A9E34
	private void OnOperationalChanged(object state)
	{
		if ((bool)state)
		{
			if (this.MarkedForDig)
			{
				this.CreateWorkableChore();
				return;
			}
		}
		else if (this.MarkedForDig)
		{
			this.CancelWorkChore();
		}
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x00172D54 File Offset: 0x00170F54
	private void DropLoot()
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x000ABC5B File Offset: 0x000A9E5B
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.DropLoot();
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x000ABC75 File Offset: 0x000A9E75
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000D3B RID: 3387 RVA: 0x000ABC78 File Offset: 0x000A9E78
	public string SidescreenButtonText
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000D3C RID: 3388 RVA: 0x000ABC97 File Offset: 0x000A9E97
	public string SidescreenButtonTooltip
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x00172E0C File Offset: 0x0017100C
	public void OnSidescreenButtonPressed()
	{
		this.MarkedForDig = !this.MarkedForDig;
		base.SetShouldShowSkillPerkStatusItem(this.MarkedForDig);
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		if (this.MarkedForDig)
		{
			this.CreateWorkableChore();
		}
		else
		{
			this.CancelWorkChore();
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x000ABCBD File Offset: 0x000A9EBD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x040009A7 RID: 2471
	[Serialize]
	public bool MarkedForDig;

	// Token: 0x040009A8 RID: 2472
	private Chore chore;

	// Token: 0x040009A9 RID: 2473
	[MyCmpGet]
	private EntombVulnerable entombComponent;

	// Token: 0x040009AA RID: 2474
	[MyCmpGet]
	private Operational operational;
}
