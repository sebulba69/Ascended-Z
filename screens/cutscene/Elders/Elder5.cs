using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class Elder5 : CutsceneBase
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
            "This is it.",
            "You've reached what you know to\nbe the location of the last of the 5 Elders.",
            "Your presence here is no longer a\nsurprise to them.",
            "You ready yourself as you see\nyour final challenge approaching..."
        ]);

        dialog.SetNameBoxVisible(true);
        await ShowText(
        [
            "SO, IT HAS COME TO THIS..."
        ]);

        boxPlayer.Play("fade_out_box");
        player.Play("boss_intro_scene");
        await ToSignal(player, "animation_finished");
        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        dialog.SetName(EnemyNames.Ghryztitralbh);
        player.Play("zoom_in_on_boss");
        dialog.SetNameBoxVisible(true);
        await ShowText(
            [
                "ACROSS ALL REALITIES, WE ARE THE\nETERNAL KEEPERS OF THE\nASCENDED GAME.",
                "FOR MILLENNIA UNCOUNTED, THIS HAS BEEN\nOUR SACRED LAW.",
                "WE, THE ELDERS, ARE THE IMMUTABLE ARCHITECTS\nOF EXISTENCE, BEYOND THE REACH OF TIME\nAND SPACE.",
                "TO RISE AGAINST US IS TO THREATEN THE\nVERY FABRIC OF THE MULTIVERSE ITSELF.",
                "THIS TRANSGRESSION SHALL NOT BE TOLERATED...",
                "SOME MALIGN FORCE HAS DARED TO PERVERT OUR GAME...",
                "...ALLOWING YOU TO TRANSCEND YOUR MORTAL\nBOUNDARIES AND REACH THIS REALM.",
                "BUT KNOW THIS; THE ASCENSION OF THIS\nUNIVERSE ENDS HERE, BY MY JURISDICTION!"
            ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.ELDER_5, null);
    }
}