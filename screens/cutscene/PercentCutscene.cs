using AscendedZ;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class PercentCutscene : CutsceneBase
{
    private readonly string ELDER_BLIPS = "res://screens/cutscene/boss_male.wav";

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        dialog.SetBlips(ELDER_BLIPS);

        PersistentGameObjects.GameObjectInstance().MusicPlayer.ResetCurrentSong();
        await Task.Delay(200);

        dialog.SetName("Voices in Unison");
        dialog.SetNameBoxVisible(false);

        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        player.Play("openingShot");
        await ShowText(
            [
                "In what feels like an instant,\nthe two relics you obtained from\nyour previous battles disappear.",
                "You find yourself alone in what looks\nlike a distorted version of the town you started in.",
                "A strange aura can be felt near\nthe area's center...",
            ]);

        audio.Play();
        boxPlayer.Play("fade_out_box");
        await ToSignal(boxPlayer, "animation_finished");
        player.Play("fade_in_bosses");
        boxPlayer.Play("panToFittotu");
        await ToSignal(player, "animation_finished");
        await Task.Delay(200);
        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        await ShowText([
            "Mortal soul...",
            "We are the boundless wills of the\nAncient Ones, overseers of the very\nfabric of this existence.",
            "By some twisted anomaly, you have broken the veil\nand crossed into the World of Things Betwixt, where no\nmortal was ever meant to tread.",
            "In summoning our presence, you dare to tamper with forces\nthat bend even time and reality to their whim.",
            "Such audacity cannot go unanswered.",
            "Your intrusion defiles the sacred order,\na crime not only against us,\nbut against the laws that bind all creation.",
            "The cosmic balance you threaten\nis woven into every star,\nevery breath, every shadow.",
            "You have committed the gravest transgression,\ndaring to defy the eternal edicts of the universe itself.",
            "Such blasphemy against the primordial laws\ncannot be allowed to persist.",
            "Heed our warning:",
            "Continue onward and you will face the judgment\nof forces older than time,\nwiser than eternity.",
            "Together, we will shatter your path\nand render your journey stillborn!"
        ]);

        player.Play("fade_out_boss");
        dialog.SetNameBoxVisible(false);
        await ShowText(
        [
            "The strange beings fade into\nthe Labrybuce.",
            "You are left alone in this\nstrange dimension to overcome this\nobstacle and continue your quest.",
            "Your Ascended Power starts to flair up.",
            "You can now level up sigils\nfurther in the Sigil Menu.",
            "You can also now level up your\nskills further in the Buce-t Skills Menu."
        ]);

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