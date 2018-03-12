using System;

namespace BlueCats.Ble.Serial.BC0xx.Commands.Base {

    public class CommandResponsePdu : Pdu< CommandHeader > {

        // Consts
        protected const int CMD_RSP_CODE_LEN = 1;

        public CommandResponseCode ResponseCode { get; set; }

        public virtual void ParsePayload() {
            if ( PayloadData.Length >= CMD_RSP_CODE_LEN ) ResponseCode = (CommandResponseCode) PayloadData[ 0 ];
        }

        public T As< T >() where T : CommandResponsePdu, new() {
            var pdu = new T { Header = Header, PayloadData = PayloadData };
            pdu.ParsePayload();
            return pdu;
        }

        public static CommandResponsePdu FromByteArray( byte[] bytes ) {
            if ( bytes.Length < Protocol.PDU_CMD_RSP_CODE_POS ) {
                return null;
            }
            if ( bytes.Length != ( Protocol.PDU_HDR_LEN + bytes[ Protocol.PDU_HDR_POS_PAY_LEN ] ) ) {
                return null;
            }
            if ( bytes[ Protocol.PDU_HDR_POS_PDU_TYPE ] != (byte) PduType.Command ) {
                return null;
            }

            CommandResponsePdu pdu = new CommandResponsePdu();
            pdu.Header.PduType = (PduType) bytes[ Protocol.PDU_HDR_POS_PDU_TYPE ];
            pdu.Header.ClassId = (ClassId) bytes[ Protocol.PDU_HDR_POS_CLS_ID ];
            pdu.Header.CommandCode = (CommandCode) bytes[ Protocol.PDU_HDR_POS_CMD_CODE ];
            pdu.Header.PayloadLength = bytes[ Protocol.PDU_HDR_POS_PAY_LEN ];
            pdu.Header.PayloadCrc8 = bytes[ Protocol.PDU_HDR_POS_PAY_CRC8 ];
            pdu.Header.HeaderCrc8 = bytes[ Protocol.PDU_HDR_POS_CRC8 ];

            if ( pdu.Header.PayloadLength > 0 ) {
                pdu.PayloadData = new byte[pdu.Header.PayloadLength];
                Buffer.BlockCopy( bytes, Protocol.PDU_PAY_POS, pdu.PayloadData, 0, pdu.Header.PayloadLength );

                pdu.ResponseCode = (CommandResponseCode) pdu.PayloadData[ 0 ];
            }

            return pdu;
        }

    }

}