using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001EDC RID: 7900
public class ResourceRemainingDisplayScreen : KScreen
{
	// Token: 0x0600A63F RID: 42559 RVA: 0x0010BD38 File Offset: 0x00109F38
	public static void DestroyInstance()
	{
		ResourceRemainingDisplayScreen.instance = null;
	}

	// Token: 0x0600A640 RID: 42560 RVA: 0x0010BD40 File Offset: 0x00109F40
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Activate();
		ResourceRemainingDisplayScreen.instance = this;
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x0600A641 RID: 42561 RVA: 0x0010BD60 File Offset: 0x00109F60
	public void ActivateDisplay(GameObject target)
	{
		this.numberOfPendingConstructions = 0;
		this.dispayPrefab.SetActive(true);
	}

	// Token: 0x0600A642 RID: 42562 RVA: 0x0010BD75 File Offset: 0x00109F75
	public void DeactivateDisplay()
	{
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x0600A643 RID: 42563 RVA: 0x003F22A4 File Offset: 0x003F04A4
	public void SetResources(IList<Tag> _selected_elements, Recipe recipe)
	{
		this.selected_elements.Clear();
		foreach (Tag item in _selected_elements)
		{
			this.selected_elements.Add(item);
		}
		this.currentRecipe = recipe;
		global::Debug.Assert(this.selected_elements.Count == recipe.Ingredients.Count, string.Format("{0} Mismatch number of selected elements {1} and recipe requirements {2}", recipe.Name, this.selected_elements.Count, recipe.Ingredients.Count));
	}

	// Token: 0x0600A644 RID: 42564 RVA: 0x0010BD83 File Offset: 0x00109F83
	public void SetNumberOfPendingConstructions(int number)
	{
		this.numberOfPendingConstructions = number;
	}

	// Token: 0x0600A645 RID: 42565 RVA: 0x003F2350 File Offset: 0x003F0550
	public void Update()
	{
		if (!this.dispayPrefab.activeSelf)
		{
			return;
		}
		if (base.canvas != null)
		{
			if (this.rect == null)
			{
				this.rect = base.GetComponent<RectTransform>();
			}
			this.rect.anchoredPosition = base.WorldToScreen(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		}
		if (this.displayedConstructionCostMultiplier == this.numberOfPendingConstructions)
		{
			this.label.text = "";
			return;
		}
		this.displayedConstructionCostMultiplier = this.numberOfPendingConstructions;
	}

	// Token: 0x0600A646 RID: 42566 RVA: 0x003F23E0 File Offset: 0x003F05E0
	public string GetString()
	{
		string text = "";
		if (this.selected_elements != null && this.currentRecipe != null)
		{
			for (int i = 0; i < this.currentRecipe.Ingredients.Count; i++)
			{
				Tag tag = this.selected_elements[i];
				float num = this.currentRecipe.Ingredients[i].amount * (float)this.numberOfPendingConstructions;
				float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, true);
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				string text2 = tag.ProperName();
				if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(tag) && MaterialSelector.GetValidMaterials(this.currentRecipe.Ingredients[i].tag, false).Count > 1)
				{
					text2 = string.Concat(new string[]
					{
						"<b>",
						UIConstants.ColorPrefixYellow,
						text2,
						UIConstants.ColorSuffix,
						"</b>"
					});
				}
				text = string.Concat(new string[]
				{
					text,
					text2,
					": ",
					GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
					" / ",
					GameUtil.GetFormattedMass(this.currentRecipe.Ingredients[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
				if (i < this.selected_elements.Count - 1)
				{
					text += "\n";
				}
			}
		}
		return text;
	}

	// Token: 0x04008296 RID: 33430
	public static ResourceRemainingDisplayScreen instance;

	// Token: 0x04008297 RID: 33431
	public GameObject dispayPrefab;

	// Token: 0x04008298 RID: 33432
	public LocText label;

	// Token: 0x04008299 RID: 33433
	private Recipe currentRecipe;

	// Token: 0x0400829A RID: 33434
	private List<Tag> selected_elements = new List<Tag>();

	// Token: 0x0400829B RID: 33435
	private int numberOfPendingConstructions;

	// Token: 0x0400829C RID: 33436
	private int displayedConstructionCostMultiplier;

	// Token: 0x0400829D RID: 33437
	private RectTransform rect;
}
