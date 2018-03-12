using System;
using BlueCats.Ble.Serial.BC0xx.Commands.Base;

namespace BlueCats.Ble.Serial.BC0xx.Commands {

    public class ReadFirmwareVersionCommandResponse : CommandResponsePdu {

        // Consts
        private const int FW_VER_LEN = 2;

        public byte[] FirmwareVersion { get; set; }

        public override void ParsePayload() {
            if ( PayloadData.Length >= ( CMD_RSP_CODE_LEN + FW_VER_LEN ) ) {
                FirmwareVersion = new byte[ FW_VER_LEN ];
                Buffer.BlockCopy( PayloadData, CMD_RSP_CODE_LEN, FirmwareVersion, 0, FW_VER_LEN );
            }
        }

    }

}