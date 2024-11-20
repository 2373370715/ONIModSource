using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C60 RID: 3168
namespace RsLib.Adapter
{
	public class MultiToggleAdapter : MonoBehaviour
	{
		// Token: 0x04004537 RID: 17719
		[Header("Settings")]
		[SerializeField]
		public ToggleState[] states;

		// Token: 0x04004538 RID: 17720
		public bool play_sound_on_click = true;

		// Token: 0x04004539 RID: 17721
		public bool play_sound_on_release;

		// Token: 0x0400453A RID: 17722
		public Image toggle_image;

		// Token: 0x0400453B RID: 17723
		protected int state;

		// Token: 0x0400453C RID: 17724
		public System.Action onClick;

		// Token: 0x0400453D RID: 17725
		private bool stateDirty = true;

		// Token: 0x0400453E RID: 17726
		public Func<bool> onDoubleClick;

		// Token: 0x0400453F RID: 17727
		public System.Action onEnter;

		// Token: 0x04004540 RID: 17728
		public System.Action onExit;

		// Token: 0x04004541 RID: 17729
		public System.Action onHold;

		// Token: 0x04004542 RID: 17730
		public System.Action onStopHold;

		// Token: 0x04004543 RID: 17731
		public bool allowRightClick = true;

		// Token: 0x04004544 RID: 17732
		protected bool clickHeldDown;

		// Token: 0x04004545 RID: 17733
		protected float totalHeldTime;

		// Token: 0x04004546 RID: 17734
		protected float heldTimeThreshold = 0.4f;

		// Token: 0x04004547 RID: 17735
		private bool pointerOver;
	}

}
