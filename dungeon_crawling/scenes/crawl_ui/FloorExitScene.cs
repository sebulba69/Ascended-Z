using Godot;
using System;

public partial class FloorExitScene : CenterContainer
{
	private EndScreenOptions _endScreenOptions;

	public EndScreenOptions EndScreenOptions { get => _endScreenOptions; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_endScreenOptions = GetNode<EndScreenOptions>("%EndScreenOptions");
    }
}
