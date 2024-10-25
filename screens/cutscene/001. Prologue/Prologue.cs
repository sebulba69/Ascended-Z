using AscendedZ;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;

public partial class Prologue : CutsceneBase
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<AnimationPlayer>("%CutscenePlayer");
		dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
		audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
		player.Play("1. openingShot");

		dialog.SetNameBoxVisible(false);
        Start();
    }

	public async void Start()
	{
        string[] monolog =
            [
                "Once upon a time, in a land known\nas Asrafel...",
                "There was a citizen known as Drakalla, a half-human\nhalf dragon hybrid who wanted to kill all\nthe sorcerers.",
                "In order to do so, he needed\nto slay the villain, Draco: the\nmagistrate of evil!",
                "To that end, he sought power beyond\nhis wildest imagination.",
                "Unbeknownst to him, he would unleash\na curse onto the land of Asrafel\nthat would change everything.",
                "Thanks to Nettala, the Scholar of Buce...",
                "Drakalla discovered a game that would\nallow the victor to gain absolute power.",
                "With a desire to defeat Draco,\nhe conquered the game and Ascended.",
                "For the next 100 years, he would\nbring a new age of peace to Asrafel...",
                "...until, to his shock, the game\nreturned, beckoning more challengers\nto overcome it.",
                "Drakalla, in an attempt to avoid a\nstruggle for power due to its existence,\ntook on the game once more.",
                "However, this wasn't the same game\nas before. It was much more challenging...",
                "...and it only worsened as more\ncontestants failed to overcome it.",
                "With each failed attempt, the game would\nsteal a contestant's power for itself,\nmaking it deadlier than before.",
                "Drakalla would disappear during his attempt,\npresumed to be dead by the hands of the\nnew Ascended game.",
                "With him gone, there was\nno hope of anyone winning...",
                "Until today...",
                "You are a villager from on far with\nthe hopes of proving your worth by\ntaking on the game.",
                "Despite the warnings from your peers\nyou are steadfast in the path you've chosen.",
                "You wish to succeed where Drakalla\nfailed.",
                "You want to conquer the game and\nput an end to its recurring cycle.",
                "All that's left is to answer the question...",
                "Do you have what it takes to Ascend?",
            ];

        await ToSignal(player, "animation_finished");
        dialog.DisplayText(monolog[0]);
        player.Play("2. fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        await ToSignal(dialog, "ReadyForMoreDialogEventHandler");

        for (int m = 1; m < monolog.Length; m++)
        {
            dialog.DisplayText(monolog[m]);
            await ToSignal(dialog, "ReadyForMoreDialogEventHandler");
        }

        _OnSkipDialogEvent();
    }

	private void _OnSkipDialogEvent()
	{
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.PROLOG_2, null);
    }
}
