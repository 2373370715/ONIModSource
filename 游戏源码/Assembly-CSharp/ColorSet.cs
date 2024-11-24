using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02001C73 RID: 7283
public class ColorSet : ScriptableObject
{
	// Token: 0x060097EC RID: 38892 RVA: 0x003AE3B8 File Offset: 0x003AC5B8
	private void Init()
	{
		if (this.namedLookup == null)
		{
			this.namedLookup = new Dictionary<string, Color32>();
			foreach (FieldInfo fieldInfo in typeof(ColorSet).GetFields())
			{
				if (fieldInfo.FieldType == typeof(Color32))
				{
					this.namedLookup[fieldInfo.Name] = (Color32)fieldInfo.GetValue(this);
				}
			}
		}
	}

	// Token: 0x060097ED RID: 38893 RVA: 0x00102BD7 File Offset: 0x00100DD7
	public Color32 GetColorByName(string name)
	{
		this.Init();
		return this.namedLookup[name];
	}

	// Token: 0x060097EE RID: 38894 RVA: 0x00102BEB File Offset: 0x00100DEB
	public void RefreshLookup()
	{
		this.namedLookup = null;
		this.Init();
	}

	// Token: 0x060097EF RID: 38895 RVA: 0x00102BFA File Offset: 0x00100DFA
	public bool IsDefaultColorSet()
	{
		return Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, this) == 0;
	}

	// Token: 0x040075F2 RID: 30194
	public string settingName;

	// Token: 0x040075F3 RID: 30195
	[Header("Logic")]
	public Color32 logicOn;

	// Token: 0x040075F4 RID: 30196
	public Color32 logicOff;

	// Token: 0x040075F5 RID: 30197
	public Color32 logicDisconnected;

	// Token: 0x040075F6 RID: 30198
	public Color32 logicOnText;

	// Token: 0x040075F7 RID: 30199
	public Color32 logicOffText;

	// Token: 0x040075F8 RID: 30200
	public Color32 logicOnSidescreen;

	// Token: 0x040075F9 RID: 30201
	public Color32 logicOffSidescreen;

	// Token: 0x040075FA RID: 30202
	[Header("Decor")]
	public Color32 decorPositive;

	// Token: 0x040075FB RID: 30203
	public Color32 decorNegative;

	// Token: 0x040075FC RID: 30204
	public Color32 decorBaseline;

	// Token: 0x040075FD RID: 30205
	public Color32 decorHighlightPositive;

	// Token: 0x040075FE RID: 30206
	public Color32 decorHighlightNegative;

	// Token: 0x040075FF RID: 30207
	[Header("Crop Overlay")]
	public Color32 cropHalted;

	// Token: 0x04007600 RID: 30208
	public Color32 cropGrowing;

	// Token: 0x04007601 RID: 30209
	public Color32 cropGrown;

	// Token: 0x04007602 RID: 30210
	[Header("Harvest Overlay")]
	public Color32 harvestEnabled;

	// Token: 0x04007603 RID: 30211
	public Color32 harvestDisabled;

	// Token: 0x04007604 RID: 30212
	[Header("Gameplay Events")]
	public Color32 eventPositive;

	// Token: 0x04007605 RID: 30213
	public Color32 eventNegative;

	// Token: 0x04007606 RID: 30214
	public Color32 eventNeutral;

	// Token: 0x04007607 RID: 30215
	[Header("Notifications")]
	public Color32 NotificationNormal;

	// Token: 0x04007608 RID: 30216
	public Color32 NotificationNormalBG;

	// Token: 0x04007609 RID: 30217
	public Color32 NotificationBad;

	// Token: 0x0400760A RID: 30218
	public Color32 NotificationBadBG;

	// Token: 0x0400760B RID: 30219
	public Color32 NotificationEvent;

	// Token: 0x0400760C RID: 30220
	public Color32 NotificationEventBG;

	// Token: 0x0400760D RID: 30221
	public Color32 NotificationMessage;

	// Token: 0x0400760E RID: 30222
	public Color32 NotificationMessageBG;

	// Token: 0x0400760F RID: 30223
	public Color32 NotificationMessageImportant;

	// Token: 0x04007610 RID: 30224
	public Color32 NotificationMessageImportantBG;

	// Token: 0x04007611 RID: 30225
	public Color32 NotificationTutorial;

	// Token: 0x04007612 RID: 30226
	public Color32 NotificationTutorialBG;

	// Token: 0x04007613 RID: 30227
	[Header("PrioritiesScreen")]
	public Color32 PrioritiesNeutralColor;

	// Token: 0x04007614 RID: 30228
	public Color32 PrioritiesLowColor;

	// Token: 0x04007615 RID: 30229
	public Color32 PrioritiesHighColor;

	// Token: 0x04007616 RID: 30230
	[Header("Info Screen Status Items")]
	public Color32 statusItemBad;

	// Token: 0x04007617 RID: 30231
	public Color32 statusItemEvent;

	// Token: 0x04007618 RID: 30232
	public Color32 statusItemMessageImportant;

	// Token: 0x04007619 RID: 30233
	[Header("Germ Overlay")]
	public Color32 germFoodPoisoning;

	// Token: 0x0400761A RID: 30234
	public Color32 germPollenGerms;

	// Token: 0x0400761B RID: 30235
	public Color32 germSlimeLung;

	// Token: 0x0400761C RID: 30236
	public Color32 germZombieSpores;

	// Token: 0x0400761D RID: 30237
	public Color32 germRadiationSickness;

	// Token: 0x0400761E RID: 30238
	[Header("Room Overlay")]
	public Color32 roomNone;

	// Token: 0x0400761F RID: 30239
	public Color32 roomFood;

	// Token: 0x04007620 RID: 30240
	public Color32 roomSleep;

	// Token: 0x04007621 RID: 30241
	public Color32 roomRecreation;

	// Token: 0x04007622 RID: 30242
	public Color32 roomBathroom;

	// Token: 0x04007623 RID: 30243
	public Color32 roomHospital;

	// Token: 0x04007624 RID: 30244
	public Color32 roomIndustrial;

	// Token: 0x04007625 RID: 30245
	public Color32 roomAgricultural;

	// Token: 0x04007626 RID: 30246
	public Color32 roomScience;

	// Token: 0x04007627 RID: 30247
	public Color32 roomBionic;

	// Token: 0x04007628 RID: 30248
	public Color32 roomPark;

	// Token: 0x04007629 RID: 30249
	[Header("Power Overlay")]
	public Color32 powerConsumer;

	// Token: 0x0400762A RID: 30250
	public Color32 powerGenerator;

	// Token: 0x0400762B RID: 30251
	public Color32 powerBuildingDisabled;

	// Token: 0x0400762C RID: 30252
	public Color32 powerCircuitUnpowered;

	// Token: 0x0400762D RID: 30253
	public Color32 powerCircuitSafe;

	// Token: 0x0400762E RID: 30254
	public Color32 powerCircuitStraining;

	// Token: 0x0400762F RID: 30255
	public Color32 powerCircuitOverloading;

	// Token: 0x04007630 RID: 30256
	[Header("Light Overlay")]
	public Color32 lightOverlay;

	// Token: 0x04007631 RID: 30257
	[Header("Conduit Overlay")]
	public Color32 conduitNormal;

	// Token: 0x04007632 RID: 30258
	public Color32 conduitInsulated;

	// Token: 0x04007633 RID: 30259
	public Color32 conduitRadiant;

	// Token: 0x04007634 RID: 30260
	[Header("Temperature Overlay")]
	public Color32 temperatureThreshold0;

	// Token: 0x04007635 RID: 30261
	public Color32 temperatureThreshold1;

	// Token: 0x04007636 RID: 30262
	public Color32 temperatureThreshold2;

	// Token: 0x04007637 RID: 30263
	public Color32 temperatureThreshold3;

	// Token: 0x04007638 RID: 30264
	public Color32 temperatureThreshold4;

	// Token: 0x04007639 RID: 30265
	public Color32 temperatureThreshold5;

	// Token: 0x0400763A RID: 30266
	public Color32 temperatureThreshold6;

	// Token: 0x0400763B RID: 30267
	public Color32 temperatureThreshold7;

	// Token: 0x0400763C RID: 30268
	public Color32 heatflowThreshold0;

	// Token: 0x0400763D RID: 30269
	public Color32 heatflowThreshold1;

	// Token: 0x0400763E RID: 30270
	public Color32 heatflowThreshold2;

	// Token: 0x0400763F RID: 30271
	private Dictionary<string, Color32> namedLookup;
}
