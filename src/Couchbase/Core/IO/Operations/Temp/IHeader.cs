﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase.Core.IO.Operations.Temp
{
    interface IHeader
    {
        byte[] Write();

        void Read(byte[] bytes);
    }
}
