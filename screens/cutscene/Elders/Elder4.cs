using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class Elder4 : CutsceneBase
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
            "You arrive at a distorted version\nof some ruins you once saw early on\nin your quest.",
            "You hear rumbling off in the distance\nas something approaches your location...",
            "The shockwaves cause you to wait\nin anticipation for their source\nto arrive."
        ]);

        boxPlayer.Play("fade_out_box");
        player.Play("boss_intro_scene");
        await ToSignal(player, "animation_finished");
        boxPlayer.Play("fade_in_box");

        dialog.SetName(EnemyNames.Aiucxaiobhlo);
        dialog.SetNameBoxVisible(true);
        player.Play("zoom_in_on_boss");
        await ShowText(
            [
                "Hark! An Ascended, prancing through the\nWorld of Things Betwixt! Oh-ho-ho!",
                "This, good folk, is no ordinary feat!\nNay, nay! I must cast mine eyes upon\nit with mine own, lest I be labeled a fool!",
                "Ahhh, yes, no mistaking it now, is there?\nHeh-heh-heh! You, oh rising star, are\nno mere contestant, not at all!",
                "And what’s this? The party that trails\nin your wake?",
                "Ah, they lack the gleam, the glint of potential\nso dull in most who play this\nperilous game! Ha-ha-ha!",
                "Methinks some contestants have defected!\nFled, perhaps, from the mighty spectacle!",
                "Ahh, a jest most cunning, one might say...",
                "Though, not even we jesters could script such a twist!",
                "To think, oh-ho, that the day has come where\neven our own beloved game works against\nits very masters! Marvelous!",
                "Forsooth, what grandeur, what folly,\nthat a slayer of Elders emerges from within\nthese very trials! Ha-ha-ha!",
                "But come! Come forth! I welcome it,\nwith arms open wide and bells\na-jingling!",
                "Prove, ye fearless wanderer, to the\nvery laws that tether your universe.",
                "Prove that you are free, free to Ascend!"
            ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.ELDER_4, null);
    }
}