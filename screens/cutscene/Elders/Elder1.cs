using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class Elder1 : CutsceneBase
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
            "You enter a completely barren part of\nthe Endless Dungeon.",
            "Suddenly, you feel an overwhelming\npresence."
        ]);

        dialog.SetNameBoxVisible(true);
        await ShowText(
        [
            "Lesser being...",
            "You have made an unthinkable mistake\nsummoning me here."
        ]);

        dialog.SetNameBoxVisible(false);
        await ShowText(
        [
            "You feel the air around you\nbecome heavy as something tries\nto materialize itself before you..."
        ]);

        boxPlayer.Play("fade_out_box");
        player.Play("boss_intro_scene");
        await ToSignal(player, "animation_finished");
        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        dialog.SetName(EnemyNames.Yacnacnalb);
        player.Play("zoom_in_on_boss");
        await ShowText(
            [
                $"From beyond time, I, {EnemyNames.Yacnacnalb}, whose name is\nknown throughout the cosmos, emerge\nbefore a contestant of yet another game.",
                "In its arrogance, it dares to violate\nthe natural order of this world.",
                "Lesser being who hails from the\nmortal realm claiming the right to\nAscension...",
                "I am but an observer to your kind,\nan overseer of your progress.",
                "By manifesting my being into this world, you\nhave committed a grave sin that will\nnot go unpunished.",
                "Come forth and face justice by my hand!"
            ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.ELDER_1, null);
    }
}
