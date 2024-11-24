using ImGuiNET;

public class DevToolMenuFontSize
{
	public enum FontSizeCategory
	{
		Fabric,
		Small,
		Regular,
		Large
	}

	public const string SETTINGS_KEY_FONT_SIZE_CATEGORY = "Imgui_font_size_category";

	private FontSizeCategory fontSizeCategory;

	public bool initialized { get; private set; }

	public void RefreshFontSize()
	{
		FontSizeCategory @int = (FontSizeCategory)KPlayerPrefs.GetInt("Imgui_font_size_category", 2);
		SetFontSizeCategory(@int);
	}

	public void InitializeIfNeeded()
	{
		if (!initialized)
		{
			initialized = true;
			RefreshFontSize();
		}
	}

	public void DrawMenu()
	{
		if (!ImGui.BeginMenu("Settings"))
		{
			return;
		}
		bool v = fontSizeCategory == FontSizeCategory.Fabric;
		bool v2 = fontSizeCategory == FontSizeCategory.Small;
		bool v3 = fontSizeCategory == FontSizeCategory.Regular;
		bool v4 = fontSizeCategory == FontSizeCategory.Large;
		if (ImGui.BeginMenu("Size"))
		{
			if (ImGui.Checkbox("Original Font", ref v) && fontSizeCategory != 0)
			{
				SetFontSizeCategory(FontSizeCategory.Fabric);
			}
			if (ImGui.Checkbox("Small Text", ref v2) && fontSizeCategory != FontSizeCategory.Small)
			{
				SetFontSizeCategory(FontSizeCategory.Small);
			}
			if (ImGui.Checkbox("Regular Text", ref v3) && fontSizeCategory != FontSizeCategory.Regular)
			{
				SetFontSizeCategory(FontSizeCategory.Regular);
			}
			if (ImGui.Checkbox("Large Text", ref v4) && fontSizeCategory != FontSizeCategory.Large)
			{
				SetFontSizeCategory(FontSizeCategory.Large);
			}
			ImGui.EndMenu();
		}
		ImGui.EndMenu();
	}

	public unsafe void SetFontSizeCategory(FontSizeCategory size)
	{
		fontSizeCategory = size;
		KPlayerPrefs.SetInt("Imgui_font_size_category", (int)size);
		ImGuiIOPtr iO = ImGui.GetIO();
		if ((int)size < iO.Fonts.Fonts.Size)
		{
			ImFontPtr imFontPtr = iO.Fonts.Fonts[(int)size];
			iO.NativePtr->FontDefault = imFontPtr;
		}
	}
}
