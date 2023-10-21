﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetLeagueGamesPublicResponse : Response<LeagueGameSearchWrapper>, ITokenResponse<GetLeagueGamesPublicRequest> {

        public GetLeagueGamesPublicResponse( GetLeagueGamesPublicRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public LeagueGameSearch LeagueGames
        {
            get { return Value.LeagueGames; }
        }

        /// <inheritdoc/>
        public GetLeagueGamesPublicRequest GetNextRequest() {
            var nextRequest = (GetLeagueGamesPublicRequest)Request.Copy();
            nextRequest.Token = Value.LeagueGames.NextToken;
            return nextRequest;
		}
	}
}
