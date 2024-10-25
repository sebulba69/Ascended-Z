using AscendedZ.screens.cutscene;
using AscendedZ;
using Godot;
using System;
using System.Threading.Tasks;
using AscendedZ.game_object;

public partial class FusinonIntroCutscene : CutsceneBase
{
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        await Task.Delay(800);

        dialog.SetName("Fittotu");
        dialog.SetNameBoxVisible(false);

        boxPlayer.Play("fade_in_box");
        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        string[] scene1 =
            [
                "You prepare to move on to the next\ntier when suddenly something catches your eye.",
                "Standing awkwardly, near a bush is\na familiar face..."
            ];

        await ShowText(scene1);

        boxPlayer.Play("fade_out_box");
        player.Play("tier14fitottu");
        await Task.Delay(200);

        boxPlayer.Play("fade_in_box");
        dialog.SetNameBoxVisible(true);
        string[] scene2 =
        [
            "Erm, um... sorry... yes.",
            "This forest, erm, scares me, yes...",
            "I'm not good w-w-w-w-with trees...",
            "I have news to report about,\nerm, the hub area yes.",
            "I have found, erm, a new way\nof using your, erm, Party Members.",
        ];
        await ShowText(scene2);

        dialog.SetNameBoxVisible(false);
        string[] scene3 = ["You ask for more information on\nthis \"new way.\""];
        await ShowText(scene3);

        dialog.SetNameBoxVisible(true);
        string[] scene4 =
        [
            "You, um, can now, erm... *cough*",
            "S-s-s-s-sorry, I am... erm... filled with\nterror at this very moment...",
            "I will, erm, try to explain. Yes."
        ];
        await ShowText(scene4);

        dialog.SetNameBoxVisible(false);
        string[] scene5 =
        [
            "You can now fuse two party members with\nthe same resistances (excluding weaknesses)\nas each other.",
            "Every party member has a \"Fusion Grade.\"\nYour current party all has a grade of 0.",
            "By going to the \"Fuse Screen\" in the Main Menu\nyou can combine them to make a Fusion Grade 1.",
            "For fusions higher than Fusion Grade 1,\nyou will need to combine Fusions with\na grade difference of 1.",
            "Fusions give you benefits like\nfree health boosts and free skill boosts.",
            "For more information on how they work,\ncheck your mailbox.",
        ];

        PersistentGameObjects.GameObjectInstance().ProgressFlagObject.AddFusionTutorial = true;
        PersistentGameObjects.Save();

        await ShowText(scene5);

        _OnSkipDialogEvent();
    }

    private async void BackToEndScreen()
    {
        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
        var root = this.GetTree().Root;

        root.AddChild(transition);

        transition.PlayFadeIn();

        await ToSignal(transition.Player, "animation_finished");

        this.Visible = false;

        transition.PlayFadeOut();
        await ToSignal(transition.Player, "animation_finished");
        try { transition.QueueFree(); } catch (Exception) { }
        QueueFree();
    }

    private void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;
        BackToEndScreen();
    }
}
