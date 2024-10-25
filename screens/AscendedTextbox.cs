using AscendedZ;
using Godot;
using System;
using static Godot.WebSocketPeer;

public partial class AscendedTextbox : VBoxContainer
{
    [Signal]
    public delegate void SkipDialogEventHandler();

    /// <summary>
    /// Time between letters showing up in the text.
    /// </summary>
    private const float TIMEOUT = 0.03f;

    /// <summary>
    /// Timer for text display.
    /// </summary>
    private Timer _timer;

    /// <summary>
    /// Textbox for displaying text.
    /// </summary>
    private Label _textbox;

    /// <summary>
    /// Flag preventing you from clicking next if text is displaying.
    /// </summary>
    private bool _canClickNextButton = false;

    private string _blips = "res://screens/cutscene/system.wav";

    private Label _nameBox;
    private Button _nextButton;

    public Button Next { get => _nextButton; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.AddUserSignal("ReadyForMoreDialogEventHandler");
        this.AddUserSignal("SkipDialogEventHandler");

        _timer = GetNode<Timer>("%TextTimer");
        _textbox = GetNode<Label>("%TextboxForDialog");
        _nameBox = GetNode<Label>("%Namebox");

        _nextButton = GetNode<Button>("%NextButton");
        Button skipButton = GetNode<Button>("%SkipButton");

        _nextButton.Pressed += _OnNextButtonPressed;
        skipButton.Pressed += _OnSkipButtonPressed;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            _OnNextButtonPressed();
        }


        if (@event.IsActionPressed(Controls.SKIP))
        {
            _OnSkipButtonPressed();
        }
    }

    public void SetName(string name)
    {
        _nameBox.Text = name;
    }

    public void SetBlips(string blipPath)
    {
        var blips = GetNode<AudioStreamPlayer>("%BlipPlayer");
        if (blipPath == "")
            blipPath = _blips;

        blips.Stream = ResourceLoader.Load<AudioStream>(blipPath);
    }

    public void SetNameBoxVisible(bool visible)
    {
        GetNode<PanelContainer>("%PanelContainer").Visible = visible;
    }

    /// <summary>
    /// Display all text in the textbox 1 character at a time.
    /// </summary>
    /// <param name="dialog"></param>
    /// <param name="callbackScene"></param>
    public async void DisplayText(string dialog)
    {
        _textbox.VisibleCharacters = 0;
        _textbox.Text = dialog;
        var blips = GetNode<AudioStreamPlayer>("%BlipPlayer");
        _canClickNextButton = false;

        var nameContainer = GetNode<PanelContainer>("%PanelContainer");
        int charIndex = 0;
        bool alt = true;
        var timer = GetNode<Timer>("%PauseTimer");

        while(charIndex < _textbox.Text.Length && _textbox.VisibleCharacters < _textbox.Text.Length)
        {
            _textbox.VisibleCharacters++;
            if (nameContainer.Visible)
            {
                char c = _textbox.Text[charIndex];
                if (c != ' ' && !".!,?;-".Contains(c))
                {
                    if(alt)
                        blips.Play();

                    alt = !alt;
                }
                else if(".!,?;-".Contains(c))
                {
                    timer.Start();
                    await ToSignal(timer, "timeout");
                }
            }

            charIndex++;
 
            _timer.Start(TIMEOUT);
            await ToSignal(_timer, "timeout");
        }

        _canClickNextButton = true;
    }

    /// <summary>
    /// Let parent classes known we're ready to receive more dialog.
    /// </summary>
    private void _OnNextButtonPressed()
    {
        if (_canClickNextButton)
        {
            this.EmitSignal("ReadyForMoreDialogEventHandler");
        }
    }

    private void _OnSkipButtonPressed()
    {
        _textbox.VisibleCharacters = _textbox.Text.Length;
        EmitSignal("SkipDialogEventHandler");
    }
}
