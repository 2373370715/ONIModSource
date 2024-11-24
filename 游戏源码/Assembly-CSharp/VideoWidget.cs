using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x0200204A RID: 8266
[AddComponentMenu("KMonoBehaviour/scripts/VideoWidget")]
public class VideoWidget : KMonoBehaviour
{
	// Token: 0x0600B000 RID: 45056 RVA: 0x00112649 File Offset: 0x00110849
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.Clicked;
		this.rawImage = this.thumbnailPlayer.GetComponent<RawImage>();
	}

	// Token: 0x0600B001 RID: 45057 RVA: 0x00422D60 File Offset: 0x00420F60
	private void Clicked()
	{
		VideoScreen.Instance.PlayVideo(this.clip, false, default(EventReference), false);
		if (!string.IsNullOrEmpty(this.overlayName))
		{
			VideoScreen.Instance.SetOverlayText(this.overlayName, this.texts);
		}
	}

	// Token: 0x0600B002 RID: 45058 RVA: 0x00422DAC File Offset: 0x00420FAC
	public void SetClip(VideoClip clip, string overlayName = null, List<string> texts = null)
	{
		if (clip == null)
		{
			global::Debug.LogWarning("Tried to assign null video clip to VideoWidget");
			return;
		}
		this.clip = clip;
		this.overlayName = overlayName;
		this.texts = texts;
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.thumbnailPlayer.targetTexture = this.renderTexture;
		this.rawImage.texture = this.renderTexture;
		base.StartCoroutine(this.ConfigureThumbnail());
	}

	// Token: 0x0600B003 RID: 45059 RVA: 0x00112679 File Offset: 0x00110879
	private IEnumerator ConfigureThumbnail()
	{
		this.thumbnailPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.thumbnailPlayer.clip = this.clip;
		this.thumbnailPlayer.time = 0.0;
		this.thumbnailPlayer.Play();
		yield return null;
		yield break;
	}

	// Token: 0x0600B004 RID: 45060 RVA: 0x00112688 File Offset: 0x00110888
	private void Update()
	{
		if (this.thumbnailPlayer.isPlaying && this.thumbnailPlayer.time > 2.0)
		{
			this.thumbnailPlayer.Pause();
		}
	}

	// Token: 0x04008AB5 RID: 35509
	[SerializeField]
	private VideoClip clip;

	// Token: 0x04008AB6 RID: 35510
	[SerializeField]
	private VideoPlayer thumbnailPlayer;

	// Token: 0x04008AB7 RID: 35511
	[SerializeField]
	private KButton button;

	// Token: 0x04008AB8 RID: 35512
	[SerializeField]
	private string overlayName;

	// Token: 0x04008AB9 RID: 35513
	[SerializeField]
	private List<string> texts;

	// Token: 0x04008ABA RID: 35514
	private RenderTexture renderTexture;

	// Token: 0x04008ABB RID: 35515
	private RawImage rawImage;
}
