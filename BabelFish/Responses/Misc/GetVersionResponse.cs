﻿using ShootersTech.DataModel.Misc;
using Newtonsoft.Json.Linq;

namespace ShootersTech.Responses.Misc
{
    public class GetVersionResponse : Response<VersionsList>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<VersionInfo> VersionList
        {
            get { return Value.Versions; }
        }
    }
}