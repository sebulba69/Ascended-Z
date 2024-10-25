using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.cutscene
{
    public partial class CutsceneBase : Transitionable2DScene
    {
        protected AnimationPlayer player, boxPlayer;
        protected AudioStreamPlayer audio;
        protected AscendedTextbox dialog;

        protected async Task ShowText(string[] text)
        {
            foreach (var t in text)
            {
                dialog.DisplayText(t);
                await ToSignal(dialog, "ReadyForMoreDialogEventHandler");
            }
        }
    }
}
