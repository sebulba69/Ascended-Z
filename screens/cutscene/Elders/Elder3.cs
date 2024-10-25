using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class Elder3 : CutsceneBase
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
            "You decide to rest at a clearing in\nthe Endless Dungeon.",
            "You've faced 2 Elders so far,\nbut you feel that the next\none won't be as simple.",
            "As you think this, you sense an\nancient power emerging in front of you."
        ]);

        boxPlayer.Play("fade_out_box");
        player.Play("boss_intro_scene");
        await ToSignal(player, "animation_finished");
        boxPlayer.Play("fade_in_box");
        player.Play("zoom_in_on_boss");
        dialog.SetName(EnemyNames.Bhotldren);
        dialog.SetNameBoxVisible(true);
        await ShowText(
            [
                "Lo, an eon past, there wast a wight,\nfoolhardy as thee, who didst dare to rise\n'gainst the boundless might of the Elders...",
                "...seeking to o'ertop the confines of this\naccursed game.",
                "In mockery of his folly, we did\ngrant him parley, yet clothed in\nthe semblance of a proving.",
                "Through the crucibles of strength which he did\nendure, we vouchsafed him sight of the Ascended Deity\nof that age: Rickeus Martinius.",
                "With might unmatched, he did vanquish the\nSovereign, and from his ruin didst\nhe seize dominion o'er his realm.",
                "Yet thou, a mere worm, art but naught,\na defiler who doth besmirch the\nvery marrow of our hallowed strife.",
                "Thine impious machinations to undo\nthe 5 Elders, whose hands do\ngird the sanctity of this game...",
                "...shall be thwarted here,\nwretched churl.",
                "Stand now before the reckoning\nthat thy pride hath sown!"
            ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.ELDER_3, null);
    }
}
