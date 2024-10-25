using AscendedZ;
using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.skills;
using Godot;
using System;

public partial class BattleInfoBox : CenterContainer
{
	private TextureRect _enemyImage;
	private ItemList _skillsList;
	private RichTextLabel _enemyOverview, _scriptDescription;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_enemyImage = GetNode<TextureRect>("%EnemyImage");

		_enemyOverview = GetNode<RichTextLabel>("%EnemyOverview");
        _scriptDescription = GetNode<RichTextLabel>("%ScriptDescription");

		_skillsList = GetNode<ItemList>("%SkillList");

		GetNode<Button>("%BackButton").Pressed += QueueFree;
    }

	public void SetEnemyInfo(Enemy enemy)
	{
        _enemyImage.Texture = ResourceLoader.Load<Texture2D>(enemy.Image);
		_enemyOverview.Text = $"{enemy.Name}\nTurns: {enemy.Turns}\n{enemy.Resistances.GetResistanceString()}";
		_scriptDescription.Text = enemy.Description;

        foreach (ISkill skill in enemy.GetDisplaySkillList())
        {
            _skillsList.AddItem(skill.GetBattleDisplayString(), SkillAssets.GenerateIcon(skill.Icon));
        }
    }
}
