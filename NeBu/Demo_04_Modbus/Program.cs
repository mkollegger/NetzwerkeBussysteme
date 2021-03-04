using System;
using System.IO.Ports;
using System.Threading;
using NModbus;
using NModbus.Serial;

namespace Demo_04_Modbus
{
    class Program
    {
        static void Main(string[] args)
        {

            using (SerialPort port = new SerialPort("COM4"))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateRtuMaster(port);

                byte slaveId = 4;
                ushort startAddress = 48;
                bool[] registers = new bool[] { true, true, false,true };

                var state =  master.ReadHoldingRegisters(slaveId, 7996, 1);
                var commando = master.ReadHoldingRegisters(slaveId, 2006, 1);
                master.WriteSingleRegister(slaveId, 2006, 0);


                var data = master.ReadHoldingRegisters(slaveId, 8000, 1);
               
                do
                {
                    data = master.ReadHoldingRegisters(slaveId, 8000, 1);
                    Console.WriteLine(data[0].ToString("X"));
                    Thread.Sleep(1000);
                    if (Console.KeyAvailable)
                        break;
                    if (data[0] > 5)
                        master.WriteSingleCoil(slaveId, 48, false);
                    else
                        master.WriteSingleCoil(slaveId, 48, true);
                } while (true);

            }

            Console.WriteLine("Any key to exit!");
            Console.ReadKey();
        }

    }
}
