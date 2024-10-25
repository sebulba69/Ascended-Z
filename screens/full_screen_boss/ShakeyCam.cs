using AscendedZ.effects;
using Godot;
using System;

public partial class ShakeyCam : Camera2D
{
    private ShakeParameters _shakeParameters;
    private RandomNumberGenerator _randomNumberGenerator;
    private AudioStreamPlayer _shakeSfx;

    private float _x;
    private Vector2 _originalPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _shakeParameters = new ShakeParameters();
        _randomNumberGenerator = new RandomNumberGenerator();
        _randomNumberGenerator.Randomize();
        _shakeSfx = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        _x = -1;
        _originalPosition = new Vector2(this.Position.X, 0);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (_shakeParameters.ShakeValue > 0)
        {
            _shakeParameters.ShakeValue = (float)Mathf.Lerp((double)_shakeParameters.ShakeValue, 0, (double)_shakeParameters.ShakeDecay * delta);
            float x = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            float y = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            this.Offset = new Vector2(_x + x, y);
        }
        else
        {
            _x = this.Offset.X;
            this.Offset = new Vector2(_x, _originalPosition.Y);
        }
    }

	public void Shake()
	{
        _shakeParameters.ShakeValue = _shakeParameters.ShakeStrength;
        _shakeSfx.Play();
    }
}
