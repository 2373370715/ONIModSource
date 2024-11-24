using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F66 RID: 8038
public class FewOptionSideScreen : SideScreenContent
{
	// Token: 0x0600A9A8 RID: 43432 RVA: 0x0010E340 File Offset: 0x0010C540
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshOptions();
		}
	}

	// Token: 0x0600A9A9 RID: 43433 RVA: 0x004021E8 File Offset: 0x004003E8
	private void RefreshOptions()
	{
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.targetFewOptions.GetSelectedOption()) ? 1 : 0);
		}
	}

	// Token: 0x0600A9AA RID: 43434 RVA: 0x00402264 File Offset: 0x00400464
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600A9AB RID: 43435 RVA: 0x004022A8 File Offset: 0x004004A8
	private void SpawnRows()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] options = this.targetFewOptions.GetOptions();
		for (int i = 0; i < options.Length; i++)
		{
			FewOptionSideScreen.IFewOptionSideScreen.Option option = options[i];
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(option.labelText);
			component.GetReference<Image>("icon").sprite = option.iconSpriteColorTuple.first;
			component.GetReference<Image>("icon").color = option.iconSpriteColorTuple.second;
			gameObject.GetComponent<ToolTip>().toolTip = option.tooltipText;
			gameObject.GetComponent<MultiToggle>().onClick = delegate()
			{
				this.targetFewOptions.OnOptionSelected(option);
				this.RefreshOptions();
			};
			this.rows.Add(option.tag, gameObject);
		}
		this.RefreshOptions();
	}

	// Token: 0x0600A9AC RID: 43436 RVA: 0x0010E352 File Offset: 0x0010C552
	public override void SetTarget(GameObject target)
	{
		this.ClearRows();
		this.targetFewOptions = target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>();
		this.SpawnRows();
	}

	// Token: 0x0600A9AD RID: 43437 RVA: 0x0010E36C File Offset: 0x0010C56C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>() != null;
	}

	// Token: 0x04008568 RID: 34152
	public GameObject rowPrefab;

	// Token: 0x04008569 RID: 34153
	public RectTransform rowContainer;

	// Token: 0x0400856A RID: 34154
	public Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

	// Token: 0x0400856B RID: 34155
	private FewOptionSideScreen.IFewOptionSideScreen targetFewOptions;

	// Token: 0x02001F67 RID: 8039
	public interface IFewOptionSideScreen
	{
		// Token: 0x0600A9AF RID: 43439
		FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions();

		// Token: 0x0600A9B0 RID: 43440
		void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option);

		// Token: 0x0600A9B1 RID: 43441
		Tag GetSelectedOption();

		// Token: 0x02001F68 RID: 8040
		public struct Option
		{
			// Token: 0x0600A9B2 RID: 43442 RVA: 0x0010E38A File Offset: 0x0010C58A
			public Option(Tag tag, string labelText, global::Tuple<Sprite, Color> iconSpriteColorTuple, string tooltipText = "")
			{
				this.tag = tag;
				this.labelText = labelText;
				this.iconSpriteColorTuple = iconSpriteColorTuple;
				this.tooltipText = tooltipText;
			}

			// Token: 0x0400856C RID: 34156
			public Tag tag;

			// Token: 0x0400856D RID: 34157
			public string labelText;

			// Token: 0x0400856E RID: 34158
			public string tooltipText;

			// Token: 0x0400856F RID: 34159
			public global::Tuple<Sprite, Color> iconSpriteColorTuple;
		}
	}
}
