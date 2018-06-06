using System;

namespace BlueCats.Ble.Serial.BC0xx.Commands.Base {

    public class CommandResponsePdu : Pdu< CommandHeader > {

        // Constants
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
            if ( bytes.Length < SerialProtocol.PDU_CMD_RSP_CODE_POS ) {
                return null;
            }
            if ( bytes.Length != ( SerialProtocol.PDU_HDR_LEN + bytes[ SerialProtocol.PDU_HDR_POS_PAY_LEN ] ) ) {
                return null;
            }
            if ( bytes[ SerialProtocol.PDU_HDR_POS_PDU_TYPE ] != (byte) PduType.Command ) {
                return null;
            }

            CommandResponsePdu pdu = new CommandResponsePdu();
            pdu.Header.PduType = (PduType) bytes[ SerialProtocol.PDU_HDR_POS_PDU_TYPE ];
            pdu.Header.ClassId = (ClassId) bytes[ SerialProtocol.PDU_HDR_POS_CLS_ID ];
            pdu.Header.CommandCode = (CommandCode) bytes[ SerialProtocol.PDU_HDR_POS_CMD_CODE ];
            pdu.Header.PayloadLength = bytes[ SerialProtocol.PDU_HDR_POS_PAY_LEN ];
            pdu.Header.PayloadCrc8 = bytes[ SerialProtocol.PDU_HDR_POS_PAY_CRC8 ];
            pdu.Header.HeaderCrc8 = bytes[ SerialProtocol.PDU_HDR_POS_CRC8 ];

            if ( pdu.Header.PayloadLength > 0 ) {
                pdu.PayloadData = new byte[pdu.Header.PayloadLength];
                Buffer.BlockCopy( bytes, SerialProtocol.PDU_PAY_POS, pdu.PayloadData, 0, pdu.Header.PayloadLength );

                pdu.ResponseCode = (CommandResponseCode) pdu.PayloadData[ 0 ];
            }

            return pdu;
        }

    }

}