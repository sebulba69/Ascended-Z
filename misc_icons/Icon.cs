using AscendedZ;
using Godot;
using System;

public partial class Icon : TextureRect
{
	public void SetIcon(string icon)
	{
		this.Texture = SkillAssets.GenerateIcon(icon);
		this.TooltipText = icon;
	}
}
