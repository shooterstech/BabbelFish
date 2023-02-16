﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.Tests.AttributeValue {

    /// <summary>
    /// The methods in this class largely test the AttributeField class, and if data types are stored and returned in their expected format.
    /// </summary>
    [TestClass]
    public class SettingFieldValueTests {

        [TestMethod]
        public void HappyPathSingleAttributeFieldDataTypes() {
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var setNameTestAttriubte = "v1.0:orion:Test Attribute";
            var testAttrValue = new Scopos.BabelFish.DataModel.AttributeValue.AttributeValue( setNameTestAttriubte );

            //Create some random data to store
            var random = new Random();
            var myInt = random.Next();
            var myFloat = random.NextDouble();
            var myBool = random.NextInt64() % 2 == 0;
            var myDate = (new DateTime( random.NextInt64( 0, DateTime.MaxValue.Ticks ) )).Date;
            var myDateTime = new DateTime( random.NextInt64( 0, DateTime.MaxValue.Ticks ) );
            var myTime = new TimeSpan( random.Next() );
            var myString = Scopos.BabelFish.Helpers.RandomStringGenerator.RandomAlphaString( 8 );

            //Set values to the attribute value.
            testAttrValue.SetFieldValue( "AString", myString );
            testAttrValue.SetFieldValue( "AnInteger", myInt );
            testAttrValue.SetFieldValue( "AFloat", myFloat );
            testAttrValue.SetFieldValue( "ABoolean", myBool );
            testAttrValue.SetFieldValue( "ADate", myDate );
            testAttrValue.SetFieldValue( "ATime", myTime );
            testAttrValue.SetFieldValue( "ADateTime", myDateTime );

            //Now test that the GetFieldValue return the same data that we stored
            Assert.AreEqual( myString, (string)testAttrValue.GetFieldValue( "AString" ) );
            Assert.AreEqual( myInt, (int)testAttrValue.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( myFloat, (double)testAttrValue.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( myBool, (bool)testAttrValue.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( myDate, (DateTime)testAttrValue.GetFieldValue( "ADate" ) );
            //Because Times and DateTime are stored with known rounding error, will allow this much tolerance in teh comparison.
            Assert.IsTrue( Math.Abs( ((TimeSpan)testAttrValue.GetFieldValue( "ATime" ) - myTime).TotalSeconds ) < .001D );
            Assert.IsTrue( Math.Abs( ((DateTime)testAttrValue.GetFieldValue( "ADateTime" ) - myDateTime).TotalMilliseconds ) < .001D );
        }

        [TestMethod]
        public void HappyPathListAttributeFieldDataTypes() {
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var setNameTestAttriubte = "v1.0:orion:Test Attribute";
            var testAttrValue = new Scopos.BabelFish.DataModel.AttributeValue.AttributeValue( setNameTestAttriubte );

            //Create some random data to store
            List<string> myListOfStrings = new List<string>();
            for( int i = 0; i < 10; i++ ) {
                myListOfStrings.Add( Scopos.BabelFish.Helpers.RandomStringGenerator.RandomAlphaString( 8 ) );
            }

            //Set values to the attribute value.
            testAttrValue.SetFieldValue( "AListOfStrings", myListOfStrings );

            //Now test that the GetFieldValue return the same data that we stored
            var myReturnedListOfStrings = (List<string>) testAttrValue.GetFieldValue( "AListOfStrings" );

            for( int i = 0; i < 10;i++ ) {
                Assert.AreEqual( myListOfStrings[i], myReturnedListOfStrings[i] );
            }
        }

        /// <summary>
        /// Tries an stores an integer to a string field. Should throw an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( AttributeValueValidationException ) )]
        public void WrongDataTypeForString() {
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var setNameTestAttriubte = "v1.0:orion:Test Attribute";
            var testAttrValue = new Scopos.BabelFish.DataModel.AttributeValue.AttributeValue( setNameTestAttriubte );

            testAttrValue.SetFieldValue( "AString", 1234 );
        }

        /// <summary>
        /// Tries an stores an double to an intgeer field. Should throw an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( AttributeValueValidationException ) )]
        public void WrongDataTypeForInt() {
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var setNameTestAttriubte = "v1.0:orion:Test Attribute";
            var testAttrValue = new Scopos.BabelFish.DataModel.AttributeValue.AttributeValue( setNameTestAttriubte );

            testAttrValue.SetFieldValue( "AnInteger", 1234.5678 );
        }

        /// <summary>
        /// Tries an stores an field value, with a field key, to a Attribute that does not have multiple values.
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( AttributeValueException ) )]
        public void WrongUseOfSetFieldValue1() {
            AttributeValueDefinitionFetcher.FETCHER.XApiKey = Constants.X_API_KEY;

            var setNameTestAttriubte = "v1.0:orion:Test Attribute";
            var testAttrValue = new Scopos.BabelFish.DataModel.AttributeValue.AttributeValue( setNameTestAttriubte );

            testAttrValue.SetFieldValue( "AString", 1234, "MyFieldKey" );
        }
    }
}
