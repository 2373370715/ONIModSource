using System;
using System.Collections.Generic;

// Token: 0x0200207A RID: 8314
public static class WorldGenProgressStages
{
	// Token: 0x04008BCE RID: 35790
	public static KeyValuePair<WorldGenProgressStages.Stages, float>[] StageWeights = new KeyValuePair<WorldGenProgressStages.Stages, float>[]
	{
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Failure, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SetupNoise, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateNoise, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateSolarSystem, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.WorldLayout, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.CompleteLayout, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NoiseMapBuilder, 9f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ClearingLevel, 0.5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Processing, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Borders, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ProcessRivers, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ConvertCellsToEdges, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DrawWorldBorder, 0.2f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlaceTemplates, 5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SettleSim, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DetectNaturalCavities, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlacingCreatures, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Complete, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NumberOfStages, 0f)
	};

	// Token: 0x0200207B RID: 8315
	public enum Stages
	{
		// Token: 0x04008BD0 RID: 35792
		Failure,
		// Token: 0x04008BD1 RID: 35793
		SetupNoise,
		// Token: 0x04008BD2 RID: 35794
		GenerateNoise,
		// Token: 0x04008BD3 RID: 35795
		GenerateSolarSystem,
		// Token: 0x04008BD4 RID: 35796
		WorldLayout,
		// Token: 0x04008BD5 RID: 35797
		CompleteLayout,
		// Token: 0x04008BD6 RID: 35798
		NoiseMapBuilder,
		// Token: 0x04008BD7 RID: 35799
		ClearingLevel,
		// Token: 0x04008BD8 RID: 35800
		Processing,
		// Token: 0x04008BD9 RID: 35801
		Borders,
		// Token: 0x04008BDA RID: 35802
		ProcessRivers,
		// Token: 0x04008BDB RID: 35803
		ConvertCellsToEdges,
		// Token: 0x04008BDC RID: 35804
		DrawWorldBorder,
		// Token: 0x04008BDD RID: 35805
		PlaceTemplates,
		// Token: 0x04008BDE RID: 35806
		SettleSim,
		// Token: 0x04008BDF RID: 35807
		DetectNaturalCavities,
		// Token: 0x04008BE0 RID: 35808
		PlacingCreatures,
		// Token: 0x04008BE1 RID: 35809
		Complete,
		// Token: 0x04008BE2 RID: 35810
		NumberOfStages
	}
}
