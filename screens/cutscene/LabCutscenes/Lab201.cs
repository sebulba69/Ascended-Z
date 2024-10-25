using AscendedZ.entities;
using AscendedZ;
using AscendedZ.screens.cutscene;
using Godot;
using System;
using System.Threading.Tasks;

public partial class Lab201 : CutsceneBase
{
    // Called when the node enters the scene tree for the first time.
    public async override void _Ready()
    {
        player = GetNode<AnimationPlayer>("%CutscenePlayer");
        boxPlayer = GetNode<AnimationPlayer>("%CutscenePlayer2");
        dialog = GetNode<AscendedTextbox>("%AscendedTextbox");
        audio = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");

        dialog.SetName("Noble Buce");
        dialog.SetNameBoxVisible(false);

        dialog.Connect("SkipDialogEventHandler", new Callable(this, "_OnSkipDialogEvent"));

        dialog.SetBlips("res://screens/cutscene/ddmale.wav");

        string[] scene1 =
        [
            "You ascend to another Stratum of the Labrybuce.",
            "Suddenly, you hear a loud voice coming from on high!",
            "You stare into the abyss above you,\nsearching for the voice's origin.",
        ];

        string[] scene2 =
        [
            "Young Ascended...",
            "You have once again overcome a major obstacle\nin your tertiary quest to conquer the Labrybuce.",
            "This next set of 50 floors will\nbe the final obstacles preventing\nour meeting.",
            "As a reward for your progress, I will gift you with\ninformation on the nature of my strange request.",
            "There are two legends that speak of the\nhero and the Ascended game.",
            "The first was that he conquered it,\nand died in its second incarnation.",
            "This, however, could not be further\nfrom the truth.",
            "In reality, his game was not\nwon, but thrown into a stalemate.",
            "After many deaths at the hands of\nthe game, the hero had managed to\nbeat it before realizing something...",
            "The forces who created the spell that\ncaused the cycle of Ascension were closer\nthan he initially thought!",
            "They reside in a pocket universe outside\nour own, viewing the game as spectators.",
            "They design new challenges, put forward\nnew obstacles, all to test those who\nwish to Ascend.",
            "Defeating them was the key to defeating\nthe game.",
            "And so, after putting his game on\nhold for a century, he returned to\nfind that it had been changed!",
            "The Ascended challenge he took part in\nwas warped by these beings into something\nelse entirely!",
            "That \"something\" else is the game you\nparticipate in today!",
            "And the world that once was...\nbecame known as the Labrybuce.",
            "Yes, the gods who control this game\nhad manifested a universe inside of another\nuniverse to perpetuate the cycle!",
            "A cosmic loophole was struck to ensure\nthe game found a victor!",
            "However, given their methods, their\nsolution to the frozen game left them\nvulnerable!",
            "Young Ascended, those gods, the Elders, are\nwithin our reach!",
            "This is the truth of the game!",
            "Now go forth and Ascend! Ascend and meet\nme at the top of this dying world!"
        ];

        string[] scene3 =
        [
            "The voice fades away.",
            "You decide to continue your adventure.",
        ];

        player.Play("opening_pan");
        await Task.Delay(800);
        boxPlayer.Play("fade_in_box");
        await ShowText(scene1);

        audio.Play();
        player.Play("pan_to_voice");
        dialog.SetNameBoxVisible(true);
        await ShowText(scene2);

        player.Play("pan_to_normal");
        dialog.SetNameBoxVisible(false);
        await ShowText(scene3);

        _OnSkipDialogEvent();
    }

    private async void _OnSkipDialogEvent()
    {
        audio.Stop();
        dialog.Visible = false;
        GetNode<Camera2D>("%Camera2D").Enabled = false;

        var transition = ResourceLoader.Load<PackedScene>(Scenes.TRANSITION).Instantiate<SceneTransition>();
        GetTree().Root.AddChild(transition);
        transition.PlayFadeIn();
        await ToSignal(transition.Player, "animation_finished");

        var dungeonScene = ResourceLoader.Load<PackedScene>(Scenes.DUNGEON_CRAWL).Instantiate<DungeonScreen>();
        this.GetTree().Root.AddChild(dungeonScene);
        await Task.Delay(150);
        dungeonScene.StartDungeon();

        transition.PlayFadeOut();
        QueueFree();
    }
}
