using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using igfxDHLib;
using System.Threading;


namespace IntelPWMCA
{
    class Program
    {
        [STAThread] // Without dh.GetDataFromDriver read wrong data
        static void Main(string[] args)
        {
            uint error = 0;
            DataHandler dh;
            byte[] baseData;
            baseData = new byte[8];
            uint WritePWMFreq = 600;     // Default
            uint ReadPWMFreq = 0;
            uint OldReadPWMFreq = 0xFFFF;
            int WatchDelay = 0;

            if (args.Length >= 1)
            {
                WritePWMFreq = uint.Parse(args[0]);
                if (WritePWMFreq > 20000)
                    WritePWMFreq = 20000;
                else if(WritePWMFreq < 50)
                    WritePWMFreq = 50;
            }
            
            if (args.Length >= 2)
            {
                WatchDelay = int.Parse(args[1])*1000;
            }


            do {
                dh = new igfxDHLib.DataHandler();
                dh.GetDataFromDriver(ESCAPEDATATYPE_ENUM.GET_SET_PWM_FREQUENCY, 4, ref error, ref baseData[0]);
                if (error != 0)
                {
                    WatchDelay = 0;
                    Console.WriteLine(string.Format("Failed to get PWM, Error: {0:X}", error));
                }
                else
                {
                    ReadPWMFreq = BitConverter.ToUInt32(baseData, 4);
                    if (ReadPWMFreq != OldReadPWMFreq)
                    {
                        Console.WriteLine($"Current PWM: {string.Format("{0}", ReadPWMFreq)} Hz");
                        OldReadPWMFreq = ReadPWMFreq;
                    }
                }

                if (ReadPWMFreq != WritePWMFreq)
                {
                    error = 0;
                    byte[] b = BitConverter.GetBytes(WritePWMFreq);
                    Array.Copy(b, 0, baseData, 4, 4);
                    dh.SendDataToDriver(ESCAPEDATATYPE_ENUM.GET_SET_PWM_FREQUENCY, 4, ref error, ref baseData[0]);
                    if (error != 0)
                    {
                        WatchDelay = 0;
                        Console.WriteLine(string.Format("Failed to set PWM, Error: {0:X}", error));
                    }
                    else
                        Console.WriteLine($"Set to: {string.Format("{0}", BitConverter.ToInt32(baseData, 4))} Hz");
                }

                if (WatchDelay!=0)
                    Thread.Sleep(WatchDelay); 
            } while (WatchDelay!=0);
        }
    }
}
