using System;
using ImGuiNET;

// Token: 0x02000BB2 RID: 2994
public class DevToolMenuFontSize
{
	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06003955 RID: 14677 RVA: 0x000C51E4 File Offset: 0x000C33E4
	// (set) Token: 0x06003954 RID: 14676 RVA: 0x000C51DB File Offset: 0x000C33DB
	public bool initialized { get; private set; }

	// Token: 0x06003956 RID: 14678 RVA: 0x0021F0E0 File Offset: 0x0021D2E0
	public void RefreshFontSize()
	{
		DevToolMenuFontSize.FontSizeCategory @int = (DevToolMenuFontSize.FontSizeCategory)KPlayerPrefs.GetInt("Imgui_font_size_category", 2);
		this.SetFontSizeCategory(@int);
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x000C51EC File Offset: 0x000C33EC
	public void InitializeIfNeeded()
	{
		if (!this.initialized)
		{
			this.initialized = true;
			this.RefreshFontSize();
		}
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x0021F100 File Offset: 0x0021D300
	public void DrawMenu()
	{
		if (ImGui.BeginMenu("Settings"))
		{
			bool flag = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Fabric;
			bool flag2 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Small;
			bool flag3 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Regular;
			bool flag4 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Large;
			if (ImGui.BeginMenu("Size"))
			{
				if (ImGui.Checkbox("Original Font", ref flag) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Fabric)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Fabric);
				}
				if (ImGui.Checkbox("Small Text", ref flag2) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Small)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Small);
				}
				if (ImGui.Checkbox("Regular Text", ref flag3) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Regular)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Regular);
				}
				if (ImGui.Checkbox("Large Text", ref flag4) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Large)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Large);
				}
				ImGui.EndMenu();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x0021F1D4 File Offset: 0x0021D3D4
	public unsafe void SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory size)
	{
		this.fontSizeCategory = size;
		KPlayerPrefs.SetInt("Imgui_font_size_category", (int)size);
		ImGuiIOPtr io = ImGui.GetIO();
		if (size < (DevToolMenuFontSize.FontSizeCategory)io.Fonts.Fonts.Size)
		{
			ImFontPtr wrappedPtr = *io.Fonts.Fonts[(int)size];
			io.NativePtr->FontDefault = wrappedPtr;
		}
	}

	// Token: 0x040026F1 RID: 9969
	public const string SETTINGS_KEY_FONT_SIZE_CATEGORY = "Imgui_font_size_category";

	// Token: 0x040026F2 RID: 9970
	private DevToolMenuFontSize.FontSizeCategory fontSizeCategory;

	// Token: 0x02000BB3 RID: 2995
	public enum FontSizeCategory
	{
		// Token: 0x040026F5 RID: 9973
		Fabric,
		// Token: 0x040026F6 RID: 9974
		Small,
		// Token: 0x040026F7 RID: 9975
		Regular,
		// Token: 0x040026F8 RID: 9976
		Large
	}
}
