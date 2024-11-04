﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ShowWhenVariable : ShowWhenBase {

        public static readonly ShowWhenVariable ALWAYS_SHOW = new ShowWhenVariable() {
            Condition = ShowWhenCondition.TRUE
        };

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ShowWhenVariable() {
            Operation = ShowWhenOperation.VARIABLE;
        }

        /// <inheritdoc/>
        public override ShowWhenBase Copy() {
            ShowWhenVariable copy = new ShowWhenVariable();
            copy.Comment = this.Comment;
            copy.Condition = this.Condition;

            return copy;
        }

        [JsonConverter( typeof( StringEnumConverter ) )]
        public ShowWhenCondition Condition { get; set; }

        public override string ToString() {
            return $"{Condition}";
        }
    }
}
