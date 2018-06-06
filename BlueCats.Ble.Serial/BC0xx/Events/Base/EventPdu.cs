using System;

namespace BlueCats.Ble.Serial.BC0xx.Events.Base {

    public class EventPdu : Pdu< EventHeader > {

        public EventPdu() {
            Header = new EventHeader();
        }

        public static EventPdu FromByteArray( byte[] bytes ) {
            if ( bytes.Length < SerialProtocol.PDU_HDR_LEN ) {
                return null;
            }
            if ( bytes.Length != ( SerialProtocol.PDU_HDR_LEN + bytes[ SerialProtocol.PDU_HDR_POS_PAY_LEN ] ) ) {
                return null;
            }
            if ( bytes[ SerialProtocol.PDU_HDR_POS_PDU_TYPE ] != (byte) PduType.Event ) {
                return null;
            }

            EventPdu pdu = new EventPdu();
            pdu.Header.PduType = (PduType) bytes[ SerialProtocol.PDU_HDR_POS_PDU_TYPE ];
            pdu.Header.ClassId = (ClassId) bytes[ SerialProtocol.PDU_HDR_POS_CLS_ID ];
            pdu.Header.EventCode = (EventCode) bytes[ SerialProtocol.PDU_HDR_POS_EVT_CODE ];
            pdu.Header.PayloadLength = bytes[ SerialProtocol.PDU_HDR_POS_PAY_LEN ];
            pdu.Header.PayloadCrc8 = bytes[ SerialProtocol.PDU_HDR_POS_PAY_CRC8 ];
            pdu.Header.HeaderCrc8 = bytes[ SerialProtocol.PDU_HDR_POS_CRC8 ];

            if ( pdu.Header.PayloadLength > 0 ) {
                pdu.PayloadData = new byte[ pdu.Header.PayloadLength ];
                Buffer.BlockCopy( bytes, SerialProtocol.PDU_PAY_POS, pdu.PayloadData, 0, pdu.Header.PayloadLength );
            }

            return pdu;
        }

        public virtual void ParsePayload() { }

        public T As< T >() where T : EventPdu, new() {
            var pdu = new T { Header = this.Header, PayloadData = this.PayloadData };
            pdu.ParsePayload();
            return pdu;
        }

    }

}