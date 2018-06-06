using System;

namespace BlueCats.Ble.Serial.BC0xx.Commands.Base {

    public class CommandPdu : Pdu< CommandHeader > {

        public CommandPdu( CommandCode commandCode ) {
            Header.CommandCode = commandCode;
        }

        public byte[] ToByteArray() {
            var bytes = new byte[SerialProtocol.PDU_HDR_LEN + Header.PayloadLength];
            bytes[ SerialProtocol.PDU_HDR_POS_PDU_TYPE ] = (byte) Header.PduType;
            bytes[ SerialProtocol.PDU_HDR_POS_CLS_ID ] = (byte) Header.ClassId;
            bytes[ SerialProtocol.PDU_HDR_POS_CMD_CODE ] = (byte) Header.CommandCode;
            bytes[ SerialProtocol.PDU_HDR_POS_PAY_LEN ] = Header.PayloadLength;
            if ( Header.PayloadLength > 0 ) {
                Buffer.BlockCopy( PayloadData, 0, bytes, SerialProtocol.PDU_PAY_POS, Header.PayloadLength );
                bytes[ SerialProtocol.PDU_HDR_POS_PAY_CRC8 ] = SerialProtocol.Crc8( bytes, SerialProtocol.PDU_PAY_POS, Header.PayloadLength );
            }
            else {
                bytes[ SerialProtocol.PDU_HDR_POS_PAY_CRC8 ] = 0x00;
            }
            bytes[ SerialProtocol.PDU_HDR_POS_CRC8 ] = SerialProtocol.Crc8( bytes, 0, ( SerialProtocol.PDU_HDR_LEN - 1 ) );
            return bytes;
        }

    }

}