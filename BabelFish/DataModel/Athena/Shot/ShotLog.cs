﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Shot
{
	[Serializable]
	public class ShotLog
    {

        public string TimeUpdated { get; set; }

        public string Updator { get; set; }

        public string Message { get; set; }
    }
}