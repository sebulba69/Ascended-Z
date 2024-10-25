using AscendedZ.entities;
using AscendedZ.screens.full_screen_boss;
using Godot;
using System;
using System.Threading.Tasks;

public partial class DrakallaBattleScene : FullScreenBossBase
{
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        await Task.Delay(200);

        var openingShotPlayer = GetNode<AnimationPlayer>("%OpeningScenePlayer");

        openingShotPlayer.Play("drakalla_opening_shot");

        _dialogBox = GetNode<AscendedTextbox>("%AscendedTextbox");
        _dialogBox.SetName(EnemyNames.Drakalla);
        _dialogBox.SetBlips("res://screens/cutscene/boss_male.wav");

        InternalReady();

        InitializeBattleScene(EnemyNames.Drakalla, true, false);

        await ToSignal(openingShotPlayer, "animation_finished");

        _actionMenu.CanInput = true;
    }
}