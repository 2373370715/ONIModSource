using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Klei;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02001499 RID: 5273
public class ElementLoader
{
	// Token: 0x06006D4E RID: 27982 RVA: 0x002EB9A4 File Offset: 0x002E9BA4
	public static float GetMinMeltingPointAmongElements(IList<Tag> elements)
	{
		float num = float.MaxValue;
		for (int i = 0; i < elements.Count; i++)
		{
			Element element = ElementLoader.GetElement(elements[i]);
			if (element != null)
			{
				num = Mathf.Min(num, element.highTemp);
			}
		}
		return num;
	}

	// Token: 0x06006D4F RID: 27983 RVA: 0x002EB9E8 File Offset: 0x002E9BE8
	public static List<ElementLoader.ElementEntry> CollectElementsFromYAML()
	{
		List<ElementLoader.ElementEntry> list = new List<ElementLoader.ElementEntry>();
		ListPool<FileHandle, ElementLoader>.PooledList pooledList = ListPool<FileHandle, ElementLoader>.Allocate();
		FileSystem.GetFiles(FileSystem.Normalize(ElementLoader.path), "*.yaml", pooledList);
		ListPool<YamlIO.Error, ElementLoader>.PooledList errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();
		YamlIO.ErrorHandler <>9__0;
		foreach (FileHandle fileHandle in pooledList)
		{
			if (!Path.GetFileName(fileHandle.full_path).StartsWith("."))
			{
				string full_path = fileHandle.full_path;
				YamlIO.ErrorHandler handle_error;
				if ((handle_error = <>9__0) == null)
				{
					handle_error = (<>9__0 = delegate(YamlIO.Error error, bool force_log_as_warning)
					{
						errors.Add(error);
					});
				}
				ElementLoader.ElementEntryCollection elementEntryCollection = YamlIO.LoadFile<ElementLoader.ElementEntryCollection>(full_path, handle_error, null);
				if (elementEntryCollection != null)
				{
					list.AddRange(elementEntryCollection.elements);
				}
			}
		}
		pooledList.Recycle();
		if (Global.Instance != null && Global.Instance.modManager != null)
		{
			Global.Instance.modManager.HandleErrors(errors);
		}
		errors.Recycle();
		return list;
	}

	// Token: 0x06006D50 RID: 27984 RVA: 0x002EBAFC File Offset: 0x002E9CFC
	public static void Load(ref Hashtable substanceList, Dictionary<string, SubstanceTable> substanceTablesByDlc)
	{
		ElementLoader.elements = new List<Element>();
		ElementLoader.elementTable = new Dictionary<int, Element>();
		ElementLoader.elementTagTable = new Dictionary<Tag, Element>();
		foreach (ElementLoader.ElementEntry elementEntry in ElementLoader.CollectElementsFromYAML())
		{
			int num = Hash.SDBMLower(elementEntry.elementId);
			if (!ElementLoader.elementTable.ContainsKey(num) && substanceTablesByDlc.ContainsKey(elementEntry.dlcId))
			{
				Element element = new Element();
				element.id = (SimHashes)num;
				element.name = Strings.Get(elementEntry.localizationID);
				element.nameUpperCase = element.name.ToUpper();
				element.description = Strings.Get(elementEntry.description);
				element.tag = TagManager.Create(elementEntry.elementId, element.name);
				ElementLoader.CopyEntryToElement(elementEntry, element);
				ElementLoader.elements.Add(element);
				ElementLoader.elementTable[num] = element;
				ElementLoader.elementTagTable[element.tag] = element;
				if (!ElementLoader.ManifestSubstanceForElement(element, ref substanceList, substanceTablesByDlc[elementEntry.dlcId]))
				{
					global::Debug.LogWarning("Missing substance for element: " + element.id.ToString());
				}
			}
		}
		ElementLoader.FinaliseElementsTable(ref substanceList);
		WorldGen.SetupDefaultElements();
	}

	// Token: 0x06006D51 RID: 27985 RVA: 0x002EBC74 File Offset: 0x002E9E74
	private static void CopyEntryToElement(ElementLoader.ElementEntry entry, Element elem)
	{
		Hash.SDBMLower(entry.elementId);
		elem.tag = TagManager.Create(entry.elementId.ToString());
		elem.specificHeatCapacity = entry.specificHeatCapacity;
		elem.thermalConductivity = entry.thermalConductivity;
		elem.molarMass = entry.molarMass;
		elem.strength = entry.strength;
		elem.disabled = entry.isDisabled;
		elem.dlcId = entry.dlcId;
		elem.flow = entry.flow;
		elem.maxMass = entry.maxMass;
		elem.maxCompression = entry.liquidCompression;
		elem.viscosity = entry.speed;
		elem.minHorizontalFlow = entry.minHorizontalFlow;
		elem.minVerticalFlow = entry.minVerticalFlow;
		elem.solidSurfaceAreaMultiplier = entry.solidSurfaceAreaMultiplier;
		elem.liquidSurfaceAreaMultiplier = entry.liquidSurfaceAreaMultiplier;
		elem.gasSurfaceAreaMultiplier = entry.gasSurfaceAreaMultiplier;
		elem.state = entry.state;
		elem.hardness = entry.hardness;
		elem.lowTemp = entry.lowTemp;
		elem.lowTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionTarget);
		elem.highTemp = entry.highTemp;
		elem.highTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.highTempTransitionTarget);
		elem.highTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.highTempTransitionOreId);
		elem.highTempTransitionOreMassConversion = entry.highTempTransitionOreMassConversion;
		elem.lowTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionOreId);
		elem.lowTempTransitionOreMassConversion = entry.lowTempTransitionOreMassConversion;
		elem.sublimateId = (SimHashes)Hash.SDBMLower(entry.sublimateId);
		elem.convertId = (SimHashes)Hash.SDBMLower(entry.convertId);
		elem.sublimateFX = (SpawnFXHashes)Hash.SDBMLower(entry.sublimateFx);
		elem.sublimateRate = entry.sublimateRate;
		elem.sublimateEfficiency = entry.sublimateEfficiency;
		elem.sublimateProbability = entry.sublimateProbability;
		elem.offGasPercentage = entry.offGasPercentage;
		elem.lightAbsorptionFactor = entry.lightAbsorptionFactor;
		elem.radiationAbsorptionFactor = entry.radiationAbsorptionFactor;
		elem.radiationPer1000Mass = entry.radiationPer1000Mass;
		elem.toxicity = entry.toxicity;
		elem.elementComposition = entry.composition;
		Tag phaseTag = TagManager.Create(entry.state.ToString());
		elem.materialCategory = ElementLoader.CreateMaterialCategoryTag(elem.id, phaseTag, entry.materialCategory);
		elem.oreTags = ElementLoader.CreateOreTags(elem.materialCategory, phaseTag, entry.tags);
		elem.buildMenuSort = entry.buildMenuSort;
		Sim.PhysicsData defaultValues = default(Sim.PhysicsData);
		defaultValues.temperature = entry.defaultTemperature;
		defaultValues.mass = entry.defaultMass;
		defaultValues.pressure = entry.defaultPressure;
		switch (entry.state)
		{
		case Element.State.Gas:
			GameTags.GasElements.Add(elem.tag);
			defaultValues.mass = 1f;
			elem.maxMass = 1.8f;
			break;
		case Element.State.Liquid:
			GameTags.LiquidElements.Add(elem.tag);
			break;
		case Element.State.Solid:
			GameTags.SolidElements.Add(elem.tag);
			break;
		}
		elem.defaultValues = defaultValues;
	}

	// Token: 0x06006D52 RID: 27986 RVA: 0x002EBF78 File Offset: 0x002EA178
	private static bool ManifestSubstanceForElement(Element elem, ref Hashtable substanceList, SubstanceTable substanceTable)
	{
		elem.substance = null;
		if (substanceList.ContainsKey(elem.id))
		{
			elem.substance = (substanceList[elem.id] as Substance);
			return false;
		}
		if (substanceTable != null)
		{
			elem.substance = substanceTable.GetSubstance(elem.id);
		}
		if (elem.substance == null)
		{
			elem.substance = new Substance();
			substanceTable.GetList().Add(elem.substance);
		}
		elem.substance.elementID = elem.id;
		elem.substance.renderedByWorld = elem.IsSolid;
		elem.substance.idx = substanceList.Count;
		if (elem.substance.uiColour == ElementLoader.noColour)
		{
			int count = ElementLoader.elements.Count;
			int idx = elem.substance.idx;
			elem.substance.uiColour = Color.HSVToRGB((float)idx / (float)count, 1f, 1f);
		}
		string name = UI.StripLinkFormatting(elem.name);
		elem.substance.name = name;
		elem.substance.nameTag = elem.tag;
		elem.substance.audioConfig = ElementsAudio.Instance.GetConfigForElement(elem.id);
		substanceList.Add(elem.id, elem.substance);
		return true;
	}

	// Token: 0x06006D53 RID: 27987 RVA: 0x000E7B77 File Offset: 0x000E5D77
	public static Element FindElementByName(string name)
	{
		return ElementLoader.FindElementByHash((SimHashes)Hash.SDBMLower(name));
	}

	// Token: 0x06006D54 RID: 27988 RVA: 0x000E7B84 File Offset: 0x000E5D84
	public static Element FindElementByTag(Tag tag)
	{
		return ElementLoader.GetElement(tag);
	}

	// Token: 0x06006D55 RID: 27989 RVA: 0x002EC0E8 File Offset: 0x002EA2E8
	public static Element FindElementByHash(SimHashes hash)
	{
		Element result = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out result);
		return result;
	}

	// Token: 0x06006D56 RID: 27990 RVA: 0x002EC108 File Offset: 0x002EA308
	public static ushort GetElementIndex(SimHashes hash)
	{
		Element element = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out element);
		if (element != null)
		{
			return element.idx;
		}
		return ushort.MaxValue;
	}

	// Token: 0x06006D57 RID: 27991 RVA: 0x002EC134 File Offset: 0x002EA334
	public static Element GetElement(Tag tag)
	{
		Element result;
		ElementLoader.elementTagTable.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06006D58 RID: 27992 RVA: 0x002EC150 File Offset: 0x002EA350
	public static SimHashes GetElementID(Tag tag)
	{
		Element element;
		ElementLoader.elementTagTable.TryGetValue(tag, out element);
		if (element != null)
		{
			return element.id;
		}
		return SimHashes.Vacuum;
	}

	// Token: 0x06006D59 RID: 27993 RVA: 0x002EC17C File Offset: 0x002EA37C
	private static SimHashes GetID(int column, int row, string[,] grid, SimHashes defaultValue = SimHashes.Vacuum)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find element at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return defaultValue;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return defaultValue;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SimHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find element {0}: {1}", text, ex.ToString()));
			return defaultValue;
		}
		return (SimHashes)obj;
	}

	// Token: 0x06006D5A RID: 27994 RVA: 0x002EC248 File Offset: 0x002EA448
	private static SpawnFXHashes GetSpawnFX(int column, int row, string[,] grid)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find SpawnFXHashes at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return SpawnFXHashes.None;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return SpawnFXHashes.None;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SpawnFXHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find FX {0}: {1}", text, ex.ToString()));
			return SpawnFXHashes.None;
		}
		return (SpawnFXHashes)obj;
	}

	// Token: 0x06006D5B RID: 27995 RVA: 0x002EC314 File Offset: 0x002EA514
	private static Tag CreateMaterialCategoryTag(SimHashes element_id, Tag phaseTag, string materialCategoryField)
	{
		if (!string.IsNullOrEmpty(materialCategoryField))
		{
			Tag tag = TagManager.Create(materialCategoryField);
			if (!GameTags.MaterialCategories.Contains(tag) && !GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				global::Debug.LogWarningFormat("Element {0} has category {1}, but that isn't in GameTags.MaterialCategores!", new object[]
				{
					element_id,
					materialCategoryField
				});
			}
			return tag;
		}
		return phaseTag;
	}

	// Token: 0x06006D5C RID: 27996 RVA: 0x002EC36C File Offset: 0x002EA56C
	private static Tag[] CreateOreTags(Tag materialCategory, Tag phaseTag, string[] ore_tags_split)
	{
		List<Tag> list = new List<Tag>();
		if (ore_tags_split != null)
		{
			foreach (string text in ore_tags_split)
			{
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(TagManager.Create(text));
				}
			}
		}
		list.Add(phaseTag);
		if (materialCategory.IsValid && !list.Contains(materialCategory))
		{
			list.Add(materialCategory);
		}
		return list.ToArray();
	}

	// Token: 0x06006D5D RID: 27997 RVA: 0x002EC3D0 File Offset: 0x002EA5D0
	private static void FinaliseElementsTable(ref Hashtable substanceList)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element != null)
			{
				if (element.substance == null)
				{
					global::Debug.LogWarning("Skipping finalise for missing element: " + element.id.ToString());
				}
				else
				{
					global::Debug.Assert(element.substance.nameTag.IsValid);
					if (element.thermalConductivity == 0f)
					{
						element.state |= Element.State.TemperatureInsulated;
					}
					if (element.strength == 0f)
					{
						element.state |= Element.State.Unbreakable;
					}
					if (element.IsSolid)
					{
						Element element2 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element2 != null)
						{
							element.highTempTransition = element2;
						}
					}
					else if (element.IsLiquid)
					{
						Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element3 != null)
						{
							element.highTempTransition = element3;
						}
						Element element4 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element4 != null)
						{
							element.lowTempTransition = element4;
						}
					}
					else if (element.IsGas)
					{
						Element element5 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element5 != null)
						{
							element.lowTempTransition = element5;
						}
					}
				}
			}
		}
		ElementLoader.elements = (from e in ElementLoader.elements
		orderby (int)(e.state & Element.State.Solid) descending, e.id
		select e).ToList<Element>();
		for (int i = 0; i < ElementLoader.elements.Count; i++)
		{
			if (ElementLoader.elements[i].substance != null)
			{
				ElementLoader.elements[i].substance.idx = i;
			}
			ElementLoader.elements[i].idx = (ushort)i;
		}
	}

	// Token: 0x06006D5E RID: 27998 RVA: 0x002EC5D8 File Offset: 0x002EA7D8
	private static void ValidateElements()
	{
		global::Debug.Log("------ Start Validating Elements ------");
		foreach (Element element in ElementLoader.elements)
		{
			string text = string.Format("{0} ({1})", element.tag.ProperNameStripLink(), element.state);
			if (element.IsLiquid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.sublimateRate == 0f, text + ": Liquids don't use sublimateRate, use offGasPercentage instead.");
				global::Debug.Assert(element.offGasPercentage > 0f, text + ": Missing offGasPercentage");
			}
			if (element.IsSolid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.offGasPercentage == 0f, text + ": Solids don't use offGasPercentage, use sublimateRate instead.");
				global::Debug.Assert(element.sublimateRate > 0f, text + ": Missing sublimationRate");
				global::Debug.Assert(element.sublimateRate * element.sublimateEfficiency > 0.001f, text + ": Sublimation rate and efficiency will result in gas that will be obliterated because its less than 1g. Increase these values and use sublimateProbability if you want a low amount of sublimation");
			}
			if (element.highTempTransition != null && element.highTempTransition.lowTempTransition == element)
			{
				global::Debug.Assert(element.highTemp >= element.highTempTransition.lowTemp, text + ": highTemp is higher than transition element's (" + element.highTempTransition.tag.ProperNameStripLink() + ") lowTemp");
			}
			global::Debug.Assert(element.defaultValues.mass <= element.maxMass, text + ": Default mass should be less than max mass");
			if (false)
			{
				if (element.IsSolid && element.highTempTransition != null && element.highTempTransition.IsLiquid && element.defaultValues.mass > element.highTempTransition.maxMass)
				{
					global::Debug.LogWarning(string.Format("{0} defaultMass {1} > {2}: maxMass {3}", new object[]
					{
						text,
						element.defaultValues.mass,
						element.highTempTransition.tag.ProperNameStripLink(),
						element.highTempTransition.maxMass
					}));
				}
				if (element.defaultValues.mass < element.maxMass && element.IsLiquid)
				{
					global::Debug.LogWarning(string.Format("{0} has defaultMass: {1} and maxMass {2}", element.tag.ProperNameStripLink(), element.defaultValues.mass, element.maxMass));
				}
			}
		}
		global::Debug.Log("------ End Validating Elements ------");
	}

	// Token: 0x040051FC RID: 20988
	public static List<Element> elements;

	// Token: 0x040051FD RID: 20989
	public static Dictionary<int, Element> elementTable;

	// Token: 0x040051FE RID: 20990
	public static Dictionary<Tag, Element> elementTagTable;

	// Token: 0x040051FF RID: 20991
	private static string path = Application.streamingAssetsPath + "/elements/";

	// Token: 0x04005200 RID: 20992
	private static readonly Color noColour = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0200149A RID: 5274
	public class ElementEntryCollection
	{
		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06006D61 RID: 28001 RVA: 0x000E7BC0 File Offset: 0x000E5DC0
		// (set) Token: 0x06006D62 RID: 28002 RVA: 0x000E7BC8 File Offset: 0x000E5DC8
		public ElementLoader.ElementEntry[] elements { get; set; }
	}

	// Token: 0x0200149B RID: 5275
	public class ElementComposition
	{
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06006D65 RID: 28005 RVA: 0x000E7BD1 File Offset: 0x000E5DD1
		// (set) Token: 0x06006D66 RID: 28006 RVA: 0x000E7BD9 File Offset: 0x000E5DD9
		public string elementID { get; set; }

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06006D67 RID: 28007 RVA: 0x000E7BE2 File Offset: 0x000E5DE2
		// (set) Token: 0x06006D68 RID: 28008 RVA: 0x000E7BEA File Offset: 0x000E5DEA
		public float percentage { get; set; }
	}

	// Token: 0x0200149C RID: 5276
	public class ElementEntry
	{
		// Token: 0x06006D69 RID: 28009 RVA: 0x000E7BF3 File Offset: 0x000E5DF3
		public ElementEntry()
		{
			this.lowTemp = 0f;
			this.highTemp = 10000f;
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06006D6A RID: 28010 RVA: 0x000E7C11 File Offset: 0x000E5E11
		// (set) Token: 0x06006D6B RID: 28011 RVA: 0x000E7C19 File Offset: 0x000E5E19
		public string elementId { get; set; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06006D6C RID: 28012 RVA: 0x000E7C22 File Offset: 0x000E5E22
		// (set) Token: 0x06006D6D RID: 28013 RVA: 0x000E7C2A File Offset: 0x000E5E2A
		public float specificHeatCapacity { get; set; }

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06006D6E RID: 28014 RVA: 0x000E7C33 File Offset: 0x000E5E33
		// (set) Token: 0x06006D6F RID: 28015 RVA: 0x000E7C3B File Offset: 0x000E5E3B
		public float thermalConductivity { get; set; }

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06006D70 RID: 28016 RVA: 0x000E7C44 File Offset: 0x000E5E44
		// (set) Token: 0x06006D71 RID: 28017 RVA: 0x000E7C4C File Offset: 0x000E5E4C
		public float solidSurfaceAreaMultiplier { get; set; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06006D72 RID: 28018 RVA: 0x000E7C55 File Offset: 0x000E5E55
		// (set) Token: 0x06006D73 RID: 28019 RVA: 0x000E7C5D File Offset: 0x000E5E5D
		public float liquidSurfaceAreaMultiplier { get; set; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06006D74 RID: 28020 RVA: 0x000E7C66 File Offset: 0x000E5E66
		// (set) Token: 0x06006D75 RID: 28021 RVA: 0x000E7C6E File Offset: 0x000E5E6E
		public float gasSurfaceAreaMultiplier { get; set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06006D76 RID: 28022 RVA: 0x000E7C77 File Offset: 0x000E5E77
		// (set) Token: 0x06006D77 RID: 28023 RVA: 0x000E7C7F File Offset: 0x000E5E7F
		public float defaultMass { get; set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06006D78 RID: 28024 RVA: 0x000E7C88 File Offset: 0x000E5E88
		// (set) Token: 0x06006D79 RID: 28025 RVA: 0x000E7C90 File Offset: 0x000E5E90
		public float defaultTemperature { get; set; }

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06006D7A RID: 28026 RVA: 0x000E7C99 File Offset: 0x000E5E99
		// (set) Token: 0x06006D7B RID: 28027 RVA: 0x000E7CA1 File Offset: 0x000E5EA1
		public float defaultPressure { get; set; }

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06006D7C RID: 28028 RVA: 0x000E7CAA File Offset: 0x000E5EAA
		// (set) Token: 0x06006D7D RID: 28029 RVA: 0x000E7CB2 File Offset: 0x000E5EB2
		public float molarMass { get; set; }

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06006D7E RID: 28030 RVA: 0x000E7CBB File Offset: 0x000E5EBB
		// (set) Token: 0x06006D7F RID: 28031 RVA: 0x000E7CC3 File Offset: 0x000E5EC3
		public float lightAbsorptionFactor { get; set; }

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06006D80 RID: 28032 RVA: 0x000E7CCC File Offset: 0x000E5ECC
		// (set) Token: 0x06006D81 RID: 28033 RVA: 0x000E7CD4 File Offset: 0x000E5ED4
		public float radiationAbsorptionFactor { get; set; }

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06006D82 RID: 28034 RVA: 0x000E7CDD File Offset: 0x000E5EDD
		// (set) Token: 0x06006D83 RID: 28035 RVA: 0x000E7CE5 File Offset: 0x000E5EE5
		public float radiationPer1000Mass { get; set; }

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06006D84 RID: 28036 RVA: 0x000E7CEE File Offset: 0x000E5EEE
		// (set) Token: 0x06006D85 RID: 28037 RVA: 0x000E7CF6 File Offset: 0x000E5EF6
		public string lowTempTransitionTarget { get; set; }

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06006D86 RID: 28038 RVA: 0x000E7CFF File Offset: 0x000E5EFF
		// (set) Token: 0x06006D87 RID: 28039 RVA: 0x000E7D07 File Offset: 0x000E5F07
		public float lowTemp { get; set; }

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06006D88 RID: 28040 RVA: 0x000E7D10 File Offset: 0x000E5F10
		// (set) Token: 0x06006D89 RID: 28041 RVA: 0x000E7D18 File Offset: 0x000E5F18
		public string highTempTransitionTarget { get; set; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06006D8A RID: 28042 RVA: 0x000E7D21 File Offset: 0x000E5F21
		// (set) Token: 0x06006D8B RID: 28043 RVA: 0x000E7D29 File Offset: 0x000E5F29
		public float highTemp { get; set; }

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06006D8C RID: 28044 RVA: 0x000E7D32 File Offset: 0x000E5F32
		// (set) Token: 0x06006D8D RID: 28045 RVA: 0x000E7D3A File Offset: 0x000E5F3A
		public string lowTempTransitionOreId { get; set; }

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06006D8E RID: 28046 RVA: 0x000E7D43 File Offset: 0x000E5F43
		// (set) Token: 0x06006D8F RID: 28047 RVA: 0x000E7D4B File Offset: 0x000E5F4B
		public float lowTempTransitionOreMassConversion { get; set; }

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06006D90 RID: 28048 RVA: 0x000E7D54 File Offset: 0x000E5F54
		// (set) Token: 0x06006D91 RID: 28049 RVA: 0x000E7D5C File Offset: 0x000E5F5C
		public string highTempTransitionOreId { get; set; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06006D92 RID: 28050 RVA: 0x000E7D65 File Offset: 0x000E5F65
		// (set) Token: 0x06006D93 RID: 28051 RVA: 0x000E7D6D File Offset: 0x000E5F6D
		public float highTempTransitionOreMassConversion { get; set; }

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06006D94 RID: 28052 RVA: 0x000E7D76 File Offset: 0x000E5F76
		// (set) Token: 0x06006D95 RID: 28053 RVA: 0x000E7D7E File Offset: 0x000E5F7E
		public string sublimateId { get; set; }

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06006D96 RID: 28054 RVA: 0x000E7D87 File Offset: 0x000E5F87
		// (set) Token: 0x06006D97 RID: 28055 RVA: 0x000E7D8F File Offset: 0x000E5F8F
		public string sublimateFx { get; set; }

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06006D98 RID: 28056 RVA: 0x000E7D98 File Offset: 0x000E5F98
		// (set) Token: 0x06006D99 RID: 28057 RVA: 0x000E7DA0 File Offset: 0x000E5FA0
		public float sublimateRate { get; set; }

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06006D9A RID: 28058 RVA: 0x000E7DA9 File Offset: 0x000E5FA9
		// (set) Token: 0x06006D9B RID: 28059 RVA: 0x000E7DB1 File Offset: 0x000E5FB1
		public float sublimateEfficiency { get; set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06006D9C RID: 28060 RVA: 0x000E7DBA File Offset: 0x000E5FBA
		// (set) Token: 0x06006D9D RID: 28061 RVA: 0x000E7DC2 File Offset: 0x000E5FC2
		public float sublimateProbability { get; set; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06006D9E RID: 28062 RVA: 0x000E7DCB File Offset: 0x000E5FCB
		// (set) Token: 0x06006D9F RID: 28063 RVA: 0x000E7DD3 File Offset: 0x000E5FD3
		public float offGasPercentage { get; set; }

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06006DA0 RID: 28064 RVA: 0x000E7DDC File Offset: 0x000E5FDC
		// (set) Token: 0x06006DA1 RID: 28065 RVA: 0x000E7DE4 File Offset: 0x000E5FE4
		public string materialCategory { get; set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06006DA2 RID: 28066 RVA: 0x000E7DED File Offset: 0x000E5FED
		// (set) Token: 0x06006DA3 RID: 28067 RVA: 0x000E7DF5 File Offset: 0x000E5FF5
		public string[] tags { get; set; }

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06006DA4 RID: 28068 RVA: 0x000E7DFE File Offset: 0x000E5FFE
		// (set) Token: 0x06006DA5 RID: 28069 RVA: 0x000E7E06 File Offset: 0x000E6006
		public bool isDisabled { get; set; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06006DA6 RID: 28070 RVA: 0x000E7E0F File Offset: 0x000E600F
		// (set) Token: 0x06006DA7 RID: 28071 RVA: 0x000E7E17 File Offset: 0x000E6017
		public float strength { get; set; }

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06006DA8 RID: 28072 RVA: 0x000E7E20 File Offset: 0x000E6020
		// (set) Token: 0x06006DA9 RID: 28073 RVA: 0x000E7E28 File Offset: 0x000E6028
		public float maxMass { get; set; }

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06006DAA RID: 28074 RVA: 0x000E7E31 File Offset: 0x000E6031
		// (set) Token: 0x06006DAB RID: 28075 RVA: 0x000E7E39 File Offset: 0x000E6039
		public byte hardness { get; set; }

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06006DAC RID: 28076 RVA: 0x000E7E42 File Offset: 0x000E6042
		// (set) Token: 0x06006DAD RID: 28077 RVA: 0x000E7E4A File Offset: 0x000E604A
		public float toxicity { get; set; }

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06006DAE RID: 28078 RVA: 0x000E7E53 File Offset: 0x000E6053
		// (set) Token: 0x06006DAF RID: 28079 RVA: 0x000E7E5B File Offset: 0x000E605B
		public float liquidCompression { get; set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06006DB0 RID: 28080 RVA: 0x000E7E64 File Offset: 0x000E6064
		// (set) Token: 0x06006DB1 RID: 28081 RVA: 0x000E7E6C File Offset: 0x000E606C
		public float speed { get; set; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06006DB2 RID: 28082 RVA: 0x000E7E75 File Offset: 0x000E6075
		// (set) Token: 0x06006DB3 RID: 28083 RVA: 0x000E7E7D File Offset: 0x000E607D
		public float minHorizontalFlow { get; set; }

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06006DB4 RID: 28084 RVA: 0x000E7E86 File Offset: 0x000E6086
		// (set) Token: 0x06006DB5 RID: 28085 RVA: 0x000E7E8E File Offset: 0x000E608E
		public float minVerticalFlow { get; set; }

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06006DB6 RID: 28086 RVA: 0x000E7E97 File Offset: 0x000E6097
		// (set) Token: 0x06006DB7 RID: 28087 RVA: 0x000E7E9F File Offset: 0x000E609F
		public string convertId { get; set; }

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06006DB8 RID: 28088 RVA: 0x000E7EA8 File Offset: 0x000E60A8
		// (set) Token: 0x06006DB9 RID: 28089 RVA: 0x000E7EB0 File Offset: 0x000E60B0
		public float flow { get; set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06006DBA RID: 28090 RVA: 0x000E7EB9 File Offset: 0x000E60B9
		// (set) Token: 0x06006DBB RID: 28091 RVA: 0x000E7EC1 File Offset: 0x000E60C1
		public int buildMenuSort { get; set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06006DBC RID: 28092 RVA: 0x000E7ECA File Offset: 0x000E60CA
		// (set) Token: 0x06006DBD RID: 28093 RVA: 0x000E7ED2 File Offset: 0x000E60D2
		public Element.State state { get; set; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06006DBE RID: 28094 RVA: 0x000E7EDB File Offset: 0x000E60DB
		// (set) Token: 0x06006DBF RID: 28095 RVA: 0x000E7EE3 File Offset: 0x000E60E3
		public string localizationID { get; set; }

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06006DC0 RID: 28096 RVA: 0x000E7EEC File Offset: 0x000E60EC
		// (set) Token: 0x06006DC1 RID: 28097 RVA: 0x000E7EF4 File Offset: 0x000E60F4
		public string dlcId { get; set; }

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06006DC2 RID: 28098 RVA: 0x000E7EFD File Offset: 0x000E60FD
		// (set) Token: 0x06006DC3 RID: 28099 RVA: 0x000E7F05 File Offset: 0x000E6105
		public ElementLoader.ElementComposition[] composition { get; set; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06006DC4 RID: 28100 RVA: 0x000E7F0E File Offset: 0x000E610E
		// (set) Token: 0x06006DC5 RID: 28101 RVA: 0x000E7F39 File Offset: 0x000E6139
		public string description
		{
			get
			{
				return this.description_backing ?? ("STRINGS.ELEMENTS." + this.elementId.ToString().ToUpper() + ".DESC");
			}
			set
			{
				this.description_backing = value;
			}
		}

		// Token: 0x04005231 RID: 21041
		private string description_backing;
	}
}
