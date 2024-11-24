using System;
using UnityEngine;

// Token: 0x0200092F RID: 2351
public class NativeAnimBatchLoader : MonoBehaviour
{
	// Token: 0x06002A77 RID: 10871 RVA: 0x001DA9EC File Offset: 0x001D8BEC
	private void Start()
	{
		if (this.generateObjects)
		{
			for (int i = 0; i < this.enableObjects.Length; i++)
			{
				if (this.enableObjects[i] != null)
				{
					this.enableObjects[i].GetComponent<KBatchedAnimController>().visibilityType = KAnimControllerBase.VisibilityType.Always;
					this.enableObjects[i].SetActive(true);
				}
			}
		}
		if (this.setTimeScale)
		{
			Time.timeScale = 1f;
		}
		if (this.destroySelf)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x001DAA68 File Offset: 0x001D8C68
	private void LateUpdate()
	{
		if (this.destroySelf)
		{
			return;
		}
		if (this.performUpdate)
		{
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
		}
		if (this.performRender)
		{
			KAnimBatchManager.Instance().Render();
		}
	}

	// Token: 0x04001C43 RID: 7235
	public bool performTimeUpdate;

	// Token: 0x04001C44 RID: 7236
	public bool performUpdate;

	// Token: 0x04001C45 RID: 7237
	public bool performRender;

	// Token: 0x04001C46 RID: 7238
	public bool setTimeScale;

	// Token: 0x04001C47 RID: 7239
	public bool destroySelf;

	// Token: 0x04001C48 RID: 7240
	public bool generateObjects;

	// Token: 0x04001C49 RID: 7241
	public GameObject[] enableObjects;
}
