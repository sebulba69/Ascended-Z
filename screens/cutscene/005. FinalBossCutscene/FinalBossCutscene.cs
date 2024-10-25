using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class FinalBossCutscene : CutsceneBase
{
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
        dialog.SetBlips("res://screens/cutscene/female.wav");
        await Task.Delay(1000);
        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        

        await ShowText(
        [
            "You ascend up a beam of energy and\nfind yourself in the ruins of a\nlong lost town.",
            "You can see the buce energy destroying\nthe ground beneath you.",
            "Suddenly, you sense an extremely\npowerful presence nearby."
        ]);

        boxPlayer.Play("fade_out_box");
        audio.Play();
        player.Play("boss_intro_scene");
        await ToSignal(player, "animation_finished");
        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        // debug
        string name = "Player";
        try
        {
            name = PersistentGameObjects.GameObjectInstance().MainPlayer.Name;
        }
        catch (Exception) { }

        await ShowText(
            [
                $"Welcome, {name}...",
                "I've been expecting you.",
                "This is your last stop before\nreceiving your reward."
            ]);

        dialog.SetName(EnemyNames.Nettala);
        await ShowText(
            [
                $"I am {EnemyNames.Nettala}, the Scholar of Buce.",
                "I am the reason Drakalla conquered\nthe first Ascended game.",
                $"{name}, I know why you are here.",
                "You, like those who came before my old\nfriend, are seeking to claim the power\nof Ascension for yourself.",
                "I cannot fault you, for we once\nthought in the same way as you did.",
                "It was only after perishing at the\nhands of the former ruler of this\ndungeon that I learned the truth.",
                "The endless cycle of Ascension cannot\ncome about by virtue of completing the game.",
                "The winner must have power beyond\nall that is thought possible...",
                "...to make sure the game does\nnot come back again.",
                $"{name}, I say unto you this:",
                "Drakalla was not strong enough to\nmaintain his society devoid of magic.",
                "The Elders knew this and, therefore,\nreinstated the game once more.",
                "You, as a non-magic user, have the\nopportunity to change that!",
                "You can rise up above your predecessors\nand up-end society as you know it!",
                "You can defy your destiny and Ascend!",
                "To that end, I, as the new keeper\nof the Ascended game...",
                "...shall see if you are truly worthy!!!"
            ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.FINAL_BOSS, null);
    }
}
