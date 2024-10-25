using AscendedZ;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using Godot;
using System;

public partial class FullScreenBossHUD : Control
{
    private readonly PackedScene _statusScene = ResourceLoader.Load<PackedScene>(Scenes.STATUS);
    private readonly PackedScene _resistanceIcon = ResourceLoader.Load<PackedScene>(Scenes.RESISTANCE);

    private ProgressBar _hp;
	private Label _hpLabel, _nameLabel, _resistanceLabel;
	private HBoxContainer _statuses;
	private BattleEntity _entity;
    private HBoxContainer _resistanceContainer;
    private Button _button;

    public Button InfoButton { get => _button; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hp = GetNode<ProgressBar>("%HPBar");
		_hpLabel = GetNode<Label>("%HPLabel");
		_statuses = GetNode<HBoxContainer>("%Statuses");
		_nameLabel = GetNode<Label>("%NameLabel");
		_resistanceLabel = GetNode<Label>("%ResistanceLabel");
        _resistanceContainer = GetNode<HBoxContainer>("%ResistanceBox");
        _button = GetNode<Button>("%InfoButton");
    }

    private void SetResistances(BattleEntity entity)
    {
        foreach (var child in _resistanceContainer.GetChildren())
        {
            _resistanceContainer.RemoveChild(child);
        }

        if (entity.Resistances.HideResistances)
        {
            _resistanceLabel.Text = entity.Resistances.GetResistanceString();
            _resistanceContainer.Visible = false;
            _resistanceLabel.Visible = true;
        }
        else
        {
            var res = entity.Resistances.GetResistanceList();
            foreach (var r in res)
            {
                var iconScene = _resistanceIcon.Instantiate<ResistanceDisplay>();
                _resistanceContainer.AddChild(iconScene);
                iconScene.SetResistance(r.ResistanceType, r.ElementIcon);
            }
        }
    }

    public void Initialize(BattleEntity boss)
	{
		_entity = boss;
        _hp.MaxValue = boss.MaxHP;
        _hp.Value = _hp.MaxValue;
        _nameLabel.Text = boss.Name;
        UpdateValues();
    }

	public void UpdateValues()
	{
		_hp.Value = _entity.HP;
        _hpLabel.Text = $"{_entity.HP:n0} HP";

        SetResistances(_entity);

        // ... show statuses ... //
        // clear old statuses
        foreach (var child in _statuses.GetChildren())
        {
            _statuses.RemoveChild(child);
            child.QueueFree();
        }

        // place our new, updated statuses on scren
        var entityStatuses = _entity.StatusHandler.Statuses;

        foreach (var status in entityStatuses)
        {
            StatusIconWrapper statusIconWrapper = status.CreateIconWrapper();
            var statusIcon = _statusScene.Instantiate();
            _statuses.AddChild(statusIcon);

            statusIcon.Call("SetIcon", statusIconWrapper);
        }
    }
}
