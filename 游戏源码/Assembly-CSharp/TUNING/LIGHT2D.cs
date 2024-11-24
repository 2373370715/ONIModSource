using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02002283 RID: 8835
	public class LIGHT2D
	{
		// Token: 0x04009A94 RID: 39572
		public const int SUNLIGHT_MAX_DEFAULT = 80000;

		// Token: 0x04009A95 RID: 39573
		public static readonly Color LIGHT_BLUE = new Color(0.38f, 0.61f, 1f, 1f);

		// Token: 0x04009A96 RID: 39574
		public static readonly Color LIGHT_PURPLE = new Color(0.9f, 0.4f, 0.74f, 1f);

		// Token: 0x04009A97 RID: 39575
		public static readonly Color LIGHT_PINK = new Color(0.9f, 0.4f, 0.6f, 1f);

		// Token: 0x04009A98 RID: 39576
		public static readonly Color LIGHT_YELLOW = new Color(0.57f, 0.55f, 0.44f, 1f);

		// Token: 0x04009A99 RID: 39577
		public static readonly Color LIGHT_OVERLAY = new Color(0.56f, 0.56f, 0.56f, 1f);

		// Token: 0x04009A9A RID: 39578
		public static readonly Vector2 DEFAULT_DIRECTION = new Vector2(0f, -1f);

		// Token: 0x04009A9B RID: 39579
		public const int FLOORLAMP_LUX = 1000;

		// Token: 0x04009A9C RID: 39580
		public const float FLOORLAMP_RANGE = 4f;

		// Token: 0x04009A9D RID: 39581
		public const float FLOORLAMP_ANGLE = 0f;

		// Token: 0x04009A9E RID: 39582
		public const global::LightShape FLOORLAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x04009A9F RID: 39583
		public static readonly Color FLOORLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009AA0 RID: 39584
		public static readonly Color FLOORLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009AA1 RID: 39585
		public static readonly Vector2 FLOORLAMP_OFFSET = new Vector2(0.05f, 1.5f);

		// Token: 0x04009AA2 RID: 39586
		public static readonly Vector2 FLOORLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009AA3 RID: 39587
		public const float CEILINGLIGHT_RANGE = 8f;

		// Token: 0x04009AA4 RID: 39588
		public const float CEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x04009AA5 RID: 39589
		public const global::LightShape CEILINGLIGHT_SHAPE = global::LightShape.Cone;

		// Token: 0x04009AA6 RID: 39590
		public static readonly Color CEILINGLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009AA7 RID: 39591
		public static readonly Color CEILINGLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009AA8 RID: 39592
		public static readonly Vector2 CEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x04009AA9 RID: 39593
		public static readonly Vector2 CEILINGLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009AAA RID: 39594
		public const int CEILINGLIGHT_LUX = 1800;

		// Token: 0x04009AAB RID: 39595
		public static readonly int SUNLAMP_LUX = (int)((float)BeachChairConfig.TAN_LUX * 4f);

		// Token: 0x04009AAC RID: 39596
		public const float SUNLAMP_RANGE = 16f;

		// Token: 0x04009AAD RID: 39597
		public const float SUNLAMP_ANGLE = 5.2f;

		// Token: 0x04009AAE RID: 39598
		public const global::LightShape SUNLAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x04009AAF RID: 39599
		public static readonly Color SUNLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009AB0 RID: 39600
		public static readonly Color SUNLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009AB1 RID: 39601
		public static readonly Vector2 SUNLAMP_OFFSET = new Vector2(0f, 3.5f);

		// Token: 0x04009AB2 RID: 39602
		public static readonly Vector2 SUNLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009AB3 RID: 39603
		public const int MERCURYCEILINGLIGHT_LUX = 60000;

		// Token: 0x04009AB4 RID: 39604
		public const float MERCURYCEILINGLIGHT_RANGE = 8f;

		// Token: 0x04009AB5 RID: 39605
		public const float MERCURYCEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x04009AB6 RID: 39606
		public const float MERCURYCEILINGLIGHT_FALLOFFRATE = 0.4f;

		// Token: 0x04009AB7 RID: 39607
		public const int MERCURYCEILINGLIGHT_WIDTH = 3;

		// Token: 0x04009AB8 RID: 39608
		public const global::LightShape MERCURYCEILINGLIGHT_SHAPE = global::LightShape.Quad;

		// Token: 0x04009AB9 RID: 39609
		public static readonly Color MERCURYCEILINGLIGHT_LUX_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009ABA RID: 39610
		public static readonly Color MERCURYCEILINGLIGHT_COLOR = LIGHT2D.LIGHT_PINK;

		// Token: 0x04009ABB RID: 39611
		public static readonly Vector2 MERCURYCEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x04009ABC RID: 39612
		public static readonly Vector2 MERCURYCEILINGLIGHT_DIRECTIONVECTOR = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009ABD RID: 39613
		public const DiscreteShadowCaster.Direction MERCURYCEILINGLIGHT_DIRECTION = DiscreteShadowCaster.Direction.South;

		// Token: 0x04009ABE RID: 39614
		public static readonly Color LIGHT_PREVIEW_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009ABF RID: 39615
		public const float HEADQUARTERS_RANGE = 5f;

		// Token: 0x04009AC0 RID: 39616
		public const global::LightShape HEADQUARTERS_SHAPE = global::LightShape.Circle;

		// Token: 0x04009AC1 RID: 39617
		public static readonly Color HEADQUARTERS_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009AC2 RID: 39618
		public static readonly Color HEADQUARTERS_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009AC3 RID: 39619
		public static readonly Vector2 HEADQUARTERS_OFFSET = new Vector2(0.5f, 3f);

		// Token: 0x04009AC4 RID: 39620
		public static readonly Vector2 EXOBASE_HEADQUARTERS_OFFSET = new Vector2(0f, 2.5f);

		// Token: 0x04009AC5 RID: 39621
		public const float POI_TECH_UNLOCK_RANGE = 5f;

		// Token: 0x04009AC6 RID: 39622
		public const float POI_TECH_UNLOCK_ANGLE = 2.6f;

		// Token: 0x04009AC7 RID: 39623
		public const global::LightShape POI_TECH_UNLOCK_SHAPE = global::LightShape.Cone;

		// Token: 0x04009AC8 RID: 39624
		public static readonly Color POI_TECH_UNLOCK_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009AC9 RID: 39625
		public static readonly Color POI_TECH_UNLOCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009ACA RID: 39626
		public static readonly Vector2 POI_TECH_UNLOCK_OFFSET = new Vector2(0f, 3.4f);

		// Token: 0x04009ACB RID: 39627
		public const int POI_TECH_UNLOCK_LUX = 1800;

		// Token: 0x04009ACC RID: 39628
		public static readonly Vector2 POI_TECH_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009ACD RID: 39629
		public const float ENGINE_RANGE = 10f;

		// Token: 0x04009ACE RID: 39630
		public const global::LightShape ENGINE_SHAPE = global::LightShape.Circle;

		// Token: 0x04009ACF RID: 39631
		public const int ENGINE_LUX = 80000;

		// Token: 0x04009AD0 RID: 39632
		public const float WALLLIGHT_RANGE = 4f;

		// Token: 0x04009AD1 RID: 39633
		public const float WALLLIGHT_ANGLE = 0f;

		// Token: 0x04009AD2 RID: 39634
		public const global::LightShape WALLLIGHT_SHAPE = global::LightShape.Circle;

		// Token: 0x04009AD3 RID: 39635
		public static readonly Color WALLLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009AD4 RID: 39636
		public static readonly Color WALLLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009AD5 RID: 39637
		public static readonly Vector2 WALLLIGHT_OFFSET = new Vector2(0f, 0.5f);

		// Token: 0x04009AD6 RID: 39638
		public static readonly Vector2 WALLLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009AD7 RID: 39639
		public const float LIGHTBUG_RANGE = 5f;

		// Token: 0x04009AD8 RID: 39640
		public const float LIGHTBUG_ANGLE = 0f;

		// Token: 0x04009AD9 RID: 39641
		public const global::LightShape LIGHTBUG_SHAPE = global::LightShape.Circle;

		// Token: 0x04009ADA RID: 39642
		public const int LIGHTBUG_LUX = 1800;

		// Token: 0x04009ADB RID: 39643
		public static readonly Color LIGHTBUG_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009ADC RID: 39644
		public static readonly Color LIGHTBUG_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009ADD RID: 39645
		public static readonly Color LIGHTBUG_COLOR_ORANGE = new Color(0.5686275f, 0.48235294f, 0.4392157f, 1f);

		// Token: 0x04009ADE RID: 39646
		public static readonly Color LIGHTBUG_COLOR_PURPLE = new Color(0.49019608f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04009ADF RID: 39647
		public static readonly Color LIGHTBUG_COLOR_PINK = new Color(0.5686275f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04009AE0 RID: 39648
		public static readonly Color LIGHTBUG_COLOR_BLUE = new Color(0.4392157f, 0.4862745f, 0.5686275f, 1f);

		// Token: 0x04009AE1 RID: 39649
		public static readonly Color LIGHTBUG_COLOR_CRYSTAL = new Color(0.5137255f, 0.6666667f, 0.6666667f, 1f);

		// Token: 0x04009AE2 RID: 39650
		public static readonly Color LIGHTBUG_COLOR_GREEN = new Color(0.43137255f, 1f, 0.53333336f, 1f);

		// Token: 0x04009AE3 RID: 39651
		public const int MAJORFOSSILDIGSITE_LAMP_LUX = 1000;

		// Token: 0x04009AE4 RID: 39652
		public const float MAJORFOSSILDIGSITE_LAMP_RANGE = 3f;

		// Token: 0x04009AE5 RID: 39653
		public static readonly Vector2 MAJORFOSSILDIGSITE_LAMP_OFFSET = new Vector2(-0.15f, 2.35f);

		// Token: 0x04009AE6 RID: 39654
		public static readonly Vector2 LIGHTBUG_OFFSET = new Vector2(0.05f, 0.25f);

		// Token: 0x04009AE7 RID: 39655
		public static readonly Vector2 LIGHTBUG_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009AE8 RID: 39656
		public const int PLASMALAMP_LUX = 666;

		// Token: 0x04009AE9 RID: 39657
		public const float PLASMALAMP_RANGE = 2f;

		// Token: 0x04009AEA RID: 39658
		public const float PLASMALAMP_ANGLE = 0f;

		// Token: 0x04009AEB RID: 39659
		public const global::LightShape PLASMALAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x04009AEC RID: 39660
		public static readonly Color PLASMALAMP_COLOR = LIGHT2D.LIGHT_PURPLE;

		// Token: 0x04009AED RID: 39661
		public static readonly Color PLASMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009AEE RID: 39662
		public static readonly Vector2 PLASMALAMP_OFFSET = new Vector2(0.05f, 0.5f);

		// Token: 0x04009AEF RID: 39663
		public static readonly Vector2 PLASMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009AF0 RID: 39664
		public const int MAGMALAMP_LUX = 666;

		// Token: 0x04009AF1 RID: 39665
		public const float MAGMALAMP_RANGE = 2f;

		// Token: 0x04009AF2 RID: 39666
		public const float MAGMALAMP_ANGLE = 0f;

		// Token: 0x04009AF3 RID: 39667
		public const global::LightShape MAGMALAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x04009AF4 RID: 39668
		public static readonly Color MAGMALAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009AF5 RID: 39669
		public static readonly Color MAGMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009AF6 RID: 39670
		public static readonly Vector2 MAGMALAMP_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04009AF7 RID: 39671
		public static readonly Vector2 MAGMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009AF8 RID: 39672
		public const int BIOLUMROCK_LUX = 666;

		// Token: 0x04009AF9 RID: 39673
		public const float BIOLUMROCK_RANGE = 2f;

		// Token: 0x04009AFA RID: 39674
		public const float BIOLUMROCK_ANGLE = 0f;

		// Token: 0x04009AFB RID: 39675
		public const global::LightShape BIOLUMROCK_SHAPE = global::LightShape.Cone;

		// Token: 0x04009AFC RID: 39676
		public static readonly Color BIOLUMROCK_COLOR = LIGHT2D.LIGHT_BLUE;

		// Token: 0x04009AFD RID: 39677
		public static readonly Color BIOLUMROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009AFE RID: 39678
		public static readonly Vector2 BIOLUMROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04009AFF RID: 39679
		public static readonly Vector2 BIOLUMROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009B00 RID: 39680
		public const float PINKROCK_RANGE = 2f;

		// Token: 0x04009B01 RID: 39681
		public const float PINKROCK_ANGLE = 0f;

		// Token: 0x04009B02 RID: 39682
		public const global::LightShape PINKROCK_SHAPE = global::LightShape.Circle;

		// Token: 0x04009B03 RID: 39683
		public static readonly Color PINKROCK_COLOR = LIGHT2D.LIGHT_PINK;

		// Token: 0x04009B04 RID: 39684
		public static readonly Color PINKROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009B05 RID: 39685
		public static readonly Vector2 PINKROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04009B06 RID: 39686
		public static readonly Vector2 PINKROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;
	}
}
