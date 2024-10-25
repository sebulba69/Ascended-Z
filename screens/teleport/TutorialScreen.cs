using AscendedZ.game_object;
using AscendedZ.game_object.mail;
using Godot;
using System;

public partial class TutorialScreen : CenterContainer
{
	private HBoxContainer _container;

	/// <summary>
	/// Reference to an instance tutorial. You will need to remove this from the holder each time
	/// you change it out.
	/// </summary>
	private Node _displayTutorialReference;

	private Mailbox _mailBox;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_container = GetNode<HBoxContainer>("%TutorialHolder");

		var gameObject = PersistentGameObjects.GameObjectInstance();
		ItemList mailTitleList = GetNode<ItemList>("%ItemList");
		Button back = GetNode<Button>("%BackButton");

		mailTitleList.ItemSelected += _OnIndexChanged;

        _mailBox = gameObject.Mail;
        _displayTutorialReference = ResourceLoader.Load<PackedScene>(_mailBox.Mail[0].MailScene).Instantiate();

		foreach(var mail in _mailBox.Mail)
		{
			mailTitleList.AddItem(mail.Title);
		}

		_OnIndexChanged(0);
        back.Pressed += () => 
		{
			PersistentGameObjects.Save();
			QueueFree();
		};
    }

	private void _OnIndexChanged(long index)
	{
		int selected = (int)index;
		_mailBox.SetMailRead(selected);
		_displayTutorialReference.QueueFree();

		_displayTutorialReference = ResourceLoader.Load<PackedScene>(_mailBox.Mail[selected].MailScene).Instantiate();
		_container.AddChild(_displayTutorialReference);
	}
}
