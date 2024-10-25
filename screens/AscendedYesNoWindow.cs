using AscendedZ;
using Godot;
using System;

public partial class AscendedYesNoWindow : CenterContainer
{
	private Label _popupMessage;
	private Button _yesButton;
	private Button _noButton;
	private bool _buttonPressed;

	public EventHandler<bool> AnswerSelected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_buttonPressed = false;

		this.AddUserSignal("AnswerSelected", new Godot.Collections.Array() 
		{
			new Godot.Collections.Dictionary()
			{
				{ "name", "yesSelectedAsAnswer" },
				{ "type", (int)Godot.Variant.Type.Bool }
			}
		});

		_popupMessage = this.GetNode<Label>("%PopupMessage");
		_yesButton = this.GetNode<Button>("%YesButton");
        _noButton = this.GetNode<Button>("%NoButton");

		_yesButton.Text = $"[{Controls.GetControlString(Controls.CONFIRM)}] Yes";
		_noButton.Text = $"[{Controls.GetControlString(Controls.BACK)}] No";

		_yesButton.Pressed += () => ButtonPressed(true);
		_noButton.Pressed += () => ButtonPressed(false);
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(Controls.CONFIRM))
		{
            ButtonPressed(true);
        }

        if (@event.IsActionPressed(Controls.BACK))
        {
            ButtonPressed(false);
        }
    }

    public void SetDialogMessage(string message)
	{
		_popupMessage.Text = message;
	}

	private void ButtonPressed(bool isYesButton)
	{
		_buttonPressed = true;

		this.EmitSignal("AnswerSelected", isYesButton);
		AnswerSelected?.Invoke(null, isYesButton);

        this.QueueFree();
	}
}
