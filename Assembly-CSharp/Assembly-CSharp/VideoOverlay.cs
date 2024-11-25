using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/VideoOverlay")]
public class VideoOverlay : KMonoBehaviour {
    public List<LocText> textFields;

    public void SetText(List<string> strings) {
        if (strings.Count != textFields.Count)
            DebugUtil.LogErrorArgs(name, "expects", textFields.Count, "strings passed to it, got", strings.Count);

        for (var i = 0; i < textFields.Count; i++) textFields[i].text = strings[i];
    }
}