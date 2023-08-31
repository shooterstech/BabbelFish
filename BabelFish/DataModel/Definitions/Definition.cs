﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations; //COMMENT OUT FOR .NET Standard 2.0
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public abstract class Definition : BaseClass {

        [NonSerialized]
        public List<string> errorList = new List<string>();

        /// <summary>
        /// Public constructor for Definition class.
        /// </summary>
        public Definition() {
            Discipline = DisciplineType.NA; //Not all definitions use Discipline. Setting it null so by default it does not get included on JSON
            Discontinued = false;
        }

        internal void OnDeserializedMethod(StreamingContext context) {
            //Note, each subclass of Definition will have to call base.OnSerializedMethod

            //Assures that Tags won't be null and as a default will be an empty list.
        }

        /// <summary>
        /// HierarchicalName is namespace:properName
        /// </summary>
        [JsonProperty(Order = 1)]
        public string HierarchicalName { get; set; } = string.Empty;

        /// <summary>
        /// A human readable description of this Definition. Can be verbose.
        /// </summary>
        [JsonProperty(Order = 2)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// A human readable short name for this Definition.
        /// </summary>
        [JsonProperty(Order = 2)]
        public string CommonName { get; set; } = string.Empty;

        /// <summary>
        /// Version number of the definiton,, represented in string form. major and minor, e.g. 1.10
        /// Version "0.0" is reserved as a copy of the most up to date version
        /// Version "x.0" is reserved as a copy of the most up to date version within the major version
        /// </summary>
        [JsonProperty(Order = 3)]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Returns the Version of the Definition as an object DefinitionVersion.
        /// the string varation is returned with the property .Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if the Version can not be parsed to format x.y</exception>
        public DefinitionVersion GetDefinitionVersion() {
            return new DefinitionVersion( Version );
        }

        /// <summary>
        /// The Definition Type
        /// </summary>
        [JsonProperty(Order = 4)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DefinitionType Type { get; set; }

        [JsonProperty(Order = 5)]
        public string SetName { get; set; } = string.Empty;

        [JsonProperty(Order = 6)]
        public string Owner { get; set; } = string.Empty;

        [JsonProperty(Order = 7)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DisciplineType Discipline { get; set; }

        [JsonProperty(Order=8)]
        [DefaultValue("")]
        public string Subdiscipline { get; set; } = string.Empty;

        [JsonProperty(Order = 9)]
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        [JsonProperty(Order = 10)]
        [DefaultValue("2020-03-31")]
        public string JSONVersion { get; set; } = string.Empty;

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
                Scopos.BabelFish.DataModel.Definitions.SetName.TryParse(SetName, out sn);
            else
                Scopos.BabelFish.DataModel.Definitions.SetName.TryParse(Version, HierarchicalName, out sn);
            return sn;
        }

        public override string ToString() {
            return $"{Type}: {SetName}";
        }

    }
}
