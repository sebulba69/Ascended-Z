using AscendedZ.entities;
using AscendedZ.entities.enemy_objects;
using Godot;
using System;

public partial class BossHPBar : HBoxContainer
{
	private ProgressBar _hp;
	private Label _barsLabel;
	private BossHP _bossHPBar;
	private StyleBoxFlat _back, _front;

    private readonly string FRONT = "res://FrontBoxBossHP.tres";
	private readonly string BACK = "res://BackBoxBossHP.tres";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_hp = this.GetNode<ProgressBar>("%HPBar");
		_barsLabel = this.GetNode<Label>("%NumberOfBarsLabel");
		_front = (StyleBoxFlat)ResourceLoader.Load<StyleBoxFlat>(FRONT).Duplicate();
		_back = (StyleBoxFlat)ResourceLoader.Load<StyleBoxFlat>(BACK).Duplicate();

        _hp.AddThemeStyleboxOverride("background", _back);
        _hp.AddThemeStyleboxOverride("fill", _front);
    }

	public void InitializeBossHPBar(int hp)
	{
		_bossHPBar = new BossHP();
        _bossHPBar.Setup(hp);
        _bossHPBar.InitializeBossHP(hp);
        UpdateBossHPUI();
    }

	public void UpdateBossHP(int hp)
	{
        _bossHPBar.ChangeHP(hp);
		UpdateBossHPUI();
    }

	private void UpdateBossHPUI()
	{
        BossHPStatus status = _bossHPBar.GetBossHPUIValues();

        _barsLabel.Text = $"x{status.NumBars}";
        _hp.MaxValue = status.MaxBarHP;
		_hp.Value = status.CurrentBarHP;

		_back.BgColor = status.BG;
		_front.BgColor = status.FG;
    }
}
