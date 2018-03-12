using System;
using BlueCats.Ble.Serial.BC0xx.Events.Base;

namespace BlueCats.Ble.Serial.BC0xx.Events {

    public class DeviceEnteredEvent : EventPdu {

        public byte[] BluetoothAddress { get; set; }
        public sbyte RSSI { get; set; }

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
        }

    }

}