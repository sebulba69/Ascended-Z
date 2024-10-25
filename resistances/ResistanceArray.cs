using AscendedZ.entities;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.resistances
{
    public partial class ResistanceArray
    {
        /// <summary>
        /// Indexed by element
        /// Values = type of resistance to elements
        /// </summary>
        public int[] RArray { get; set; }

        public bool HideResistances { get; set; }

        public ResistanceArray()
        {
            if (this.RArray == null)
            {
                // our number of columns
                int elements = Enum.GetNames(typeof(Elements)).Length;
                this.RArray = new int[elements];

                // Neutral resistance by default
                for (int i = 0; i < elements; i++)
                    this.RArray[i] = (int)ResistanceType.None;
            }
                
        }

        /// <summary>
        /// For asset creation purposes only. This information should be stored in the json database.
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="element"></param>
        public void SetResistance(ResistanceType rtype, Elements element)
        {
            this.RArray[(int)element] = (int)rtype;
        }

        public ResistanceType GetResistance(Elements element)
        {
            try
            {
                return (ResistanceType)this.RArray[(int)element];
            }
            catch (Exception)
            {
                return ResistanceType.None;
            }
        }

        public void ClearResistances()
        {
            for (int i = 0; i < RArray.Length; i++)
                this.RArray[i] = (int)ResistanceType.None;
        }

        /// <summary>
        /// Get resistances greater than None.
        /// Returns an Empty list if no resistances satisfy this condition.
        /// </summary>
        /// <returns></returns>
        public List<Elements> GetPrimaryElements()
        {
            int noneResistance = (int)ResistanceType.None;
            List<Elements> elements = new List<Elements>();

            // i = element, res = resistance type
            for(int i = 0; i < RArray.Length; i++)
            {
                int res = RArray[i];
                if (res > noneResistance)
                    elements.Add((Elements)i);
            }

            return elements;
        }

        /// <summary>
        /// Grabs the elemental opposite of whatever this entity is weak to
        /// and treats it as its primary element. Should not be done if there
        /// exist resistances greater than a weakness.
        /// </summary>
        /// <returns></returns>
        public List<Elements> GetPrimaryWeaknessElements()
        {
            int weakness = (int)ResistanceType.Wk;
            List<Elements> elements = new List<Elements>();

            // i = element, res = resistance type
            for (int i = 0; i < RArray.Length; i++)
            {
                int res = RArray[i];
                if (res == weakness)
                {
                    Elements element = SkillDatabase.ElementalOpposites[(Elements)i];
                    elements.Add(element);
                }
            }

            return elements;
        }

        /// <summary>
        /// Check if all Resistances are null
        /// </summary>
        /// <returns></returns>
        public bool HasNoResistances()
        {
            bool hasNoResistances = true;

            foreach(int res in this.RArray)
            {
                if(res != (int)ResistanceType.None)
                {
                    hasNoResistances = false;
                    break;
                }
            }

            return hasNoResistances;
        }

        /// <summary>
        /// Return a clean printout of our resistances.
        /// </summary>
        /// <returns></returns>
        public string GetResistanceString()
        {
            if (HideResistances)
                return "???";

            string[] resistNames = Enum.GetNames(typeof(ResistanceType));
            string[] elementNames = Enum.GetNames(typeof(Elements));

            StringBuilder resString = new StringBuilder();

            string elementDivider = ", ";
            string resDivider = " - ";

            // for each resistance
            for(int i = 0; i < resistNames.Length; i++)
            {
                if (i != (int)ResistanceType.None)
                {
                    StringBuilder elementRes = new StringBuilder();

                    // for each element, check what our resistance is
                    for (int j = 0; j < this.RArray.Length; j++)
                    {
                        if (this.RArray[j] == i)
                        {
                            elementRes.Append($"{elementNames[j]}{elementDivider}");
                        }
                    }

                    string elementString = elementRes.ToString();
                    if (!string.IsNullOrEmpty(elementString))
                    {
                        resString.Append($"{resistNames[i]}: {elementString.Substr(0, elementString.Length - elementDivider.Length)}{resDivider}");
                    }
                }
            }

            // output = "R1: E1, E2 - R2: E1, ..."
            string rString = resString.ToString();
            return (rString.Length == 0) ? "[None]" : rString.Substring(0, resString.Length - resDivider.Length);
        }

        /// <summary>
        /// Return a clean printout of our resistances.
        /// </summary>
        /// <returns></returns>
        public List<ResWrapper> GetResistanceList()
        {
            int length = Enum.GetValues<ResistanceType>().Length;
            var result = new List<ResWrapper>();

            var elements = Enum.GetValues<Elements>();

            if (HideResistances)
                return result;

            // for each resistance
            for (int i = 0; i < length; i++)
            {
                if (i != (int)ResistanceType.None)
                {
                    StringBuilder elementRes = new StringBuilder();

                    // for each element, check what our resistance is
                    for (int j = 0; j < this.RArray.Length; j++)
                    {
                        if (this.RArray[j] == i)
                        {
                            result.Add(new ResWrapper
                            {
                                ResistanceType = (ResistanceType)i,
                                ElementIcon = SkillAssets.GetElementIconByElementEnum(elements[j])
                            });
                        }
                    }
                }
            }

            return result;
        }

        #region Resistance Checks
        public bool IsWeakToElement(Elements element)
        {
            try
            {
                int wk = (int)ResistanceType.Wk;
                return this.RArray[(int)element] == wk;
            }
            catch (Exception)
            {
                return false; 
            }
        }

        public bool IsResistantToElement(Elements element)
        {
            try
            {
                int rs = (int)ResistanceType.Rs;
                return this.RArray[(int)element] == rs;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsNullElement(Elements element)
        {
            try
            {
                int nu = (int)ResistanceType.Nu;
                return this.RArray[(int)element] == nu;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsDrainElement(Elements element)
        {
            try
            {
                int dr = (int)ResistanceType.Dr;
                return this.RArray[(int)element] == dr;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
