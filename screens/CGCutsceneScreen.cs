using AscendedZ;
using AscendedZ.screens;
using Godot;
using System;
using System.Threading.Tasks;

public partial class CGCutsceneScreen : Transitionable2DScene
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		await Task.Delay(200);
		GetNode<Button>("%NewGameButton").Pressed += Quit;
		await ToSignal(GetNode<AnimationPlayer>("%AnimationPlayer"), "animation_finished");
		Quit();
    }

	private void Quit()
	{
        GetNode<AudioStreamPlayer>("%AudioStreamPlayer").Stop();
        GetNode<Button>("%NewGameButton").Disabled = true;
        TransitionScenes(Scenes.START, null);
    }
}
