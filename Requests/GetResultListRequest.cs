﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.Requests {
    public class GetResultListRequest : Request
    {

        public string MatchID { get; set; } = string.Empty;
        public string ResultListName { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/match/{MatchID}/result-list/{ResultListName}"; }
        }
    }
}