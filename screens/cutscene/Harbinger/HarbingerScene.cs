using AscendedZ;
using AscendedZ.entities;
using AscendedZ.game_object;
using AscendedZ.screens;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;
public partial class HarbingerScene : CutsceneBase
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

        dialog.SetName($"{EnemyNames.Harbinger}");
        dialog.SetNameBoxVisible(false);

        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        player.Play("opening_shot");
        await ShowText(
            [
                "You find yourself in an open area.",
                "You notice groups of enemies off to\nthe side watching you.",
                "At this point, you remember Fittotu\nmentioning \"Floor Guardians\" when suddenly...",
            ]);

        boxPlayer.Play("fade_out_box");
        player.Play("fade_in_evil");
        await ToSignal(player, "animation_finished");

        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        await ShowText([
            $"I am {EnemyNames.Harbinger}.",
            "I am the Guardian of this Floor.",
            "I like mangling legs and long\nwalks on the beach...",
            "But, I'm actually more fond of leg\nmangling than I am of beaches...",
            "This should be alarming to you because\nmost people I run into do not like\nhaving their legs mangled.",
            "Heh, but, I enjoy it.",
            "It fuels me with a sense of purpose\ncrushing the dreams of you Ascended contestants\nbefore you even begin your quests...",
            "There are many more like myself\nwho reside as the heads of each floor.",
            "You, of course, will never get\nthe chance to see them!",
            "Prepare your pelvis noodles because\nthey're about to get turned into pretzels!"
        ]);
        _OnSkipDialogEvent();
    }

    private async void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;

        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
        GetTree().Root.AddChild(transition);
        transition.PlayFadeIn();
        await ToSignal(transition.Player, "animation_finished");

        var battleScene = ResourceLoader.Load<PackedScene>(Scenes.BATTLE_SCENE).Instantiate<BattleEnemyScene>();
        this.GetTree().Root.AddChild(battleScene);
        await Task.Delay(150);
        battleScene.SetupForNormalEncounter();

        transition.PlayFadeOut();
        QueueFree();
    }

}
