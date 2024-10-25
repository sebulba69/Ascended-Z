using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class FinalBossCutsceneLab : CutsceneBase
{
    private readonly string BLIPS_NOBLE_BUCE = "res://screens/cutscene/ddmale.wav";
    private readonly string BLIPS_DRACO_REVEAL = "res://screens/cutscene/boss_male.wav";
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        
        player.Play("opening_pan");
        dialog.SetName("Noble Buce");
        dialog.SetNameBoxVisible(false);
        dialog.SetBlips(BLIPS_NOBLE_BUCE);
        await Task.Delay(1000);
        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        dialog.SetNameBoxVisible(false);
        await ShowText(
        [
            "You reach the top of the Labrybuce.",
            "You finally made it.",
            "From what feels like every direction,\nyou hear a Noble Buce."
        ]);

        audio.Play();
        dialog.SetNameBoxVisible(true);
        await ShowText(
        [
            "You have done it, young Ascended.",
            "In your quest to seek the truth,\nyou have conquered the Labrybuce.",
            "On your way, I'm sure you've\nheard many things about this game...",
            "Things about the true nature of\nyour quest...",
            "That it's shallow, a placebo even...",
            "That you are not worthy enough\nto take part in the real Ascension\ngoing on under everyone's noses.",
            "These doubts, young Ascended, are\nnormal for someone in your situation.",
            "For even the greats that came\nbefore you had similar questions.",
            "However, in this particular case..."
        ]);

        audio.Stop();
        dialog.SetName("???");
        dialog.SetBlips(BLIPS_DRACO_REVEAL);
        await ShowText(
        [
            "Those doubts are correct!"
        ]);

        dialog.SetNameBoxVisible(false);
        await ShowText(
        [
            "You feel the energy around\nyou start to shift..."
        ]);

        boxPlayer.Play("fade_out_box");
        player.Play("boss_intro_scene_draco");
        await ToSignal(player, "animation_finished");
        boxPlayer.Play("fade_in_box");
        dialog.SetName("???");
        dialog.SetNameBoxVisible(true);
        await ShowText(
        [
            "Fascinating isn't it, this game's existence?",
            "Ascending and obtaining near-unlimited power?",
            "It's no wonder I see countless souls\nlike yourself come through and\ntake on this challenge!",
            "Thanks to the tall-tales created by\na renown warrior...",
            "An assembly line of sheep willing\nto die due to their lust for power\nwas born!",
            "You, who answered my call to reach\nthis place, are no different!",
            "HA HA HA HA!",
            "Ah, you seem confused.\nAllow me to introduce myself..."
        ]);

        dialog.SetName(EnemyNames.Draco);
        await ShowText(
        [
            $"I am {EnemyNames.Draco}.",
            "I need not say more as the legends created\ndue to that charlatan have given me\nenough credit as is!",
            "Confused? Don't be!",
            "For the first time in your futile existence,\nyou are about to be apart\nof something much larger than yourself!",
            "Power that far exceeds your\ncomprehension is on the line!",
            "Now that the Labrybuce has trained you,\nI can kill you and take your power\nfor myself...",
            "So that I, in the truest sense\nof the word...",
            "...may leave this petty game the\nElders have put on and Ascend!!!"
        ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.FINAL_BOSS_LAB, null);
    }
}
