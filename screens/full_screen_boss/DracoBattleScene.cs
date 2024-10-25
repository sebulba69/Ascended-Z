using AscendedZ.entities;
using AscendedZ.screens.full_screen_boss;
using Godot;
using System;
using System.Threading.Tasks;

public partial class DracoBattleScene : FullScreenBossBase
{
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        await Task.Delay(500);

        _dialogBox = GetNode<AscendedTextbox>("%AscendedTextbox");
        _dialogBox.SetName(EnemyNames.Draco);
        _dialogBox.SetBlips("res://screens/cutscene/boss_male.wav");

        InternalReady();

        InitializeBattleScene(EnemyNames.Draco, true, true);

        await ToSignal(GetNode<AnimationPlayer>("%OpeningScenePlayer"), "animation_finished");

        _actionMenu.CanInput = true;
    }
}
