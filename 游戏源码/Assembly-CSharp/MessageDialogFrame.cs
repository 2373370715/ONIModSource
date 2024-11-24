using System;
using UnityEngine;

// Token: 0x02001DFA RID: 7674
public class MessageDialogFrame : KScreen
{
	// Token: 0x0600A095 RID: 41109 RVA: 0x0010856F File Offset: 0x0010676F
	public override float GetSortKey()
	{
		return 15f;
	}

	// Token: 0x0600A096 RID: 41110 RVA: 0x003D5BEC File Offset: 0x003D3DEC
	protected override void OnActivate()
	{
		this.closeButton.onClick += this.OnClickClose;
		this.nextMessageButton.onClick += this.OnClickNextMessage;
		MultiToggle multiToggle = this.dontShowAgainButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickDontShowAgain));
		bool flag = KPlayerPrefs.GetInt("HideTutorial_CheckState", 0) == 1;
		this.dontShowAgainButton.ChangeState(flag ? 0 : 1);
		base.Subscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
		this.OnMessagesChanged(null);
	}

	// Token: 0x0600A097 RID: 41111 RVA: 0x00108576 File Offset: 0x00106776
	protected override void OnDeactivate()
	{
		base.Unsubscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
	}

	// Token: 0x0600A098 RID: 41112 RVA: 0x00108599 File Offset: 0x00106799
	private void OnClickClose()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600A099 RID: 41113 RVA: 0x001085AC File Offset: 0x001067AC
	private void OnClickNextMessage()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
		NotificationScreen.Instance.OnClickNextMessage();
	}

	// Token: 0x0600A09A RID: 41114 RVA: 0x003D5C98 File Offset: 0x003D3E98
	private void OnClickDontShowAgain()
	{
		this.dontShowAgainButton.NextState();
		bool flag = this.dontShowAgainButton.CurrentState == 0;
		KPlayerPrefs.SetInt("HideTutorial_CheckState", flag ? 1 : 0);
	}

	// Token: 0x0600A09B RID: 41115 RVA: 0x001085C9 File Offset: 0x001067C9
	private void OnMessagesChanged(object data)
	{
		this.nextMessageButton.gameObject.SetActive(Messenger.Instance.Count != 0);
	}

	// Token: 0x0600A09C RID: 41116 RVA: 0x003D5CD0 File Offset: 0x003D3ED0
	public void SetMessage(MessageDialog dialog, Message message)
	{
		this.title.text = message.GetTitle().ToUpper();
		dialog.GetComponent<RectTransform>().SetParent(this.body.GetComponent<RectTransform>());
		RectTransform component = dialog.GetComponent<RectTransform>();
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		dialog.transform.SetLocalPosition(Vector3.zero);
		dialog.SetMessage(message);
		dialog.OnClickAction();
		if (dialog.CanDontShowAgain)
		{
			this.dontShowAgainElement.SetActive(true);
			this.dontShowAgainDelegate = new System.Action(dialog.OnDontShowAgain);
			return;
		}
		this.dontShowAgainElement.SetActive(false);
		this.dontShowAgainDelegate = null;
	}

	// Token: 0x0600A09D RID: 41117 RVA: 0x001085E8 File Offset: 0x001067E8
	private void TryDontShowAgain()
	{
		if (this.dontShowAgainDelegate != null && this.dontShowAgainButton.CurrentState == 0)
		{
			this.dontShowAgainDelegate();
		}
	}

	// Token: 0x04007D7B RID: 32123
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007D7C RID: 32124
	[SerializeField]
	private KToggle nextMessageButton;

	// Token: 0x04007D7D RID: 32125
	[SerializeField]
	private GameObject dontShowAgainElement;

	// Token: 0x04007D7E RID: 32126
	[SerializeField]
	private MultiToggle dontShowAgainButton;

	// Token: 0x04007D7F RID: 32127
	[SerializeField]
	private LocText title;

	// Token: 0x04007D80 RID: 32128
	[SerializeField]
	private RectTransform body;

	// Token: 0x04007D81 RID: 32129
	private System.Action dontShowAgainDelegate;
}
