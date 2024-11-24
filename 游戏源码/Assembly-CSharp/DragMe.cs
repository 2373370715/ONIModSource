using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200000D RID: 13
public class DragMe : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x0600002F RID: 47 RVA: 0x0013DF30 File Offset: 0x0013C130
	public void OnBeginDrag(PointerEventData eventData)
	{
		Canvas canvas = DragMe.FindInParents<Canvas>(base.gameObject);
		if (canvas == null)
		{
			return;
		}
		this.m_DraggingIcon = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		GraphicRaycaster component = this.m_DraggingIcon.GetComponent<GraphicRaycaster>();
		if (component != null)
		{
			component.enabled = false;
		}
		this.m_DraggingIcon.name = "dragObj";
		this.m_DraggingIcon.transform.SetParent(canvas.transform, false);
		this.m_DraggingIcon.transform.SetAsLastSibling();
		this.m_DraggingIcon.GetComponent<RectTransform>().pivot = Vector2.zero;
		if (this.dragOnSurfaces)
		{
			this.m_DraggingPlane = (base.transform as RectTransform);
		}
		else
		{
			this.m_DraggingPlane = (canvas.transform as RectTransform);
		}
		this.SetDraggedPosition(eventData);
		this.listener.OnBeginDrag(eventData.position);
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000A5E42 File Offset: 0x000A4042
	public void OnDrag(PointerEventData data)
	{
		if (this.m_DraggingIcon != null)
		{
			this.SetDraggedPosition(data);
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0013E010 File Offset: 0x0013C210
	private void SetDraggedPosition(PointerEventData data)
	{
		if (this.dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
		{
			this.m_DraggingPlane = (data.pointerEnter.transform as RectTransform);
		}
		RectTransform component = this.m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 position;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out position))
		{
			component.position = position;
			component.rotation = this.m_DraggingPlane.rotation;
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000A5E59 File Offset: 0x000A4059
	public void OnEndDrag(PointerEventData eventData)
	{
		this.listener.OnEndDrag(eventData.position);
		if (this.m_DraggingIcon != null)
		{
			UnityEngine.Object.Destroy(this.m_DraggingIcon);
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x0013E0A0 File Offset: 0x0013C2A0
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return default(T);
		}
		T t = default(T);
		Transform parent = go.transform.parent;
		while (parent != null && t == null)
		{
			t = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return t;
	}

	// Token: 0x04000034 RID: 52
	public bool dragOnSurfaces = true;

	// Token: 0x04000035 RID: 53
	private GameObject m_DraggingIcon;

	// Token: 0x04000036 RID: 54
	private RectTransform m_DraggingPlane;

	// Token: 0x04000037 RID: 55
	public DragMe.IDragListener listener;

	// Token: 0x0200000E RID: 14
	public interface IDragListener
	{
		// Token: 0x06000035 RID: 53
		void OnBeginDrag(Vector2 position);

		// Token: 0x06000036 RID: 54
		void OnEndDrag(Vector2 position);
	}
}
