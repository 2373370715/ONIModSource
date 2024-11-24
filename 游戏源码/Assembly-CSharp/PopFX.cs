using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EAF RID: 7855
[AddComponentMenu("KMonoBehaviour/scripts/PopFX")]
public class PopFX : KMonoBehaviour
{
	// Token: 0x0600A4EF RID: 42223 RVA: 0x003EA23C File Offset: 0x003E843C
	public void Recycle()
	{
		this.icon = null;
		this.text = "";
		this.targetTransform = null;
		this.lifeElapsed = 0f;
		this.trackTarget = false;
		this.startPos = Vector3.zero;
		this.IconDisplay.color = Color.white;
		this.TextDisplay.color = Color.white;
		PopFXManager.Instance.RecycleFX(this);
		this.canvasGroup.alpha = 0f;
		base.gameObject.SetActive(false);
		this.isLive = false;
		this.isActiveWorld = false;
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
	}

	// Token: 0x0600A4F0 RID: 42224 RVA: 0x003EA2F0 File Offset: 0x003E84F0
	public void Spawn(Sprite Icon, string Text, Transform TargetTransform, Vector3 Offset, float LifeTime = 1.5f, bool TrackTarget = false)
	{
		this.icon = Icon;
		this.text = Text;
		this.targetTransform = TargetTransform;
		this.trackTarget = TrackTarget;
		this.lifetime = LifeTime;
		this.offset = Offset;
		if (this.targetTransform != null)
		{
			this.startPos = this.targetTransform.GetPosition();
			int num;
			int num2;
			Grid.PosToXY(this.startPos, out num, out num2);
			if (num2 % 2 != 0)
			{
				this.startPos.x = this.startPos.x + 0.5f;
			}
		}
		this.TextDisplay.text = this.text;
		this.IconDisplay.sprite = this.icon;
		this.canvasGroup.alpha = 1f;
		this.isLive = true;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		this.Update();
	}

	// Token: 0x0600A4F1 RID: 42225 RVA: 0x003EA3DC File Offset: 0x003E85DC
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		if (this.isLive)
		{
			this.SetWorldActive(tuple.first);
		}
	}

	// Token: 0x0600A4F2 RID: 42226 RVA: 0x003EA404 File Offset: 0x003E8604
	private void SetWorldActive(int worldId)
	{
		int num = Grid.PosToCell((this.trackTarget && this.targetTransform != null) ? this.targetTransform.position : (this.startPos + this.offset));
		this.isActiveWorld = (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] == worldId);
	}

	// Token: 0x0600A4F3 RID: 42227 RVA: 0x003EA468 File Offset: 0x003E8668
	private void Update()
	{
		if (!this.isLive)
		{
			return;
		}
		if (!PopFXManager.Instance.Ready())
		{
			return;
		}
		this.lifeElapsed += Time.unscaledDeltaTime;
		if (this.lifeElapsed >= this.lifetime)
		{
			this.Recycle();
		}
		if (this.trackTarget && this.targetTransform != null)
		{
			Vector3 v = PopFXManager.Instance.WorldToScreen(this.targetTransform.GetPosition() + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * this.lifeElapsed));
			v.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v;
		}
		else
		{
			Vector3 v2 = PopFXManager.Instance.WorldToScreen(this.startPos + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * (this.lifeElapsed / 2f)));
			v2.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v2;
		}
		this.canvasGroup.alpha = (this.isActiveWorld ? (1.5f * ((this.lifetime - this.lifeElapsed) / this.lifetime)) : 0f);
	}

	// Token: 0x0400810E RID: 33038
	private float Speed = 2f;

	// Token: 0x0400810F RID: 33039
	private Sprite icon;

	// Token: 0x04008110 RID: 33040
	private string text;

	// Token: 0x04008111 RID: 33041
	private Transform targetTransform;

	// Token: 0x04008112 RID: 33042
	private Vector3 offset;

	// Token: 0x04008113 RID: 33043
	public Image IconDisplay;

	// Token: 0x04008114 RID: 33044
	public LocText TextDisplay;

	// Token: 0x04008115 RID: 33045
	public CanvasGroup canvasGroup;

	// Token: 0x04008116 RID: 33046
	private Camera uiCamera;

	// Token: 0x04008117 RID: 33047
	private float lifetime;

	// Token: 0x04008118 RID: 33048
	private float lifeElapsed;

	// Token: 0x04008119 RID: 33049
	private bool trackTarget;

	// Token: 0x0400811A RID: 33050
	private Vector3 startPos;

	// Token: 0x0400811B RID: 33051
	private bool isLive;

	// Token: 0x0400811C RID: 33052
	private bool isActiveWorld;
}
