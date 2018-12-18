﻿namespace Couchbase
{
    public interface IScope
    {
        string Id { get; }

        string Name { get; }

        ICollection this[string name] { get; }

        ICollection GetCollection(string name);
    }
}
