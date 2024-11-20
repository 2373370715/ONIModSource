using System;
using System.Collections;
using System.Collections.Generic;
using RsLib.Adapter;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocTextAdapter))]
public class LocTextAdapterEditor : Editor
{
   private void OnEnable()
   {
   }

   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();
   }
}
