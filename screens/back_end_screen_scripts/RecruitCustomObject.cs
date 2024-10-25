using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.back_end_screen_scripts
{
    public class RecruitCustomObject
    {
        private int _cost;
        private int[] _fusionCosts;
        private int[] _fusionHP;
        private int[] _fusionVV;
        private int[] _fusionLvls;

        // Preview UI
        public OverworldEntity SelectedEntity { get; set; }
        public int Cost { get => _cost; }

        // Left/Right List Boxes
        public List<OverworldEntity> AvailableMembers { get; set; }
        public List<OverworldEntity> DisplayMembers { get; set; }
        public List<ISkill> AvailableSkills { get; set; }
        public List<ISkill> AllSkills { get; set; }

        private GameObject _gameObject;

        public RecruitCustomObject()
        {
            AvailableMembers = new List<OverworldEntity>();
            DisplayMembers = new List<OverworldEntity>();
            AvailableSkills = new List<ISkill>();
            AllSkills = new List<ISkill>();
            _fusionCosts = new int[MiscGlobals.FUSION_GRADE_CAP + 1];
            _fusionHP = new int[_fusionCosts.Length];
            _fusionVV = new int[_fusionCosts.Length];
            _fusionLvls = new int[_fusionCosts.Length];
        }

        public void Initialize()
        {
            _gameObject = PersistentGameObjects.GameObjectInstance();

            AvailableMembers = EntityDatabase.MakeShopVendorWares(_gameObject, true);
            AllSkills = SkillDatabase.GetAllGeneratableSkills(_gameObject.MaxTier);
            DisplayMembers.Clear();

            foreach (var skill in AllSkills)
            {
                for (int i = 0; i < _gameObject.ShopLevel; i++)
                    skill.LevelUp();
            }

            foreach (var member in AvailableMembers)
            {
                member.Skills.Clear();
                for (int i = 0; i < _gameObject.ShopLevel; i++)
                    member.LevelUp();

                if (_gameObject.Checkboxes[member.FusionGrade])
                {
                    DisplayMembers.Add(member);
                }
            }

            _fusionLvls[0] = _gameObject.ShopLevel;
            _fusionLvls[1] = _fusionLvls[0] + 1;

            _fusionCosts[0] = (int)(_gameObject.ShopLevel * 1.5) + 1;
            _fusionCosts[1] = _fusionCosts[0] * 2;

            // everyone at f0 is always at the end of the list
            var baseMember = AvailableMembers[AvailableMembers.Count - 1];
            _fusionHP[0] = baseMember.MaxHP;
            _fusionHP[1] = (int)((_fusionHP[0]*2)/1.5);

            _fusionVV[0] = baseMember.VorpexValue;
            _fusionVV[1] = _fusionVV[0] - (int)(_fusionVV[0] * 0.05);

            for(int i = 2; i < _fusionCosts.Length; i++)
            {
                int index1 = i - 1;
                int index2 = i - 2;

                // fusion cost
                _fusionCosts[i] = _fusionCosts[index1] + _fusionCosts[index2];
                _fusionCosts[i] += 50 * index1;

                // max hp
                _fusionHP[i] = (int)((_fusionHP[index1] + _fusionHP[index2])/1.5);

                // vorpex value
                double discount = 0.05 * (i + 1);
                _fusionVV[i] = (_fusionVV[index1] + _fusionVV[index2]) / 2;
                _fusionVV[i] -= (int)(_fusionVV[i] * discount);

                // skill levels
                _fusionLvls[i] = _fusionLvls[index1] + i;
            }
        }

        public void FilterResults()
        {
            DisplayMembers.Clear();
            foreach (var member in AvailableMembers)
            {
                if (_gameObject.Checkboxes[member.FusionGrade])
                {
                    DisplayMembers.Add(member);
                }
            }
        }

        public OverworldEntity BuyAndReturnSelected()
        {
            var member = PartyMemberGenerator.MakePartyMember(SelectedEntity.Name);
            member.Skills.Clear();

            member.Level = SelectedEntity.Level;
            member.MaxHP = SelectedEntity.MaxHP;
            member.VorpexValue = SelectedEntity.VorpexValue;

            foreach (var skill in SelectedEntity.Skills)
                member.Skills.Add(skill.Clone());

            return member;
        }

        public void SetPreviewPartyMember(int index)
        {
            // clear skills from the new entity since we want to customize them
            if (SelectedEntity != null)
            {
                SelectedEntity.Skills.Clear();
            }

            if (DisplayMembers.Count == 0)
                return;

            SelectedEntity = DisplayMembers[index];

            int fusionGrade = SelectedEntity.FusionGrade;
            SelectedEntity.MaxHP = _fusionHP[fusionGrade];
            SelectedEntity.VorpexValue = _fusionVV[fusionGrade];

            List<ISkill> skills = SkillDatabase.GetAllGeneratableSkills(_gameObject.MaxTier);
            AvailableSkills = skills.FindAll(skill => 
            {
                bool isValidSkill = true;

                if(skill.Id == SkillId.Elemental)
                {
                    ElementSkill elementSkill = (ElementSkill)skill;
                    isValidSkill = !(SelectedEntity.Resistances.IsWeakToElement(elementSkill.Element)); // returns true if weak, but that makes it invalid, so we invert it to false
                }

                return isValidSkill;
            });

            int shopLevel = _fusionLvls[fusionGrade];
            for (int i = 0; i < shopLevel; i++)
            {
                foreach (var skill in AvailableSkills)
                    skill.LevelUp();
            }

            _cost = _fusionCosts[fusionGrade];
        }

        public string GetSkillDescription(int index)
        {
            string description = "";

            description = AvailableSkills[index].Description;

            return description;
        }

        public void SetSkill(int index)
        {
            if(!DoesSelectedHaveSkill(index) && SelectedEntity.Skills.Count != SelectedEntity.SkillCap)
            {
                SelectedEntity.Skills.Add(AvailableSkills[index].Clone());
            }
            else
            {
                if(DoesSelectedHaveSkill(index))
                {
                    SelectedEntity.Skills.RemoveAll(skill =>
                    {
                        return AvailableSkills[index].Name.Equals(skill.Name);
                    });
                }
            }
        }

        private bool DoesSelectedHaveSkill(int index)
        {
            ISkill skillInSelected = SelectedEntity.Skills.Find(skill => skill.Name.Equals(AvailableSkills[index].Name));
            return skillInSelected != null;
        }
    }
}
