using Godot;
using System;

public partial class AscendedDialogBox : Godot.TextEdit
{
    private const int MAX_CHARS = 15;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("text_changed",new Callable(this,"_OnTextChanged"));
    }

    private void _OnTextChanged()
    {
        int characters = this.Text.Length;
        if(characters == MAX_CHARS)
        {
            this.Text = this.Text.Substr(0, MAX_CHARS - 1);
            this.SetCaretColumn(MAX_CHARS - 1);
        }
    }
}
