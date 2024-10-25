using AscendedZ;
using AscendedZ.game_object;
using Godot;
using System;

public partial class DungeonEntity : Node2D
{
    private readonly PackedScene _damageNumScene = ResourceLoader.Load<PackedScene>(Scenes.DAMAGE_NUM);

    private Sprite2D _pic;
	private Button _left, _right, _up, _down;

	public Button Up => _up;
	public Button Down => _down;
	public Button Left => _left;
	public Button Right => _right;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pic = this.GetNode<Sprite2D>("%EntityPic");
		_up = GetNode<Button>("%MineUpButton");
		_down = GetNode<Button>("%MineDownButton");
        _right = GetNode<Button>("%MineRightButton");
        _left = GetNode<Button>("%MineLeftButton");
	}

	public void SetGraphic(string graphic)
	{
        _pic.Texture = ResourceLoader.Load<Texture2D>(graphic);
    }

    public void DisplayPopup(string popupText)
    {
        var displayArea = GetNode<CenterContainer>("%OrbSpawner");
        var dmgNumber = _damageNumScene.Instantiate<DamageNumber>();
        dmgNumber.SetDisplayLabrybuce(popupText);
        displayArea.AddChild(dmgNumber);
    }

    public void SetArrows(bool up, bool down, bool left, bool right) 
	{
		if(PersistentGameObjects.GameObjectInstance().TierDC % 50 == 0)
		{
            _up.Visible = false;
            _down.Visible = false;
            _left.Visible = false;
            _right.Visible = false;
        }
		else
		{
            _up.Visible = up;
            _down.Visible = down;
            _left.Visible = left;
            _right.Visible = right;
        }
	}
}
