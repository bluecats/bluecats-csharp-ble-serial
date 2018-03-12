# BlueCats BLE serial library
`.NET Standard 2.0` class library for communication with BC0xx serial enabled beacons

[Serial API Documentation](https://github.com/bluecats/bluecats-docs/blob/master/BlueCats-BC010-Serial-Public-API-0.1.0.markdown)

[NuGet Package](https://www.nuget.org/packages/BlueCats.Ble.Serial)

This library is transport agnostic, allowing any serial device library to be used for communication with serial enabled beacons. It handles the parsing and encoding of serial packets being sent and recieved over serial. At this time, .NET Standard 2.0 has the highest level of [platform compatibility](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) supporting platforms:
* .NET Core 2.0
* .NET Framework 4.6.1
* Mono 5.4
* Xamarin.iOS 10.14
* Xamarin.Mac 3.8
* Xamarin.Android 8.0
* UWP 10.0.16299
