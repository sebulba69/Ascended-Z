using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public static class CutsceneAssets
    {
        /// <summary>
        /// Cutscenes that play at the end of a tier.
        /// </summary>
        public static Dictionary<int, string> EndOfMainTierCutscenes = new()
        {
            { 9, "res://screens/cutscene/003. Labrybuce Introduction/Labrybuce Intro.tscn" }, // Labrybuce
            { 15, "res://screens/cutscene/004B. Fusion Intro Cutscene/FusinonIntroCutscene.tscn" } // Labrybuce
        };

        /// <summary>
        /// Cutscenes that play at the start of a tier (before combat)
        /// </summary>
        public static Dictionary<int, string> StartTierCutscenes = new() 
        {
            { 10, "res://screens/cutscene/Harbinger/HarbingerScene.tscn" },
            { 260, "res://screens/cutscene/Elders/Elder1.tscn" },
            { 270, "res://screens/cutscene/Elders/Elder2.tscn" },
            { 280, "res://screens/cutscene/Elders/Elder3.tscn" },
            { 290, "res://screens/cutscene/Elders/Elder4.tscn" },
            { 300, "res://screens/cutscene/Elders/Elder5.tscn" },
        };

        /// <summary>
        /// Cutscenes that play at the start of a labrybuce tier
        /// </summary>
        public static Dictionary<int, string> StartLabrybuceFloor = new()
        {
            { 1, "res://screens/cutscene/003B. Labrybuce Introduction 2/LabrybuceIntro2.tscn" },
            { 51, "res://screens/cutscene/LabCutscenes/Lab51.tscn" },
            { 101, "res://screens/cutscene/LabCutscenes/Lab101.tscn" },
            { 151, "res://screens/cutscene/LabCutscenes/Lab151.tscn" },
            { 201, "res://screens/cutscene/LabCutscenes/Lab201.tscn" },
        };

        /// <summary>
        /// Cutscenes that play at the start of a tier (before combat)
        /// </summary>
        public static Dictionary<int, string> StartTierCutscenesSpecial = new() 
        {
            { 251, "res://screens/cutscene/005. FinalBossCutscene/FinalBossCutscene.tscn" }
        };

        /// <summary>
        /// Cutscenes that play at the start of a tier (before combat)
        /// </summary>
        public static Dictionary<int, string> StartTierCutscenesSpecialLab = new()
        {
            { 251, "res://screens/cutscene/005. FinalBossCutscene/FinalBossCutsceneLab.tscn" }
        };
    }
}
