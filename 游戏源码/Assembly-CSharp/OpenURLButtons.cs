using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// Token: 0x02001E62 RID: 7778
[AddComponentMenu("KMonoBehaviour/scripts/OpenURLButtons")]
public class OpenURLButtons : KMonoBehaviour
{
	// Token: 0x0600A316 RID: 41750 RVA: 0x003E000C File Offset: 0x003DE20C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		for (int i = 0; i < this.buttonData.Count; i++)
		{
			OpenURLButtons.URLButtonData data = this.buttonData[i];
			GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, base.gameObject, true);
			string text = Strings.Get(data.stringKey);
			gameObject.GetComponentInChildren<LocText>().SetText(text);
			switch (data.urlType)
			{
			case OpenURLButtons.URLButtonType.url:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.platformUrl:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPlatformURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.patchNotes:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPatchNotes();
				};
				break;
			case OpenURLButtons.URLButtonType.feedbackScreen:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenFeedbackScreen();
				};
				break;
			}
		}
	}

	// Token: 0x0600A317 RID: 41751 RVA: 0x00109CB4 File Offset: 0x00107EB4
	public void OpenPatchNotes()
	{
		Util.KInstantiateUI(this.patchNotesScreenPrefab, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x0600A318 RID: 41752 RVA: 0x00109CCD File Offset: 0x00107ECD
	public void OpenFeedbackScreen()
	{
		Util.KInstantiateUI(this.feedbackScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x0600A319 RID: 41753 RVA: 0x00109CEB File Offset: 0x00107EEB
	public void OpenURL(string URL)
	{
		App.OpenWebURL(URL);
	}

	// Token: 0x0600A31A RID: 41754 RVA: 0x003E0118 File Offset: 0x003DE318
	public void OpenPlatformURL(string URL)
	{
		if (DistributionPlatform.Inst.Platform == "Steam" && DistributionPlatform.Inst.Initialized)
		{
			DistributionPlatform.Inst.GetAuthTicket(delegate(byte[] ticket)
			{
				string newValue = string.Concat(Array.ConvertAll<byte, string>(ticket, (byte x) => x.ToString("X2")));
				App.OpenWebURL(URL.Replace("{SteamID}", DistributionPlatform.Inst.LocalUser.Id.ToInt64().ToString()).Replace("{SteamTicket}", newValue));
			});
			return;
		}
		string value = URL.Replace("{SteamID}", "").Replace("{SteamTicket}", "");
		App.OpenWebURL("https://accounts.klei.com/login?goto={gotoUrl}".Replace("{gotoUrl}", WebUtility.HtmlEncode(value)));
	}

	// Token: 0x04007F46 RID: 32582
	public GameObject buttonPrefab;

	// Token: 0x04007F47 RID: 32583
	public List<OpenURLButtons.URLButtonData> buttonData;

	// Token: 0x04007F48 RID: 32584
	[SerializeField]
	private GameObject patchNotesScreenPrefab;

	// Token: 0x04007F49 RID: 32585
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x02001E63 RID: 7779
	public enum URLButtonType
	{
		// Token: 0x04007F4B RID: 32587
		url,
		// Token: 0x04007F4C RID: 32588
		platformUrl,
		// Token: 0x04007F4D RID: 32589
		patchNotes,
		// Token: 0x04007F4E RID: 32590
		feedbackScreen
	}

	// Token: 0x02001E64 RID: 7780
	[Serializable]
	public class URLButtonData
	{
		// Token: 0x04007F4F RID: 32591
		public string stringKey;

		// Token: 0x04007F50 RID: 32592
		public OpenURLButtons.URLButtonType urlType;

		// Token: 0x04007F51 RID: 32593
		public string url;
	}
}
