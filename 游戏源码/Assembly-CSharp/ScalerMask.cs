using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001CA8 RID: 7336
[AddComponentMenu("KMonoBehaviour/scripts/ScalerMask")]
public class ScalerMask : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x06009913 RID: 39187 RVA: 0x00103964 File Offset: 0x00101B64
	private RectTransform ThisTransform
	{
		get
		{
			if (this._thisTransform == null)
			{
				this._thisTransform = base.GetComponent<RectTransform>();
			}
			return this._thisTransform;
		}
	}

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x06009914 RID: 39188 RVA: 0x00103986 File Offset: 0x00101B86
	private LayoutElement ThisLayoutElement
	{
		get
		{
			if (this._thisLayoutElement == null)
			{
				this._thisLayoutElement = base.GetComponent<LayoutElement>();
			}
			return this._thisLayoutElement;
		}
	}

	// Token: 0x06009915 RID: 39189 RVA: 0x003B35F0 File Offset: 0x003B17F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
	}

	// Token: 0x06009916 RID: 39190 RVA: 0x003B3658 File Offset: 0x003B1858
	protected override void OnCleanUp()
	{
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Remove(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Remove(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
		base.OnCleanUp();
	}

	// Token: 0x06009917 RID: 39191 RVA: 0x003B36C0 File Offset: 0x003B18C0
	private void Update()
	{
		if (this.SourceTransform != null)
		{
			this.SourceTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.ThisTransform.rect.width);
		}
		if (this.SourceTransform != null && (!this.hoverLock || !this.grandparentIsHovered || this.isHovered || this.queuedSizeUpdate))
		{
			this.ThisLayoutElement.minHeight = this.SourceTransform.rect.height + this.topPadding + this.bottomPadding;
			this.SourceTransform.anchoredPosition = new Vector2(0f, -this.topPadding);
			this.queuedSizeUpdate = false;
		}
		if (this.hoverIndicator != null)
		{
			if (this.SourceTransform != null && this.SourceTransform.rect.height > this.ThisTransform.rect.height)
			{
				this.hoverIndicator.SetActive(true);
				return;
			}
			this.hoverIndicator.SetActive(false);
		}
	}

	// Token: 0x06009918 RID: 39192 RVA: 0x001039A8 File Offset: 0x00101BA8
	public void UpdateSize()
	{
		this.queuedSizeUpdate = true;
	}

	// Token: 0x06009919 RID: 39193 RVA: 0x001039B1 File Offset: 0x00101BB1
	public void OnPointerEnterGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = true;
	}

	// Token: 0x0600991A RID: 39194 RVA: 0x001039BA File Offset: 0x00101BBA
	public void OnPointerExitGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = false;
	}

	// Token: 0x0600991B RID: 39195 RVA: 0x001039C3 File Offset: 0x00101BC3
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isHovered = true;
	}

	// Token: 0x0600991C RID: 39196 RVA: 0x001039CC File Offset: 0x00101BCC
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isHovered = false;
	}

	// Token: 0x04007758 RID: 30552
	public RectTransform SourceTransform;

	// Token: 0x04007759 RID: 30553
	private RectTransform _thisTransform;

	// Token: 0x0400775A RID: 30554
	private LayoutElement _thisLayoutElement;

	// Token: 0x0400775B RID: 30555
	public GameObject hoverIndicator;

	// Token: 0x0400775C RID: 30556
	public bool hoverLock;

	// Token: 0x0400775D RID: 30557
	private bool grandparentIsHovered;

	// Token: 0x0400775E RID: 30558
	private bool isHovered;

	// Token: 0x0400775F RID: 30559
	private bool queuedSizeUpdate = true;

	// Token: 0x04007760 RID: 30560
	public float topPadding;

	// Token: 0x04007761 RID: 30561
	public float bottomPadding;
}
