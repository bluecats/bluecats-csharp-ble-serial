using System;
using BlueCats.Ble.Serial.BC0xx.Commands.Base;

namespace BlueCats.Ble.Serial.BC0xx.Commands {

    public class ReadModelNumberCommandResponse : CommandResponsePdu {

        // Consts
        private const int MODEL_NUM_LEN = 2;

        public byte[] ModelNumber { get; set; }

        public override void ParsePayload() {
            if ( PayloadData.Length >= ( CMD_RSP_CODE_LEN + MODEL_NUM_LEN ) ) {
                ModelNumber = new byte[ MODEL_NUM_LEN ];
                Buffer.BlockCopy( PayloadData, CMD_RSP_CODE_LEN, ModelNumber, 0, MODEL_NUM_LEN );
            }
        }

    }

}