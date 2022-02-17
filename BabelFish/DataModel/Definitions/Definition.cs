﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.Definitions {
    [Serializable]
    public abstract class Definition {


        /// <summary>
        /// The Reconfigurable Rulebook Definition types.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DefinitionType {
            /*
             * In order to get an Enum to serialize / deserialize to values with spaces have to do a couple of things.
             * First use the EnumMember(Value = "   ") attribute. This is what does the serialzing / deserialzing.
             * In order to get the Descriptions in code, have to use the Description attribute in conjunction with the
             * ExtensionMethod .Description() (Located in the ExtensionMethods.cs class).
            */         
            /// <summary>
            /// ATTRIBUTE Definition
            /// </summary>
            [Description("ATTRIBUTE")] [EnumMember(Value = "ATTRIBUTE")] ATTRIBUTE,

            /// <summary>
            /// COURSE OF FIRE Definition
            /// </summary>
            [Description("COURSE OF FIRE")] [EnumMember(Value = "COURSE OF FIRE")] COURSEOFFIRE,

            /// <summary>
            /// EVENT STYLE Definition
            /// </summary>
            [Description("EVENT STYLE")] [EnumMember(Value = "EVENT STYLE")] EVENTSTYLE,

            /// <summary>
            /// RANGE Definition
            /// </summary>
            [Description("RANGE")] [EnumMember(Value = "RANGE")] RANGE,

            /// <summary>
            /// RANKING RULES Definition
            /// </summary>
            [Description("RANKING RULES")] [EnumMember(Value = "RANKING RULES")] RANKINGRULES,

            /// <summary>
            /// RESULT Definition
            /// </summary>
            [Description("RESULT")] [EnumMember(Value = "RESULT")] RESULT,

            /// <summary>
            /// SCORE FORMAT COLLECTION Definition
            /// </summary>
            [Description("SCORE FORMAT COLLECTION")] [EnumMember(Value = "SCORE FORMAT COLLECTION")] SCOREFORMATCOLLECTION,

            /// <summary>
            /// STAGE STYLE Definition
            /// </summary>
            [Description("STAGE STYLE")] [EnumMember(Value = "STAGE STYLE")] STAGESTYLE,

            /// <summary>
            /// TARGET Definition
            /// </summary>
            [Description("TARGET")] [EnumMember(Value = "TARGET")] TARGET,

            /// <summary>
            /// TARGET COLLECTION Definition
            /// </summary>
            [Description("TARGET COLLECTION")] [EnumMember(Value = "TARGET COLLECTION")] TARGETCOLLECTION,

            /// <summary>
            /// TARGET SCHEME Collection
            /// </summary>
            [Description("TARGET SCHEME")] [EnumMember(Value = "TARGET SCHEME")] TARGETSCHEME
        }

        /// <summary>
        /// Defines the different high level disciplines in use with Shooting. Largely defined by the ISSF.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DisciplineType {
            /// <summary>
            /// The Discipline Archery
            /// </summary>
            [Description("ARCHERY")] [EnumMember(Value = "ARCHERY")] ARCHERY,

            /// <summary>
            /// The Discipline Biathlon
            /// </summary>
            [Description("BIATHLON")] [EnumMember(Value = "BIATHLON")] BIATHLON,

            /// <summary>
            /// Hybrid Discipline, which is when two or more Disciplins are used together.
            /// </summary>
            [Description("HYBRID")] [EnumMember(Value = "HYBRID")] HYBRID,

            /// <summary>
            /// The Pistol Discipline
            /// </summary>
            [Description("PISTOL")] [EnumMember(Value = "PISTOL")] PISTOL,

            /// <summary>
            /// The Rifle Discipline
            /// </summary>
            [Description("RIFLE")] [EnumMember(Value = "RIFLE")] RIFLE,

            /// <summary>
            /// The Running Target Discipline
            /// </summary>
            [Description("RUNNING TARGET")] [EnumMember(Value = "RUNNING TARGET")] RUNNINGTARGET,

            /// <summary>
            /// Shotgun Discipline
            /// </summary>
            [Description("SHOTGUN")] [EnumMember(Value = "SHOTGUN")] SHOTGUN,

            /// <summary>
            /// Not Applicable
            /// </summary>
            [Description("NOT APPLICABLE")] [EnumMember(Value = "NOT APPLICABLE")] NA
        }

        [NonSerialized]
        public List<string> errorList = new List<string>();

        /// <summary>
        /// Public constructor for Definition class.
        /// </summary>
        public Definition() {
            Discipline = DisciplineType.NA; //Not all definitions use Discipline. Setting it null so by default it does not get included on JSON
            Discontinued = false;
            Tags = new List<string>();
        }

        internal void OnDeserializedMethod(StreamingContext context) {
            //Note, each subclass of Definition will have to call base.OnSerializedMethod

            //Assures that Tags won't be null and as a default will be an empty list.
            if (Tags == null)
                Tags = new List<string>();
        }

        /// <summary>
        /// HierarchicalName is namespace:properName
        /// </summary>
        [JsonProperty(Order = 1)]
        public string HierarchicalName { get; set; }

        /// <summary>
        /// A human readable description of this Definition. Can be verbose.
        /// </summary>
        [JsonProperty(Order = 2)]
        public string Description { get; set; }

        /// <summary>
        /// A human readable short name for this Definition.
        /// </summary>
        [JsonProperty(Order = 2)]
        public string CommonName { get; set; }

        /// <summary>
        /// Version number of the definiton. major and minor, e.g. 1.10
        /// Version "0.0" is reserved as a copy of the most up to date version
        /// Version "x.0" is reserved as a copy of the most up to date version within the major version
        /// </summary>
        [JsonProperty(Order = 3)]
        public string Version { get; set; }

        /// <summary>
        /// The Definition Type
        /// </summary>
        [JsonProperty(Order = 4)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DefinitionType Type { get; set; }

        [JsonProperty(Order = 5)]
        public string SetName { get; set; }

        [JsonProperty(Order = 6)]
        public string Owner { get; set; }

        [JsonProperty(Order = 7)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DisciplineType Discipline { get; set; }

        [JsonProperty(Order=8)]
        [DefaultValue("")]
        public string Subdiscipline { get; set; }
        
        [JsonProperty(Order = 9)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        [JsonProperty(Order = 10)]
        [DefaultValue("2020-03-31")]
        public string JSONVersion { get; set; }

        [JsonProperty(Order = 11)]
        [DefaultValue(false)]
        public bool Discontinued { get; set; }

        /// <summary>
        /// Returns a SetName object based on the Definitions Version and HierrchcialName
        /// If originalSetName is true, returns the setname as was loaded, usually with v1.0, or v0.0
        /// If false, returns the Version based on the version in the file
        /// </summary>
        /// <returns></returns>
        public SetName GetSetName(bool originalSetName = false) {
            SetName sn;
            if (originalSetName)
                BabelFish.DataModel.Definitions.SetName.TryParse(SetName, out sn);
            else
                BabelFish.DataModel.Definitions.SetName.TryParse(Version, HierarchicalName, out sn);
            return sn;
        }

    }
}