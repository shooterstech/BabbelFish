﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetSquaddingListPublicResponse : Response<SquaddingListWrapper>, ITokenResponse<GetSquaddingListPublicRequest>
    {

        public GetSquaddingListPublicResponse( GetSquaddingListPublicRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public SquaddingList SquaddingList {
            get { return Value.SquaddingList; }
        }

        /// <inheritdoc/>
        public GetSquaddingListPublicRequest GetNextRequest() {
            var nextRequest = (GetSquaddingListPublicRequest) Request.Copy();
            nextRequest.Token = Value.SquaddingList.NextToken;
            return nextRequest;
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            try {
                var timeSinceLastUpdate = DateTime.UtcNow - SquaddingList.LastUpdated;

                //If it was recently updated, set the expiry time fairly quickly, as more changes may be coming.
                if (timeSinceLastUpdate.TotalMinutes < 5)
                    return DateTime.UtcNow.AddSeconds( 30 );

                if (timeSinceLastUpdate.TotalMinutes < 60)
                    return DateTime.UtcNow.AddMinutes( 1 );

                if (timeSinceLastUpdate.TotalHours < 10)
                    return DateTime.UtcNow.AddMinutes( 5 );

                return DateTime.UtcNow.AddMinutes( 10 );
            } catch (Exception ex) {
                //Likely will never get here, if so, likely from a very old match.
                return DateTime.UtcNow.AddMinutes( 10 );
            }
        }
    }
}
