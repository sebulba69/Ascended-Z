using AscendedZ.resistances;
using Godot;
using System;

public partial class ResistanceDisplay : Control
{
	private Icon _icon;
	private RichTextLabel _label;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_icon = GetNode<Icon>("%Icon");
		_label = GetNode<RichTextLabel>("%RichTextLabel");
	}

	public void SetResistance(ResistanceType type, string icon)
	{
		_icon.SetIcon(icon);

		_label.Text = type.ToString();
		if(type == ResistanceType.Wk)
		{
			_label.Text = $" [color=paleturquoise]{_label.Text}[/color]";
		}
		else if (type == ResistanceType.Nu)
		{
			_label.Text = $" [color=violet]{_label.Text}[/color]";
        }
		else if(type == ResistanceType.Rs)
		{
            _label.Text = $" [color=ivory]{_label.Text}[/color]";
        }
        else if (type == ResistanceType.Dr)
        {
            _label.Text = $" [color=00BFFF]{_label.Text}[/color]";
        }
    }
}
