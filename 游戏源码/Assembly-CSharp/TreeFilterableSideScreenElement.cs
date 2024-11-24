using System;
using UnityEngine;

// Token: 0x02001FEC RID: 8172
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenElement")]
public class TreeFilterableSideScreenElement : KMonoBehaviour
{
	// Token: 0x0600AD75 RID: 44405 RVA: 0x00110E2E File Offset: 0x0010F02E
	public Tag GetElementTag()
	{
		return this.elementTag;
	}

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x0600AD76 RID: 44406 RVA: 0x00110E36 File Offset: 0x0010F036
	public bool IsSelected
	{
		get
		{
			return this.checkBox.CurrentState == 1;
		}
	}

	// Token: 0x0600AD77 RID: 44407 RVA: 0x00110E46 File Offset: 0x0010F046
	public MultiToggle GetCheckboxToggle()
	{
		return this.checkBox;
	}

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x0600AD78 RID: 44408 RVA: 0x00110E4E File Offset: 0x0010F04E
	// (set) Token: 0x0600AD79 RID: 44409 RVA: 0x00110E56 File Offset: 0x0010F056
	public TreeFilterableSideScreen Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	// Token: 0x0600AD7A RID: 44410 RVA: 0x00110E5F File Offset: 0x0010F05F
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.checkBoxImg = this.checkBox.gameObject.GetComponentInChildrenOnly<KImage>();
		this.checkBox.onClick = new System.Action(this.CheckBoxClicked);
		this.initialized = true;
	}

	// Token: 0x0600AD7B RID: 44411 RVA: 0x00110E9E File Offset: 0x0010F09E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
	}

	// Token: 0x0600AD7C RID: 44412 RVA: 0x004122DC File Offset: 0x004104DC
	public Sprite GetStorageObjectSprite(Tag t)
	{
		Sprite result = null;
		GameObject prefab = Assets.GetPrefab(t);
		if (prefab != null)
		{
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x0600AD7D RID: 44413 RVA: 0x00412328 File Offset: 0x00410528
	public void SetSprite(Tag t)
	{
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(t, "ui", false);
		this.elementImg.sprite = uisprite.first;
		this.elementImg.color = uisprite.second;
		this.elementImg.gameObject.SetActive(true);
	}

	// Token: 0x0600AD7E RID: 44414 RVA: 0x0041237C File Offset: 0x0041057C
	public void SetTag(Tag newTag)
	{
		this.Initialize();
		this.elementTag = newTag;
		this.SetSprite(this.elementTag);
		string text = this.elementTag.ProperName();
		if (this.parent.IsStorage)
		{
			float amountInStorage = this.parent.GetAmountInStorage(this.elementTag);
			text = text + ": " + GameUtil.GetFormattedMass(amountInStorage, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		}
		this.elementName.text = text;
	}

	// Token: 0x0600AD7F RID: 44415 RVA: 0x00110EAC File Offset: 0x0010F0AC
	private void CheckBoxClicked()
	{
		this.SetCheckBox(!this.parent.IsTagAllowed(this.GetElementTag()));
	}

	// Token: 0x0600AD80 RID: 44416 RVA: 0x00110EC8 File Offset: 0x0010F0C8
	public void SetCheckBox(bool checkBoxState)
	{
		this.checkBox.ChangeState(checkBoxState ? 1 : 0);
		this.checkBoxImg.enabled = checkBoxState;
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this.GetElementTag(), checkBoxState);
		}
	}

	// Token: 0x0400881C RID: 34844
	[SerializeField]
	private LocText elementName;

	// Token: 0x0400881D RID: 34845
	[SerializeField]
	private MultiToggle checkBox;

	// Token: 0x0400881E RID: 34846
	[SerializeField]
	private KImage elementImg;

	// Token: 0x0400881F RID: 34847
	private KImage checkBoxImg;

	// Token: 0x04008820 RID: 34848
	private Tag elementTag;

	// Token: 0x04008821 RID: 34849
	public Action<Tag, bool> OnSelectionChanged;

	// Token: 0x04008822 RID: 34850
	private TreeFilterableSideScreen parent;

	// Token: 0x04008823 RID: 34851
	private bool initialized;
}
