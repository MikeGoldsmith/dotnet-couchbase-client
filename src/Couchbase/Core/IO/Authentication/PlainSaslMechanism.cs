﻿using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Transcoders;
using SaslStart = Couchbase.Core.IO.Operations.Legacy.Authentication.SaslStart;
using SequenceGenerator = Couchbase.Core.IO.Operations.Legacy.SequenceGenerator;

namespace Couchbase.Core.IO.Authentication
{
    public class PlainSaslMechanism : ISaslMechanism
    {
        public PlainSaslMechanism(string username, string password)
        {
            Username = username;
            Password = password;
        }   

        public string Username { get; }
        public string Password { get; }
        public string MechanismType => "PLAIN";

        public async Task<bool> AuthenticateAsync(IConnection connection)
        {
            var tcs = new TaskCompletionSource<bool>();
            var op = new SaslStart
            {
                Key = MechanismType,
                Content =  GetAuthData(Username, Password),
                Opaque = SequenceGenerator.GetNext(),
                Converter = new DefaultConverter(),
                Transcoder = new DefaultTranscoder(new DefaultConverter()),
                Completed = s => 
                { 
                    //Status will be AuthenticationError if auth failed otherwise false
                    tcs.SetResult(s.Status == ResponseStatus.Success);
                    return tcs.Task;
                }
            };

            await connection.SendAsync(op.Write(), op.Completed).ConfigureAwait(false);
            return await tcs.Task.ConfigureAwait(false);
        }

        static string GetAuthData(string userName, string passWord)
        {
            const string empty = "\0";
            var sb = new StringBuilder();
            sb.Append(userName);
            sb.Append(empty);
            sb.Append(userName);
            sb.Append(empty);
            sb.Append(passWord);
            return sb.ToString();
        }
    }
}
