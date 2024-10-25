using AscendedZ;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;

public partial class ChangeRoomScreen : CenterContainer
{
	private TextureRect _playerPicture;
	private Button _leftButton, _rightButton, _backButton;
	private GameObject _gameObject;
	private int _pictureIndex;

	public override void _Ready()
	{
		_gameObject = PersistentGameObjects.GameObjectInstance();
		_pictureIndex = 0;

        _playerPicture = GetNode<TextureRect>("%NewPlayerPicture");

        _leftButton = GetNode<Button>("%LeftButton");
        _rightButton = GetNode<Button>("%RightButton");
        _backButton = GetNode<Button>("%GoBackButton");

		_playerPicture.Texture = ResourceLoader.Load<Texture2D>(CharacterImageAssets.PlayerPics[_pictureIndex]);
        _backButton.Pressed += () =>
        {
            PersistentGameObjects.Save();
            QueueFree();
        };

        _leftButton.Pressed += _OnPlayerPicLeftButtonPressed;
        _rightButton.Pressed += _OnPlayerPicRightButtonPressed;
	}

    private void _OnPlayerPicLeftButtonPressed()
    {
        List<string> pictures = CharacterImageAssets.PlayerPics;

        _pictureIndex--;
        if (_pictureIndex < 0)
            _pictureIndex = pictures.Count - 1;

        // set player pic here
        _playerPicture.Texture = ResourceLoader.Load<Texture2D>(pictures[_pictureIndex]);
        _gameObject.MainPlayer.Image = pictures[_pictureIndex];
    }

    private void _OnPlayerPicRightButtonPressed()
    {
        List<string> pictures = CharacterImageAssets.PlayerPics;

        _pictureIndex++;
        if (_pictureIndex == pictures.Count)
            _pictureIndex = 0;

        // set player pic here
        _playerPicture.Texture = ResourceLoader.Load<Texture2D>(pictures[_pictureIndex]);
        _gameObject.MainPlayer.Image = pictures[_pictureIndex];
    }
}
