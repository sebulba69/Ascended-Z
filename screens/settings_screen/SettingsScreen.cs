using AscendedZ.game_object;
using AscendedZ.screens.settings_screen;
using Godot;
using System;

public partial class SettingsScreen : CenterContainer
{
	private OptionButton _screenSizeType, _windowedMode;
	private HSlider _volumeSlider;
	private SettingsObject _settings;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button back = GetNode<Button>("%BackButton");
        _settings = PersistentGameObjects.Settings();

        _screenSizeType = GetNode<OptionButton>("%ScreenSizeType");
		_windowedMode = GetNode<OptionButton>("%WindowedMode");
		_volumeSlider = GetNode<HSlider>("%VolumeSlider");

        _screenSizeType.Select((int)_settings.WindowModeIndex);
        _windowedMode.Select((int)_settings.WindowSizeIndex);
        _volumeSlider.Value = (_settings.Volume * 5);
        _windowedMode.Disabled = _settings.FullScreenEnabled;

        if (_settings.WindowModeIndex != 0)
			_windowedMode.Disabled = false;

        back.Pressed += QueueFree;
		_screenSizeType.ItemSelected += _OnScreenSelectedChanged;
		_windowedMode.ItemSelected += _OnWindowSizeChanged;
		_volumeSlider.ValueChanged += _OnVolumeChanged;
    }

	private void _OnScreenSelectedChanged(long index)
	{
        _settings.SetWindowMode(index);
		_windowedMode.Disabled = _settings.FullScreenEnabled;
		PersistentGameObjects.SaveSettings();
    }

	private void _OnWindowSizeChanged(long index)
	{
		_settings.SetWindowSize(index);
        PersistentGameObjects.SaveSettings();
    }

    private void _OnVolumeChanged(double value)
    {
		_settings.SetVolume(value);
        PersistentGameObjects.SaveSettings();
    }
}
