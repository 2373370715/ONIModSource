using TMPro;
using UnityEngine;

namespace RsLib.Adapter;

public class KInputTextFieldAdapter : TMP_InputField {
    public KInputTextFieldAdapter() {
        onFocus = onFocus +
                  (System.Action)(() => {
                                      onEndEdit.AddListener(FixInput);

                                      if (!SteamGamepadTextInput.IsActive()) return;

                                      SteamGamepadTextInput.ShowTextInputScreen("", text, OnGamepadInputDismissed);
                                  });
    }

    private void OnGamepadInputDismissed(SteamGamepadTextInputData data) {
        if (data.submitted) text = data.input;
        OnDeselect(null);
    }

    private void FixInput(string str) {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) Input.ResetInputAxes();
    }
}