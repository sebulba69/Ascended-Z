using AscendedZ;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class LabrybuceIntro : CutsceneBase
{
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
	{
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        await Task.Delay(800);

        dialog.SetName("Fittotu");
        dialog.SetNameBoxVisible(false);

        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        string[] scene1 =
            [
                "You prepare to move on to the next\ntier when suddenly something catches your eye.",
                "Behind the tree is a familiar face..."
            ];

        await ShowText(scene1);

        boxPlayer.Play("fade_out_box");
        player.Play("tier9fitottu");
        await Task.Delay(200);

        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        string[] scene2 =
        [
            "Erm, my apologies, but...",
            "I have news to report about,\nerm, the hub area yes.",
            "It seems the Labrybuce is now available.",
        ];
        await ShowText(scene2);

        dialog.SetNameBoxVisible(false);
        string[] scene3 = [ "You ask for more information on\nthis \"Labrybuce.\"" ];
        await ShowText(scene3);

        dialog.SetNameBoxVisible(true);
        string[] scene4 =
        [
            "Erm, I cannot say. Yes.",
            "The most I can... confirm... is that I\nhear it is good for gathering\nresources.",
            "Apparently... the dungeon wasn't, erm\naround when Drakalla was, er, alive. Yes.",
            "It er, also came around, um,\nafter I died in, er, this game... Yes.",
            "Because of... erm, my predicament.\nI cannot go in myself.",
            "Only contestants are allowed. Yes.",
            "To... erm, understand... yes...\nYou will have to, er, explore it.",
            "By \"it\" I mean... erm...\nthe Labrybuce. Yes.",
        ];
        await ShowText(scene4);

        dialog.SetNameBoxVisible(false);
        string[] scene5 = 
        [
            "You decide you'll have to find\nthe answer you seek yourself.",
            "From here on out, you can access\nthe Labrybuce from the Embark menu.",
            "Good luck!",
        ];
        await ShowText(scene5);

        _OnSkipDialogEvent();
    }

    private async void BackToEndScreen()
    {
        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
        var root = this.GetTree().Root;

        root.AddChild(transition);

        transition.PlayFadeIn();

        await ToSignal(transition.Player, "animation_finished");

        this.Visible = false;

        transition.PlayFadeOut();
        await ToSignal(transition.Player, "animation_finished");
        try { transition.QueueFree(); } catch (Exception) { }
        QueueFree();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        BackToEndScreen();
    }
}
