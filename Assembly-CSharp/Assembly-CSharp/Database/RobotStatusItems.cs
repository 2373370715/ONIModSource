using System;
using UnityEngine;

namespace Database
{
		public class RobotStatusItems : StatusItems
	{
				public RobotStatusItems(ResourceSet parent) : base("RobotStatusItems", parent)
		{
			this.CreateStatusItems();
		}

				private void CreateStatusItems()
		{
			this.CantReachStation = new StatusItem("CantReachStation", "ROBOTS", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.CantReachStation.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.LowBattery = new StatusItem("LowBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.LowBattery.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.LowBatteryNoCharge = new StatusItem("LowBatteryNoCharge", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.LowBatteryNoCharge.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DeadBattery = new StatusItem("DeadBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.DeadBattery.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DeadBatteryFlydo = new StatusItem("DeadBatteryFlydo", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.DeadBatteryFlydo.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DustBinFull = new StatusItem("DustBinFull", "ROBOTS", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.DustBinFull.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.Working = new StatusItem("Working", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.Working.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.MovingToChargeStation = new StatusItem("MovingToChargeStation", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.MovingToChargeStation.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.UnloadingStorage = new StatusItem("UnloadingStorage", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.UnloadingStorage.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.ReactPositive = new StatusItem("ReactPositive", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.ReactPositive.resolveStringCallback = ((string str, object data) => str);
			this.ReactNegative = new StatusItem("ReactNegative", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.ReactNegative.resolveStringCallback = ((string str, object data) => str);
		}

				public StatusItem LowBattery;

				public StatusItem LowBatteryNoCharge;

				public StatusItem DeadBattery;

				public StatusItem DeadBatteryFlydo;

				public StatusItem CantReachStation;

				public StatusItem DustBinFull;

				public StatusItem Working;

				public StatusItem UnloadingStorage;

				public StatusItem ReactPositive;

				public StatusItem ReactNegative;

				public StatusItem MovingToChargeStation;
	}
}
