﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A ShowWhenSegmentGroup is a Show-When expression that evalutes to true or false, based on the value of the SegmentGroupName
    /// stored in teh ResultList's MetaData.
    /// </summary>
    public class ShowWhenSegmentGroup : ShowWhenBase {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ShowWhenSegmentGroup() {
            Operation = ShowWhenOperation.SEGMENT_GROUP;
        }

        /// <inheritdoc/>
        public override ShowWhenBase Copy() {
            ShowWhenSegmentGroup copy = new ShowWhenSegmentGroup();
            copy.Comment = this.Comment;
            copy.SegmentGroupName = this.SegmentGroupName;

            return copy;
        }

        /// <summary>
        /// the name of the SegmentGroup (based on the Coruse of Fire's Range Script), that the competition must be in 
        /// in order for this ShowWhen to be true. It is stored in the Result List's metadata.
        /// </summary>
        public string SegmentGroupName { get; set; }

        public override string ToString() {
            return $"{SegmentGroupName}";
        }
    }
}
