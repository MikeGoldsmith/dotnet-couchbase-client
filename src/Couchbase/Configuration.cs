﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Couchbase
{
    public class Configuration : IConfiguration
    {
        private ConcurrentBag<Uri>_servers = new ConcurrentBag<Uri>();
        protected ConcurrentBag<string> _buckets = new ConcurrentBag<string>();

        public IConfiguration WithServers(params string[] ips)
        {
            if (ips == null)
            {
                throw new ArgumentNullException(nameof(ips));
            }

            //for now just copy over - later ensure only new nodes are added
            return new Configuration
            {
                UserName = UserName,
                Password = Password,
                _servers = new ConcurrentBag<Uri>(ips.Select(x=>new Uri(x))),
                _buckets = _buckets
            };
        }

        public IConfiguration WithBucket(params string[] bucketNames)
        {
            if(bucketNames == null)
            {
                throw new ArgumentNullException(nameof(bucketNames ));
            }

            //just the name of the bucket for now - later make and actual config
            return new Configuration
            {
                UserName = UserName,
                Password = Password,
                _buckets = new ConcurrentBag<string>(bucketNames.ToList()),
                _servers = _servers
            };
        }

        public IConfiguration WithCredentials(string username, string password)
        {
            return new Configuration
            {
                UserName = username,
                Password = password,
                _servers = _servers,
                _buckets = _buckets
            };
        }

        public IEnumerable<Uri> Servers => _servers;
        public IEnumerable<string> Buckets => _buckets;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool OrphanedResponseLoggingEnabled { get; set; }
    }
}
