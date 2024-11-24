using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200120B RID: 4619
[AddComponentMenu("KMonoBehaviour/scripts/DebugText")]
public class DebugText : KMonoBehaviour
{
	// Token: 0x06005E50 RID: 24144 RVA: 0x000DD9EF File Offset: 0x000DBBEF
	public static void DestroyInstance()
	{
		DebugText.Instance = null;
	}

	// Token: 0x06005E51 RID: 24145 RVA: 0x000DD9F7 File Offset: 0x000DBBF7
	protected override void OnPrefabInit()
	{
		DebugText.Instance = this;
	}

	// Token: 0x06005E52 RID: 24146 RVA: 0x002A2F7C File Offset: 0x002A117C
	public void Draw(string text, Vector3 pos, Color color)
	{
		DebugText.Entry item = new DebugText.Entry
		{
			text = text,
			pos = pos,
			color = color
		};
		this.entries.Add(item);
	}

	// Token: 0x06005E53 RID: 24147 RVA: 0x002A2FB8 File Offset: 0x002A11B8
	private void LateUpdate()
	{
		foreach (Text text in this.texts)
		{
			UnityEngine.Object.Destroy(text.gameObject);
		}
		this.texts.Clear();
		foreach (DebugText.Entry entry in this.entries)
		{
			GameObject gameObject = new GameObject();
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
			gameObject.transform.SetPosition(entry.pos);
			rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);
			Text text2 = gameObject.AddComponent<Text>();
			text2.font = Assets.DebugFont;
			text2.text = entry.text;
			text2.color = entry.color;
			text2.horizontalOverflow = HorizontalWrapMode.Overflow;
			text2.verticalOverflow = VerticalWrapMode.Overflow;
			text2.alignment = TextAnchor.MiddleCenter;
			this.texts.Add(text2);
		}
		this.entries.Clear();
	}

	// Token: 0x040042CA RID: 17098
	public static DebugText Instance;

	// Token: 0x040042CB RID: 17099
	private List<DebugText.Entry> entries = new List<DebugText.Entry>();

	// Token: 0x040042CC RID: 17100
	private List<Text> texts = new List<Text>();

	// Token: 0x0200120C RID: 4620
	private struct Entry
	{
		// Token: 0x040042CD RID: 17101
		public string text;

		// Token: 0x040042CE RID: 17102
		public Vector3 pos;

		// Token: 0x040042CF RID: 17103
		public Color color;
	}
}
