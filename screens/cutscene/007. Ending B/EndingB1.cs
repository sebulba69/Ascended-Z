using AscendedZ;
using AscendedZ.game_object;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class EndingB1 : CutsceneBase
{
    // Called when the node enters the scene tree for the first time.
    public async override void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));
        dialog.SetNameBoxVisible(false);
        string name = "Player";

        try
        {
            name = PersistentGameObjects.GameObjectInstance().MainPlayer.Name;
        }
        catch (Exception) { }
        player.Play("2. fade_in_box");
        await ShowText([
            "The area around you immediately goes\ndark after you strike the final blow.",
            "You can hear Drakalla's voice."
        ]);

        dialog.SetName("Drakalla");
        dialog.SetNameBoxVisible(true);
        dialog.SetBlips("res://screens/cutscene/boss_male.wav");
        await ShowText([
            "AScenDeeEedd...",
            "ThE patH you WaLk is a DANGerOUs one...",
            "Do NOT repEAT the SAMe\nMiSTAKes I dId duRIng My TIME...",
            "..OR elSE We WILL bE seeING each\nOTHEr AGaIN... in the BATTLEfiELD of the\nNEXT game...",
        ]);

        await ShowText([
            "His voice fades into obscurity.",
            "You can feel the aether around you\ncollapsing in on the area.",
            "The dimension you fought in is falling apart!",
            "After killing Drakalla, it was no\nlonger able to support itself.",
            "You feel a strange sension pulling\nyou from behind...",
            "It seems as though both Drakalla's game\nand your game are starting to crumble.",
            "You've accomplished your goal!",
            "You have... in the truest sense\nof the word...",
            "Ascended."
        ]);
        player.Play("fade_out_box");
        await ToSignal(player, "animation_finished");
        player.Play("fade_in_bg");
        await ToSignal(player, "animation_finished");
        player.Play("1. openingShot");
        dialog.SetNameBoxVisible(false);
        await ToSignal(player, "animation_finished");
        player.Play("2. fade_in_box");
        dialog.SetBlips("");
        await ShowText([
                "Once upon a time, in a land known\nas Asrafel...",
                "There were two heroes:",
                "The Mage of Cosmos, Drakalla...",
                $"And {name}, The Ascended.",
                "Both heroes came into the world with\none goal: usher in an age of peace.",
                "One bred a society of weakness, while\nthe other stood on their behalf.",
                "In the end, these two would clash\nat the edge of the world for their\nideals.",
                $"In the process, {name} would slay\ngods of incomprehensible power...",
                "...and discover the truth behind Drakalla's\nlegends which had been passed down for\ngenerations.",
                "The power struggle the world was\nthrown into would be put to an end.",
                "The souls of the fallen trapped within\nthe Ascended game's many layers were\nreleased...",
                $"...and {name} could rest easy knowing\nthat they not only proved to the\nworld that they could overcome the game...",
                "...but that they, indeed, were worthy\nof Ascension."
            ]);

        player.Play("fade_out_box");
        await ToSignal(player, "animation_finished");
        
        GetNode<Sprite2D>("%CutsceneEntity").Texture = ResourceLoader.Load<Texture2D>(PersistentGameObjects.GameObjectInstance().MainPlayer.Image);
        player.Play("fade_in_end_screen");
        await ToSignal(player, "animation_finished");
        dialog.SetNameBoxVisible(true);
        dialog.SetName(name);
        player.Play("2. fade_in_box");
        await ShowText([
            "Hello!",
            $"I am {name}, but you probably\nalready knew that.",
            "After all, YOU are the one who named me!",
            "Thank you for helping me accomplish\nmy goal of Ascending.",
            "I know it wasn't easy, but we\nmanaged to do it!",
            "We made a great team!",
            "I hope to see you again on\nour next Ascended adventure!",
            "Goodbye, and THANK YOU FOR PLAYING!!!!"
        ]);
        dialog.SetNameBoxVisible(false);
        player.Play("fade_out_box");
        await ToSignal(player, "animation_finished");
        player.Play("fade_out_end_screen");
        await ToSignal(player, "animation_finished");

        GetNode<Label>("%MainAscendedLabel").Text = "THE END!";
        player.Play("fade_in_ascended_label");
        await ToSignal(player, "animation_finished");

        GetNode<AnimationPlayer>("%FadeOutSong").Play("fade_out_song");

        await Task.Delay(1000);

        player.Play("2. fade_in_box");
        await ShowText(
        [
            "100% Route: End",
        ]);

        GetNode<Label>("%MainAscendedLabel").Visible = false;
        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.CREDITS, null);
    }
}
