using AscendedZ;
using AscendedZ.game_object;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class EndingA2 : CutsceneBase
{
    // Called when the node enters the scene tree for the first time.
    public async override void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        string name = "Player";

        try
        {
            name = PersistentGameObjects.GameObjectInstance().MainPlayer.Name;
        }
        catch (Exception) { }

        player.Play("1. openingShot");
        dialog.SetNameBoxVisible(false);
        await ToSignal(player, "animation_finished");
        player.Play("2. fade_in_box");

        await ShowText([
                "Once upon a time, in a land known\nas Asrafel...",
                "There was a game known as the Ascended game\nwhich forced the land into an endless\ncycle of combat.",
                $"A citizen known as {name} wished for a world\nwhere this ceaseless fighting would come\nto an end.",
                "In order to do so, they needed to\nconquer the game and Ascend.",
                "They were nothing more than a peasant\nbut their determination was a form of\nstrength above all else.",
                "To everyone's surprise, they took on every\nchallenge that came their way.",
                "Finally, with their group of souls\nwho wished for revenge against\nthe game that kept them hostage...",
                "...they worked together to put an\nEND to the game once and for all.",
                $"To prevent a new cycle of Ascension\nfrom beginning once more, {name} used\ntheir newfound power to cast a seal...",
                "...a seal that would prevent the\ngame they conquered from disappearing.",
                $"All other contestants were barred\nentry into the game while {name} worked to find\na permanent solution to its existence.",
                $"Thanks to their dilligence, {name} ushered\nin a new era of peace unlike anything\nDrakalla could have dreamed of.",
                "Magic was no longer barred from society\nand could be practiced freely.",
                $"{name}'s village would eventually\nsee prosperity unlike its ever seen before.",
                "Everyone celebrated their success in\nconquering the game.",
                $"Despite changing the world for the\nbetter, {name} still had several questions\nat the back of their mind.",
                "Can the Ascended cycle truly end?\nWill this age of peace be temporary\nlike it was before?",
                $"With the now-conquered Ascended game\nstill operational due to {name}'s magic,\nonly time will tell...",
                "...what it truly means to Ascend."
            ]);

        player.Play("fade_out_box");
        await ToSignal(player, "animation_finished");
        player.Play("fade_to_black");
        await ToSignal(player, "animation_finished");

        GetNode<Label>("%MainAscendedLabel").Text = "The End...?";
        player.Play("fade_in_ascended_label");
        await ToSignal(player, "animation_finished");

        GetNode<AnimationPlayer>("%FadeOutSong").Play("fade_out_song");

        await Task.Delay(1000);

        player.Play("2. fade_in_box");
        await ShowText(
        [
            "Normal Route: End",
            "Thank you for playing Ascended Z.",
            "You will be sent back to the\nStart Screen shortly.",
            "Your progress from this session\nhas automatically been saved for you.",
            "If you'd like to re-enter the\ngame, you can do so by reloading\nyour save.",
            "Once again, congrats on ascending, AND GOODBYE."
        ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.START, null);
    }
}
