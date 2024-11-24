using System;
using System.Collections.Generic;

// Token: 0x02001298 RID: 4760
public class EnergySim
{
	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x060061E8 RID: 25064 RVA: 0x000DFE39 File Offset: 0x000DE039
	public HashSet<Generator> Generators
	{
		get
		{
			return this.generators;
		}
	}

	// Token: 0x060061E9 RID: 25065 RVA: 0x000DFE41 File Offset: 0x000DE041
	public void AddGenerator(Generator generator)
	{
		this.generators.Add(generator);
	}

	// Token: 0x060061EA RID: 25066 RVA: 0x000DFE50 File Offset: 0x000DE050
	public void RemoveGenerator(Generator generator)
	{
		this.generators.Remove(generator);
	}

	// Token: 0x060061EB RID: 25067 RVA: 0x000DFE5F File Offset: 0x000DE05F
	public void AddManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Add(manual_generator);
	}

	// Token: 0x060061EC RID: 25068 RVA: 0x000DFE6E File Offset: 0x000DE06E
	public void RemoveManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Remove(manual_generator);
	}

	// Token: 0x060061ED RID: 25069 RVA: 0x000DFE7D File Offset: 0x000DE07D
	public void AddBattery(Battery battery)
	{
		this.batteries.Add(battery);
	}

	// Token: 0x060061EE RID: 25070 RVA: 0x000DFE8C File Offset: 0x000DE08C
	public void RemoveBattery(Battery battery)
	{
		this.batteries.Remove(battery);
	}

	// Token: 0x060061EF RID: 25071 RVA: 0x000DFE9B File Offset: 0x000DE09B
	public void AddEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Add(energy_consumer);
	}

	// Token: 0x060061F0 RID: 25072 RVA: 0x000DFEAA File Offset: 0x000DE0AA
	public void RemoveEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Remove(energy_consumer);
	}

	// Token: 0x060061F1 RID: 25073 RVA: 0x002B4A44 File Offset: 0x002B2C44
	public void EnergySim200ms(float dt)
	{
		foreach (Generator generator in this.generators)
		{
			generator.EnergySim200ms(dt);
		}
		foreach (ManualGenerator manualGenerator in this.manualGenerators)
		{
			manualGenerator.EnergySim200ms(dt);
		}
		foreach (Battery battery in this.batteries)
		{
			battery.EnergySim200ms(dt);
		}
		foreach (EnergyConsumer energyConsumer in this.energyConsumers)
		{
			energyConsumer.EnergySim200ms(dt);
		}
	}

	// Token: 0x040045AB RID: 17835
	private HashSet<Generator> generators = new HashSet<Generator>();

	// Token: 0x040045AC RID: 17836
	private HashSet<ManualGenerator> manualGenerators = new HashSet<ManualGenerator>();

	// Token: 0x040045AD RID: 17837
	private HashSet<Battery> batteries = new HashSet<Battery>();

	// Token: 0x040045AE RID: 17838
	private HashSet<EnergyConsumer> energyConsumers = new HashSet<EnergyConsumer>();
}
