using AscendedZ.entities;
using AscendedZ;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class Lab101 : CutsceneBase
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
            "You have once again overcome a major obstacle\nin your tertiary quest to conquer the Labrybuce.",
            "As a reward, I will gift you with\ninformation on the nature of my\nstrange request.",
            "There once was a world much like\nthe one you came from.",
            "In it lived a great hero to its people.",
            "This hero would eventually attempt a\nnoble quest to change the world.",
            "To his credit, he almost succeeded...",
            "...had his adversaries not cast a\nspell that was forbidden for millenia.",
            "This spell summoned the first Ascended game.",
            "Young Ascended, if you wish to\nunderstand the truth behind what\nI speak...",
            "Defeat the next Acolyte and Ascend!!!"
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
