using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x020012B1 RID: 4785
public class EquipmentDef : Def
{
	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x0600625D RID: 25181 RVA: 0x000E0276 File Offset: 0x000DE476
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".NAME");
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x0600625E RID: 25182 RVA: 0x000E029C File Offset: 0x000DE49C
	public string Desc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".DESC");
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x0600625F RID: 25183 RVA: 0x000E02C2 File Offset: 0x000DE4C2
	public string Effect
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".EFFECT");
		}
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06006260 RID: 25184 RVA: 0x000E02E8 File Offset: 0x000DE4E8
	public string GenericName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".GENERICNAME");
		}
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x06006261 RID: 25185 RVA: 0x000E030E File Offset: 0x000DE50E
	public string WornName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_NAME");
		}
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06006262 RID: 25186 RVA: 0x000E0334 File Offset: 0x000DE534
	public string WornDesc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_DESC");
		}
	}

	// Token: 0x040045F8 RID: 17912
	public string Id;

	// Token: 0x040045F9 RID: 17913
	public string Slot;

	// Token: 0x040045FA RID: 17914
	public string FabricatorId;

	// Token: 0x040045FB RID: 17915
	public float FabricationTime;

	// Token: 0x040045FC RID: 17916
	public string RecipeTechUnlock;

	// Token: 0x040045FD RID: 17917
	public SimHashes OutputElement;

	// Token: 0x040045FE RID: 17918
	public Dictionary<string, float> InputElementMassMap;

	// Token: 0x040045FF RID: 17919
	public float Mass;

	// Token: 0x04004600 RID: 17920
	public KAnimFile Anim;

	// Token: 0x04004601 RID: 17921
	public string SnapOn;

	// Token: 0x04004602 RID: 17922
	public string SnapOn1;

	// Token: 0x04004603 RID: 17923
	public KAnimFile BuildOverride;

	// Token: 0x04004604 RID: 17924
	public int BuildOverridePriority;

	// Token: 0x04004605 RID: 17925
	public bool IsBody;

	// Token: 0x04004606 RID: 17926
	public List<AttributeModifier> AttributeModifiers;

	// Token: 0x04004607 RID: 17927
	public string RecipeDescription;

	// Token: 0x04004608 RID: 17928
	public List<Effect> EffectImmunites = new List<Effect>();

	// Token: 0x04004609 RID: 17929
	public Action<Equippable> OnEquipCallBack;

	// Token: 0x0400460A RID: 17930
	public Action<Equippable> OnUnequipCallBack;

	// Token: 0x0400460B RID: 17931
	public EntityTemplates.CollisionShape CollisionShape;

	// Token: 0x0400460C RID: 17932
	public float width;

	// Token: 0x0400460D RID: 17933
	public float height = 0.325f;

	// Token: 0x0400460E RID: 17934
	public Tag[] AdditionalTags;

	// Token: 0x0400460F RID: 17935
	public string wornID;

	// Token: 0x04004610 RID: 17936
	public List<Descriptor> additionalDescriptors = new List<Descriptor>();
}
