using AscendedZ;
using Godot;
using Godot.Collections;
using System;

public partial class EffectAnimation : Sprite2D
{
	private AnimationPlayer _player;
	private AudioStreamPlayer _audioStreamPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddUserSignal("EffectAnimationCompletedEventHandler");
		_player = this.GetNode<AnimationPlayer>("%AnimationPlayer");
		_audioStreamPlayer = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        _player.AnimationFinished += _OnAnimationCompleted;
	}

	public void PlayAnimation(string animationName)
	{
		string sfx = SfxAssets.EFFECT_SOUNDS[animationName];
        _audioStreamPlayer.Stream = ResourceLoader.Load<AudioStream>(sfx);
		_audioStreamPlayer.Play();
        _player.Play(animationName);
	}

    private void _OnAnimationCompleted(StringName animationName)
    {
		this.EmitSignal("EffectAnimationCompletedEventHandler");
    }
}
