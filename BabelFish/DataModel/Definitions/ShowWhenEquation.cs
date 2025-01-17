﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ShowWhenEquation : ShowWhenBase {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ShowWhenEquation() {
            Operation = ShowWhenOperation.EQUATION;
        }

        /// <inheritdoc/>
        public override ShowWhenBase Copy() {
            ShowWhenEquation copy = new ShowWhenEquation();
            copy.Comment = this.Comment;
            copy.Boolean = this.Boolean;
            foreach( var arg in Arguments)
                copy.Arguments.Add(arg.Copy());

            return copy;
        }

        /// <summary>
        /// The type of boolean operation that should be applied to all of the Arguments.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [DefaultValue( ShowWhenBoolean.AND )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include )]
        public ShowWhenBoolean Boolean { get; set; } = ShowWhenBoolean.AND;

        public List<ShowWhenBase> Arguments { get; set; } = new List<ShowWhenBase>();

        /// <inheritdoc/>
        public override string ToString() {
            return $"{Boolean} {Arguments.Count} arguments";
        }

    }
}
