using System;
using System.Text;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace App1
{
    class Program
    {
        static void Main(string[] args)
        {
            BluetoothLEAdvertisementPublisher publisher = new BluetoothLEAdvertisementPublisher();

            var manufacturerData = new BluetoothLEManufacturerData();
            manufacturerData.CompanyId = 0xFFFE;

            var writer = new DataWriter();
            writer.WriteString("fuck you blair");

            manufacturerData.Data = writer.DetachBuffer();

            publisher.Advertisement.ManufacturerData.Add(manufacturerData);

            publisher.Start();

            BluetoothLEAdvertisementWatcher xwatcher = new BluetoothLEAdvertisementWatcher();
            var filter = new BluetoothLEManufacturerData();
            filter.CompanyId = 0xFFFE;
            xwatcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(filter);
            xwatcher.Received += OnAdvertisementReceived;
            xwatcher.Start();

            async void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
            {
                foreach(var arg in eventArgs.Advertisement.ManufacturerData)
                {
                    var data = new byte[arg.Data.Length];
                    using( var reader = DataReader.FromBuffer(arg.Data))
                    {
                        reader.ReadBytes(data);
                    }
                    var str = Encoding.ASCII.GetString(data);
                    Console.WriteLine(str);
                }
            }
            Console.ReadLine();
        }

    }
}
