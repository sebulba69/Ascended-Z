using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.json_interface_converters
{
    /// <summary>
    /// Newtonsoft cannot natively handle interface deserialization so we
    /// have to extend its converter to allow for our interfaces to be processed
    /// correctly.
    /// </summary>
    public partial class SkillConverter : JsonConverter<ISkill>
    {
        private readonly Dictionary<SkillId, Type> _idSkillTypes = new Dictionary<SkillId, Type>() 
        {
            { SkillId.Elemental, typeof(ElementSkill) },
            { SkillId.Status, typeof(StatusSkill) },
            { SkillId.Healing, typeof(HealSkill) },
            { SkillId.Eye, typeof(EyeSkill) },
        };

        public override ISkill Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ISkill skill = default(ISkill);

            using(var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                int id = jsonDocument.RootElement.GetProperty("Id").GetInt32();
                skill = (ISkill)JsonSerializer.Deserialize(jsonDocument, _idSkillTypes[(SkillId)id]);
            }

            return skill;
        }

        public override void Write(Utf8JsonWriter writer, ISkill value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, _idSkillTypes[value.Id], options);
        }
    }
}
