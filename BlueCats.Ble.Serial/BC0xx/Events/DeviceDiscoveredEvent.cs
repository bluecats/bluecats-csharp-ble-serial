using System;
using BlueCats.Ble.Serial.BC0xx.Events.Base;

namespace BlueCats.Ble.Serial.BC0xx.Events {

    public class DeviceDiscoveredEvent : EventPdu {

        public byte[] BluetoothAddress { get; set; }
        public sbyte RSSI { get; set; }
        public byte[] AdData { get; set; }

        public override void ParsePayload() {
            var payloadPos = 0;
            if ( PayloadData?.Length >= Protocol.BLUETOOTH_ADDRESS_LEN ) {
                BluetoothAddress = new byte[ Protocol.BLUETOOTH_ADDRESS_LEN ];
                Buffer.BlockCopy( PayloadData, payloadPos, BluetoothAddress, 0, Protocol.BLUETOOTH_ADDRESS_LEN );
                payloadPos += Protocol.BLUETOOTH_ADDRESS_LEN;
            }

            if ( PayloadData?.Length >= ( payloadPos + 1 ) ) {
                RSSI = (sbyte) PayloadData[ payloadPos ];
                payloadPos += 1;
            }

            if ( PayloadData?.Length > ( payloadPos + 1 ) ) {
                var adDataLen = PayloadData.Length - ( payloadPos + 1 );
                AdData = new byte[ adDataLen ];
                Buffer.BlockCopy( PayloadData, ( Protocol.BLUETOOTH_ADDRESS_LEN + 1 ), AdData, 0, adDataLen );
                payloadPos += adDataLen;
            }
        }

    }

}