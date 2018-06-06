using System;
using BlueCats.Ble.Serial.BC0xx.Events.Base;

namespace BlueCats.Ble.Serial.BC0xx.Events {

    public class DeviceExitedEvent : EventPdu {

        public byte[] BluetoothAddress { get; set; }

        public override void ParsePayload() {
            var payloadPos = 0;
            if ( PayloadData?.Length >= SerialProtocol.BLUETOOTH_ADDRESS_LEN ) {
                BluetoothAddress = new byte[ SerialProtocol.BLUETOOTH_ADDRESS_LEN ];
                Buffer.BlockCopy( PayloadData, payloadPos, BluetoothAddress, 0, SerialProtocol.BLUETOOTH_ADDRESS_LEN );
                payloadPos += SerialProtocol.BLUETOOTH_ADDRESS_LEN;
            }
        }

    }

}