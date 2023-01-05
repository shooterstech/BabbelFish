﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.Requests {

    /// <summary>
    /// An IToken interface is for Request objects that follow
    /// the standard NextToken / Token programming model. 
    /// 
    /// The request needs to have 'token' as an optional query string parameter (that's this interface).
    /// 
    /// The top level response object may return NextToken, to indicate more data is avaliable.
    /// The response object's primary data model must also include a List<>. It is this list that could be combined accross multiple calls. 
    /// </summary>
    public interface IToken {

        /// <summary>
        /// Token is used in a request when there is more data to return. With this pattern, the initial response will contain a value
        /// for NextToken. That value is used in the next request to return the next set of data. 
        /// When making the request, a Token value of null or empty string means to return the first set of data.
        /// </summary>
        string Token { get; set; }
    }
}
