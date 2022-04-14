﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider.Model;

namespace BabelFish.Tests {
    [TestClass]
    public class CognitoTest {

        private static string clientID = "3mbaq4124a5emsap21krllrcrj";
        private static string poolID = "us-east-1_ujMUC50fP";
        private static string identityPoolID = "us-east-1:ecdb1323-5308-445f-845e-55871ebf14e2";
        private static string username = "test_dev_7@shooterstech.net";
        private static string password = "abcd1234";
        private static string myRandomAssDevicePassword = "simple";

        [TestMethod]
        public async Task LoginTest() {

            //See:https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/cognito-authentication-extension.html
            //See: https://github.com/aws/aws-sdk-net-extensions-cognito#authenticating-with-secure-remote-protocol-srp

            //Create anonymous credentials to create a Cognito Identify Provider
            AmazonCognitoIdentityProviderClient provider =
                new AmazonCognitoIdentityProviderClient( new Amazon.Runtime.AnonymousAWSCredentials() );

            //Set up the initial request when we don't have a refresh token. Obviously assumes an existing user.
            CognitoUserPool userPool = new CognitoUserPool( poolID, clientID, provider );
            CognitoUser user = new CognitoUser( username, clientID, userPool, provider );
            //This is the only time we need the user's password (assuming the soon to be obtained refresh token stays valid)
            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest() {
                Password = password                 
            };

            //Send the request to Cognito for authentication
            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync( authRequest ).ConfigureAwait( false );
            //TODO: Determine the response when the username/password don't match.


            //The returned authResponse may present a second set of challenges. 
            // * New Passwword -> RespondToNewPasswordRequiredAsync()
            // * MFA Response -> RespondToSmsMfaAuthAsync() or RespondToMfaAuthAsyn (probable doesn't apply to us)

            //Both the AccessToken (good for 60 minutes by default) and RefreshToken (good for 30 days) need to be saved
            //DeviceKey also needs to be saved. See below, may also need to save DeviceGroupKey, and the device password.
            var accessToken = authResponse.AuthenticationResult.AccessToken;
            var refreshToken = authResponse.AuthenticationResult.RefreshToken;
            var deviceKey = authResponse.AuthenticationResult.NewDeviceMetadata.DeviceKey;

            //Now we can get AWS Credentials for the user.
            CognitoAWSCredentials credentials =
                user.GetCognitoAWSCredentials( identityPoolID, RegionEndpoint.USEast1 );
            //TODO Using the credentials, need to obtain the Key ID and SessionToken
            //To do so, use the .get_id() and .get_credentials_for_identity() calls

            //Above, when we made the StartWithSrpAuthAsync Call, the call also generates a temporary 'Device'
            //Now need to tell Cognito to confirm this temporary device.
            //TODO: Determine if we will need to remember the device password.
            //TODO: Determine how device authentication will work with users signing onto www server (instead of an App).

            //Device Verification happens locally. the Salt and password verifier gets sent to Cognito in the next .ConfirmDeviceAsync() call
            var deviceVerifier = user.GenerateDeviceVerifier(
                authResponse.AuthenticationResult.NewDeviceMetadata.DeviceGroupKey,
                myRandomAssDevicePassword,
                username );

            ConfirmDeviceResponse confirmDeviceResponse = await user.ConfirmDeviceAsync(
                accessToken, 
                deviceKey,
                "Bob",
                deviceVerifier.PasswordVerifier,
                deviceVerifier.Salt);

            //Now attach the newly attached Device to the user. We need this step to successfully complete the StartWithRefreshTokenAuthAsync() call
            //NOTE There may be a better way to do this, but method here is to first create a generic Device, with the known 
            //device key. Then Use GetDeviceAsync() to pull the real details from Cognito
            CognitoDevice device = new CognitoDevice(
                deviceKey,
                new Dictionary<string, string>(),
                DateTime.Today,
                DateTime.Today,
                DateTime.Today,
                user );
            await device.GetDeviceAsync();
            user.Device = device;

            //Now pretend we need ot fast foward in time and refresh the tokens

            //See: https://github.com/aws/aws-sdk-net-extensions-cognito#authenticating-using-a-refresh-token-from-a-previous-session

            //I'm actaully not sure what this step does ..
            user.SessionTokens = new CognitoUserSession( null, null, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours( 1 ) );

            //Create the refresh request object
            InitiateRefreshTokenAuthRequest refreshRequest = new InitiateRefreshTokenAuthRequest() {
                AuthFlowType = AuthFlowType.REFRESH_TOKEN_AUTH
            };
                        
            //CAll Cognito to refresh the token
            AuthFlowResponse authResponse2 = await user.StartWithRefreshTokenAuthAsync( refreshRequest ).ConfigureAwait( false );

            //Now we have a new accessToken and a new refreshToken, both of which need to be re-saved
            var accessToken2 = authResponse2.AuthenticationResult.AccessToken;
            var refreshToken2 = authResponse2.AuthenticationResult.RefreshToken;

            //Again, we can get new credentials for signing API requests
            CognitoAWSCredentials credentials2 =
                user.GetCognitoAWSCredentials( identityPoolID, RegionEndpoint.USEast1 );
        }
    }
}
