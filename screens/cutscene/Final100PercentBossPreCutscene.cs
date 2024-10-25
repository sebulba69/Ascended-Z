using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class Final100PercentBossPreCutscene : CutsceneBase
{
    private readonly string ELDER_BLIPS = "res://screens/cutscene/boss_male.wav";

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        PersistentGameObjects.GameObjectInstance().MusicPlayer.ResetCurrentSong();
        await Task.Delay(200);

        dialog.SetName("Fittotu");
        dialog.SetNameBoxVisible(false);

        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        player.Play("openingShot");
        await ShowText(
            [
                "You've done it.",
                "The Elders that presided over the\nAscended game are no more.",
                "You can feel the buce energy around\nyou start to swirl as the world tries to adjust to\nthis newfound change you've brought forward.",
                "Deep down, though, you know this\nisn't truly the end.",
                "Draco somehow appearing in the Labrybuce...\nNetalla being the final challenge for your Ascended game...",
                "Your journey in the World of Things Betwixt\nmirroring your adventure to reach this point...",
                "Everything points to one, obvious conclusion:",
            ]);

        boxPlayer.Play("fade_out_box");
        await ToSignal(boxPlayer, "animation_finished");
        await Task.Delay(1000);
        player.Play("fade_in_fittotu");
        await ToSignal(player, "animation_finished");

        player.Play("panToFittotu");
        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        await ShowText([
            ".........",
            "Hello again...",
            "You, erm, don't seem surprised.\nUm... to see me here, yes.",
            "I guess, erm, that was... to be\nexpected. Yes.",
            "Contestants couldn't use, er, magic\nso... it was weird that, erm...",
            "I could somehow... do it. Yes.",
            "..........",
            "Well! Erm, you look like, er,\nI should get to the point. Yes.",
        ]);

        dialog.SetName("Fittotu?");
        await ShowText(
            [
                "The point is, you've managed to\nundo the spell the Elders placed on this world.",
                "The \"World of Things Betwixt\" as they called it.",
                "Of course, this is not its true name.",
                "No, the world you're in should be\nquite familiar to you.",
            ]);

        dialog.SetBlips(ELDER_BLIPS);
        await ShowText(
        [
            "For it is a land known as Asrafel."
        ]);

        boxPlayer.Play("fade_out_box");
        player.Play("fade_out_boss");
        await ToSignal(player, "animation_finished");

        player.Play("boss_appears");
        await ToSignal(player, "animation_finished");

        dialog.SetName($"{EnemyNames.Drakalla}");
        boxPlayer.Play("fade_in_box");
        player.Play("slow_cam_zoom");
        await ShowText(
        [
            "Congratulations, Young Ascended.",
            "You've managed to undo the Elder's game\nand released me from my century-long punishment.",
            "Now, the game can attempt to resume\nfrom the point at which I defected\nagainst the Elders all those years ago.",
            "Of course, your goal is to end the\ncycle of Ascension all together, yes?",
            "Then, I'm sure you're aware of what\nmust happen next.",
            "In the same way you are keeping\nthe game you were victorious in alive,\nmy presence does much the same for this world.",
            "To escape it would require you killing\noff the only being holding it together.",
            "Seeing as how our goals are one\nin the same, I cannot truly end the\nAscended game unless you perish...",
            "...and you cannot end your game\nuntil I perish.",
            "We are victims of circumstance\nwith the same ideals.",
            "I, however, am not simply going\nto lie down and sacrifice myself for\nthe sake of others...",
            "...for that is the mentality of losers.",
            "I assume you will not throw your\nlife away when you too are on the\ncusp of accomplishing your dream.",
            "There is nothing more to be said on the matter.",
            "Let's see if your efforts compare to\nmy limitless knowledge of the forbidden arts!",
            "Face me here in the place where\nboth of our adventures began!",
            "Have at you!"
        ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.FINAL_BOSS_100_PERCENT, null);
    }

}