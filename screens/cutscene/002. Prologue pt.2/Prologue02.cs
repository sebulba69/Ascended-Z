using AscendedZ;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class Prologue02 : CutsceneBase
{
    private readonly string FLASHBACK = "res://screens/cutscene/spring watch.ogg";

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
	{
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        audio.Play();

        dialog.SetName("???");
        dialog.SetNameBoxVisible(false);

        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        string[] scene1 = 
            [
                "You enter through the gates of the castle\nwall and end up in a small town.",
                "You see other contestants like yourself\nrunning around.",
                "Some are visiting shops, while\nothers are honing their weapons.",
                "One of them, standing by the\nwell in front of you, seems to\nbe calling you over.",
            ];

        player.Play("openingShot");
        await ShowText(scene1);

        boxPlayer.Play("fade_out_box");
        player.Play("panToFittotu");
        await Task.Delay(200);

        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        string[] scene2 =
        [
            "Erm, you there, yes...",
            "The front door, you went through it.",
            "You must be... a new Ascended, yes."
        ];
        await ShowText(scene2);

        dialog.SetName("Fittotu");
        string[] scene3 =
        [
            "I am Fittotu... yes.",
            "I am a guide for Ascended players.",
            "You. You are unable to\ncast magic, yes?",
            "By your clothes, I can see it.",
            "You are a peasant, like\nthose who came before you. Yes.",
            "As, erm, a guide... in this\ngame... I have been ordered\nto help, yes.",
            "In your, erm, world...\nmagic is... forbidden, yes.",
            ".........",
            "Oh, erm, this is sudden. Yes.",
            "You, erm, have not been exposed\nto the truth, yes.",
            "Allow me to explain:",
        ];
        await ShowText(scene3);

        boxPlayer.Play("fade_out_box");
        audio.Stop();
        audio.Stream = ResourceLoader.Load<AudioStream>(FLASHBACK);
        audio.Play();
        player.Play("fade_in_backstory");
        await ToSignal(player, "animation_finished");
        await Task.Delay(500);
        boxPlayer.Play("fade_in_box");
        string[] scene8 =
        [
            "I'm sure, erm, you're familiar\nwith the legends.",
            "Drakalla conquered the game. This game, yes.",
            "When he did, erm, he fulfilled his\ngoal of wiping out \"sorcery,\" yes.",
            "In doing so, he wiped out\nalmost all practioners... of magic.",
        ];
        await ShowText(scene8);

        player.Play("fade_out_the_boys");
        await ToSignal(player, "animation_finished");
        await Task.Delay(200);

        string[] scene9 =
        [
            "His goal... was to create a\nworld where power was, erm,\nequalized yes.",
            "By elimating sorcerers,\nthe practice of magic would,\nerm, go extinct.",
            "That... was what he must have\nbelieved, yes.",
            "But, erm, the opposite happened.",
            "A power vacuum was created, yes.",
            "Only society, its uppermost members, could learn it.",
            "By \"it\" I am of course,\nreferring to forbidden sorcery.",
            "You, a peasant, was forbade\nfrom, erm, progressing yes.",
            "But, the Ascended game, this game...",
            "...doesn't care about, erm, \"politics,\" yes.",
            "Since Drakalla's death, the\ngame, erm, almost expects you\nto use the \"dark arts.\" Yes."
        ];
        await ShowText(scene9);

        boxPlayer.Play("fade_out_box");
        player.Play("fade_out_flashback");
        await ToSignal(player, "animation_finished");
        await Task.Delay(200);

        boxPlayer.Play("fade_in_box");
        string[] scene10 =
        [
            "That is, erm... where I come in\nyes.",
            "I am, erm, a summoner who, erm,\nonce attempted this game.",
            "Sadly, I, erm, failed and, well...\nI am now bound here until the game\nis conquered.",
            "My power is, er, not restricted. Yes.",
            "As you, uh, know... yes...",
            "Those who die in the game, er,\nare forced to exist within it...",
            "...until the time comes when\na new victor is, er, crowned...",
            "Most are not as lucky, erm,\nas I and have been forced\nto serve the \"Floor Guardians.\"",
            "I'm sure you, erm,\nwill meet them yes.",
            "My point, erm, being...",
            "I can summon troops to aid\nyou on, erm, your quest.",
            "The game has bestowed unto\nyou a coin, yes, to be used\nfor my services.",
            "I recommend, erm, you get to\nsummoning quickly so you may, er,\nfree me, yes.",
        ];
        await ShowText(scene10);

        audio.Stop();
        boxPlayer.Play("fade_out_box");
        player.Play("panAwayFromFittotu");
        await Task.Delay(200);

        dialog.SetNameBoxVisible(false);
        boxPlayer.Play("fade_in_box");
        string[] scene11 =
        [
            "You check your pocket and realize you have 1 Party Coin.",
            "Party Coins can be used to purchase\nparty members from Fittotu in the\nRecruit menu.",
            "Make sure you check on the\nshop every so often for new updates!",
            "With Fittotu's help, you feel that\nyou ready yourself...",
            "Your long journey to the top\nstarts here!",
        ];
        await ShowText(scene11);
        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.MAIN, null);
    }

}
