public readonly struct ClothingOutfitNameProposal
{
	public enum Result
	{
		None,
		NewOutfit,
		SameOutfit,
		Error_NoInputName,
		Error_NameAlreadyExists,
		Error_SameOutfitReadonly
	}

	public readonly string candidateName;

	public readonly Result result;

	private ClothingOutfitNameProposal(string candidateName, Result result)
	{
		this.candidateName = candidateName;
		this.result = result;
	}

	public static ClothingOutfitNameProposal ForNewOutfit(string candidateName)
	{
		if (string.IsNullOrEmpty(candidateName))
		{
			return Make(Result.Error_NoInputName);
		}
		if (ClothingOutfitTarget.DoesTemplateExist(candidateName))
		{
			return Make(Result.Error_NameAlreadyExists);
		}
		return Make(Result.NewOutfit);
		ClothingOutfitNameProposal Make(Result result)
		{
			return new ClothingOutfitNameProposal(candidateName, result);
		}
	}

	public static ClothingOutfitNameProposal FromExistingOutfit(string candidateName, ClothingOutfitTarget existingOutfit, bool isSameNameAllowed)
	{
		if (string.IsNullOrEmpty(candidateName))
		{
			return Make(Result.Error_NoInputName);
		}
		if (ClothingOutfitTarget.DoesTemplateExist(candidateName))
		{
			if (isSameNameAllowed && candidateName == existingOutfit.ReadName())
			{
				if (existingOutfit.CanWriteName)
				{
					return Make(Result.SameOutfit);
				}
				return Make(Result.Error_SameOutfitReadonly);
			}
			return Make(Result.Error_NameAlreadyExists);
		}
		return Make(Result.NewOutfit);
		ClothingOutfitNameProposal Make(Result result)
		{
			return new ClothingOutfitNameProposal(candidateName, result);
		}
	}
}
