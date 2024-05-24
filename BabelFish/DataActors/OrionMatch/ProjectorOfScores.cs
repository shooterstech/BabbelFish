﻿using System;
using System.Collections.Generic;
using System.Text;
using NLog.Filters;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// Abstract base class, for classes that try to predict the final scores a participant will 
    /// finish with, based on the scores they already shot.
    /// 
    /// Each concrete class that implements ProjectorOfScores will have its own algorithm for
    /// making the predicted scores.
    /// </summary>
    public abstract class ProjectorOfScores {

        public ProjectorOfScores( CourseOfFire courseOfFire ) {
            this.CourseOfFire = courseOfFire;

            //The top level event should (better be) the only Event Type == EVENT.
            this.TopLevelEvent = EventComposite.GrowEventTree( this.CourseOfFire );
        }

        public CourseOfFire CourseOfFire { get; private set; }

        public EventComposite TopLevelEvent { get; private set; }

        /// <summary>
        /// Calculates projected scores for the passed in IEventScoreProjection. Stores
        /// the projected scores in the EventScore's .Projected Score instance.
        /// </summary>
        /// <param name="projection"></param>
        public abstract void ProjectEventScores( IEventScoreProjection projection );

        /// <summary>
        /// When generating a Projected Result List, the Result List needs to identify who/what
        /// made the projection. This string prepresents the concrete class that made the projection
        /// and shold populate that Result List value.
        /// </summary>
        public abstract string ProjectionMadeBy { get; }
    }
}
