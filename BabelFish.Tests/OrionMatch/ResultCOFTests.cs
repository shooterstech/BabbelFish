﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;
using System.Runtime.CompilerServices;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class ResultCOFTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        [TestMethod]
        public async Task TranslateShotsToDictionaryUsingEventNames() {

            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );
            var cofResponse = await client.GetResultCourseOfFireDetailPublicAsync( "91eafa91-6569-434f-85ca-4d99a2f9bc74" );
            var resultCof = cofResponse.ResultCOF;

            var shots = resultCof.GetShotsByEventName();

            Assert.AreEqual( 60, shots.Count );

        }

        [TestMethod]
        public async Task ReadConvertedResultCOFTest() {

            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            //This result cof id is one that was generated by an old version of Orion and needs to be converted.
            //So old that the attri values listed, are not known and have to be removed during the serializetion / constructor process
            var cofResponse = await client.GetResultCourseOfFireDetailPublicAsync( "9605b455-5de6-49a5-83f7-e0681db257f8" ); // "9605b455-5de6-49a5-83f7-e0681db257f8" );
            var resultCof = cofResponse.ResultCOF;

            var shots = resultCof.GetShotsByEventName();
            Assert.AreEqual( 30, shots.Count );

            Assert.AreEqual( 0, resultCof.Participant.AttributeValues.Count );

        }

        [TestMethod]
        public async Task ResultCOFCachingTest() {

            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var cofResponse1 = await client.GetResultCourseOfFireDetailPublicAsync( "fd668b05-0073-4a7b-adcc-25bd45f8b199" );
            var resultCof1 = cofResponse1.ResultCOF;

            var cofResponse2 = await client.GetResultCourseOfFireDetailPublicAsync( "fd668b05-0073-4a7b-adcc-25bd45f8b199" );
            var resultCof2 = cofResponse1.ResultCOF;

            Assert.IsTrue( cofResponse1.TimeToRun > cofResponse2.TimeToRun * 10 );

        }

        [TestMethod]
        public async Task GetRCOFLastShot()
        {

            var client = new OrionMatchAPIClient(APIStage.PRODUCTION);
            var cofResponse = await client.GetResultCourseOfFireDetailPublicAsync("91eafa91-6569-434f-85ca-4d99a2f9bc74");
            var resultCof = cofResponse.ResultCOF;

            var shots = resultCof.Shots;

            var lastShot = resultCof.GetLastShot();

            Assert.AreEqual(shots["60.0"].TimeScored, lastShot.TimeScored);

        }
    }
}
