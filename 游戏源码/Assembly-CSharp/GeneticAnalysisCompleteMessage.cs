using System;
using KSerialization;
using STRINGS;

// Token: 0x02001DF7 RID: 7671
public class GeneticAnalysisCompleteMessage : Message
{
	// Token: 0x0600A07C RID: 41084 RVA: 0x001082E7 File Offset: 0x001064E7
	public GeneticAnalysisCompleteMessage()
	{
	}

	// Token: 0x0600A07D RID: 41085 RVA: 0x00108547 File Offset: 0x00106747
	public GeneticAnalysisCompleteMessage(Tag subSpeciesID)
	{
		this.subSpeciesID = subSpeciesID;
	}

	// Token: 0x0600A07E RID: 41086 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x0600A07F RID: 41087 RVA: 0x003D5B4C File Offset: 0x003D3D4C
	public override string GetMessageBody()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.MESSAGEBODY.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName()).Replace("{Subspecies}", subSpeciesInfo.GetNameWithMutations(subSpeciesInfo.speciesID.ProperName(), true, false)).Replace("{Info}", subSpeciesInfo.GetMutationsTooltip());
	}

	// Token: 0x0600A080 RID: 41088 RVA: 0x00108556 File Offset: 0x00106756
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.NAME;
	}

	// Token: 0x0600A081 RID: 41089 RVA: 0x003D5BB4 File Offset: 0x003D3DB4
	public override string GetTooltip()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x0600A082 RID: 41090 RVA: 0x00108562 File Offset: 0x00106762
	public override bool IsValid()
	{
		return this.subSpeciesID.IsValid;
	}

	// Token: 0x04007D7A RID: 32122
	[Serialize]
	public Tag subSpeciesID;
}
