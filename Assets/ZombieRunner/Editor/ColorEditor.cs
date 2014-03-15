using UnityEngine;
using System.Collections;

public class ColorEditor
{
	public static Color Title = new Color(62.0f/255.0f, 185.0f/255.0f,250.0f/255.0f);


	public static Color HexToRgb(uint color)
	{
		var red = (float)((color >> 16) & 0xFF) / 255.0f;
		var green = (float)((color >> 8) & 0xFF) / 255.0f;
		var blue = (float)(color & 0xFF) / 255.0f;
		return new Color(red, green, blue);
	}

	public static Color HexToRgba(uint color)
	{
		var alpha = ((float)((color >> 24) & 0xFF)) / 255.0f;
		var red = ((float)((color >> 16) & 0xFF)) / 255.0f;
		var green = ((float)((color >> 8) & 0xFF)) / 255.0f;
		var blue = ((float)(color & 0xFF)) / 255.0f;
		return new Color(red, green, blue, alpha);
	}

	public static uint ColorToHex(Color color)
	{
		var alpha = (uint)((float)color.a * 255.0f);
		var red = (uint)((float)color.r * 255.0f);
		var green = (uint)((float)color.g * 255.0f);
		var blue = (uint)((float)color.b * 255.0f);
		return alpha << 24 ^ red << 16 ^ green << 8 ^ blue;
	}

	public static Color RgbToColor(uint red, uint green, uint blue)
	{
		var redf = ((float)(red)) / 255.0f;
		var greenf = ((float)(green)) / 255.0f;
		var bluef = ((float)(blue)) / 255.0f;
		return new Color(redf, greenf, bluef);
	}

	public static Color RgbaToColor(uint red, uint green, uint blue, uint alpha)
	{
		var redf = ((float)(red)) / 255.0f;
		var greenf = ((float)(green)) / 255.0f;
		var bluef = ((float)(blue)) / 255.0f;
		var alphaf = ((float)(alpha)) / 255.0f;
		return new Color(redf, greenf, bluef, alphaf);
	}

	public static uint RgbaToHex(uint red, uint green, uint blue, uint alpha)
	{
		return alpha << 24 ^ red << 16 ^ green << 8 ^ blue;
	}
	
	public static uint RgbToHex(uint red, uint green, uint blue)
	{
		return red << 16 ^ green << 8 ^ blue;
	}
}

