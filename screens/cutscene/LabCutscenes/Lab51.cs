using AscendedZ.entities;
using AscendedZ;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class Lab51 : CutsceneBase
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
            "You ascend to another Stratum of the Labrybuce.",
            "Suddenly, you hear a loud voice coming from on high!",
            "You stare into the abyss above you,\nsearching for the voice's origin.",
        ];

        string[] scene2 =
        [
            "Young Ascended...",
            "Ocura falls and you still climb.\nHow... admirable. Or, should I say... predictable?",
            "I have seen your progress so far, you have\nmuch strength, Young Ascended.",
            "Keep going! Yes, yes, ascend! I am eagerly\nawaiting your visit from on high!",
            "Every 50 floors, an Acolyte will face you!\nWhen you defeat them all, only\nthen can we finally meet!",
            "I cannot reveal much now, but you are\non the path of accomplishing something far greater\nthan what you think this game has to offer!",
            "Ascend, young one! Ascend!!!"
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
