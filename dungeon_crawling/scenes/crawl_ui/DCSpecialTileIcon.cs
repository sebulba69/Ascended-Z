using AscendedZ.dungeon_crawling.backend;
using Godot;
using System;

public partial class DCSpecialTileIcon : HBoxContainer
{
	private TextureRect _image;
	private Label _coords;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_image = GetNode<TextureRect>("%ImagePicture");
		_coords = GetNode<Label>("%Coordinates");
	}

	public void SetSpecialTile(SpecialTile tile)
	{
		int x = 1;
		int y = 0;

		_image.Texture = ResourceLoader.Load<Texture2D>(tile.Graphic);
		_coords.Text = $"[{tile.Coordinates[x]}, {tile.Coordinates[y]}]";
	}
}
