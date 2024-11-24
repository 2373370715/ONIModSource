using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C17 RID: 7191
public class ClusterMapHex : MultiToggle, ICanvasRaycastFilter
{
	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x0600957B RID: 38267 RVA: 0x00101578 File Offset: 0x000FF778
	// (set) Token: 0x0600957C RID: 38268 RVA: 0x00101580 File Offset: 0x000FF780
	public AxialI location { get; private set; }

	// Token: 0x0600957D RID: 38269 RVA: 0x0039CA2C File Offset: 0x0039AC2C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rectTransform = base.GetComponent<RectTransform>();
		this.onClick = new System.Action(this.TrySelect);
		this.onDoubleClick = new Func<bool>(this.TryGoTo);
		this.onEnter = new System.Action(this.OnHover);
		this.onExit = new System.Action(this.OnUnhover);
	}

	// Token: 0x0600957E RID: 38270 RVA: 0x00101589 File Offset: 0x000FF789
	public void SetLocation(AxialI location)
	{
		this.location = location;
	}

	// Token: 0x0600957F RID: 38271 RVA: 0x0039CA94 File Offset: 0x0039AC94
	public void SetRevealed(ClusterRevealLevel level)
	{
		this._revealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			this.fogOfWar.gameObject.SetActive(true);
			this.peekedTile.gameObject.SetActive(false);
			return;
		case ClusterRevealLevel.Peeked:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(true);
			return;
		case ClusterRevealLevel.Visible:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06009580 RID: 38272 RVA: 0x00101592 File Offset: 0x000FF792
	public void SetDestinationStatus(string fail_reason)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x06009581 RID: 38273 RVA: 0x0039CB24 File Offset: 0x0039AD24
	public void SetDestinationStatus(string fail_reason, int pathLength, int rocketRange, bool repeat)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		if (pathLength > 0)
		{
			string text = repeat ? UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH_RETURN : UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH;
			if (repeat)
			{
				pathLength *= 2;
			}
			text = string.Format(text, pathLength, GameUtil.GetFormattedRocketRange(rocketRange, true));
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x06009582 RID: 38274 RVA: 0x0039CBAC File Offset: 0x0039ADAC
	public void UpdateToggleState(ClusterMapHex.ToggleState state)
	{
		int new_state_index = -1;
		switch (state)
		{
		case ClusterMapHex.ToggleState.Unselected:
			new_state_index = 0;
			break;
		case ClusterMapHex.ToggleState.Selected:
			new_state_index = 1;
			break;
		case ClusterMapHex.ToggleState.OrbitHighlight:
			new_state_index = 2;
			break;
		}
		base.ChangeState(new_state_index);
	}

	// Token: 0x06009583 RID: 38275 RVA: 0x001015C5 File Offset: 0x000FF7C5
	private void TrySelect()
	{
		if (DebugHandler.InstantBuildMode)
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.location, 0);
		}
		ClusterMapScreen.Instance.SelectHex(this);
	}

	// Token: 0x06009584 RID: 38276 RVA: 0x0039CBE0 File Offset: 0x0039ADE0
	private bool TryGoTo()
	{
		List<WorldContainer> list = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(this.location)
		select entity.GetComponent<WorldContainer>() into x
		where x != null
		select x).ToList<WorldContainer>();
		if (list.Count == 1)
		{
			CameraController.Instance.ActiveWorldStarWipe(list[0].id, null);
			return true;
		}
		return false;
	}

	// Token: 0x06009585 RID: 38277 RVA: 0x0039CC70 File Offset: 0x0039AE70
	private void OnHover()
	{
		this.m_tooltip.ClearMultiStringTooltip();
		string text = "";
		switch (this._revealLevel)
		{
		case ClusterRevealLevel.Hidden:
			text = UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX;
			break;
		case ClusterRevealLevel.Peeked:
		{
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.Asteroid);
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell2 = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.POI);
			text = ((hiddenEntitiesOfLayerAtCell.Count > 0 || hiddenEntitiesOfLayerAtCell2.Count > 0) ? UI.CLUSTERMAP.TOOLTIP_PEEKED_HEX_WITH_OBJECT : UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX);
			break;
		}
		case ClusterRevealLevel.Visible:
			if (ClusterGrid.Instance.GetEntitiesOnCell(this.location).Count == 0)
			{
				text = UI.CLUSTERMAP.TOOLTIP_EMPTY_HEX;
			}
			break;
		}
		if (!text.IsNullOrWhiteSpace())
		{
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(true);
		ClusterMapScreen.Instance.OnHoverHex(this);
	}

	// Token: 0x06009586 RID: 38278 RVA: 0x001015EF File Offset: 0x000FF7EF
	private void OnUnhover()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.OnUnhoverHex(this);
		}
	}

	// Token: 0x06009587 RID: 38279 RVA: 0x0039CD4C File Offset: 0x0039AF4C
	private void UpdateHoverColors(bool validDestination)
	{
		Color color_on_hover = validDestination ? this.hoverColorValid : this.hoverColorInvalid;
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].color_on_hover = color_on_hover;
			for (int j = 0; j < this.states[i].additional_display_settings.Length; j++)
			{
				this.states[i].additional_display_settings[j].color_on_hover = color_on_hover;
			}
		}
		base.RefreshHoverColor();
	}

	// Token: 0x06009588 RID: 38280 RVA: 0x0039CDD4 File Offset: 0x0039AFD4
	public bool IsRaycastLocationValid(Vector2 inputPoint, Camera eventCamera)
	{
		Vector2 vector = this.rectTransform.position;
		float num = Mathf.Abs(inputPoint.x - vector.x);
		float num2 = Mathf.Abs(inputPoint.y - vector.y);
		Vector2 vector2 = this.rectTransform.lossyScale;
		return num <= vector2.x && num2 <= vector2.y && vector2.y * vector2.x - vector2.y / 2f * num - vector2.x * num2 >= 0f;
	}

	// Token: 0x04007414 RID: 29716
	private RectTransform rectTransform;

	// Token: 0x04007415 RID: 29717
	public Color hoverColorValid;

	// Token: 0x04007416 RID: 29718
	public Color hoverColorInvalid;

	// Token: 0x04007417 RID: 29719
	public Image fogOfWar;

	// Token: 0x04007418 RID: 29720
	public Image peekedTile;

	// Token: 0x04007419 RID: 29721
	public TextStyleSetting invalidDestinationTooltipStyle;

	// Token: 0x0400741A RID: 29722
	public TextStyleSetting informationTooltipStyle;

	// Token: 0x0400741B RID: 29723
	[MyCmpGet]
	private ToolTip m_tooltip;

	// Token: 0x0400741C RID: 29724
	private ClusterRevealLevel _revealLevel;

	// Token: 0x02001C18 RID: 7192
	public enum ToggleState
	{
		// Token: 0x0400741E RID: 29726
		Unselected,
		// Token: 0x0400741F RID: 29727
		Selected,
		// Token: 0x04007420 RID: 29728
		OrbitHighlight
	}
}
