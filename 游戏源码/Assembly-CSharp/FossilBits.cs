using System;
using KSerialization;
using STRINGS;

public class FossilBits : FossilExcavationWorkable, ISidescreenButtonControl
{
	[Serialize]
	public bool MarkedForDig;

	private Chore chore;

	[MyCmpGet]
	private EntombVulnerable entombComponent;

	[MyCmpGet]
	private Operational operational;

	public string SidescreenButtonText
	{
		get
		{
			if (!MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON;
		}
	}

	public string SidescreenButtonTooltip
	{
		get
		{
			if (!MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	protected override bool IsMarkedForExcavation()
	{
		return MarkedForDig;
	}

	public void SetEntombStatusItemVisibility(bool visible)
	{
		entombComponent.SetShowStatusItemOnEntombed(visible);
	}

	public void CreateWorkableChore()
	{
		if (chore == null && operational.IsOperational)
		{
			chore = new WorkChore<FossilBits>(Db.Get().ChoreTypes.ExcavateFossil, this, null, run_until_complete: true, null, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false);
		}
	}

	public void CancelWorkChore()
	{
		if (chore != null)
		{
			chore.Cancel("FossilBits.CancelChore");
			chore = null;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_sculpture_kanim") };
		Subscribe(-592767678, OnOperationalChanged);
		SetWorkTime(30f);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		SetEntombStatusItemVisibility(MarkedForDig);
		SetShouldShowSkillPerkStatusItem(IsMarkedForExcavation());
	}

	private void OnOperationalChanged(object state)
	{
		if ((bool)state)
		{
			if (MarkedForDig)
			{
				CreateWorkableChore();
			}
		}
		else if (MarkedForDig)
		{
			CancelWorkChore();
		}
	}

	private void DropLoot()
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element == null)
		{
			return;
		}
		float num = component.Mass;
		for (int i = 0; (float)i < component.Mass / 400f; i++)
		{
			float num2 = num;
			if (num > 400f)
			{
				num2 = 400f;
				num -= 400f;
			}
			int disease_count = (int)((float)component.DiseaseCount * (num2 / component.Mass));
			element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num2, component.Temperature, component.DiseaseIdx, disease_count);
		}
	}

	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		DropLoot();
		Util.KDestroyGameObject(base.gameObject);
	}

	public int HorizontalGroupID()
	{
		return -1;
	}

	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	public bool SidescreenEnabled()
	{
		return true;
	}

	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	public void OnSidescreenButtonPressed()
	{
		MarkedForDig = !MarkedForDig;
		SetShouldShowSkillPerkStatusItem(MarkedForDig);
		SetEntombStatusItemVisibility(MarkedForDig);
		if (MarkedForDig)
		{
			CreateWorkableChore();
		}
		else
		{
			CancelWorkChore();
		}
		UpdateStatusItem();
	}

	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}
}
