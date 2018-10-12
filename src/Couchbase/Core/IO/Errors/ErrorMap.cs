﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Couchbase.Core.IO.Errors
{
    /// <summary>
    /// A map of errors provided by the server that can be used to lookup messages.
    /// </summary>
    public class ErrorMap
    {
        private const string HexFormat = "X";
        private readonly ILogger _logger;

        /// <summary>
        /// Gets or sets the version of the error map.
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the revision of the error map.
        /// </summary>
        [JsonProperty("revision")]
        public int Revision { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of errors codes.
        /// </summary>
        [JsonProperty("errors")]
        public Dictionary<string, ErrorCode> Errors { get; set; }

        /// <summary>
        /// Tries the get get error code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns>True if the provided error code was in the error code map, otherwise false.</returns>
        public bool TryGetGetErrorCode(short code, out ErrorCode errorCode)
        {
            if (Errors.TryGetValue(code.ToString(HexFormat).ToLower(), out errorCode))
            {
                return true;
            }

            _logger.LogWarning("Unexpected ResponseStatus for KeyValue operation not found in Error Map: 0x{0}",
                code.ToString("X4"));
            return false;
        }
    }
}
