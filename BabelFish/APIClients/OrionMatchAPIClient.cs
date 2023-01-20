﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.APIClients {
    public class OrionMatchAPIClient : APIClient {

        /// <summary>
        /// Default constructor.
        /// Assumes Production stage level.
        /// </summary>
        /// <param name="xapikey"></param>
        public OrionMatchAPIClient( string xapikey ) : base( xapikey ) { }

        public OrionMatchAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) { }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchPublicResponse> GetMatchDetailPublicAsync( GetMatchPublicRequest requestParameters ) {

            GetMatchPublicResponse response = new GetMatchPublicResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="withAuthentication">default false</param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchPublicResponse> GetMatchDetailPublicAsync( string matchid, bool withAuthentication = false ) {
            var request = new GetMatchPublicRequest( matchid );

            return await GetMatchDetailPublicAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="requestParameters">GetResultListRequest object</param>
        /// <returns>ResultList Object</returns>
        public async Task<GetResultListPublicResponse> GetResultListPublicAsync( GetResultListPublicRequest requestParameters ) {
            GetResultListPublicResponse response = new GetResultListPublicResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="listname"></param>
        /// <returns>ResultList Object</returns>
        public async Task<GetResultListPublicResponse> GetResultListPublicAsync( string matchid, string listname ) {
            return await GetResultListPublicAsync( new GetResultListPublicRequest( matchid, listname ) ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="requestParameters">GetSquaddingListRequest object</param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListPublicResponse> GetSquaddingListPublicAsync( GetSquaddingListPublicRequest requestParameters ) {

            GetSquaddingListPublicResponse response = new GetSquaddingListPublicResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <param name="withAuthentication">default false</param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListPublicResponse> GetSquaddingListPublicAsync( string matchid, string squaddinglistname, bool withAuthentication = false ) {
            var request = new GetSquaddingListPublicRequest( matchid, squaddinglistname );

            return await GetSquaddingListPublicAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters">GetResultCOFDetailRequest object</param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFDetailPublicResponse> GetResultCourseOfFireDetailPublicAsync( GetResultCOFDetailPublicRequest requestParameters ) {
            GetResultCOFDetailPublicResponse response = new GetResultCOFDetailPublicResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFDetailPublicResponse> GetResultCourseOfFireDetailPublicAsync( string resultCOFID ) {
            return await GetResultCourseOfFireDetailPublicAsync( new GetResultCOFDetailPublicRequest( resultCOFID ) ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Match Locations API
        /// </summary>
        /// <returns>List<MatchLocation> Object</returns>
        public async Task<GetMatchLocationsPublicResponse> GetMatchLocationsPublicAsync() {
            GetMatchLocationsPublicResponse response = new GetMatchLocationsPublicResponse( new GetMatchLocationsPublicRequest() );

            var request = new GetMatchLocationsPublicRequest();

            await this.CallAPI( request, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Match Search API
        /// </summary>
        /// <param name="requestParameters">GetMatchSearchRequest object</param>
        /// <returns>List<Match> Object</returns>
        public async Task<GetMatchSearchResponse> GetMatchSearchPublicAsync( GetMatchSearchPublicRequest requestParameters ) {
            GetMatchSearchResponse response = new GetMatchSearchResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Participant List API
        /// </summary>
        /// <param name="requestParameters">GetParticipantListRequest object</param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetParticipantListPublicResponse> GetMatchParticipantListPublicAsync( GetParticipantListPublicRequest requestParameters ) {
            GetParticipantListPublicResponse response = new GetParticipantListPublicResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Participant List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetParticipantListPublicResponse> GetMatchParticipantListPublicAsync( string matchid ) {
            var request = new GetParticipantListPublicRequest( matchid );

            return await GetMatchParticipantListPublicAsync( request ).ConfigureAwait( false );
        }

    }
}