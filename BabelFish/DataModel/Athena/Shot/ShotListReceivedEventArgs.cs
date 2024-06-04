﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel.Athena.Shot
{
    public class ShotListReceivedEventArgs : EventArgs
    {

        public ShotListReceivedEventArgs()
        {

        }

        public Scopos.BabelFish.DataModel.Athena.Shot.ShotList ShotList { get; set; }

        /// <summary>
        /// Original topic received from IOT
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// The Orion Account Number received, as inpterpreted from the topic
        /// </summary>
        public long OrionAccountNumber { get; set; }

        /// <summary>
        /// The MatchID received, as interpreted from the topic
        /// </summary>
        public Scopos.BabelFish.DataModel.OrionMatch.MatchID MatchID { get; set; }

        /// <summary>
        /// Original message received from IOT
        /// </summary>
        public string Message { get; set; }
    }
}
