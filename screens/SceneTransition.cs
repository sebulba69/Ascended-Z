using Godot;
using System;

public partial class SceneTransition : CanvasLayer
{
	private AnimationPlayer _player;

	public AnimationPlayer Player { get => _player; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_player = this.GetNode<AnimationPlayer>("%AnimationPlayer");
	}

	public void PlayFadeIn()
	{
		_player.Play("fade_in");
	}

	public void PlayFadeOut()
	{
		_player.Play("fade_out");
		_player.AnimationFinished += (anim) => QueueFree();
	}
}
