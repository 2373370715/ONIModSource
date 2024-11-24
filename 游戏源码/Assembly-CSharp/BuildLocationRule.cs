using System;

// Token: 0x02001212 RID: 4626
public enum BuildLocationRule
{
	// Token: 0x040042F4 RID: 17140
	Anywhere,
	// Token: 0x040042F5 RID: 17141
	OnFloor,
	// Token: 0x040042F6 RID: 17142
	OnFloorOverSpace,
	// Token: 0x040042F7 RID: 17143
	OnCeiling,
	// Token: 0x040042F8 RID: 17144
	OnWall,
	// Token: 0x040042F9 RID: 17145
	InCorner,
	// Token: 0x040042FA RID: 17146
	Tile,
	// Token: 0x040042FB RID: 17147
	NotInTiles,
	// Token: 0x040042FC RID: 17148
	Conduit,
	// Token: 0x040042FD RID: 17149
	LogicBridge,
	// Token: 0x040042FE RID: 17150
	WireBridge,
	// Token: 0x040042FF RID: 17151
	HighWattBridgeTile,
	// Token: 0x04004300 RID: 17152
	BuildingAttachPoint,
	// Token: 0x04004301 RID: 17153
	OnFloorOrBuildingAttachPoint,
	// Token: 0x04004302 RID: 17154
	OnFoundationRotatable,
	// Token: 0x04004303 RID: 17155
	BelowRocketCeiling,
	// Token: 0x04004304 RID: 17156
	OnRocketEnvelope,
	// Token: 0x04004305 RID: 17157
	WallFloor,
	// Token: 0x04004306 RID: 17158
	NoLiquidConduitAtOrigin
}
