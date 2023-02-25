﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class MatchID : IEquatable<MatchID> {

        public const int SUBMATCHID_LOCAL = 0;
        public const int SUBMATCHID_VIRTUAL_PARENT = 1;
        public const int SUBMATCHID_MATCH_GROUP = 2;
        public const int SUBMATCHID_LEAGUE = 3;
        public const int SUBMATCHID_PRACTICE = 4;

        private long domainID = 1;
        private long componentID = 0;
        private long primaryMatchID = 0;
        private long subMatchID = 0;

        /// <summary>
        /// Creates a new instance of a MatchID object based on the passed in string, that it expects to be in the MatchID format. 
        /// </summary>
        /// <param name="fullMatchID"></param>
        /// <exception cref="FormatException">Thrown if the passed in fullMatchID string is not in the expected format.</exception>
        public MatchID( string fullMatchID ) {
            string[] parts = fullMatchID.Split( new char[] { '.' } );

            if (parts.Length == 4
             && long.TryParse( parts[0], out domainID ) && domainID > 0
             && long.TryParse( parts[1], out componentID ) && componentID > 0
             && long.TryParse( parts[2], out primaryMatchID ) && primaryMatchID > 0
             && long.TryParse( parts[3], out subMatchID ) && subMatchID >= 0) {
                //Expected Match ID format
                return;
            }

            throw new FormatException( $"The match ID {fullMatchID} is not in the expected Match ID format" );
        }

        private MatchID( long domainID, long componentID, long primaryMatchID, long subMatchID ) {
            this.domainID = domainID;
            this.componentID = componentID;
            this.primaryMatchID = primaryMatchID;
            this.subMatchID = subMatchID;
        }

        /// <summary>
        /// Creates a new MatchID based on the passed in values for domain id, component id, and sub match id.
        /// The primary match id value is filled in using a time stamp.
        /// </summary>
        /// <param name="domainID"></param>
        /// <param name="componentID"></param>
        /// <param name="subMatchID"></param>
        /// <exception cref="FormatException">Thrown if one of the passed in values is illegal.</exception>
        public MatchID( long domainID, long componentID, long subMatchID ) {
            switch (subMatchID) {
                case SUBMATCHID_LOCAL:
                case SUBMATCHID_LEAGUE:
                case SUBMATCHID_MATCH_GROUP:
                case SUBMATCHID_PRACTICE:
                case SUBMATCHID_VIRTUAL_PARENT:
                    break;
                default:
                    if (subMatchID > 1000)
                        break;
                    else {
                        throw new FormatException( $"The subMatchID is an illegal value." );
                    }
            }

            this.domainID = domainID;
            this.componentID = componentID;
            this.primaryMatchID = newPrimatchMatchID();
            this.subMatchID = subMatchID;
        }

        private long newPrimatchMatchID() {
            var now = DateTime.Now.ToString( "yyyyMMddHHmmssff" );
            return long.Parse( now );
        }

        public override string ToString() {
            StringBuilder id = new StringBuilder();
            id.Append( domainID );
            id.Append( '.' );
            id.Append( componentID );
            id.Append( '.' );
            id.Append( primaryMatchID );
            id.Append( '.' );
            id.Append( subMatchID );
            return id.ToString();
        }

        /// <summary>
        /// Returns the MatchID of the Virtual Match Parent.
        /// If this is a local match (or anything other than a Child Match) returns the existing ID
        /// </summary>
        /// <returns></returns>
        public MatchID GetParentMatchID() {
            if (VirtualMatchChild)
                return new MatchID( domainID, componentID, primaryMatchID, 1 );
            else
                return this;
        }

        /// <summary>
        /// Returns the Domain value from this Match ID
        /// </summary>
        public long DomainID {
            get { return domainID; }
        }

        /// <summary>
        /// Returns the Component value from this Match ID. This is usually a reference to the account number of the owner of the match.
        /// </summary>
        public long ComponentID {
            get { return componentID; }
        }

        /// <summary>
        /// Returns the Primary Match ID value from this Match ID. Usually this is formatted as a time stamp.
        /// </summary>
        public long PrimaryMatchID {
            get { return primaryMatchID; }
        }

        /// <summary>
        /// Returns the Sub Match ID value form this Match ID. This signifies if it is a local, virtual parent, virtual child, tournament, or a league.
        /// </summary>
        public long SubMatchID {
            get { return subMatchID; }
        }

        /// <summary>
        /// Returns a boolean indicating if this Match ID represents a Local match. A Local match is when only one Orion instance is used in scoring.
        /// </summary>
        public bool LocalMatch {
            get {
                switch (subMatchID) {
                    case SUBMATCHID_LOCAL:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Returns a boolean indicating if this Match ID represents a Virtual Match. Either because it is a Parent or a Child.
        /// </summary>
        public bool VirtualMatch {
            get {
                switch (subMatchID) {
                    case SUBMATCHID_LOCAL:
                    case SUBMATCHID_MATCH_GROUP:
                    case SUBMATCHID_LEAGUE:
                    case SUBMATCHID_PRACTICE:
                        return false;
                    default:
                        return true;
                }
            }
        }

        /// <summary>
        /// Returns a boolean indicating if this Match ID represents a Virtual Match Parent.
        /// </summary>
        public bool VirtualMatchParent {
            get {
                switch (subMatchID) {
                    case SUBMATCHID_VIRTUAL_PARENT:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Returns a boolean indicating if this Match ID represents a Virtual Match Child.
        /// </summary>
        public bool VirtualMatchChild {
            get {
                return (subMatchID >= 1000);
            }
        }

        /// <summary>
        /// Returns a boolean indicating if this Match ID represents a Match Group. A Match Group is a synonym for a Tournament.
        /// </summary>
        public bool MatchGroup {
            get {
                switch (subMatchID) {
                    case SUBMATCHID_MATCH_GROUP:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Returns a boolean indicating if this Match ID represents a League.
        /// </summary>
        public bool League {
            get {
                switch (subMatchID) {
                    case SUBMATCHID_LEAGUE:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Returns a boolean indicating if this Match ID is for a practice match.
        /// </summary>
        public bool Practice {
            get {
                switch (subMatchID) {
                    case SUBMATCHID_PRACTICE:
                        return true;
                    default:
                        return false;
                }
            }
        }

        #region IEquatable<MatchID> Members

        /// <summary>
        /// Returns true if the passed in MatchID has the same value as the current MatchID.
        /// Otherwise returns false.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals( MatchID other ) {
            if (other == null)
                return false;

            return (this.DomainID == other.DomainID
                && this.ComponentID == other.ComponentID
                && this.PrimaryMatchID == other.PrimaryMatchID
                && this.SubMatchID == other.SubMatchID);
        }

        /// <summary>
        /// Returns an int, representing the hash code of the current MatchID.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return this.DomainID.GetHashCode() | this.ComponentID.GetHashCode() | this.PrimaryMatchID.GetHashCode() | this.SubMatchID.GetHashCode();
        }

        #endregion
    }
}