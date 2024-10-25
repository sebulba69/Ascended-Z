using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.game_object;
using AscendedZ;
using AscendedZ.screens.full_screen_boss;
using Godot;
using System;
using System.Collections.Generic;
using static Godot.Projection;
using AscendedZ.entities;
using System.Threading.Tasks;

public partial class NetallaBattleScene : FullScreenBossBase
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
        await Task.Delay(500);

        _dialogBox = GetNode<AscendedTextbox>("%AscendedTextbox");
        _dialogBox.SetName(EnemyNames.Nettala);
        _dialogBox.SetBlips("res://screens/cutscene/female.wav");

        InternalReady();

        InitializeBattleScene(EnemyNames.Nettala, true, false);

        await ToSignal(GetNode<AnimationPlayer>("%OpeningScenePlayer"), "animation_finished");

        _actionMenu.CanInput = true;
    }
}
