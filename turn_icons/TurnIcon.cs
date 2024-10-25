using Godot;
using System;

public partial class TurnIcon : TextureRect
{
	private AnimationPlayer _animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = this.GetNode<AnimationPlayer>("%AnimationPlayer");
	}

    /// <summary>
    /// player_turn_full
	/// player_turn_half
	/// enemy_turn_full
	/// enemy_turn_half
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <param name="isHalfTurn"></param>
    public void SetIconState(bool isPlayer, bool isHalfTurn)
	{
		string iconType = (isPlayer) ? "player" : "enemy";
		string iconState = (isHalfTurn) ? "half" : "full";
		string animation = $"{iconType}_turn_{iconState}";
		_animationPlayer.Play(animation);
	}
}
