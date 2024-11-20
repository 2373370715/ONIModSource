using System;
using UnityEngine;

public class StampToolPreviewContext
{
	public Transform previewParent;

	public InterfaceTool tool;

	public TemplateContainer stampTemplate;

	public System.Action frameAfterSetupFn;

	public Action<int> refreshFn;

	public System.Action onPlaceFn;

	public Action<string> onErrorChangeFn;

	public System.Action cleanupFn;
}
