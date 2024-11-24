using System;
using System.Runtime.CompilerServices;

// Token: 0x0200107E RID: 4222
public readonly struct ClothingOutfitNameProposal
{
	// Token: 0x0600564F RID: 22095 RVA: 0x000D8590 File Offset: 0x000D6790
	private ClothingOutfitNameProposal(string candidateName, ClothingOutfitNameProposal.Result result)
	{
		this.candidateName = candidateName;
		this.result = result;
	}

	// Token: 0x06005650 RID: 22096 RVA: 0x0028268C File Offset: 0x0028088C
	public static ClothingOutfitNameProposal ForNewOutfit(string candidateName)
	{
		ClothingOutfitNameProposal.<>c__DisplayClass3_0 CS$<>8__locals1;
		CS$<>8__locals1.candidateName = candidateName;
		if (string.IsNullOrEmpty(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.Error_NoInputName, ref CS$<>8__locals1);
		}
		if (ClothingOutfitTarget.DoesTemplateExist(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists, ref CS$<>8__locals1);
		}
		return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.NewOutfit, ref CS$<>8__locals1);
	}

	// Token: 0x06005651 RID: 22097 RVA: 0x002826D8 File Offset: 0x002808D8
	public static ClothingOutfitNameProposal FromExistingOutfit(string candidateName, ClothingOutfitTarget existingOutfit, bool isSameNameAllowed)
	{
		ClothingOutfitNameProposal.<>c__DisplayClass4_0 CS$<>8__locals1;
		CS$<>8__locals1.candidateName = candidateName;
		if (string.IsNullOrEmpty(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_NoInputName, ref CS$<>8__locals1);
		}
		if (!ClothingOutfitTarget.DoesTemplateExist(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.NewOutfit, ref CS$<>8__locals1);
		}
		if (!isSameNameAllowed || !(CS$<>8__locals1.candidateName == existingOutfit.ReadName()))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists, ref CS$<>8__locals1);
		}
		if (existingOutfit.CanWriteName)
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.SameOutfit, ref CS$<>8__locals1);
		}
		return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly, ref CS$<>8__locals1);
	}

	// Token: 0x06005652 RID: 22098 RVA: 0x000D85A0 File Offset: 0x000D67A0
	[CompilerGenerated]
	internal static ClothingOutfitNameProposal <ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result result, ref ClothingOutfitNameProposal.<>c__DisplayClass3_0 A_1)
	{
		return new ClothingOutfitNameProposal(A_1.candidateName, result);
	}

	// Token: 0x06005653 RID: 22099 RVA: 0x000D85AE File Offset: 0x000D67AE
	[CompilerGenerated]
	internal static ClothingOutfitNameProposal <FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result result, ref ClothingOutfitNameProposal.<>c__DisplayClass4_0 A_1)
	{
		return new ClothingOutfitNameProposal(A_1.candidateName, result);
	}

	// Token: 0x04003C90 RID: 15504
	public readonly string candidateName;

	// Token: 0x04003C91 RID: 15505
	public readonly ClothingOutfitNameProposal.Result result;

	// Token: 0x0200107F RID: 4223
	public enum Result
	{
		// Token: 0x04003C93 RID: 15507
		None,
		// Token: 0x04003C94 RID: 15508
		NewOutfit,
		// Token: 0x04003C95 RID: 15509
		SameOutfit,
		// Token: 0x04003C96 RID: 15510
		Error_NoInputName,
		// Token: 0x04003C97 RID: 15511
		Error_NameAlreadyExists,
		// Token: 0x04003C98 RID: 15512
		Error_SameOutfitReadonly
	}
}
