﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class GetResultListTests {


        [TestMethod]
        public void GetResultListBasicPublicTest() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var resultListName = "Individual - All";

            var taskResultListResponse = client.GetResultListPublicAsync( matchId, resultListName );
            var resultListResponse = taskResultListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, resultListResponse.StatusCode );
            var resultList = resultListResponse.ResultList;

            Assert.AreEqual( matchId.ToString(), resultList.MatchID );
            Assert.AreEqual( resultListName, resultList.ResultName );

            Assert.IsTrue( resultList.Items.Count > 0 );
        }

        [TestMethod]
        public void GetResultListBasicAuthenticatedTest() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023011915575119.0" );
            var resultListName = "Individual - All";

            var taskResultListResponse = client.GetResultListAuthenticatedAsync( matchId, resultListName, userAuthentication );
            var resultListResponse = taskResultListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, resultListResponse.StatusCode );
            var resultList = resultListResponse.ResultList;

            Assert.AreEqual( matchId.ToString(), resultList.MatchID );
            Assert.AreEqual( resultListName, resultList.ResultName );

            Assert.IsTrue( resultList.Items.Count > 0 );
        }

        [TestMethod]
        public void GetResultListTokenizedTest() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023011915575119.0" ); 
            var resultListName = "Individual - All";

            var requestInit = new GetResultListPublicRequest( matchId, resultListName ) {
                Limit = 2 //Setting to a small value so we get the next token in the call.
            };
            var taskResultListResponseInit = client.GetResultListPublicAsync( requestInit );
            var resultListResponseInit = taskResultListResponseInit.Result;

            Assert.AreEqual( HttpStatusCode.OK, resultListResponseInit.StatusCode );
            var resultListInit = resultListResponseInit.ResultList;

            Assert.AreEqual( matchId.ToString(), resultListInit.MatchID );
            Assert.AreEqual( resultListName, resultListInit.ResultName );
            Assert.IsTrue( resultListInit.Items.Count > 0 );
            Assert.AreNotEqual( "", resultListInit.NextToken );

            //Set up the next request
            var requestNext = resultListResponseInit.GetNextRequest();
            Assert.AreEqual( resultListInit.NextToken, requestNext.Token );

            var taskResultListResponseNext = client.GetResultListPublicAsync( requestNext );
            var resultListResponseNext = taskResultListResponseNext.Result;

            Assert.AreEqual( HttpStatusCode.OK, resultListResponseNext.StatusCode );
            var resultListNext = resultListResponseNext.ResultList;

            Assert.AreEqual( matchId.ToString(), resultListNext.MatchID );
            Assert.AreEqual( resultListName, resultListNext.ResultName );
            Assert.IsTrue( resultListNext.Items.Count > 0 );
            Assert.AreNotEqual( "", resultListNext.NextToken );
        }
    }
}
