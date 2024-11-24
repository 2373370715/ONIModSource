using UnityEngine;

public abstract class DevToolEntityTarget
{
	public class ForUIGameObject : DevToolEntityTarget
	{
		public GameObject gameObject;

		public ForUIGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		public override Option<(Vector2 cornerA, Vector2 cornerB)> GetScreenRect()
		{
			if (gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Canvas componentInParent = gameObject.GetComponentInParent<Canvas>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Camera camera;
			if (!componentInParent.worldCamera.IsNullOrDestroyed())
			{
				camera = componentInParent.worldCamera;
				Vector3[] array = new Vector3[4];
				component.GetWorldCorners(array);
				return (ScreenPointToScreenPosition(camera.WorldToScreenPoint(array[0])), ScreenPointToScreenPosition(camera.WorldToScreenPoint(array[2])));
			}
			if (componentInParent.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				Vector3[] array2 = new Vector3[4];
				component.GetWorldCorners(array2);
				return (ScreenPointToScreenPosition(array2[0]), ScreenPointToScreenPosition(array2[2]));
			}
			return Option.None;
			Vector2 ScreenPointToScreenPosition(Vector2 coord)
			{
				return new Vector2(coord.x, (float)camera.pixelHeight - coord.y);
			}
			static Vector2 ScreenPointToScreenPosition(Vector2 coord)
			{
				return new Vector2(coord.x, (float)Screen.height - coord.y);
			}
		}

		public override string GetTag()
		{
			return "UI";
		}

		public override string ToString()
		{
			return DevToolEntity.GetNameFor(gameObject);
		}
	}

	public class ForWorldGameObject : DevToolEntityTarget
	{
		public GameObject gameObject;

		public ForWorldGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		public override Option<(Vector2 cornerA, Vector2 cornerB)> GetScreenRect()
		{
			if (gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Camera camera = Camera.main;
			if (camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			KCollider2D component = gameObject.GetComponent<KCollider2D>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return (ScreenPointToScreenPosition(camera.WorldToScreenPoint(component.bounds.min)), ScreenPointToScreenPosition(camera.WorldToScreenPoint(component.bounds.max)));
			Vector2 ScreenPointToScreenPosition(Vector2 coord)
			{
				return new Vector2(coord.x, (float)camera.pixelHeight - coord.y);
			}
		}

		public override string GetTag()
		{
			return "World";
		}

		public override string ToString()
		{
			return DevToolEntity.GetNameFor(gameObject);
		}
	}

	public class ForSimCell : DevToolEntityTarget
	{
		public int cellIndex;

		public ForSimCell(int cellIndex)
		{
			this.cellIndex = cellIndex;
		}

		public override Option<(Vector2 cornerA, Vector2 cornerB)> GetScreenRect()
		{
			Camera camera = Camera.main;
			if (camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Vector2 vector = Grid.CellToPosCCC(cellIndex, Grid.SceneLayer.Background);
			Vector2 vector2 = Grid.HalfCellSizeInMeters * Vector2.one;
			Vector2 vector3 = vector - vector2;
			Vector2 vector4 = vector + vector2;
			return (ScreenPointToScreenPosition(camera.WorldToScreenPoint(vector3)), ScreenPointToScreenPosition(camera.WorldToScreenPoint(vector4)));
			Vector2 ScreenPointToScreenPosition(Vector2 coord)
			{
				return new Vector2(coord.x, (float)camera.pixelHeight - coord.y);
			}
		}

		public override string GetTag()
		{
			return "Sim Cell";
		}

		public override string ToString()
		{
			return cellIndex.ToString();
		}
	}

	public abstract string GetTag();

	public abstract Option<(Vector2 cornerA, Vector2 cornerB)> GetScreenRect();

	public string GetDebugName()
	{
		return "[" + GetTag() + "] " + ToString();
	}
}
