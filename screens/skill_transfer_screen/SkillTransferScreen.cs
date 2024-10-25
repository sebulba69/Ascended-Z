using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.skill_transfer_screen;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;

public partial class SkillTransferScreen : CenterContainer
{
    private readonly string SELECT = "Select Party Member";
    private readonly string TRANSFER = "Transfer";
	private readonly string TOOLTIP_DEFAULT = "Transfer your skills between party members with the same Fusion Grade!";

    private bool _isTransferSkillState;
	private PartyMemberTransferSelectScreen _partyMemberTransferSelectScreen;
    private SkillTransferSelectScreen _skillTransferSelectScreen;
    private Button _transferButton, _backButton;
	private Label _tooltip;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _partyMemberTransferSelectScreen = GetNode<PartyMemberTransferSelectScreen>("%PartyMemberTransferSelectScreen");
		_skillTransferSelectScreen = GetNode<SkillTransferSelectScreen>("%SkillTransferSelectScreen");
		_tooltip = GetNode<Label>("%Tooltip");
		_transferButton = GetNode<Button>("%TransferButton");
		_backButton = GetNode<Button>("%BackButton");

		_isTransferSkillState = false;

        _transferButton.Text = SELECT;

		_transferButton.Pressed += _OnTransferButtonPressed;
        _backButton.Pressed += _OnBackButtonPressed;
    }

	private void _OnTransferButtonPressed()
	{
		if (!_isTransferSkillState)
		{
			_isTransferSkillState = true;

            _transferButton.Text = TRANSFER;
			var members = _partyMemberTransferSelectScreen.GetSelectedEntities();

            int levels = Math.Abs(members[0].Level - members[1].Level);
            bool levelCheck = members[0].FusionGrade == members[1].FusionGrade;
            if (members[0] == members[1] || !levelCheck)
			{
				_isTransferSkillState = false;
                _transferButton.Text = SELECT;
                GetNode<AudioStreamPlayer>("%WarningPlayer").Play();
            }
			else
			{
				_partyMemberTransferSelectScreen.Visible = false;

				var scenes = _skillTransferSelectScreen.GetSkillSelectionScenes();
                for (int i = 0; i < members.Length; i++)
                {
                    scenes[i].SetSelected(members[i]);
                }
                _skillTransferSelectScreen.Visible = true;
            }
        }
		else
		{
            var scenes = _skillTransferSelectScreen.GetSkillSelectionScenes();
            var members = _partyMemberTransferSelectScreen.GetSelectedEntities();
            List<List<SkillIndexWrapper>> skills = [ scenes[0].GetSelectedSkills(), scenes[1].GetSelectedSkills() ];

            bool isValidSkillTransfer = DoWeaknessCheck(scenes[0], members[1]);
            if(isValidSkillTransfer) // don't do this if it's not valid
			    isValidSkillTransfer = DoWeaknessCheck(scenes[1], members[0]);

            int levels = Math.Abs(members[0].Level - members[1].Level);
            bool levelCheck = members[0].FusionGrade == members[1].FusionGrade;

            if (isValidSkillTransfer && levelCheck)
			{
                bool transferCheck = (skills[0].Count != skills[1].Count) || skills[0].Count == 0 || skills[1].Count == 0;
                if (!transferCheck)
                {
                    bool dupeCheck1 = CheckForDuplicates(skills[0], skills[1], members[1]);
                    bool dupeCheck2 = CheckForDuplicates(skills[1], skills[0], members[0]);

                    if(dupeCheck1 || dupeCheck2)
                    {
                        _tooltip.Text = "Cannot transfer a skill that a party member already has.";
                    }
                    else
                    {
                        _tooltip.Text = TOOLTIP_DEFAULT;

                        var s1 = skills[0];
                        var s2 = skills[1];

                        for (int i = 0; i < s1.Count; i++)
                        {
                            s1[i].Skill.TransferLevel = members[0].Level;
                            s2[i].Skill.TransferLevel = members[1].Level;
                            AddOrInsertSkill(s1[i], s2[i], members[0]);
                            AddOrInsertSkill(s2[i], s1[i], members[1]);
                            members[0].Skills.Remove(s1[i].Skill);
                            members[1].Skills.Remove(s2[i].Skill);
                        }

                        for (int i = 0; i < members.Length; i++)
                        {
                            scenes[i].SetSelected(members[i]);
                        }

                        _partyMemberTransferSelectScreen.ReDisplayEntities();

                        PersistentGameObjects.Save();
                    }
                }
                else
                {
                    if (skills[0].Count == 0 || skills[1].Count == 0)
                        _tooltip.Text = "You must transfer at least 1 skill.";
                    else
                        _tooltip.Text = "You must transfer the same number of skills between Party Members.";
                }
            }
			else
			{
                string wexWarning = "You can't transfer a skill a party member is weak to.";
                string levelWarning = "Party members must have the same Fusion Grade to transfer skills.";

                _tooltip.Text = "";
                if (!isValidSkillTransfer)
                    _tooltip.Text += wexWarning;

                if(!levelCheck)
                {
                    if(_tooltip.Text != "")
                    {
                        _tooltip.Text += "\n";
                    }

                    _tooltip.Text += levelWarning;
                }

                _tooltip.Text = _tooltip.Text.Trim();
            }
        }
	}

    private void AddOrInsertSkill(SkillIndexWrapper s1, SkillIndexWrapper s2, OverworldEntity member) 
    {
        if (s1.Index >= member.Skills.Count)
        {
            member.Skills.Add(s2.Skill);
        }
        else
        {
            member.Skills.Insert(s1.Index, s2.Skill);
        }
    }

	private bool DoWeaknessCheck(SkillSelectionScene scene, OverworldEntity compareTo)
	{
        var skills = scene.GetSelectedSkills();

        foreach (var wrapper in skills)
        {
            var skill = wrapper.Skill;
            if (skill.Id == SkillId.Elemental)
            {
                var elemental = (ElementSkill)skill;
                if (compareTo.Resistances.IsWeakToElement(elemental.Element))
                {
					return false;
                }
            }
        }

		return true;
    }

    private bool CheckForDuplicates(List<SkillIndexWrapper> skillsToBeAdded, List<SkillIndexWrapper> referenceSkills, OverworldEntity member)
    {
        HashSet<string> duplicates = new HashSet<string>();

        foreach(var skill in member.Skills)
        {
            duplicates.Add(skill.BaseName);
        }

        bool hasDuplicateSkills = false;
        int index = 0;
        foreach (var skill in skillsToBeAdded) 
        {
            // does the member already have this skill on them?
            if (duplicates.Contains(skill.Skill.BaseName))
            {
                // is this the skill we are swapping?
                // compare to our reference list to make sure we are not swapping it out
                var findQuery = referenceSkills.Find(rs => rs.Skill.BaseName == skill.Skill.BaseName);
                if(findQuery == null)
                {
                    hasDuplicateSkills = true;
                    break;
                }

            }
            index++;
        }

        return hasDuplicateSkills;
    }

	private void _OnBackButtonPressed()
	{
		if (_isTransferSkillState)
		{
            _tooltip.Text = TOOLTIP_DEFAULT;
            _isTransferSkillState = false;
			_partyMemberTransferSelectScreen.Visible = true;
			_skillTransferSelectScreen.Visible = false;
            _transferButton.Text = SELECT;
        }
		else
		{
            QueueFree();
        }
		
	}
}
