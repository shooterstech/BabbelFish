﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDate : AttributeField, ICopy<AttributeFieldDate> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDate() {
            MultipleValues = false;
            ValueType = ValueType.DATE;
        }

        /// <inheritdoc />
        public AttributeFieldDate Copy() {

            var copy = new AttributeFieldDate();
            base.Copy( copy );

            copy.DefaultValue = this.DefaultValue;

            return copy;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime DefaultValue { get; set; } = DateTime.Now;
    }
}
