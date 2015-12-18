// Copyright (c) 2015 Illyriad Games Ltd. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.md in the project root for license information.

using System;
using System.Numerics;
using BenchmarkDotNet;
using BenchmarkDotNet.Tasks;
using IllyriadGames.ByteArrayExtensions;

namespace IllyriadGames.ByteArrayExtensionsBenchmarks
{
    [BenchmarkTask(platform: BenchmarkPlatform.X64, jitVersion: BenchmarkJitVersion.RyuJit)]
    public class VectorizedCopyBenchmark
    {
        [Params(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 
            33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64,
            65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96,
            508, 509, 510, 511, 512, 513, 512 + 31, 512 + 32, 512 + 35, 512 + 64,
            1024, 
            4096, 
            8512)]
        public int MaxCounter = 0;

        private readonly static byte[] bufferFrom = new byte[8512];
        private readonly static byte[] bufferTo = new byte[8512];

        public VectorizedCopyBenchmark()
        {

        }

        [Setup]
        public void SetupData()
        {
            if (!Vector.IsHardwareAccelerated)
            {
                Console.WriteLine("Warning Not Vector.IsHardwareAccelerated");
            }
        }

        [Benchmark]
        [OperationsPerInvoke(2)]
        public void ArrayCopy()
        {
            Array.Copy(bufferFrom, 0, bufferTo, 0, MaxCounter);
        }

        [Benchmark]
        [OperationsPerInvoke(2)]
        public void BlockCopy()
        {
            Buffer.BlockCopy(bufferFrom, 0, bufferTo, 0, MaxCounter);
        }

        //[Benchmark]
        //[OperationsPerInvoke(2)]
        //public void VectorCopy()
        //{
        //    VectorCopy(bufferFrom, 0, bufferTo, 0, MaxCounter);
        //}

        //[Benchmark]
        //[OperationsPerInvoke(2)]
        //public void VectorCopyUnrolled2()
        //{
        //    VectorCopyUnrolled2(bufferFrom, 0, bufferTo, 0, MaxCounter);
        //}

        //[Benchmark]
        //[OperationsPerInvoke(2)]
        //public void VectorCopyUnrolled4()
        //{
        //    VectorCopyUnrolled4(bufferFrom, 0, bufferTo, 0, MaxCounter);
        //}

        [Benchmark]
        [OperationsPerInvoke(2)]
        public void VectorizedCopyExtension()
        {
            bufferFrom.VectorizedCopy(0, bufferTo, 0, MaxCounter);
        }

        //private static readonly int _vectorSpan = Vector<byte>.Count;
        //private static readonly int _vectorSpan2 = Vector<byte>.Count + Vector<byte>.Count;
        //private static readonly int _vectorSpan3 = Vector<byte>.Count + Vector<byte>.Count + Vector<byte>.Count;
        //private static readonly int _vectorSpan4 = Vector<byte>.Count + Vector<byte>.Count + Vector<byte>.Count + Vector<byte>.Count;

        //public unsafe static void VectorCopy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
        //{
        //    while (count >= _vectorSpan)
        //    {
        //        new Vector<byte>(src, srcOffset).CopyTo(dst, dstOffset);
        //        srcOffset += _vectorSpan;
        //        dstOffset += _vectorSpan;
        //        count -= _vectorSpan;
        //    }

        //    if (count > 0)
        //    {
        //        fixed (byte* srcOrigin = src)
        //        fixed (byte* dstOrigin = dst)
        //        {
        //            var pSrc = srcOrigin + srcOffset;
        //            var dSrc = dstOrigin + dstOffset;
        //            while (count >= 4)
        //            {
        //                *dSrc = *pSrc;
        //                *(dSrc + 1) = *(pSrc + 1);
        //                *(dSrc + 2) = *(pSrc + 2);
        //                *(dSrc + 3) = *(pSrc + 3);
        //                pSrc += 4;
        //                dSrc += 4;
        //                count -= 4;
        //            }
        //            while (count > 0)
        //            {
        //                *(dSrc++) = *(pSrc++);
        //                count--;
        //            }
        //        }
        //    }
        //}

        //public unsafe static void VectorCopyUnrolled2(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
        //{
        //    while (count >= _vectorSpan2)
        //    {
        //        new Vector<byte>(src, srcOffset).CopyTo(dst, dstOffset);
        //        new Vector<byte>(src, srcOffset + _vectorSpan).CopyTo(dst, dstOffset + _vectorSpan);
        //        srcOffset += _vectorSpan2;
        //        dstOffset += _vectorSpan2;
        //        count -= _vectorSpan2;
        //    }

        //    if (count >= _vectorSpan)
        //    {
        //        new Vector<byte>(src, srcOffset).CopyTo(dst, dstOffset);
        //        srcOffset += _vectorSpan;
        //        dstOffset += _vectorSpan;
        //        count -= _vectorSpan;
        //    }

        //    if (count > 0)
        //    {
        //        fixed (byte* srcOrigin = src)
        //        fixed (byte* dstOrigin = dst)
        //        {
        //            var pSrc = srcOrigin + srcOffset;
        //            var dSrc = dstOrigin + dstOffset;
        //            while (count >= 4)
        //            {
        //                *dSrc = *pSrc;
        //                *(dSrc + 1) = *(pSrc + 1);
        //                *(dSrc + 2) = *(pSrc + 2);
        //                *(dSrc + 3) = *(pSrc + 3);
        //                pSrc += 4;
        //                dSrc += 4;
        //                count -= 4;
        //            }
        //            while (count > 0)
        //            {
        //                *(dSrc++) = *(pSrc++);
        //                count--;
        //            }
        //        }
        //    }
        //}

        //public unsafe static void VectorCopyUnrolled4(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
        //{
        //    while (count >= _vectorSpan4)
        //    {
        //        new Vector<byte>(src, srcOffset).CopyTo(dst, dstOffset);
        //        new Vector<byte>(src, srcOffset + _vectorSpan).CopyTo(dst, dstOffset + _vectorSpan);
        //        new Vector<byte>(src, srcOffset + _vectorSpan2).CopyTo(dst, dstOffset + _vectorSpan2);
        //        new Vector<byte>(src, srcOffset + _vectorSpan3).CopyTo(dst, dstOffset + _vectorSpan3);
        //        srcOffset += _vectorSpan4;
        //        dstOffset += _vectorSpan4;
        //        count -= _vectorSpan4;
        //    }

        //    while (count >= _vectorSpan)
        //    {
        //        new Vector<byte>(src, srcOffset).CopyTo(dst, dstOffset);
        //        srcOffset += _vectorSpan;
        //        dstOffset += _vectorSpan;
        //        count -= _vectorSpan;
        //    }

        //    if (count > 0)
        //    {
        //        fixed (byte* srcOrigin = src)
        //        fixed (byte* dstOrigin = dst)
        //        {
        //            var pSrc = srcOrigin + srcOffset;
        //            var dSrc = dstOrigin + dstOffset;
        //            while (count >= 4)
        //            {
        //                *dSrc = *pSrc;
        //                *(dSrc + 1) = *(pSrc + 1);
        //                *(dSrc + 2) = *(pSrc + 2);
        //                *(dSrc + 3) = *(pSrc + 3);
        //                pSrc += 4;
        //                dSrc += 4;
        //                count -= 4;
        //            }
        //            while (count > 0)
        //            {
        //                *(dSrc++) = *(pSrc++);
        //                count--;
        //            }
        //        }
        //    }
        //}
    }
}