﻿
using System;

namespace Couchbase.Core.Utils
{
    public static class Leb128
    {
        public static byte[] Write(uint value, int size)
        {
            var remaining = value >> 7;
            var bytes = new byte[size];
            var count = 0;

            while (remaining != 0)
            {
                bytes[count++] = (byte) ((value & 0x7f) | 0x80);
                value = remaining;
                remaining = remaining >> 7;
            }
            bytes[count] = (byte) (value & 0x7f);
            return bytes;
        }

        public static uint Read(byte[] bytes)
        {
            var result = 0u;
            uint current;
            var count = 0;

            do
            {
                current = (uint) bytes[count] & 0xff;
                result |= (current & 0x7f) << (count * 7);
                count++;
            } while ((current & 0x80) == 0x80 && count < 5);

            if ((current & 0x80) == 0x80)
            {
                throw new Exception("Invalid LEB128 sequence.");
            }
            return result;
        }

        public static int WrittenSize(uint value)
        {
            var remaining = value >> 7;
            var count = 0;

            while (remaining != 0)
            {
                remaining >>= 7;
                count++;
            }
            return count + 1;
        }
    }
}
