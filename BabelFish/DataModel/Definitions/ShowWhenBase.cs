﻿using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {


    [Serializable]
    public abstract class ShowWhenBase: IReconfigurableRulebookObject {

        public ShowWhenOperation Operation { get; protected set; } = ShowWhenOperation.VARIABLE;

        /// <inheritdoc/>
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
