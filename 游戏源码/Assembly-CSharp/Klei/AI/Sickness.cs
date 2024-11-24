using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B14 RID: 15124
	[DebuggerDisplay("{base.Id}")]
	public abstract class Sickness : Resource
	{
		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x0600E8D1 RID: 59601 RVA: 0x0013BAAC File Offset: 0x00139CAC
		public new string Name
		{
			get
			{
				return Strings.Get(this.name);
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x0600E8D2 RID: 59602 RVA: 0x0013BABE File Offset: 0x00139CBE
		public float SicknessDuration
		{
			get
			{
				return this.sicknessDuration;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x0600E8D3 RID: 59603 RVA: 0x0013BAC6 File Offset: 0x00139CC6
		public StringKey DescriptiveSymptoms
		{
			get
			{
				return this.descriptiveSymptoms;
			}
		}

		// Token: 0x0600E8D4 RID: 59604 RVA: 0x004C365C File Offset: 0x004C185C
		public Sickness(string id, Sickness.SicknessType type, Sickness.Severity severity, float immune_attack_strength, List<Sickness.InfectionVector> infection_vectors, float sickness_duration, string recovery_effect = null) : base(id, null, null)
		{
			this.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
			this.id = id;
			this.sicknessType = type;
			this.severity = severity;
			this.infectionVectors = infection_vectors;
			this.sicknessDuration = sickness_duration;
			this.recoveryEffect = recovery_effect;
			this.descriptiveSymptoms = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".DESCRIPTIVE_SYMPTOMS");
			this.cureSpeedBase = new Attribute(id + "CureSpeed", false, Attribute.Display.Normal, false, 0f, null, null, null, null);
			this.cureSpeedBase.BaseValue = 1f;
			this.cureSpeedBase.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
			Db.Get().Attributes.Add(this.cureSpeedBase);
		}

		// Token: 0x0600E8D5 RID: 59605 RVA: 0x004C3758 File Offset: 0x004C1958
		public object[] Infect(GameObject go, SicknessInstance diseaseInstance, SicknessExposureInfo exposure_info)
		{
			object[] array = new object[this.components.Count];
			for (int i = 0; i < this.components.Count; i++)
			{
				array[i] = this.components[i].OnInfect(go, diseaseInstance);
			}
			return array;
		}

		// Token: 0x0600E8D6 RID: 59606 RVA: 0x004C37A4 File Offset: 0x004C19A4
		public void Cure(GameObject go, object[] componentData)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				this.components[i].OnCure(go, componentData[i]);
			}
		}

		// Token: 0x0600E8D7 RID: 59607 RVA: 0x004C37DC File Offset: 0x004C19DC
		public List<Descriptor> GetSymptoms()
		{
			List<Descriptor> list = new List<Descriptor>();
			for (int i = 0; i < this.components.Count; i++)
			{
				List<Descriptor> symptoms = this.components[i].GetSymptoms();
				if (symptoms != null)
				{
					list.AddRange(symptoms);
				}
			}
			if (this.fatalityDuration > 0f)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM_TOOLTIP, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), Descriptor.DescriptorType.SymptomAidable, false));
			}
			return list;
		}

		// Token: 0x0600E8D8 RID: 59608 RVA: 0x0013BACE File Offset: 0x00139CCE
		protected void AddSicknessComponent(Sickness.SicknessComponent cmp)
		{
			this.components.Add(cmp);
		}

		// Token: 0x0600E8D9 RID: 59609 RVA: 0x004C387C File Offset: 0x004C1A7C
		public T GetSicknessComponent<T>() where T : Sickness.SicknessComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i] is T)
				{
					return this.components[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x0600E8DA RID: 59610 RVA: 0x000C9B47 File Offset: 0x000C7D47
		public virtual List<Descriptor> GetSicknessSourceDescriptors()
		{
			return new List<Descriptor>();
		}

		// Token: 0x0600E8DB RID: 59611 RVA: 0x004C38D4 File Offset: 0x004C1AD4
		public List<Descriptor> GetQualitativeDescriptors()
		{
			List<Descriptor> list = new List<Descriptor>();
			using (List<Sickness.InfectionVector>.Enumerator enumerator = this.infectionVectors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case Sickness.InfectionVector.Contact:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Digestion:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Inhalation:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Exposure:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					}
				}
			}
			list.Add(new Descriptor(Strings.Get(this.descriptiveSymptoms), "", Descriptor.DescriptorType.Information, false));
			return list;
		}

		// Token: 0x0400E476 RID: 58486
		private StringKey name;

		// Token: 0x0400E477 RID: 58487
		private StringKey descriptiveSymptoms;

		// Token: 0x0400E478 RID: 58488
		private float sicknessDuration = 600f;

		// Token: 0x0400E479 RID: 58489
		public float fatalityDuration;

		// Token: 0x0400E47A RID: 58490
		public HashedString id;

		// Token: 0x0400E47B RID: 58491
		public Sickness.SicknessType sicknessType;

		// Token: 0x0400E47C RID: 58492
		public Sickness.Severity severity;

		// Token: 0x0400E47D RID: 58493
		public string recoveryEffect;

		// Token: 0x0400E47E RID: 58494
		public List<Sickness.InfectionVector> infectionVectors;

		// Token: 0x0400E47F RID: 58495
		private List<Sickness.SicknessComponent> components = new List<Sickness.SicknessComponent>();

		// Token: 0x0400E480 RID: 58496
		public Amount amount;

		// Token: 0x0400E481 RID: 58497
		public Attribute amountDeltaAttribute;

		// Token: 0x0400E482 RID: 58498
		public Attribute cureSpeedBase;

		// Token: 0x02003B15 RID: 15125
		public abstract class SicknessComponent
		{
			// Token: 0x0600E8DC RID: 59612
			public abstract object OnInfect(GameObject go, SicknessInstance diseaseInstance);

			// Token: 0x0600E8DD RID: 59613
			public abstract void OnCure(GameObject go, object instance_data);

			// Token: 0x0600E8DE RID: 59614 RVA: 0x000AD332 File Offset: 0x000AB532
			public virtual List<Descriptor> GetSymptoms()
			{
				return null;
			}
		}

		// Token: 0x02003B16 RID: 15126
		public enum InfectionVector
		{
			// Token: 0x0400E484 RID: 58500
			Contact,
			// Token: 0x0400E485 RID: 58501
			Digestion,
			// Token: 0x0400E486 RID: 58502
			Inhalation,
			// Token: 0x0400E487 RID: 58503
			Exposure
		}

		// Token: 0x02003B17 RID: 15127
		public enum SicknessType
		{
			// Token: 0x0400E489 RID: 58505
			Pathogen,
			// Token: 0x0400E48A RID: 58506
			Ailment,
			// Token: 0x0400E48B RID: 58507
			Injury
		}

		// Token: 0x02003B18 RID: 15128
		public enum Severity
		{
			// Token: 0x0400E48D RID: 58509
			Benign,
			// Token: 0x0400E48E RID: 58510
			Minor,
			// Token: 0x0400E48F RID: 58511
			Major,
			// Token: 0x0400E490 RID: 58512
			Critical
		}
	}
}
