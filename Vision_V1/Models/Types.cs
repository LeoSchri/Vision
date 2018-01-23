using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Vision_V1.Models
{
    public class Types
    {
        public enum Mood
        {
            Happy,
            Sad,
            Angry,
            Afraid,
            Confused,
            NS
        };

        public enum Weather
        {
            Sunny,
            PartialCloudy,
            Cloudy,
            Fog,
            Rain,
            Sleet,
            Snow,
            Hail,
            Storm,
            Thunderstorm,
            NS
        };

        public enum Orientation
        {
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest,
            Center,
            NS
        };

        public enum Climate
        {
            WarmTemperate,
            WarmTemperateMoist,
            WarmTemperateDry,
            CoolTemperate,
            CoolTemperateMoist,
            CoolTemperateDry,
            Polar,
            PolarMoist,
            PolarDry,
            Boreal,
            BorealMoist,
            BorealDry,
            Tropical,
            TropicalMontane,
            TropicalWet,
            TropicalMoist,
            TropicalDry,
            NS
        };

        public enum LocationType
        {
            Castle,
            Palace,
            Tower,
            City,
            Town,
            Village,
            Farm,
            ResidentialBuilding,
            District,
            Marketplace,
            Dwelling,
            School,
            Stable,
            AmusementFacility,
            MilitaryFacility,
            AdministrativeFacility,
            HealthFacility,
            SupplyFacility,
            ServiceProvider,
            ReligiousFacility,
            Graveyard,
            Monument,
            Ornamentation,
            Garden,
            Park,
            Well,
            Lake,
            Pond,
            Spring,
            Waterfall,
            Stream,
            River,
            Sea,
            Beach,
            Coast,
            Mountain,
            Hill,
            Valley,
            Canyon,
            Swamp,
            Meadow,
            Forest,
            Grove,
            Field,
            Plantation,
            Savanna,
            Veld,
            Desert,
            StoneDesert,
            SaltDesert,
            IceDesert,
            Snowfield,
            Tundra,
            NS
        }

        public enum Relation
        {
            Romantic,
            Friendship,
            Family,
            Casual,
            Professional,
            NS
        }

        public enum Closeness
        {
            Inseparable,
            VeryClose,
            Close,
            Distant,
            VeryDistant,
            Foreign,
            NS
        }

        public enum PageType
        {
            List,
            Create,
            Create2,
            Create3,
            Details,
            Edit,
            Delete,
            Pick,
            Pick2,
            Pick3,
            Pick4,
            ListWS
        }

        public enum InputFieldSize
        {
            Small,
            Medium,
            Wide
        }

        public class PropInfo<T> where T:class
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public Type Type { get; set; }

            public T Item { get; set; }

            public PropInfo(string name, string value, Type type, T item)
            {
                Name = name;
                Value = value;
                Type = type;
                Item = item;
            }
        }

        public class ColorSchema
        {
            public string Name { get; private set; }
            public string Dark { get; private set; }
            public string Medium { get; private set; }
            public string Light { get; private set; }

            public static string PrimaryDark { get; private set; } = "#F8F5EC";
            public static string PrimaryMedium { get; private set; } = "#FFFDF7";

            public static string BeigeLight { get; set; } = "#E8DDCD";
            public static string BeigeMedium { get; set; } = "#B3936A";

            private ColorSchema(string name, string c1, string c2, string c3)
            {
                Name = name;
                Dark = c1;
                Medium = c2;
                Light = c3;
            }

            public static ColorSchema Blue = new ColorSchema("Blue", "#181749","#2C2A82", "#686797");
            public static ColorSchema Red = new ColorSchema("Red", "#711831", "#A32247", "#BE768F");
            public static ColorSchema Green = new ColorSchema("Green", "#4C7E1A","#66A723", "#9FC379");
            public static ColorSchema Gray = new ColorSchema("Gray", "#636363", "#999999", "#CECECE");
            public static ColorSchema Sun = new ColorSchema("Sun", "#F7A902","#FDC62D", "#FDDB7D");

            public static ColorSchema GetColorSchemaByName(string name)
            {
                switch (name)
                {
                    case "Blue": return Blue;
                    case "Red": return Red;
                    case "Green": return Green;
                    case "Gray": return Gray;
                    case "Yellow": return Sun;
                    default: return null;
                }
            }

            public static List<string> GetColorNames()
            {
                return new List<string>()
                {
                    "Blue","Gray","Green","Red"
                };
            }
        }

        public static List<string> GetTypesAsList(string name)
        {
            var types = new Types();
            var props = types.GetType().GetProperties();
            var items = new List<string>();

            if (props != null && props.Any())
            {
                props.ToList().ForEach(prop =>
                {
                    if (prop.Name == name)
                    {
                        var typ = prop.PropertyType;
                        FieldInfo[] fields = typ.GetFields();

                        foreach (var field in fields)
                        {
                            items.Add(field.GetRawConstantValue().ToString());
                        }
                    }
                });
            }

            return items;
        }

        public static List<string> GetTypesAsList<T>() where T : struct, IConvertible
        {
            var list = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var result = new List<string>();

            list.ForEach(item =>
            {
                result.Add(item.ToString());
            });

            return result;
        }

        public static List<string> GetTypesAsListWithoutNone<T>() where T : struct, IConvertible
        {
            var list = GetTypesAsList<T>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == "NS")
                    list.RemoveAt(i);
            }

            return list;
        }
    }
}