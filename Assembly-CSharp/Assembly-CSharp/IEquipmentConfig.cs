﻿using System;
using UnityEngine;

public interface IEquipmentConfig
{
		EquipmentDef CreateEquipmentDef();

		void DoPostConfigure(GameObject go);

		string[] GetDlcIds();
}
