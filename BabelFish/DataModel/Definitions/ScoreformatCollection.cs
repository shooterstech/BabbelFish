﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Definitions {

    [Serializable]
    public class ScoreFormatCollectionDefinition : Definition {
        
        public ScoreFormatCollectionDefinition() : base() {
            Type = Definition.DefinitionType.SCOREFORMATCOLLECTION;
            ScoreConfigs = new List<ScoreConfig>();
        }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        public List<string> ScoreFormats { get; set; }

        public List<ScoreConfig> ScoreConfigs { get; set; }
    }
}