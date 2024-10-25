using Godot;
using System;

public partial class SigilItemIcon : PanelContainer
{
	private TextureRect _pic;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pic = GetNode<TextureRect>("%SigilPic");
    }

	public void SetPic(string image)
	{
		_pic.Texture = ResourceLoader.Load<Texture2D>(image);
	}
}
