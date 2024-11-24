using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/BuildingCellVisualizer")]
public class BuildingCellVisualizer : EntityCellVisualizer
{
	[MyCmpReq]
	private Building building;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	protected override void LoadDiseaseIcon()
	{
		DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo(building.Def.DiseaseCellVisName);
		if (info.name != null)
		{
			diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
			diseaseSourceColour = GlobalAssets.Instance.colorSet.GetColorByName(info.overlayColourName);
		}
	}

	protected override void DefinePorts()
	{
		BuildingDef def = building.Def;
		if (def.CheckRequiresPowerInput())
		{
			AddPort(Ports.PowerIn, building.Def.PowerInputOffset, base.Resources.electricityInputColor, Color.gray, 1f);
		}
		if (def.CheckRequiresPowerOutput())
		{
			AddPort(Ports.PowerOut, building.Def.PowerOutputOffset, building.Def.UseWhitePowerOutputConnectorColour ? base.Resources.electricityInputColor : base.Resources.electricityOutputColor, Color.gray, 1f);
		}
		if (def.CheckRequiresGasInput())
		{
			AddPort(Ports.GasIn, building.Def.UtilityInputOffset, base.Resources.gasIOColours.input.connected, base.Resources.gasIOColours.input.disconnected);
		}
		if (def.CheckRequiresGasOutput())
		{
			AddPort(Ports.GasOut, building.Def.UtilityOutputOffset, base.Resources.gasIOColours.output.connected, base.Resources.gasIOColours.output.disconnected);
		}
		if (def.CheckRequiresLiquidInput())
		{
			AddPort(Ports.LiquidIn, building.Def.UtilityInputOffset, base.Resources.liquidIOColours.input.connected, base.Resources.liquidIOColours.input.disconnected);
		}
		if (def.CheckRequiresLiquidOutput())
		{
			AddPort(Ports.LiquidOut, building.Def.UtilityOutputOffset, base.Resources.liquidIOColours.output.connected, base.Resources.liquidIOColours.output.disconnected);
		}
		if (def.CheckRequiresSolidInput())
		{
			AddPort(Ports.SolidIn, building.Def.UtilityInputOffset, base.Resources.liquidIOColours.input.connected, base.Resources.liquidIOColours.input.disconnected);
		}
		if (def.CheckRequiresSolidOutput())
		{
			AddPort(Ports.SolidOut, building.Def.UtilityOutputOffset, base.Resources.liquidIOColours.output.connected, base.Resources.liquidIOColours.output.disconnected);
		}
		if (def.CheckRequiresHighEnergyParticleInput())
		{
			AddPort(Ports.HighEnergyParticleIn, building.Def.HighEnergyParticleInputOffset, base.Resources.highEnergyParticleInputColour, Color.white, 3f);
		}
		if (def.CheckRequiresHighEnergyParticleOutput())
		{
			AddPort(Ports.HighEnergyParticleOut, building.Def.HighEnergyParticleOutputOffset, base.Resources.highEnergyParticleOutputColour, Color.white, 3f);
		}
		if (def.SelfHeatKilowattsWhenActive > 0f || def.ExhaustKilowattsWhenActive > 0f)
		{
			AddPort(Ports.HeatSource, default(CellOffset));
		}
		if (def.SelfHeatKilowattsWhenActive < 0f || def.ExhaustKilowattsWhenActive < 0f)
		{
			AddPort(Ports.HeatSink, default(CellOffset));
		}
		if (diseaseSourceSprite != null)
		{
			AddPort(Ports.DiseaseOut, building.Def.UtilityOutputOffset, diseaseSourceColour);
		}
		ISecondaryInput[] components = def.BuildingComplete.GetComponents<ISecondaryInput>();
		foreach (ISecondaryInput secondaryInput in components)
		{
			if (secondaryInput == null)
			{
				continue;
			}
			if (secondaryInput.HasSecondaryConduitType(ConduitType.Gas))
			{
				BuildingCellVisualizerResources.ConnectedDisconnectedColours connectedDisconnectedColours = (def.CheckRequiresGasInput() ? base.Resources.alternateIOColours.input : base.Resources.gasIOColours.input);
				AddPort(Ports.GasIn, secondaryInput.GetSecondaryConduitOffset(ConduitType.Gas), connectedDisconnectedColours.connected, connectedDisconnectedColours.disconnected);
			}
			if (secondaryInput.HasSecondaryConduitType(ConduitType.Liquid))
			{
				if (!def.CheckRequiresLiquidInput())
				{
					_ = base.Resources.liquidIOColours;
				}
				else
				{
					_ = base.Resources.alternateIOColours;
				}
				AddPort(Ports.LiquidIn, secondaryInput.GetSecondaryConduitOffset(ConduitType.Liquid));
			}
			if (secondaryInput.HasSecondaryConduitType(ConduitType.Solid))
			{
				if (!def.CheckRequiresSolidInput())
				{
					_ = base.Resources.liquidIOColours;
				}
				else
				{
					_ = base.Resources.alternateIOColours;
				}
				AddPort(Ports.SolidIn, secondaryInput.GetSecondaryConduitOffset(ConduitType.Solid));
			}
		}
		ISecondaryOutput[] components2 = def.BuildingComplete.GetComponents<ISecondaryOutput>();
		foreach (ISecondaryOutput secondaryOutput in components2)
		{
			if (secondaryOutput != null)
			{
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Gas))
				{
					BuildingCellVisualizerResources.ConnectedDisconnectedColours connectedDisconnectedColours2 = (def.CheckRequiresGasOutput() ? base.Resources.alternateIOColours.output : base.Resources.gasIOColours.output);
					AddPort(Ports.GasOut, secondaryOutput.GetSecondaryConduitOffset(ConduitType.Gas), connectedDisconnectedColours2.connected, connectedDisconnectedColours2.disconnected);
				}
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Liquid))
				{
					BuildingCellVisualizerResources.ConnectedDisconnectedColours connectedDisconnectedColours3 = (def.CheckRequiresLiquidOutput() ? base.Resources.alternateIOColours.output : base.Resources.liquidIOColours.output);
					AddPort(Ports.LiquidOut, secondaryOutput.GetSecondaryConduitOffset(ConduitType.Liquid), connectedDisconnectedColours3.connected, connectedDisconnectedColours3.disconnected);
				}
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Solid))
				{
					BuildingCellVisualizerResources.ConnectedDisconnectedColours connectedDisconnectedColours4 = (def.CheckRequiresSolidOutput() ? base.Resources.alternateIOColours.output : base.Resources.liquidIOColours.output);
					AddPort(Ports.SolidOut, secondaryOutput.GetSecondaryConduitOffset(ConduitType.Solid), connectedDisconnectedColours4.connected, connectedDisconnectedColours4.disconnected);
				}
			}
		}
	}

	protected override void OnCmpEnable()
	{
		enableRaycast = building as BuildingComplete != null;
		base.OnCmpEnable();
	}
}
