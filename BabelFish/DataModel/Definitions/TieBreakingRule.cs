﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.XPath;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class TieBreakingRule : IReconfigurableRulebookObject, ICopy<TieBreakingRule> {

        public TieBreakingRule() { }

        /// <inheritdoc />
        public TieBreakingRule Copy() {
            TieBreakingRule copy = new TieBreakingRule();
            copy.EventName = this.EventName;
            copy.Values = this.Values;
            copy.Method = this.Method;
            copy.SortOrder = this.SortOrder;
            copy.ResultStatus = this.ResultStatus;
            copy.Source = this.Source;

            return copy;
        }

        /// <summary>
        /// The EventName to apply this rule to that is defined by the Course of Fire and found in the participant’s ResultCOF. 
        /// 
        /// The result engine must use this rule if the EventName is found in the participant’s ResultCOF. If the EventName is not found this TieBreakingRule is skipped.
        /// 
        /// May contain a place holder "{}". If used, ValueSeries must be included to compile the list of EventNames to check.
        /// 
        /// This attribute is required when Method is Score or CountOf.Ignored when Method is ParticipantAttribute or Attribute.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// When EventName contains a place holder "{}", the ValueSeries are used to compile the actual list of EventNames to check against.
        /// 
        /// Required when EventName has a placeholder, ignored otherwise.
        /// </summary>
        public string Values {  get; set; }

        /// <summary>
        /// Specifies the method to use to compare two competitors.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public TieBreakingRuleMethod Method { get; set; }

        /// <summary>
        /// Added information to work with Method.
        /// </summary>
        public dynamic Source { get; set; }

        /// <summary>
        /// How the comparison should be made.
        /// </summary>
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Populate )]
        public SortBy SortOrder { get; set; }

        /// <summary>
        /// Apply this TieBreakingRule only if the Result List status is greater than
        /// or equal to .ResultStatus.
        /// 
        /// For example, if .ResultStatus is FUTURE (which is the default), then this 
        /// TieBreakRule should always be used. If .ResultStatus is UNOFFICIAL, then it
        /// should only be used when the result list status is UNOFFICIAL or OFFICIAL, and
        /// should not be used when it is FUTURE or INTERMEDIATE.
        /// </summary>
        [DefaultValue( ResultStatus.FUTURE )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ResultStatus ResultStatus { get; set; } = ResultStatus.FUTURE;

        /// <summary>
        /// If the fields EventName and Values require interpretation, CompiledTieBreakingRules
        /// interpres them and returns a new list of TieBreakingRules cooresponding to the interpretation.
        /// If interpretation is not required, then it returns a list of one tie breaking rule, itself.
        /// </summary>
        public List<TieBreakingRule> GetCompiledTieBreakingRules() {
            if (string.IsNullOrEmpty( EventName ) || !EventName.Contains( "{}" )) {
                return new List<TieBreakingRule>() { this };
            } else {
                List<TieBreakingRule> list = new List<TieBreakingRule>();
                ValueSeries vs = new ValueSeries( this.Values );
                foreach (var eventName in vs.GetAsList( this.EventName )) {
                    var newTieBreakingRule = this.Copy();
                    newTieBreakingRule.EventName = eventName;
                    newTieBreakingRule.Values = "";
                    list.Add( newTieBreakingRule );
                }
                return list;
            }
        }

        public override string ToString() {
            return $"{Method} {SortOrder} {Source}";
        }

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;

    }
}
