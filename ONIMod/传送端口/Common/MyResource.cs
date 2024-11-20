using RsLib;
using UnityEngine;
using UnityEngine.UI;



public class MyResource {
    public static void InitAllTask() {
        PortChannelDiagram.InitPrefab();

        RsResources.AddLoadPrefabTask("prefabs/priority_image",
                                      parent => {
                                          var root = RsUIBuilder.UIGameObject("Priority", parent);
                                          root.rectTransform().localScale = new Vector3(0.4f, 0.4f, 0.4f);
                                          root.AddComponent<Image>();
                                          root.AddComponent<PriorityImage>();
                                          var canvasGroup = root.AddComponent<CanvasGroup>();
                                          canvasGroup.interactable   = false;
                                          canvasGroup.blocksRaycasts = false;
                                          return root;
                                      });
    }
}