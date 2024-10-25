using AscendedZ.currency;
using AscendedZ.entities;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.back_end_screen_scripts
{
    public class FusionScreenObject
    {
        private List<FusionObject> _fusions;

        private int _costAddition = 0;
        private int _fusionIndex;

        public int FusionIndex { get => _fusionIndex; set => _fusionIndex = value; }
        public List<FusionObject> Fusions { get => _fusions; }
        public FusionObject DisplayFusion { get => _fusions[FusionIndex]; }

        private Currency _partyCoins;

        public int OwnedPartyCoins { get => _partyCoins.Amount; }


        public FusionScreenObject()
        {
            _fusions = new List<FusionObject>();
            _fusionIndex = 0;
            var mp = PersistentGameObjects.GameObjectInstance().MainPlayer;
            _partyCoins = mp.Wallet.Currency[SkillAssets.PARTY_COIN_ICON];
        }

        public bool TryFuse()
        {
            bool isSuccessful = false;

            if (DisplayFusion.Fusion.Skills.Count > 0)
            {
                if (GetCost() > OwnedPartyCoins)
                    return false;

                var go = PersistentGameObjects.GameObjectInstance();
                MainPlayer mainPlayer = go.MainPlayer;

                foreach (var reserve in mainPlayer.ReserveMembers)
                {
                    if (reserve.Name.Equals(DisplayFusion.Fusion.Name))
                        return false;
                }

                RemoveMaterialFromMainPlayer(mainPlayer, DisplayFusion.Material1);
                RemoveMaterialFromMainPlayer(mainPlayer, DisplayFusion.Material2);

                _partyCoins.Amount -= GetCost();

                foreach (var skill in DisplayFusion.Fusion.Skills) 
                {
                    for (int f = 0; f < DisplayFusion.Fusion.FusionGrade; f++)
                    {
                        skill.LevelUp();
                    }
                }

                mainPlayer.ReserveMembers.Add(DisplayFusion.Fusion);

                var set = go.DiscoveredFusions[DisplayFusion.Fusion.FusionGrade];
                if (!set.Contains(DisplayFusion.Fusion.Name))
                {
                    set.Add(DisplayFusion.Fusion.Name);
                }

                _fusions.Remove(DisplayFusion);
                FusionIndex = 0;
                isSuccessful = true;

                PersistentGameObjects.Save();
            }

            return isSuccessful;
        }

        public bool IsCorrectTier()
        {
            int grade = DisplayFusion.Fusion.FusionGrade;
            int tier = PersistentGameObjects.GameObjectInstance().MaxTier;

            int fusionTierReq = TierRequirements.GetFusionTierRequirement(grade);

            if (fusionTierReq < 0)
                return false;
            else
                return tier > fusionTierReq;
        }

        public bool IsCorrectFusionLevel()
        {
            int level = TierRequirements.GetFusionTierRequirement(DisplayFusion.Fusion.FusionGrade);

            if (level < 0)
                return false;

            var m1 = DisplayFusion.Material1;
            var m2 = DisplayFusion.Material2;

            return m1.Level >= level && m2.Level >= level;
        }

        private void RemoveMaterialFromMainPlayer(MainPlayer mainPlayer, OverworldEntity material)
        {
            if (!mainPlayer.ReserveMembers.Contains(material))
                return;

            if (material.IsInParty)
                mainPlayer.Party.RemovePartyMember(material);

            mainPlayer.ReserveMembers.Remove(material);
        }

        public void PopulateMaterialFusionListReserves()
        {
            List<OverworldEntity> reserves = PersistentGameObjects.GameObjectInstance().MainPlayer.ReserveMembers;

            _costAddition = 0;

            PopulateFusionsFromList(reserves);
        }

        private void PopulateFusionsFromList(List<OverworldEntity> reserves)
        {
            _fusions.Clear();

            // save the materials + fusion results
            for (int m1 = 0; m1 < reserves.Count; m1++)
            {
                if (reserves[m1].FusionGrade < MiscGlobals.FUSION_GRADE_CAP)
                {
                    for (int m2 = 0; m2 < reserves.Count; m2++)
                    {
                        if (reserves[m2].FusionGrade < MiscGlobals.FUSION_GRADE_CAP) 
                        {
                            if (m1 != m2)
                            {
                                if (!IsFusionRecipeGenerated(reserves[m1], reserves[m2]))
                                {
                                    var fusions = EntityDatabase.MakeFusionEntities(reserves[m1], reserves[m2]);

                                    if(fusions.Count > 0)
                                    {
                                        foreach(var fusion in fusions)
                                        {
                                            if(reserves.Find(r => r.Name == fusion.Fusion.Name) == null)
                                            {
                                                _fusions.Add(fusion);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            if (_fusionIndex == _fusions.Count)
                _fusionIndex = _fusions.Count - 1;

            if (_fusionIndex < 0)
                _fusionIndex = 0;

            _fusions = _fusions.OrderByDescending(fusion => fusion.Fusion.FusionGrade).ToList();
        }

        public int GetCost()
        {
            if (_fusions.Count == 0)
            {
                return -1;
            } 
            else
            {
                int grade = DisplayFusion.Fusion.FusionGrade - 1;
                return (grade * 50) + _costAddition;
            }
        }

        private bool IsFusionRecipeGenerated(OverworldEntity m1, OverworldEntity m2)
        {
            bool isGenerated = false;
            
            foreach(var fusion in _fusions)
            {
                if(fusion.Material1 == m2 && fusion.Material2 == m1)
                {
                    isGenerated = true;
                    break;
                }
            }

            return isGenerated;
        }

        public List<ISkill> GetFusionMaterialSkills()
        {
            List<ISkill> skills = new List<ISkill>();

            skills.AddRange(PopulateSubFusionList(DisplayFusion.Material1));
            skills.AddRange(PopulateSubFusionList(DisplayFusion.Material2));

            return skills;
        }

        private List<ISkill> PopulateSubFusionList(OverworldEntity displayFusion)
        {
            List<ISkill> skills = new List<ISkill>();
            foreach (var skill in displayFusion.Skills)
            {
                skills.Add(skill);
            }
            return skills;
        }

        public void AddOrRemoveSkillFromFusion(int selected)
        {
            List<ISkill> skills = GetFusionMaterialSkills();
            ISkill skill = skills[selected];
            var fusionSkills = DisplayFusion.Fusion.Skills;
            var searchSkill = fusionSkills.Find((fSkill) => fSkill.BaseName == skill.BaseName);

            if (searchSkill == null)
            {
                if(fusionSkills.Count < DisplayFusion.Fusion.SkillCap)
                {
                    if(skill.Id == SkillId.Elemental)
                    {
                        var ele = (ElementSkill)skill;
                        if (!DisplayFusion.Fusion.Resistances.IsWeakToElement(ele.Element))
                        {
                            fusionSkills.Add(skill);
                        }
                    }
                    else
                    {
                        fusionSkills.Add(skill);
                    }
                }
            }
            else
            {
                if(fusionSkills.Contains(skill)) 
                {
                    fusionSkills.Remove(searchSkill);
                }
            }
        }
    }
}
