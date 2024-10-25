using AscendedZ.entities.partymember_objects;
using AscendedZ.screens.sigil_screen;
using Godot;
using System;

public partial class SigilEquipMenuScene : Control
{
	private VBoxContainer _sigilEquipMenu;
	private SigilScene _sigilScene;
    private ItemList _sigilList;

    private SigilWrapper _wrapper;
    private int _index;

    public EventHandler<int> EquipChanged;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _sigilEquipMenu = GetNode<VBoxContainer>("%SigilEquipMenu");
        _sigilList = GetNode<ItemList>("%SigilList");
        _sigilScene = GetNode<SigilScene>("%SigilScene");
        _sigilList.ItemClicked += _OnEquipSigil;
    }

    public void SetSigils(
        SigilWrapper wrapper,
        int index)
    {
        _wrapper = wrapper;
        _index = index;

        _sigilScene.UnequipSigil += _OnUnequipSigil;

        UpdateSigils(_index);
    }

    public void UpdateSigils(int index)
    {
        _sigilList.Clear();

        foreach (var sigil in _wrapper.Entity.Sigils)
        {
            if (!sigil.Name.Contains("Almighty"))
            {
                _sigilList.AddItem(sigil.GetDisplayString(), ResourceLoader.Load<Texture2D>(sigil.Image));

                if (index == sigil.Index)
                {
                    _sigilEquipMenu.Visible = false;
                    _sigilScene.Initialize(_wrapper, sigil);
                    _sigilScene.Visible = true;
                }
            }
        }
    }

    private void _OnEquipSigil(long index, Vector2 at_position, long mouse_button_index)
    {
        if (mouse_button_index == (long)MouseButton.Right)
        {
            int selected = (int)index;
            if (_wrapper.Entity.Sigils[selected].Index == -1)
            {
                _wrapper.Entity.Sigils[selected].Index = _index;
                _sigilScene.Initialize(_wrapper, _wrapper.Entity.Sigils[selected]);

                _sigilEquipMenu.Visible = false;
                _sigilScene.Visible = true;
                EquipChanged?.Invoke(null, _index);
            }
        }
    }

    private void _OnUnequipSigil(object sender, EventArgs e)
    {
        _sigilEquipMenu.Visible = true;
        _sigilScene.Visible = false;

        EquipChanged?.Invoke(null, _index);
    }
}
