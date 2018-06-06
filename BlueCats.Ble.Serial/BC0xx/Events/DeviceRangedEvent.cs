using System;
using System.Linq;
using BlueCats.Ble.Serial.BC0xx.Events.Base;

namespace BlueCats.Ble.Serial.BC0xx.Events {

    public class DeviceRangedEvent : EventPdu {

        public byte[] BluetoothAddress { get; set; }
        public sbyte RSSI { get; set; }
        public float RSSIVariance { get; set; }
        public byte AdCount { get; set; }

        public override void ParsePayload() {
            var payloadPos = 0;
            if ( PayloadData?.Length >= SerialProtocol.BLUETOOTH_ADDRESS_LEN ) {
                BluetoothAddress = new byte[ SerialProtocol.BLUETOOTH_ADDRESS_LEN ];
                Buffer.BlockCopy( PayloadData, payloadPos, BluetoothAddress, 0, SerialProtocol.BLUETOOTH_ADDRESS_LEN );
                payloadPos += SerialProtocol.BLUETOOTH_ADDRESS_LEN;
            }

            if ( PayloadData?.Length >= ( payloadPos + 1 ) ) {
                RSSI = (sbyte) PayloadData[ payloadPos ];
                payloadPos += 1;
            }

            if ( PayloadData?.Length > ( payloadPos + 4 ) ) {
                RSSIVariance = (float) Convert.ToDouble( PayloadData.Skip( payloadPos ).Take( 4 ).ToArray() );
                payloadPos += 4;
            }

            if ( PayloadData?.Length >= ( payloadPos + 1 ) ) {
                AdCount = (byte) PayloadData[ payloadPos ];
                payloadPos += 1;
            }
        }

    }

}