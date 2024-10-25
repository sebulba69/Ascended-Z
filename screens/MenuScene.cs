using AscendedZ;
using AscendedZ.game_object;
using Godot;
using System;

public partial class MenuScene : CenterContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.AddUserSignal("EndMenuScene", new Godot.Collections.Array() 
		{
			new Godot.Collections.Dictionary
			{
				{ "name", "isQuitToTitle" },
				{ "type", (int)Godot.Variant.Type.Bool }
			}
		});

		Button settingsButton = this.GetNode<Button>("%SettingsButton");
		Button saveButton = this.GetNode<Button>("%SaveButton");
		Button quitToTitleButton = this.GetNode<Button>("%QuitToTitleButton");
		Button quitGameButton = this.GetNode<Button>("%QuitGameButton");
		Button backButton = this.GetNode<Button>("%BackButton");

		Label tooltip = this.GetNode<Label>("%Tooltip");

		settingsButton.Pressed += async () => 
		{
			var menu = GetNode<VBoxContainer>("%VBoxContainer");
			menu.Visible = false;
			var settings = ResourceLoader.Load<PackedScene>(Scenes.SETTINGS).Instantiate<SettingsScreen>();

			AddChild(settings);

			await ToSignal(settings, "tree_exited");
			menu.Visible = true;
		};

		saveButton.Pressed += () => 
		{
			PersistentGameObjects.Save();
			tooltip.Text = "Game saved!";
        };

		quitToTitleButton.Pressed += _OnQuitToTitleButtonPressed;
        quitGameButton.Pressed += _OnQuitGameButtonPressed;

        backButton.Pressed += () => 
		{
            this.EmitSignal("EndMenuScene", false);
			this.QueueFree();
        };
	}

	private void _OnQuitToTitleButtonPressed()
	{
		this.Visible = false;

		var popupWindow = ResourceLoader.Load<PackedScene>(Scenes.YES_NO_POPUP).Instantiate();
        this.GetTree().Root.AddChild(popupWindow);
        popupWindow.Call("SetDialogMessage", "Are you sure you want to save and quit to title?");
		popupWindow.Connect("AnswerSelected", new Callable(this, "_OnQuitToTitleDialogResult"));
    }

    private void _OnQuitGameButtonPressed()
    {
        this.Visible = false;

        var popupWindow = ResourceLoader.Load<PackedScene>(Scenes.YES_NO_POPUP).Instantiate();
        this.GetTree().Root.AddChild(popupWindow);
        popupWindow.Call("SetDialogMessage", "Are you sure you want to save and quit the game?");
        popupWindow.Connect("AnswerSelected", new Callable(this, "_OnQuitGameDialogResult"));
    }

    private void _OnQuitToTitleDialogResult(bool isYesButtonPressed)
	{
		if (isYesButtonPressed)
		{
            PersistentGameObjects.Save();
            this.EmitSignal("EndMenuScene", true);
        }

        this.Visible = true;
    }

    private void _OnQuitGameDialogResult(bool isYesButtonPressed)
    {
        if (isYesButtonPressed)
        {
            PersistentGameObjects.Save();
            this.GetTree().Quit();
        }

        this.Visible = true;
    }
}
