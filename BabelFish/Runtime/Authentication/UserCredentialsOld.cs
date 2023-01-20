﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.Authentication.Credentials;
using Scopos.BabelFish.Responses.Authentication.Credentials;
using Newtonsoft.Json;

namespace Scopos.BabelFish.Runtime.Authentication
{
    /// <summary>
    /// AWS Credentials
    /// </summary>
    [Serializable]
    public class UserCredentialsOld
    {
        private AWSCognitoAuthenticationOld CognitoAuthentication;

        public UserCredentialsOld() {
            CognitoAuthentication = new AWSCognitoAuthenticationOld( this );
        }

        public UserCredentialsOld(string username, string password)
        {
            Username = username;
            Password = password;

            CognitoAuthentication = new AWSCognitoAuthenticationOld( this );
        }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string AccessKeyId { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public string SessionToken { get; set; } = string.Empty;

        #region CognitoAuthenticationSpecific
        public string RefreshToken { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string IdToken { get; set; } = string.Empty;

        public string DeviceId { get; set; } = string.Empty;

        public string DevicePassword { get; set; } = string.Empty;

        public string LastException { get; set; } = string.Empty;
        #endregion CognitoAuthenticationSpecific

        public CognitoUser CognitoUser { get; set; }



        /// <summary>
        /// Track last time the temporary tokens were generated
        /// These are good for 15 minutes so re-generate if still being used after that
        /// https://docs.aws.amazon.com/AmazonS3/latest/API/sig-v4-authenticating-requests.html
        /// The signed portions (using AWS Signatures) of requests are valid within 15 minutes of the timestamp in the request. An unauthorized party who has access to a signed request can modify the unsigned portions of the request without affecting the request's validity in the 15 minute window.
        /// Using null to ignore refresh because permanent tokens
        /// </summary>
        public DateTime? ContinuationToken { get; set; } = null;

        private double AWSExpirationLimit { get; } = 15;

        public bool TokensExpired()
        {
            if (ContinuationToken == null ||
                (DateTime.Now - ContinuationToken).Value < TimeSpan.FromMinutes(AWSExpirationLimit))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Get temporary credentials based on username/password and populated AccessKey, SecretKey, and SessionToken
        /// </summary>
        /// <returns>True if Existing Tokens not expired or new Tokens retrieved successfully</returns>
        public async Task<bool> GetTempCredentials() {
            try {
                if (TokensExpired()) {
                    if (await CognitoAuthentication.GetCognitoCredentialsAsync().ConfigureAwait( false )) {

                        ContinuationToken = DateTime.Now;
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }
            } catch (Exception ex) {
                LastException = ex.ToString();
                return false;
            }
        }

        public override string ToString()
        {
            return $"Credentials for {Username}";
        }

    }
}