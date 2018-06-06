using System;
using BlueCats.Ble.Serial.BC0xx.Events.Base;

namespace BlueCats.Ble.Serial.BC0xx.Events {

    public class DeviceDiscoveredEvent : EventPdu {

        public byte[] BluetoothAddress { get; set; }
        public sbyte RSSI { get; set; }
        public byte[] AdData { get; set; }

        public override void ParsePayload() {
            var payloadPos = 0;
            if ( PayloadData?.Length >= SerialProtocol.BLUETOOTH_ADDRESS_LEN ) {
                BluetoothAddress = new byte[ SerialProtocol.BLUETOOTH_ADDRESS_LEN ];
                // Bluetooth address comes over in Little-endian
                Buffer.BlockCopy( PayloadData, payloadPos, BluetoothAddress, 0, SerialProtocol.BLUETOOTH_ADDRESS_LEN );
                payloadPos += SerialProtocol.BLUETOOTH_ADDRESS_LEN;
            }

            if ( PayloadData?.Length >= ( payloadPos + 1 ) ) {
                RSSI = (sbyte) PayloadData[ payloadPos ];
                payloadPos += 1;
            }

            if ( PayloadData?.Length > ( payloadPos + 1 ) ) {
                var adDataLen = PayloadData.Length - ( payloadPos + 1 );
                AdData = new byte[ adDataLen ];
                Buffer.BlockCopy( PayloadData, ( SerialProtocol.BLUETOOTH_ADDRESS_LEN + 1 ), AdData, 0, adDataLen );
                payloadPos += adDataLen;
            }
        }

    }

}