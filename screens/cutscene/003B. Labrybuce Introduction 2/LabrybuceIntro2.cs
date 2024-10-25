using AscendedZ.entities;
using AscendedZ;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class LabrybuceIntro2 : CutsceneBase
{
	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        dialog.SetName("Noble Buce");
        dialog.SetNameBoxVisible(false);

        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        dialog.SetBlips("res://screens/cutscene/ddmale.wav");

        string[] scene1 =
        [
            "You step into the Labrybuce.",
            "The area is surrounded by a strange\nBuce-like energy.",
            "You search the area for resources\nwhen suddenly, you hear a loud voice\ncoming from on high!",
            "You stare into the abyss above you,\nsearching for the voice's origin.",
        ];

        string[] scene2 =
        [
            "Young Ascended...",
            "You have travelled far to reach\nyour goal and now you are here...",
            "...forced to rely on the strength\nof others to complete your journey.",
            "Is your power truly your own or are you\nsimply using the power of others\nto fulfill your dreams?",
            "This is what the Labrybuce will test.",
            "In here, you will be challenged\nto push your party to its utmost\npotential!",
            "Use them to your advantage to conquer\nthis dungeon!",
            "For there is a secret to this game;\na secret that the first Ascended could\nnot discover the answer to.",
            "You and he both align in your ideals.",
            "I will be waiting on the floor\nbeyond floors, in the Buce realm...",
            "Find me so I may test you myself!",
            "Go forth... and Ascend!",
        ];

        string[] scene3 =
        [
            "The voice fades away.",
            "You decide to continue your adventure.",
        ];

        player.Play("opening_pan");
        await Task.Delay(800);
        boxPlayer.Play("fade_in_box");
        await ShowText(scene1);

        audio.Play();
        player.Play("pan_to_voice");
        dialog.SetNameBoxVisible(true);
        await ShowText(scene2);

        player.Play("pan_to_normal");
        dialog.SetNameBoxVisible(false);
        await ShowText(scene3);

        _OnSkipDialogEvent();
    }

    private async void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;

        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
        GetTree().Root.AddChild(transition);
        transition.PlayFadeIn();
        await ToSignal(transition.Player, "animation_finished");

        var dungeonScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_CRAWL).Instantiate<DungeonScreen>();
        this.GetTree().Root.AddChild(dungeonScene);
        await Task.Delay(150);
        dungeonScene.StartDungeon();

        transition.PlayFadeOut();
        QueueFree();
    }
}
