using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens
{
    public partial class Transitionable2DScene : Node2D
    {
        protected async void TransitionScenes(string endScene, AudioStreamPlayer audioStreamPlayer)
        {
            var destination = ResourceLoader.Load<PackedScene>(endScene).Instantiate();
            var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
            var root = this.GetTree().Root;

            if (audioStreamPlayer != null)
                audioStreamPlayer.Stop();

            root.AddChild(transition);

            transition.PlayFadeIn();

            await ToSignal(transition.Player, "animation_finished");

            root.AddChild(destination);

            await Task.Delay(250);

            try { QueueFree(); } catch (Exception) { }
            transition.PlayFadeOut();
        }
    }
}
