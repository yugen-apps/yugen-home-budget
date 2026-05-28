using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Yugen.Home.Budget.Server.Helpers.Icons;

public static class IconHelper
{
	public static IconStorage IconTypes = new()
	{
		{ IconType.Filled, typeof(MudBlazor.Icons.Material.Filled) },
		{ IconType.Outlined, typeof(MudBlazor.Icons.Material.Outlined) },
		{ IconType.Rounded, typeof(MudBlazor.Icons.Material.Rounded) },
		{ IconType.Sharp, typeof(MudBlazor.Icons.Material.Sharp) },
		{ IconType.TwoTone, typeof(MudBlazor.Icons.Material.TwoTone) },
		{ IconType.Brands, typeof(MudBlazor.Icons.Custom.Brands) },
		{ IconType.FileFormats, typeof(MudBlazor.Icons.Custom.FileFormats) },
		{ IconType.Uncategorized, typeof(MudBlazor.Icons.Custom.Uncategorized) }
	};

	public static List<MudIcons> GetMudIconsByType(string type)
	{
		var iconType = IconTypes[type];
		return GetMudIconsByTypeCategory(iconType, type);
	}

	public static List<MudIcons> GetMudIconsByTypeCategory(Type iconType, string category)
	{
		return iconType
			.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
			.Select(prop => new MudIcons(prop.Name, GetIconCodeOrDefault(prop), category))
			.ToList();
	}

	public static string GetIconCodeOrDefault(FieldInfo fieldInfo) => fieldInfo.GetRawConstantValue()?.ToString() ?? string.Empty;

	public static string GetIconName(string iconName)
	{
		return string.IsNullOrWhiteSpace(iconName)
			? nameof(MudBlazor.Icons.Material.Filled.FormatBold)
			: iconName;
	}

	public static string GetIconCode(string iconCode)
	{
		return string.IsNullOrWhiteSpace(iconCode)
			? MudBlazor.Icons.Material.Filled.FormatBold
			: iconCode;
	}
}