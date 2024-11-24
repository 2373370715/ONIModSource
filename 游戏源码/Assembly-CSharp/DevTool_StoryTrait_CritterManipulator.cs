using System.Collections.Generic;
using ImGuiNET;
using STRINGS;

public class DevTool_StoryTrait_CritterManipulator : DevTool
{
	protected override void RenderTo(DevPanel panel)
	{
		if (!ImGui.CollapsingHeader("Debug species lore unlock popup", ImGuiTreeNodeFlags.DefaultOpen))
		{
			return;
		}
		Button_OpenSpecies(Tag.Invalid, "Unknown Species");
		ImGui.Separator();
		foreach (Tag critterSpeciesTag in GetCritterSpeciesTags())
		{
			Button_OpenSpecies(critterSpeciesTag, GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(critterSpeciesTag).Unwrap());
		}
	}

	public void Button_OpenSpecies(Tag species, string speciesName = null)
	{
		speciesName = ((speciesName != null) ? $"\"{UI.StripLinkFormatting(speciesName)}\" (ID: {species})" : species.Name);
		if (ImGui.Button("Show popup for: " + speciesName))
		{
			GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(species);
		}
	}

	public IEnumerable<Tag> GetCritterSpeciesTags()
	{
		yield return GameTags.Creatures.Species.HatchSpecies;
		yield return GameTags.Creatures.Species.LightBugSpecies;
		yield return GameTags.Creatures.Species.OilFloaterSpecies;
		yield return GameTags.Creatures.Species.DreckoSpecies;
		yield return GameTags.Creatures.Species.GlomSpecies;
		yield return GameTags.Creatures.Species.PuftSpecies;
		yield return GameTags.Creatures.Species.PacuSpecies;
		yield return GameTags.Creatures.Species.MooSpecies;
		yield return GameTags.Creatures.Species.MoleSpecies;
		yield return GameTags.Creatures.Species.SquirrelSpecies;
		yield return GameTags.Creatures.Species.CrabSpecies;
		yield return GameTags.Creatures.Species.DivergentSpecies;
		yield return GameTags.Creatures.Species.StaterpillarSpecies;
		yield return GameTags.Creatures.Species.BeetaSpecies;
	}
}
