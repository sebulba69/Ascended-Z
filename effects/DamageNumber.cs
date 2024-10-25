using Godot;
using System;

public partial class DamageNumber : Label
{
	private const string GREEN = "9aff00";
	private const string RED = "ff6e67";
	private const string YELLOW = "ffff67";

	private string _displayColor;
	private string _displayText;

	private Tween _tween;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		this.Text = _displayText;
		this.AddThemeColorOverride("font_color",new Color(_displayColor));

		_tween = this.GetTree().CreateTween();
		_tween.TweenProperty(this, "position", new Vector2(this.Position.X, this.Position.Y - 50), 0.75f).AsRelative();
		_tween.Finished += () => this.QueueFree();
		_tween.Play();
    }

	public void SetDisplayLabrybuce(string display)
	{
		_displayText = display;
		_displayColor = YELLOW;
    }

	public void SetDisplayInfo(System.Numerics.BigInteger dmg, bool isHPGainedFromMove, string resultString)
	{
		if (isHPGainedFromMove)
		{
            _displayColor = GREEN;
            _displayText = $"+{dmg:n0}";
		}
		else
		{
            _displayColor = RED;
            _displayText = $"-{dmg:n0}";

			if (!string.IsNullOrEmpty(resultString))
			{
				_displayText += $"\n{resultString}";
			}
        }
	}
}
