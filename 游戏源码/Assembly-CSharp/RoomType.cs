using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000B8D RID: 2957
public class RoomType : Resource
{
	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06003875 RID: 14453 RVA: 0x000C48DA File Offset: 0x000C2ADA
	// (set) Token: 0x06003876 RID: 14454 RVA: 0x000C48E2 File Offset: 0x000C2AE2
	public string tooltip { get; private set; }

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06003877 RID: 14455 RVA: 0x000C48EB File Offset: 0x000C2AEB
	// (set) Token: 0x06003878 RID: 14456 RVA: 0x000C48F3 File Offset: 0x000C2AF3
	public string description { get; set; }

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06003879 RID: 14457 RVA: 0x000C48FC File Offset: 0x000C2AFC
	// (set) Token: 0x0600387A RID: 14458 RVA: 0x000C4904 File Offset: 0x000C2B04
	public string effect { get; private set; }

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x0600387B RID: 14459 RVA: 0x000C490D File Offset: 0x000C2B0D
	// (set) Token: 0x0600387C RID: 14460 RVA: 0x000C4915 File Offset: 0x000C2B15
	public RoomConstraints.Constraint primary_constraint { get; private set; }

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x0600387D RID: 14461 RVA: 0x000C491E File Offset: 0x000C2B1E
	// (set) Token: 0x0600387E RID: 14462 RVA: 0x000C4926 File Offset: 0x000C2B26
	public RoomConstraints.Constraint[] additional_constraints { get; private set; }

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x0600387F RID: 14463 RVA: 0x000C492F File Offset: 0x000C2B2F
	// (set) Token: 0x06003880 RID: 14464 RVA: 0x000C4937 File Offset: 0x000C2B37
	public int priority { get; private set; }

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06003881 RID: 14465 RVA: 0x000C4940 File Offset: 0x000C2B40
	// (set) Token: 0x06003882 RID: 14466 RVA: 0x000C4948 File Offset: 0x000C2B48
	public bool single_assignee { get; private set; }

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06003883 RID: 14467 RVA: 0x000C4951 File Offset: 0x000C2B51
	// (set) Token: 0x06003884 RID: 14468 RVA: 0x000C4959 File Offset: 0x000C2B59
	public RoomDetails.Detail[] display_details { get; private set; }

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06003885 RID: 14469 RVA: 0x000C4962 File Offset: 0x000C2B62
	// (set) Token: 0x06003886 RID: 14470 RVA: 0x000C496A File Offset: 0x000C2B6A
	public bool priority_building_use { get; private set; }

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06003887 RID: 14471 RVA: 0x000C4973 File Offset: 0x000C2B73
	// (set) Token: 0x06003888 RID: 14472 RVA: 0x000C497B File Offset: 0x000C2B7B
	public RoomTypeCategory category { get; private set; }

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06003889 RID: 14473 RVA: 0x000C4984 File Offset: 0x000C2B84
	// (set) Token: 0x0600388A RID: 14474 RVA: 0x000C498C File Offset: 0x000C2B8C
	public RoomType[] upgrade_paths { get; private set; }

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x0600388B RID: 14475 RVA: 0x000C4995 File Offset: 0x000C2B95
	// (set) Token: 0x0600388C RID: 14476 RVA: 0x000C499D File Offset: 0x000C2B9D
	public string[] effects { get; private set; }

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x0600388D RID: 14477 RVA: 0x000C49A6 File Offset: 0x000C2BA6
	// (set) Token: 0x0600388E RID: 14478 RVA: 0x000C49AE File Offset: 0x000C2BAE
	public int sortKey { get; private set; }

	// Token: 0x0600388F RID: 14479 RVA: 0x0021B0B0 File Offset: 0x002192B0
	public RoomType(string id, string name, string description, string tooltip, string effect, RoomTypeCategory category, RoomConstraints.Constraint primary_constraint, RoomConstraints.Constraint[] additional_constraints, RoomDetails.Detail[] display_details, int priority = 0, RoomType[] upgrade_paths = null, bool single_assignee = false, bool priority_building_use = false, string[] effects = null, int sortKey = 0) : base(id, name)
	{
		this.tooltip = tooltip;
		this.description = description;
		this.effect = effect;
		this.category = category;
		this.primary_constraint = primary_constraint;
		this.additional_constraints = additional_constraints;
		this.display_details = display_details;
		this.priority = priority;
		this.upgrade_paths = upgrade_paths;
		this.single_assignee = single_assignee;
		this.priority_building_use = priority_building_use;
		this.effects = effects;
		this.sortKey = sortKey;
		if (this.upgrade_paths != null)
		{
			RoomType[] upgrade_paths2 = this.upgrade_paths;
			for (int i = 0; i < upgrade_paths2.Length; i++)
			{
				Debug.Assert(upgrade_paths2[i] != null, name + " has a null upgrade path. Maybe it wasn't initialized yet.");
			}
		}
	}

	// Token: 0x06003890 RID: 14480 RVA: 0x0021B160 File Offset: 0x00219360
	public RoomType.RoomIdentificationResult isSatisfactory(Room candidate_room)
	{
		if (this.primary_constraint != null && !this.primary_constraint.isSatisfied(candidate_room))
		{
			return RoomType.RoomIdentificationResult.primary_unsatisfied;
		}
		if (this.additional_constraints != null)
		{
			RoomConstraints.Constraint[] additional_constraints = this.additional_constraints;
			for (int i = 0; i < additional_constraints.Length; i++)
			{
				if (!additional_constraints[i].isSatisfied(candidate_room))
				{
					return RoomType.RoomIdentificationResult.primary_satisfied;
				}
			}
		}
		return RoomType.RoomIdentificationResult.all_satisfied;
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x0021B1B0 File Offset: 0x002193B0
	public string GetCriteriaString()
	{
		string text = string.Concat(new string[]
		{
			"<b>",
			this.Name,
			"</b>\n",
			this.tooltip,
			"\n\n",
			ROOMS.CRITERIA.HEADER
		});
		if (this == Db.Get().RoomTypes.Neutral)
		{
			text = text + "\n    • " + ROOMS.CRITERIA.NEUTRAL_TYPE;
		}
		text += ((this.primary_constraint == null) ? "" : ("\n    • " + this.primary_constraint.name));
		if (this.additional_constraints != null)
		{
			foreach (RoomConstraints.Constraint constraint in this.additional_constraints)
			{
				text = text + "\n    • " + constraint.name;
			}
		}
		return text;
	}

	// Token: 0x06003892 RID: 14482 RVA: 0x0021B288 File Offset: 0x00219488
	public string GetRoomEffectsString()
	{
		if (this.effects != null && this.effects.Length != 0)
		{
			string text = ROOMS.EFFECTS.HEADER;
			foreach (string id in this.effects)
			{
				Effect effect = Db.Get().effects.Get(id);
				text += Effect.CreateTooltip(effect, false, "\n    • ", false);
			}
			return text;
		}
		return null;
	}

	// Token: 0x06003893 RID: 14483 RVA: 0x0021B2F4 File Offset: 0x002194F4
	public void TriggerRoomEffects(KPrefabID triggerer, Effects target)
	{
		if (this.primary_constraint == null)
		{
			return;
		}
		if (triggerer == null)
		{
			return;
		}
		if (this.effects == null)
		{
			return;
		}
		if (this.primary_constraint.building_criteria(triggerer))
		{
			foreach (string effect_id in this.effects)
			{
				target.Add(effect_id, true);
			}
		}
	}

	// Token: 0x02000B8E RID: 2958
	public enum RoomIdentificationResult
	{
		// Token: 0x04002699 RID: 9881
		all_satisfied,
		// Token: 0x0400269A RID: 9882
		primary_satisfied,
		// Token: 0x0400269B RID: 9883
		primary_unsatisfied
	}
}
