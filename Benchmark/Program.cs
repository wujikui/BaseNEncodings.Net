using System;
using System.Diagnostics;

namespace WallF.BaseNEncodings.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[][] source = { // 1M bytes
                       new byte[0x10]
                     , new byte[0x41]
                     , new byte[0x100]
                     , new byte[0x430]
                     , new byte[0x1050]
                     , new byte[0x4107]
                     , new byte[0x17090]
                     , new byte[0x40A0A]
                     , new byte[0xA2E8E]};
            FlushBytes(source);
            BaseEncoding[] encodings = {
                       BaseEncoding.Base16
                     , BaseEncoding.Base64
                     , BaseEncoding.Base64Safe
                     , BaseEncoding.Base32
                     , BaseEncoding.Base32Hex
                     , BaseEncoding.Base16 };
            Stopwatch watch = new Stopwatch();
            long times = 0, bytes = 0;
            byte[] rb = new byte[0xFFFFFC];
            char[] rc = new char[0xCFFFFF];
            Console.WriteLine("BENCHMARK BEGIN");
            watch.Start();
            for (int i = 0; i < 8; i++)
            {
                // Running 19*9*6 times ~= 1K
                for (int j = 0; j < 19; j++)
                {
                    foreach (BaseEncoding encoding in encodings)
                    {
                        foreach (byte[] bs in source)
                        {
                            times++;
                            bytes += bs.Length;
                            int offset = (int)(((i + j) % 2 == 0 ? 0xCC55 : 0xAA33) - bytes & 0xFFF);
                            int num = encoding.Encode(bs, 0, bs.Length, rc, offset);
                            encoding.Decode(rc, offset, num, rb, offset * 2 + 1);
                            // encoding.Decode(encoding.Encode(bs));
                        }
                    }
                }
                Console.WriteLine("\tLOOP " + (i + 1) + "/" + 8);
            }
            watch.Stop();
            Console.WriteLine("Total:  " + watch.ElapsedMilliseconds + "ms (" + LongToHex(times) + " TIMES, " + LongToHex(bytes) + " BYTES)");
            Console.WriteLine("BENCHMARK END");
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        static void FlushBytes(byte[][] bs)
        {
            Random r = new Random();
            foreach (byte[] t in bs)
                r.NextBytes(t);
        }

        static string LongToHex(long l)
        {
            double d = (double)l;
            int i = 0;
            do
            {
                i++;
                d /= 1024;
            }
            while (d / 1024 > 1);
            return string.Format("{0:N1}", d) + " KMG"[i];
        }
    }
}