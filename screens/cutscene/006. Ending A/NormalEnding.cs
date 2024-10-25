using AscendedZ;
using AscendedZ.game_object;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class NormalEnding : CutsceneBase
{
    // Called when the node enters the scene tree for the first time.
    public async override void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        dialog.SetName("Fittotu");
        dialog.SetNameBoxVisible(false);

        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        await ShowText([
            "You stand triumphant after defeating\nyour final challenge.",
            "You look at the strange artifact\nyou obtained.",
            "It starts to pulse as you feel\nit absorbing large quantities of buce\nenergy around you!",
            "Suddenly, a familiar face appears."
        ]);

        boxPlayer.Play("fade_out_box");
        player.Play("fade_in_fittotu");
        await ToSignal(player, "animation_finished");
        await Task.Delay(1000);

        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        await ShowText([
            "Erm, sorry to, er... interrupt your\nvictory, erm... yes...",
            "I just, er, wanted to say... congratulations.",
            "You have, um, gone above and\nbeyond all obstacles... and\nAscended yes.",
            "You erm, will also have...\nthe ability to summon your party\nwithout me... yes.",
            "So um... I, uh...\nerm.. I...",
            "I suppose, er, my job has been\nfulfilled so, er, I would...",
            "...like to say... my goodbyes, yes."
        ]);

        string name = "Player";

        try
        {
            name = PersistentGameObjects.GameObjectInstance().MainPlayer.Name;
        }
        catch (Exception) { }

        audio.Play();
        boxPlayer.Play("fade_out_box");
        player.Play("zoom_in_fittotu");
        await ToSignal(player, "animation_finished");
        boxPlayer.Play("fade_in_box");
        await ShowText(
        [
            $"{name}, this has been, er, a long\njourney. Yes.",
            "The power you now hold can,\num, change the world. Yes.",
            "You have, er, succeeded where Drakalla failed.",
            "The, er, path forward...\nwill not be simple. Yes.",
            "As, um, the world becomes, complacent...",
            "The Elders will, er, restart the game\nonce more... and the cycle will\ncontinue...",
            $"{name}, I am... counting on you. Yes.\nTo make sure that doesn't happen.",
            "My soul, erm, is forever bound\nto the game. I can only do\nas it dictates.",
            "You, er... have the freedom and,\nerm, the strength to create a truly\nequal world.",
            "And, er... if you cannot, well...",
            "My services will always be open\nfor the next king's adventure."
        ]);

        dialog.SetNameBoxVisible(false);
        await ShowText([
            "You nod your head, accepting the\ngravity of the situation.",
            "With great Ascending, comes great\nre-buce-ability.",
            "You look to the sky, and begin to\nrise outside the confines of the game..."
        ]);

        _OnSkipDialogEvent();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        TransitionScenes(Scenes.ENDING_A2, null);
    }

}
