using System;
using BlueCats.Ble.Serial.BC0xx.Commands.Base;

namespace BlueCats.Ble.Serial.BC0xx.Commands {

    public class ReadBluetoothAddressCommandResponse : CommandResponsePdu {

        public byte[] BluetoothAddress { get; set; }

        public override void ParsePayload() {
            if ( PayloadData.Length >= ( CMD_RSP_CODE_LEN + SerialProtocol.BLUETOOTH_ADDRESS_LEN ) ) {
                BluetoothAddress = new byte[ SerialProtocol.BLUETOOTH_ADDRESS_LEN ];
                Buffer.BlockCopy( PayloadData, CMD_RSP_CODE_LEN, BluetoothAddress, 0, SerialProtocol.BLUETOOTH_ADDRESS_LEN );
                // Convert Bluetooth address to Little-endian
                Array.Reverse( BluetoothAddress );
            }
        }

    }

}