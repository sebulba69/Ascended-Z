using AscendedZ;
using AscendedZ.battle;
using AscendedZ.effects;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.skills;
using AscendedZ.statuses;
using Godot;
using System;
using System.Threading.Tasks;


public partial class EntityDisplayBox : PanelContainer
{
    private readonly PackedScene _damageNumScene = ResourceLoader.Load<PackedScene>(Scenes.DAMAGE_NUM);
    private readonly PackedScene _statusScene = ResourceLoader.Load<PackedScene>(Scenes.STATUS);
    private readonly PackedScene _resistanceIcon = ResourceLoader.Load<PackedScene>(Scenes.RESISTANCE);

    private Sprite2D _effect;
    private AudioStreamPlayer _shakeSfx;
    private Vector2 _originalPosition;
    private GridContainer _statuses;
    private Label _resistances;
    private Label _hp;
    private float _x;
    private HBoxContainer _resistanceContainer;

    private Texture2D _entityImage;
    private Texture2D _deadImage;

    // screen shake
    private ShakeParameters _shakeParameters;
    private RandomNumberGenerator _randomNumberGenerator;

    private Button _button;
    public Button InfoButton { get => _button; }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("EffectPlayed");

        _shakeParameters = new ShakeParameters();
        _randomNumberGenerator = new RandomNumberGenerator();
        _randomNumberGenerator.Randomize();

        _resistanceContainer = GetNode<HBoxContainer>("%ResistanceBox");

        _effect = this.GetNode<Sprite2D>("%EffectSprite");
        _shakeSfx = this.GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
        _statuses = this.GetNode<GridContainer>("%Statuses");
        _resistances = this.GetNode<Label>("%ResistanceLabel");
        _hp = this.GetNode<Label>("%HPLabel");
        _x = -1;
        _originalPosition = new Vector2(this.Position.X, 0);

        _deadImage = ResourceLoader.Load<Texture2D>("res://entity_pics/dead_entity.png");
    }

    /// <summary>
    /// We're going to use Process to reset the strength of our Screen Shake back to 0.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta)
    {
        if(_shakeParameters.ShakeValue > 0)
        {
            _shakeParameters.ShakeValue = (float)Mathf.Lerp((double)_shakeParameters.ShakeValue, 0, (double)_shakeParameters.ShakeDecay * delta);
            float x = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            float y = _randomNumberGenerator.RandfRange(-_shakeParameters.ShakeValue, _shakeParameters.ShakeValue);
            this.Position = new Vector2(_x + x, y);

            if (_shakeParameters.ShakeValue < 1 && _shakeParameters.ShakeValue > 0)
                _shakeParameters.ShakeValue = 0;
        }
        else
        {
            _x = this.Position.X;
            this.Position = new Vector2(_x, _originalPosition.Y);
        }
    }

    public void SetEnemyInfo(Enemy enemy)
    {
        _button = GetNode<Button>("%InfoButton");
    }

    public void InstanceEntity(EntityWrapper wrapper, bool random=false)
    {
        Label name = this.GetNode<Label>("%NameLabel");
        TextureRect picture = this.GetNode<TextureRect>("%Picture");

        BattleEntity entity = wrapper.BattleEntity;

        if (wrapper.IsBoss)
        {
            var bossHP = this.GetNode<BossHPBar>("%HP");
            bossHP.InitializeBossHPBar(entity.HP);
        }
        else
        {
            var hp = this.GetNode<TextureProgressBar>("%HP");
            hp.MaxValue = entity.MaxHP;
            hp.Value = hp.MaxValue;
        }

        name.Text = entity.Name;
        _hp.Text = $"{entity.HP:n0} HP";

        SetResistances(entity);

        _entityImage = ResourceLoader.Load<Texture2D>(entity.Image);
        picture.Texture = _entityImage;
    }

    private void SetResistances(BattleEntity entity)
    {
        foreach(var child in _resistanceContainer.GetChildren())
        {
            _resistanceContainer.RemoveChild(child);
        }

        var res = entity.Resistances.GetResistanceList();
        foreach(var r in res)
        {
            var iconScene = _resistanceIcon.Instantiate<ResistanceDisplay>();
            _resistanceContainer.AddChild(iconScene);
            iconScene.SetResistance(r.ResistanceType, r.ElementIcon);
        }
    }

    public void UpdateEntityDisplay(EntityWrapper wrapper)
    {
        var entity = wrapper.BattleEntity;

        // ... change hp status ... //
        // set HP values
        if (wrapper.IsBoss)
        {
            var bossHP = this.GetNode<BossHPBar>("%HP");
            bossHP.UpdateBossHP(entity.HP);
        }
        else
        {
            var hp = this.GetNode<TextureProgressBar>("%HP");
            hp.Value = entity.HP;
        }

        TextureRect picture = this.GetNode<TextureRect>("%Picture");
        if (entity.HP == 0)
        {
            picture.Texture = _deadImage;
            _resistanceContainer.Visible = false;
        }
        else
        {
            if (picture.Texture != _entityImage)
            {
                picture.Texture = _entityImage;
                _resistanceContainer.Visible = true;
            }
        }

        SetResistances(entity);

        // ... change active status ... //
        // change active status if it's a player (players have the graphic, enemies don't)
        if (entity.GetType().Equals(typeof(BattlePlayer)))
        {
            
            TextureRect activePlayerTag = this.GetNode<TextureRect>("%ActivePlayerTag");
            CenterContainer spacer = this.GetNode<CenterContainer>("%Spacer");
            if (activePlayerTag.Visible != entity.IsActiveEntity)
            {
                activePlayerTag.Visible = entity.IsActiveEntity;
                spacer.Visible = entity.IsActiveEntity;
            }
                
        }

        this.GetNode<Label>("%NameLabel").Text = entity.Name;
        _hp.Text = $"{entity.HP:n0} HP";

        // ... show statuses ... //
        // clear old statuses
        foreach (var child in _statuses.GetChildren())
        {
            _statuses.RemoveChild(child);
            child.QueueFree();
        }

        // place our new, updated statuses on scren
        var entityStatuses = entity.StatusHandler.Statuses;

        foreach (var status in entityStatuses)
        {
            StatusIconWrapper statusIconWrapper = status.CreateIconWrapper();
            var statusIcon = _statusScene.Instantiate();
            _statuses.AddChild(statusIcon);

            statusIcon.Call("SetIcon", statusIconWrapper);
        }
    }

    public async Task UpdateBattleEffects(BattleEffectWrapper effectWrapper)
    {
        BattleResult result = effectWrapper.Result;

        if(result.SkillUsed != null)
        {
            // if we're the skill user, we want to play the startup animation
            if (effectWrapper.IsEntitySkillUser)
            {
                string startupAnimationString = result.SkillUsed.StartupAnimation;
                if (!string.IsNullOrEmpty(startupAnimationString))
                {
                    // wait for play effect to finish before proceeding
                    await PlayEffect(startupAnimationString);
                }
            }
            else
            {
                string endupAnimationString = result.SkillUsed.EndupAnimation;
               
                bool isHPGainedFromMove = (result.ResultType == BattleResultType.HPGain || result.ResultType == BattleResultType.Dr);

                if (!string.IsNullOrEmpty(endupAnimationString))
                {
                    await PlayEffect(endupAnimationString);
                }

                if((int)(result.ResultType) < (int)BattleResultType.StatusApplied)
                {
                    // play damage sfx
                    if (!isHPGainedFromMove && result.HPChanged > 0)
                    {
                        _shakeParameters.ShakeValue = _shakeParameters.ShakeStrength;
                        _shakeSfx.Play();
                    }
   
                    // play damage number
                    var dmgNumber = _damageNumScene.Instantiate<DamageNumber>();
                    dmgNumber.SetDisplayInfo(result.HPChanged, isHPGainedFromMove, result.GetResultString());
                    CallDeferred("add_child", dmgNumber);
                }
            }
        }
    }

    private async Task PlayEffect(string effectName)
    {
        _effect.Visible = true;
        _effect.Call("PlayAnimation", effectName);
        await ToSignal(_effect, "EffectAnimationCompletedEventHandler");

        _effect.Visible = false;
    }
}
