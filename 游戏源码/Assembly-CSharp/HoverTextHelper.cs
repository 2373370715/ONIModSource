using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001ACD RID: 6861
public class HoverTextHelper
{
	// Token: 0x06008FB0 RID: 36784 RVA: 0x000FDEA5 File Offset: 0x000FC0A5
	public static void DestroyStatics()
	{
		HoverTextHelper.cachedElement = null;
		HoverTextHelper.cachedMass = -1f;
	}

	// Token: 0x06008FB1 RID: 36785 RVA: 0x00377A84 File Offset: 0x00375C84
	public static string[] MassStringsReadOnly(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return HoverTextHelper.invalidCellMassStrings;
		}
		Element element = Grid.Element[cell];
		float num = Grid.Mass[cell];
		if (element == HoverTextHelper.cachedElement && num == HoverTextHelper.cachedMass)
		{
			return HoverTextHelper.massStrings;
		}
		HoverTextHelper.cachedElement = element;
		HoverTextHelper.cachedMass = num;
		HoverTextHelper.massStrings[3] = " " + GameUtil.GetBreathableString(element, num);
		if (element.id == SimHashes.Vacuum)
		{
			HoverTextHelper.massStrings[0] = UI.NA;
			HoverTextHelper.massStrings[1] = "";
			HoverTextHelper.massStrings[2] = "";
		}
		else if (element.id == SimHashes.Unobtanium)
		{
			HoverTextHelper.massStrings[0] = UI.NEUTRONIUMMASS;
			HoverTextHelper.massStrings[1] = "";
			HoverTextHelper.massStrings[2] = "";
		}
		else
		{
			HoverTextHelper.massStrings[2] = UI.UNITSUFFIXES.MASS.KILOGRAM;
			if (num < 5f)
			{
				num *= 1000f;
				HoverTextHelper.massStrings[2] = UI.UNITSUFFIXES.MASS.GRAM;
			}
			if (num < 5f)
			{
				num *= 1000f;
				HoverTextHelper.massStrings[2] = UI.UNITSUFFIXES.MASS.MILLIGRAM;
			}
			if (num < 5f)
			{
				num *= 1000f;
				HoverTextHelper.massStrings[2] = UI.UNITSUFFIXES.MASS.MICROGRAM;
				num = Mathf.Floor(num);
			}
			int num2 = Mathf.FloorToInt(num);
			int num3 = Mathf.RoundToInt(10f * (num - (float)num2));
			if (num3 == 10)
			{
				num2++;
				num3 = 0;
			}
			HoverTextHelper.massStrings[0] = num2.ToString();
			HoverTextHelper.massStrings[1] = "." + num3.ToString();
		}
		return HoverTextHelper.massStrings;
	}

	// Token: 0x04006C75 RID: 27765
	private static readonly string[] massStrings = new string[4];

	// Token: 0x04006C76 RID: 27766
	private static readonly string[] invalidCellMassStrings = new string[]
	{
		"",
		"",
		"",
		""
	};

	// Token: 0x04006C77 RID: 27767
	private static float cachedMass = -1f;

	// Token: 0x04006C78 RID: 27768
	private static Element cachedElement;
}
