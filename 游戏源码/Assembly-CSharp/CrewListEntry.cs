using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001D98 RID: 7576
[AddComponentMenu("KMonoBehaviour/scripts/CrewListEntry")]
public class CrewListEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x06009E52 RID: 40530 RVA: 0x00107132 File Offset: 0x00105332
	public MinionIdentity Identity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x06009E53 RID: 40531 RVA: 0x0010713A File Offset: 0x0010533A
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.BGImage.enabled = true;
		this.BorderHighlight.color = new Color(0.65882355f, 0.2901961f, 0.4745098f);
	}

	// Token: 0x06009E54 RID: 40532 RVA: 0x0010716E File Offset: 0x0010536E
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.BGImage.enabled = false;
		this.BorderHighlight.color = new Color(0.8f, 0.8f, 0.8f);
	}

	// Token: 0x06009E55 RID: 40533 RVA: 0x003CB3CC File Offset: 0x003C95CC
	public void OnPointerClick(PointerEventData eventData)
	{
		bool focus = Time.unscaledTime - this.lastClickTime < 0.3f;
		this.SelectCrewMember(focus);
		this.lastClickTime = Time.unscaledTime;
	}

	// Token: 0x06009E56 RID: 40534 RVA: 0x003CB400 File Offset: 0x003C9600
	public virtual void Populate(MinionIdentity _identity)
	{
		this.identity = _identity;
		if (this.portrait == null)
		{
			GameObject parent = (this.crewPortraitParent != null) ? this.crewPortraitParent : base.gameObject;
			this.portrait = Util.KInstantiateUI<CrewPortrait>(this.PortraitPrefab.gameObject, parent, false);
			if (this.crewPortraitParent == null)
			{
				this.portrait.transform.SetSiblingIndex(2);
			}
		}
		this.portrait.SetIdentityObject(_identity, true);
	}

	// Token: 0x06009E57 RID: 40535 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void Refresh()
	{
	}

	// Token: 0x06009E58 RID: 40536 RVA: 0x001071A2 File Offset: 0x001053A2
	public void RefreshCrewPortraitContent()
	{
		if (this.portrait != null)
		{
			this.portrait.ForceRefresh();
		}
	}

	// Token: 0x06009E59 RID: 40537 RVA: 0x001071BD File Offset: 0x001053BD
	private string seniorityString()
	{
		return this.identity.GetAttributes().GetProfessionString(true);
	}

	// Token: 0x06009E5A RID: 40538 RVA: 0x003CB484 File Offset: 0x003C9684
	public void SelectCrewMember(bool focus)
	{
		if (focus)
		{
			SelectTool.Instance.SelectAndFocus(this.identity.transform.GetPosition(), this.identity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
			return;
		}
		SelectTool.Instance.Select(this.identity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x04007C1F RID: 31775
	protected MinionIdentity identity;

	// Token: 0x04007C20 RID: 31776
	protected CrewPortrait portrait;

	// Token: 0x04007C21 RID: 31777
	public CrewPortrait PortraitPrefab;

	// Token: 0x04007C22 RID: 31778
	public GameObject crewPortraitParent;

	// Token: 0x04007C23 RID: 31779
	protected bool mouseOver;

	// Token: 0x04007C24 RID: 31780
	public Image BorderHighlight;

	// Token: 0x04007C25 RID: 31781
	public Image BGImage;

	// Token: 0x04007C26 RID: 31782
	public float lastClickTime;
}
