﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldFloatList : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldFloatList() {
            MultipleValues = true;
            ValueType = ValueType.FLOAT;
            Validation = new AttributeValidationFloat();
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<float> DefaultValue { get; private set; } = new List<float>();

        private AttributeValidationFloat validation = new AttributeValidationFloat();

        /// <inheritdoc />
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationFloat) {
                    validation = (AttributeValidationFloat)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationFloat, instead received {value.GetType()}" );
                }
            }
        }
    }
}
