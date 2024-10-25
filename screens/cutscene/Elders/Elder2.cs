using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class Elder2 : CutsceneBase
{
    private readonly string ELDER_BLIPS = "res://screens/cutscene/boss_male.wav";

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        player.Play("opening_pan");
        dialog.SetName("???");
        dialog.SetNameBoxVisible(false);
        dialog.SetBlips(ELDER_BLIPS);

        await Task.Delay(1000);
        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        await ShowText(
        [
            "You stop in your tracks after\nclearing the previous floor.",
            "You can sense it, another being\nof great power is attempting to\nappear before you..."
        ]);

        boxPlayer.Play("fade_out_box");
        player.Play("boss_intro_scene");
        await ToSignal(player, "animation_finished");
        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        dialog.SetName(EnemyNames.Mhaarvosh);
        player.Play("zoom_in_on_boss");
        await ShowText(
            [
                "Ho ho ho ho! It's happening again!",
                "Across time and space, another\nAscended has decided to break\nthe boundaries of the game!",
                "You've already claimed one of\nour heads yes?",
                "Truly, your potential rivals the\nfirst of your kind!",
                "Oh, excuse me...",
                "I've forgotten that, as a\nlesser being, you wouldn't be aware...",
                "...of the first true Ascended who\nclaimed the throne in another universe\nfar off from this one!",
                "Despite that, you've chosen to\ngo a different route!",
                "You've decided to eradicate a\nUniversal Constant!",
                "Lesser being of a feeble existence,\nallow me to congratulate you here!",
                "In honor of your triumph, I\nwill slay you myself that you may\ntake my fallen comrade's place!",
                "Submit yourself to me that I\nmay guide you to a new form of\nexistence!"
            ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.ELDER_2, null);
    }
}
