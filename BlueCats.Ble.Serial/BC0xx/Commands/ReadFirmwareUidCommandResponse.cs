using System;
using BlueCats.Ble.Serial.BC0xx.Commands.Base;

namespace BlueCats.Ble.Serial.BC0xx.Commands {

    public class ReadFirmwareUidCommandResponse : CommandResponsePdu {

        // Consts
        private const int FW_UID_LEN = 4;

        public byte[] FirmwareUID { get; set; }

        public override void ParsePayload() {
            if ( PayloadData.Length >= ( CMD_RSP_CODE_LEN + FW_UID_LEN ) ) {
                FirmwareUID = new byte[ FW_UID_LEN ];
                Buffer.BlockCopy( PayloadData, CMD_RSP_CODE_LEN, FirmwareUID, 0, FW_UID_LEN );
            }
        }

    }

}