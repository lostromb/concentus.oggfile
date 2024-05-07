using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concentus.Oggfile
{
    internal static class BinaryHelpers
    {
        internal static void Int16ToByteArrayLittleEndian(short val, byte[] target, int targetOffset)
        {
            UInt16ToByteArrayLittleEndian((ushort)val, target, targetOffset);
        }

        internal static void UInt16ToByteArrayLittleEndian(ushort val, byte[] target, int targetOffset)
        {
            target[targetOffset + 1] = (byte)(val >> 8 & 0xFF);
            target[targetOffset + 0] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int16ToByteSpanLittleEndian(short val, ref Span<byte> target)
        {
            UInt16ToByteArraySpanEndian((ushort)val, ref target);
        }

        internal static void UInt16ToByteArraySpanEndian(ushort val, ref Span<byte> target)
        {
            target[1] = (byte)(val >> 8 & 0xFF);
            target[0] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int16ToByteArrayBigEndian(short val, byte[] target, int targetOffset)
        {
            UInt16ToByteArrayBigEndian((ushort)val, target, targetOffset);
        }

        internal static void UInt16ToByteArrayBigEndian(ushort val, byte[] target, int targetOffset)
        {
            target[targetOffset + 0] = (byte)(val >> 8 & 0xFF);
            target[targetOffset + 1] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int16ToByteSpanBigEndian(short val, ref Span<byte> target)
        {
            UInt16ToByteSpanBigEndian((ushort)val, ref target);
        }

        internal static void UInt16ToByteSpanBigEndian(ushort val, ref Span<byte> target)
        {
            target[0] = (byte)(val >> 8 & 0xFF);
            target[1] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int32ToByteArrayLittleEndian(int val, byte[] target, int targetOffset)
        {
            UInt32ToByteArrayLittleEndian((uint)val, target, targetOffset);
        }

        internal static void UInt32ToByteArrayLittleEndian(uint val, byte[] target, int targetOffset)
        {
            target[targetOffset + 3] = (byte)(val >> 24 & 0xFF);
            target[targetOffset + 2] = (byte)(val >> 16 & 0xFF);
            target[targetOffset + 1] = (byte)(val >> 8 & 0xFF);
            target[targetOffset + 0] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int32ToByteSpanLittleEndian(int val, ref Span<byte> target)
        {
            UInt32ToByteSpanLittleEndian((uint)val, ref target);
        }

        internal static void UInt32ToByteSpanLittleEndian(uint val, ref Span<byte> target)
        {
            target[3] = (byte)(val >> 24 & 0xFF);
            target[2] = (byte)(val >> 16 & 0xFF);
            target[1] = (byte)(val >> 8 & 0xFF);
            target[0] = (byte)(val >> 0 & 0xFF);
        }

        internal static void UInt24ToByteArrayBigEndian(uint val, byte[] target, int targetOffset)
        {
            target[targetOffset + 0] = (byte)(val >> 16 & 0xFF);
            target[targetOffset + 1] = (byte)(val >> 8 & 0xFF);
            target[targetOffset + 2] = (byte)(val >> 0 & 0xFF);
        }

        internal static void UInt24ToByteSpanBigEndian(uint val, ref Span<byte> target)
        {
            target[0] = (byte)(val >> 16 & 0xFF);
            target[1] = (byte)(val >> 8 & 0xFF);
            target[2] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int32ToByteArrayBigEndian(int val, byte[] target, int targetOffset)
        {
            UInt32ToByteArrayBigEndian((uint)val, target, targetOffset);
        }

        internal static void UInt32ToByteArrayBigEndian(uint val, byte[] target, int targetOffset)
        {
            target[targetOffset + 0] = (byte)(val >> 24 & 0xFF);
            target[targetOffset + 1] = (byte)(val >> 16 & 0xFF);
            target[targetOffset + 2] = (byte)(val >> 8 & 0xFF);
            target[targetOffset + 3] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int32ToByteSpanBigEndian(int val, ref Span<byte> target)
        {
            UInt32ToByteSpanBigEndian((uint)val, ref target);
        }

        internal static void UInt32ToByteSpanBigEndian(uint val, ref Span<byte> target)
        {
            target[0] = (byte)(val >> 24 & 0xFF);
            target[1] = (byte)(val >> 16 & 0xFF);
            target[2] = (byte)(val >> 8 & 0xFF);
            target[3] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int64ToByteArrayLittleEndian(long val, byte[] target, int targetOffset)
        {
            UInt64ToByteArrayLittleEndian((ulong)val, target, targetOffset);
        }

        internal static void UInt64ToByteArrayLittleEndian(ulong val, byte[] target, int targetOffset)
        {
            target[targetOffset + 7] = (byte)(val >> 56 & 0xFF);
            target[targetOffset + 6] = (byte)(val >> 48 & 0xFF);
            target[targetOffset + 5] = (byte)(val >> 40 & 0xFF);
            target[targetOffset + 4] = (byte)(val >> 32 & 0xFF);
            target[targetOffset + 3] = (byte)(val >> 24 & 0xFF);
            target[targetOffset + 2] = (byte)(val >> 16 & 0xFF);
            target[targetOffset + 1] = (byte)(val >> 8 & 0xFF);
            target[targetOffset + 0] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int64ToByteSpanLittleEndian(long val, ref Span<byte> target)
        {
            UInt64ToByteSpanLittleEndian((ulong)val, ref target);
        }

        internal static void UInt64ToByteSpanLittleEndian(ulong val, ref Span<byte> target)
        {
            target[7] = (byte)(val >> 56 & 0xFF);
            target[6] = (byte)(val >> 48 & 0xFF);
            target[5] = (byte)(val >> 40 & 0xFF);
            target[4] = (byte)(val >> 32 & 0xFF);
            target[3] = (byte)(val >> 24 & 0xFF);
            target[2] = (byte)(val >> 16 & 0xFF);
            target[1] = (byte)(val >> 8 & 0xFF);
            target[0] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int64ToByteArrayBigEndian(long val, byte[] target, int targetOffset)
        {
            UInt64ToByteArrayBigEndian((ulong)val, target, targetOffset);
        }

        internal static void UInt64ToByteArrayBigEndian(ulong val, byte[] target, int targetOffset)
        {
            target[targetOffset + 0] = (byte)(val >> 56 & 0xFF);
            target[targetOffset + 1] = (byte)(val >> 48 & 0xFF);
            target[targetOffset + 2] = (byte)(val >> 40 & 0xFF);
            target[targetOffset + 3] = (byte)(val >> 32 & 0xFF);
            target[targetOffset + 4] = (byte)(val >> 24 & 0xFF);
            target[targetOffset + 5] = (byte)(val >> 16 & 0xFF);
            target[targetOffset + 6] = (byte)(val >> 8 & 0xFF);
            target[targetOffset + 7] = (byte)(val >> 0 & 0xFF);
        }

        internal static void Int64ToByteSpanBigEndian(long val, ref Span<byte> target)
        {
            UInt64ToByteSpanBigEndian((ulong)val, ref target);
        }

        internal static void UInt64ToByteSpanBigEndian(ulong val, ref Span<byte> target)
        {
            target[0] = (byte)(val >> 56 & 0xFF);
            target[1] = (byte)(val >> 48 & 0xFF);
            target[2] = (byte)(val >> 40 & 0xFF);
            target[3] = (byte)(val >> 32 & 0xFF);
            target[4] = (byte)(val >> 24 & 0xFF);
            target[5] = (byte)(val >> 16 & 0xFF);
            target[6] = (byte)(val >> 8 & 0xFF);
            target[7] = (byte)(val >> 0 & 0xFF);
        }

        internal static short ByteArrayToInt16LittleEndian(byte[] source, int offset)
        {
            short returnVal = 0;
            returnVal |= (short)(source[offset + 1] << 8);
            returnVal |= (short)(source[offset + 0] << 0);
            return returnVal;
        }

        internal static short ByteSpanToInt16LittleEndian(ref Span<byte> source)
        {
            short returnVal = 0;
            returnVal |= (short)(source[1] << 8);
            returnVal |= (short)(source[0] << 0);
            return returnVal;
        }

        internal static short ByteSpanToInt16LittleEndian(ref ReadOnlySpan<byte> source)
        {
            short returnVal = 0;
            returnVal |= (short)(source[1] << 8);
            returnVal |= (short)(source[0] << 0);
            return returnVal;
        }

        internal static ushort ByteArrayToUInt16LittleEndian(byte[] source, int offset)
        {
            ushort returnVal = 0;
            returnVal |= (ushort)(source[offset + 1] << 8);
            returnVal |= (ushort)(source[offset + 0] << 0);
            return returnVal;
        }

        internal static ushort ByteSpanToUInt16LittleEndian(ref Span<byte> source)
        {
            ushort returnVal = 0;
            returnVal |= (ushort)(source[1] << 8);
            returnVal |= (ushort)(source[0] << 0);
            return returnVal;
        }

        internal static ushort ByteSpanToUInt16LittleEndian(ref ReadOnlySpan<byte> source)
        {
            ushort returnVal = 0;
            returnVal |= (ushort)(source[1] << 8);
            returnVal |= (ushort)(source[0] << 0);
            return returnVal;
        }

        internal static short ByteArrayToInt16BigEndian(byte[] source, int offset)
        {
            short returnVal = 0;
            returnVal |= (short)(source[offset + 0] << 8);
            returnVal |= (short)(source[offset + 1] << 0);
            return returnVal;
        }

        internal static short ByteSpanToInt16BigEndian(ref Span<byte> source)
        {
            short returnVal = 0;
            returnVal |= (short)(source[0] << 8);
            returnVal |= (short)(source[1] << 0);
            return returnVal;
        }

        internal static short ByteSpanToInt16BigEndian(ref ReadOnlySpan<byte> source)
        {
            short returnVal = 0;
            returnVal |= (short)(source[0] << 8);
            returnVal |= (short)(source[1] << 0);
            return returnVal;
        }

        internal static ushort ByteArrayToUInt16BigEndian(byte[] source, int offset)
        {
            ushort returnVal = 0;
            returnVal |= (ushort)(source[offset + 0] << 8);
            returnVal |= (ushort)(source[offset + 1] << 0);
            return returnVal;
        }

        internal static ushort ByteSpanToUInt16BigEndian(ref Span<byte> source)
        {
            ushort returnVal = 0;
            returnVal |= (ushort)(source[0] << 8);
            returnVal |= (ushort)(source[1] << 0);
            return returnVal;
        }

        internal static ushort ByteSpanToUInt16BigEndian(ref ReadOnlySpan<byte> source)
        {
            ushort returnVal = 0;
            returnVal |= (ushort)(source[0] << 8);
            returnVal |= (ushort)(source[1] << 0);
            return returnVal;
        }

        internal static int ByteArrayToInt32LittleEndian(byte[] source, int offset)
        {
            int returnVal = 0;
            returnVal |= (int)source[offset + 3] << 24;
            returnVal |= (int)source[offset + 2] << 16;
            returnVal |= (int)source[offset + 1] << 8;
            returnVal |= (int)source[offset + 0] << 0;
            return returnVal;
        }

        internal static int ByteSpanToInt32LittleEndian(ref Span<byte> source)
        {
            int returnVal = 0;
            returnVal |= (int)source[3] << 24;
            returnVal |= (int)source[2] << 16;
            returnVal |= (int)source[1] << 8;
            returnVal |= (int)source[0] << 0;
            return returnVal;
        }

        internal static int ByteSpanToInt32LittleEndian(ref ReadOnlySpan<byte> source)
        {
            int returnVal = 0;
            returnVal |= (int)source[3] << 24;
            returnVal |= (int)source[2] << 16;
            returnVal |= (int)source[1] << 8;
            returnVal |= (int)source[0] << 0;
            return returnVal;
        }

        internal static uint ByteArrayToUInt32LittleEndian(byte[] source, int offset)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[offset + 3] << 24;
            returnVal |= (uint)source[offset + 2] << 16;
            returnVal |= (uint)source[offset + 1] << 8;
            returnVal |= (uint)source[offset + 0] << 0;
            return returnVal;
        }

        internal static uint ByteSpanToUInt32LittleEndian(ref Span<byte> source)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[3] << 24;
            returnVal |= (uint)source[2] << 16;
            returnVal |= (uint)source[1] << 8;
            returnVal |= (uint)source[0] << 0;
            return returnVal;
        }

        internal static uint ByteSpanToUInt32LittleEndian(ref ReadOnlySpan<byte> source)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[3] << 24;
            returnVal |= (uint)source[2] << 16;
            returnVal |= (uint)source[1] << 8;
            returnVal |= (uint)source[0] << 0;
            return returnVal;
        }

        internal static uint ByteArrayToUInt24BigEndian(byte[] source, int offset)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[offset + 0] << 16;
            returnVal |= (uint)source[offset + 1] << 8;
            returnVal |= (uint)source[offset + 2] << 0;
            return returnVal;
        }

        internal static uint ByteSpanToUInt24BigEndian(ref Span<byte> source)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[0] << 16;
            returnVal |= (uint)source[1] << 8;
            returnVal |= (uint)source[2] << 0;
            return returnVal;
        }

        internal static uint ByteSpanToUInt24BigEndian(ref ReadOnlySpan<byte> source)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[0] << 16;
            returnVal |= (uint)source[1] << 8;
            returnVal |= (uint)source[2] << 0;
            return returnVal;
        }

        internal static int ByteArrayToInt32BigEndian(byte[] source, int offset)
        {
            int returnVal = 0;
            returnVal |= (int)source[offset + 0] << 24;
            returnVal |= (int)source[offset + 1] << 16;
            returnVal |= (int)source[offset + 2] << 8;
            returnVal |= (int)source[offset + 3] << 0;
            return returnVal;
        }

        internal static int ByteSpanToInt32BigEndian(ref Span<byte> source)
        {
            int returnVal = 0;
            returnVal |= (int)source[0] << 24;
            returnVal |= (int)source[1] << 16;
            returnVal |= (int)source[2] << 8;
            returnVal |= (int)source[3] << 0;
            return returnVal;
        }

        internal static int ByteSpanToInt32BigEndian(ref ReadOnlySpan<byte> source)
        {
            int returnVal = 0;
            returnVal |= (int)source[0] << 24;
            returnVal |= (int)source[1] << 16;
            returnVal |= (int)source[2] << 8;
            returnVal |= (int)source[3] << 0;
            return returnVal;
        }

        internal static uint ByteArrayToUInt32BigEndian(byte[] source, int offset)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[offset + 0] << 24;
            returnVal |= (uint)source[offset + 1] << 16;
            returnVal |= (uint)source[offset + 2] << 8;
            returnVal |= (uint)source[offset + 3] << 0;
            return returnVal;
        }

        internal static uint ByteSpanToUInt32BigEndian(ref Span<byte> source)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[0] << 24;
            returnVal |= (uint)source[1] << 16;
            returnVal |= (uint)source[2] << 8;
            returnVal |= (uint)source[3] << 0;
            return returnVal;
        }

        internal static uint ByteSpanToUInt32BigEndian(ref ReadOnlySpan<byte> source)
        {
            uint returnVal = 0;
            returnVal |= (uint)source[0] << 24;
            returnVal |= (uint)source[1] << 16;
            returnVal |= (uint)source[2] << 8;
            returnVal |= (uint)source[3] << 0;
            return returnVal;
        }

        internal static long ByteArrayToInt64LittleEndian(byte[] source, int offset)
        {
            long returnVal = 0;
            returnVal |= (long)source[offset + 7] << 56;
            returnVal |= (long)source[offset + 6] << 48;
            returnVal |= (long)source[offset + 5] << 40;
            returnVal |= (long)source[offset + 4] << 32;
            returnVal |= (long)source[offset + 3] << 24;
            returnVal |= (long)source[offset + 2] << 16;
            returnVal |= (long)source[offset + 1] << 8;
            returnVal |= (long)source[offset + 0] << 0;
            return returnVal;
        }

        internal static long ByteSpanToInt64LittleEndian(ref Span<byte> source)
        {
            long returnVal = 0;
            returnVal |= (long)source[7] << 56;
            returnVal |= (long)source[6] << 48;
            returnVal |= (long)source[5] << 40;
            returnVal |= (long)source[4] << 32;
            returnVal |= (long)source[3] << 24;
            returnVal |= (long)source[2] << 16;
            returnVal |= (long)source[1] << 8;
            returnVal |= (long)source[0] << 0;
            return returnVal;
        }

        internal static long ByteSpanToInt64LittleEndian(ref ReadOnlySpan<byte> source)
        {
            long returnVal = 0;
            returnVal |= (long)source[7] << 56;
            returnVal |= (long)source[6] << 48;
            returnVal |= (long)source[5] << 40;
            returnVal |= (long)source[4] << 32;
            returnVal |= (long)source[3] << 24;
            returnVal |= (long)source[2] << 16;
            returnVal |= (long)source[1] << 8;
            returnVal |= (long)source[0] << 0;
            return returnVal;
        }

        internal static ulong ByteArrayToUInt64LittleEndian(byte[] source, int offset)
        {
            ulong returnVal = 0;
            returnVal |= (ulong)source[offset + 7] << 56;
            returnVal |= (ulong)source[offset + 6] << 48;
            returnVal |= (ulong)source[offset + 5] << 40;
            returnVal |= (ulong)source[offset + 4] << 32;
            returnVal |= (ulong)source[offset + 3] << 24;
            returnVal |= (ulong)source[offset + 2] << 16;
            returnVal |= (ulong)source[offset + 1] << 8;
            returnVal |= (ulong)source[offset + 0] << 0;
            return returnVal;
        }

        internal static ulong ByteSpanToUInt64LittleEndian(ref Span<byte> source)
        {
            ulong returnVal = 0;
            returnVal |= (ulong)source[7] << 56;
            returnVal |= (ulong)source[6] << 48;
            returnVal |= (ulong)source[5] << 40;
            returnVal |= (ulong)source[4] << 32;
            returnVal |= (ulong)source[3] << 24;
            returnVal |= (ulong)source[2] << 16;
            returnVal |= (ulong)source[1] << 8;
            returnVal |= (ulong)source[0] << 0;
            return returnVal;
        }

        internal static ulong ByteSpanToUInt64LittleEndian(ref ReadOnlySpan<byte> source)
        {
            ulong returnVal = 0;
            returnVal |= (ulong)source[7] << 56;
            returnVal |= (ulong)source[6] << 48;
            returnVal |= (ulong)source[5] << 40;
            returnVal |= (ulong)source[4] << 32;
            returnVal |= (ulong)source[3] << 24;
            returnVal |= (ulong)source[2] << 16;
            returnVal |= (ulong)source[1] << 8;
            returnVal |= (ulong)source[0] << 0;
            return returnVal;
        }

        internal static long ByteArrayToInt64BigEndian(byte[] source, int offset)
        {
            long returnVal = 0;
            returnVal |= (long)source[offset + 0] << 56;
            returnVal |= (long)source[offset + 1] << 48;
            returnVal |= (long)source[offset + 2] << 40;
            returnVal |= (long)source[offset + 3] << 32;
            returnVal |= (long)source[offset + 4] << 24;
            returnVal |= (long)source[offset + 5] << 16;
            returnVal |= (long)source[offset + 6] << 8;
            returnVal |= (long)source[offset + 7] << 0;
            return returnVal;
        }

        internal static long ByteSpanToInt64BigEndian(ref Span<byte> source)
        {
            long returnVal = 0;
            returnVal |= (long)source[0] << 56;
            returnVal |= (long)source[1] << 48;
            returnVal |= (long)source[2] << 40;
            returnVal |= (long)source[3] << 32;
            returnVal |= (long)source[4] << 24;
            returnVal |= (long)source[5] << 16;
            returnVal |= (long)source[6] << 8;
            returnVal |= (long)source[7] << 0;
            return returnVal;
        }

        internal static long ByteSpanToInt64BigEndian(ref ReadOnlySpan<byte> source)
        {
            long returnVal = 0;
            returnVal |= (long)source[0] << 56;
            returnVal |= (long)source[1] << 48;
            returnVal |= (long)source[2] << 40;
            returnVal |= (long)source[3] << 32;
            returnVal |= (long)source[4] << 24;
            returnVal |= (long)source[5] << 16;
            returnVal |= (long)source[6] << 8;
            returnVal |= (long)source[7] << 0;
            return returnVal;
        }

        internal static ulong ByteArrayToUInt64BigEndian(byte[] source, int offset)
        {
            ulong returnVal = 0;
            returnVal |= (ulong)source[offset + 0] << 56;
            returnVal |= (ulong)source[offset + 1] << 48;
            returnVal |= (ulong)source[offset + 2] << 40;
            returnVal |= (ulong)source[offset + 3] << 32;
            returnVal |= (ulong)source[offset + 4] << 24;
            returnVal |= (ulong)source[offset + 5] << 16;
            returnVal |= (ulong)source[offset + 6] << 8;
            returnVal |= (ulong)source[offset + 7] << 0;
            return returnVal;
        }

        internal static ulong ByteSpanToUInt64BigEndian(ref Span<byte> source)
        {
            ulong returnVal = 0;
            returnVal |= (ulong)source[0] << 56;
            returnVal |= (ulong)source[1] << 48;
            returnVal |= (ulong)source[2] << 40;
            returnVal |= (ulong)source[3] << 32;
            returnVal |= (ulong)source[4] << 24;
            returnVal |= (ulong)source[5] << 16;
            returnVal |= (ulong)source[6] << 8;
            returnVal |= (ulong)source[7] << 0;
            return returnVal;
        }

        internal static ulong ByteSpanToUInt64BigEndian(ref ReadOnlySpan<byte> source)
        {
            ulong returnVal = 0;
            returnVal |= (ulong)source[0] << 56;
            returnVal |= (ulong)source[1] << 48;
            returnVal |= (ulong)source[2] << 40;
            returnVal |= (ulong)source[3] << 32;
            returnVal |= (ulong)source[4] << 24;
            returnVal |= (ulong)source[5] << 16;
            returnVal |= (ulong)source[6] << 8;
            returnVal |= (ulong)source[7] << 0;
            return returnVal;
        }
    }
}
