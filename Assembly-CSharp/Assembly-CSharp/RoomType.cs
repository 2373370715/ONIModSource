﻿using System;
using Klei.AI;
using STRINGS;

public class RoomType : Resource
{
			public string tooltip { get; private set; }

			public string description { get; set; }

			public string effect { get; private set; }

			public RoomConstraints.Constraint primary_constraint { get; private set; }

			public RoomConstraints.Constraint[] additional_constraints { get; private set; }

			public int priority { get; private set; }

			public bool single_assignee { get; private set; }

			public RoomDetails.Detail[] display_details { get; private set; }

			public bool priority_building_use { get; private set; }

			public RoomTypeCategory category { get; private set; }

			public RoomType[] upgrade_paths { get; private set; }

			public string[] effects { get; private set; }

			public int sortKey { get; private set; }

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

	public enum RoomIdentificationResult
	{
		all_satisfied,
		primary_satisfied,
		primary_unsatisfied
	}
}
