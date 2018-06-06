using System;
using System.Linq;
using BlueCats.Ble.Serial.BC0xx.Commands.Base;
using BlueCats.Ble.Serial.BC0xx.Events.Base;

namespace BlueCats.Ble.Serial.BC0xx {

    public class SerialProtocol {

        // Consts
        public const int DEFAULT_BAUD_RATE = 921600;

        public const int BLUETOOTH_ADDRESS_LEN = 6;

        public const int PDU_HDR_LEN = 6;
        public const int PDU_HDR_POS_PDU_TYPE = 0;
        public const int PDU_HDR_POS_CLS_ID = 1;
        public const int PDU_HDR_POS_CMD_CODE = 2;
        public const int PDU_HDR_POS_EVT_CODE = PDU_HDR_POS_CMD_CODE;
        public const int PDU_HDR_POS_PAY_LEN = 3;
        public const int PDU_HDR_POS_PAY_CRC8 = 4;
        public const int PDU_HDR_POS_CRC8 = 5;
        public const int PDU_PAY_POS = PDU_HDR_LEN;
        public const int PDU_CMD_RSP_CODE_POS = PDU_PAY_POS;

        public const int PDU_PAYLOAD_SIZE_MAX = 255;
        public const int PDU_BUF_SIZE = PDU_PAYLOAD_SIZE_MAX + PDU_HDR_LEN;

        // Events
        public event EventHandler< string > ProtocolError;
        public event EventHandler< CommandResponsePdu > CommandResponseReceived;
        public event EventHandler< EventPdu > EventReceived;

        private readonly byte[] _rxPduBuf = new byte[ PDU_BUF_SIZE ];
        private int _pduBufPos;
        private int _pduPayLen;

        public void Parse( byte ch ) {
            if ( _pduBufPos >= PDU_BUF_SIZE ) {
                _pduBufPos = 0;
            }
            //-----------------
            // Parse PDU_TYPE 
            //-----------------
            // check packet position
            if ( _pduBufPos == 0 ) {
                // beginning of packet, check for correct framing/expected byte(s)
                // Packet must be either Command/Response (0x00) or Event (0x80)
                if ( ( ch & 0x7F ) == 0x00 ) {
                    // store new character in RX buffer
                    _rxPduBuf[ _pduBufPos++ ] = ch;
                }
            }
            else {
                _rxPduBuf[ _pduBufPos++ ] = ch;
                if ( _pduBufPos == ( PDU_HDR_POS_CLS_ID + 1 ) ) {
                    //------------
                    // Parse CLSID 
                    //------------
                    if ( _rxPduBuf[ PDU_HDR_POS_CLS_ID ] != (byte) ClassId.BlueCats ) {
                        if ( ( _rxPduBuf[ PDU_HDR_POS_CLS_ID ] & 0x7F ) == 0x00 ) {
                            _rxPduBuf[ 0 ] = _rxPduBuf[ PDU_HDR_POS_CLS_ID ];
                            _pduBufPos = 1;
                        }
                        else {
                            _pduBufPos = 0; // bad sync, reset pdu buffer
                        }
                    }
                }
                //-----------------
                // Parse PAY_LEN 
                //-----------------
                else if ( _pduBufPos == ( PDU_HDR_POS_PAY_LEN + 1 ) ) {
                    _pduPayLen = ch;
                }
                else if ( _pduBufPos >= PDU_HDR_LEN ) {
                    //-----------------------------
                    // Parse header CRC8 and verify 
                    //-----------------------------
                    if ( _pduBufPos == PDU_HDR_LEN ) {
                        // check pdu header crc8
                        byte hdrCrc8 = Crc8( _rxPduBuf, 0, PDU_HDR_LEN - 1 );
                        if ( hdrCrc8 != _rxPduBuf[ PDU_HDR_POS_CRC8 ] ) {
                            _pduBufPos = 0; // crc failed, reset pdu buffer           
                            var errMsg = $"Header CRC8 does not match (calculated: 0x{hdrCrc8:2X}, given: 0x{_rxPduBuf[ PDU_HDR_POS_CRC8 ]:2X}";
                            ProtocolError?.Invoke( this, errMsg );
                        }
                    }

                    //---------------------
                    // Full packet received
                    //---------------------
                    if ( _pduBufPos == ( _pduPayLen + PDU_HDR_LEN ) ) {
                        // just received last expected byte, reset pdu buffer
                        _pduBufPos = 0;

                        //------------------------------
                        // Parse payload CRC8 and verify 
                        //------------------------------
                        if ( _pduPayLen > 0 ) {
                            // check pdu payload crc8
                            byte payCrc8 = Crc8( _rxPduBuf, PDU_PAY_POS, _pduPayLen );
                            if ( payCrc8 != _rxPduBuf[ PDU_HDR_POS_PAY_CRC8 ] ) {
                                var errMsg = $"Header CRC8 does not match (calculated: 0x{payCrc8:X2}, given: 0x{_rxPduBuf[ PDU_HDR_POS_CRC8 ]:X2})";
                                ProtocolError?.Invoke( this, errMsg );
                                return;
                            }
                        }

                        //-----------
                        // Handle RSP 
                        //-----------
                        if ( _rxPduBuf[ PDU_HDR_POS_PDU_TYPE ] == (byte) PduType.Command ) {
                            if ( _pduPayLen <= 0 ) {
                                _pduBufPos = 0; // reset pdu buffer
                                ProtocolError?.Invoke( this, "Missing response code for response" );
                            }
                            var payloadBytes = _rxPduBuf.Take( _pduPayLen + PDU_HDR_LEN ).ToArray();
                            var pdu = CommandResponsePdu.FromByteArray( payloadBytes );
                            CommandResponseReceived?.Invoke( this, pdu );
                        }
                        //-----------
                        // Handle EVT 
                        //-----------
                        else if ( _rxPduBuf[ PDU_HDR_POS_PDU_TYPE ] == (byte) PduType.Event ) {
                            var payloadBytes = _rxPduBuf.Take( _pduPayLen + PDU_HDR_LEN ).ToArray();
                            var pdu = EventPdu.FromByteArray( payloadBytes );
                            EventReceived?.Invoke( this, pdu );
                        }
                    }
                }
            }
        }

        public static byte Crc8( byte[] buf, int offset, int len ) {
            int bufIdx = offset; // index to buf
            UInt32 crc = 0;

            for ( int j = 0; j < len; j++, bufIdx++ ) {
                // iterate over each byte of buffer
                // increment buffer byte pointer and index
                crc ^= (UInt16) ( buf[ bufIdx ] << 8 );
                for ( int i = 0; i < 8; i++ ) {
                    // loop over 8 bits

                    if ( ( crc & 0x8000U ) != 0 ) {
                        // if highest bit is set in crc
                        crc ^= ( 0x1070 << 3 ); // xor crc with this weird ass value
                    }
                    crc <<= 1;
                }
            }
            return (byte) ( crc >> 8 );
        }

        // Command PDUs generators
        public CommandPdu CreateMeowCommand() {
            return new CommandPdu( CommandCode.Meow );
        }

        public static CommandPdu CreateReadBluetoothAddressCommand() {
            return new CommandPdu( CommandCode.ReadBluetoothAddress );
        }

        public static CommandPdu CreateReadFirmwareVersionCommand() {
            return new CommandPdu( CommandCode.ReadFirmwareVersion );
        }

        public static CommandPdu CreateReadFirmwareUIDCommand() {
            return new CommandPdu( CommandCode.ReadFirmwareUID );
        }

        public static CommandPdu CreateReadModelNumberCommand() {
            return new CommandPdu( CommandCode.ReadModelNumber );
        }

        public static CommandPdu CreateReadEncryptedStatusCommand() {
            return new CommandPdu( CommandCode.ReadEncryptedStatus );
        }

        public static CommandPdu CreateReadStatusCommand() {
            return new CommandPdu( CommandCode.ReadStatus );
        }

        public static CommandPdu CreateStartScanCommand( byte discoveryMode = 3, ushort scanDuration = 100, ushort scanInterval = 53, ushort scanWindow = 53, byte scanResults = 5, bool discoveryActiveScan = false ) {
            var pdu = new CommandPdu( CommandCode.StartScan );
            pdu.PayloadData = new byte[ 9 ];
            pdu.PayloadData[ 0 ] = discoveryMode;
            pdu.PayloadData[ 1 ] = scanResults;
            pdu.PayloadData[ 2 ] = (byte) ( scanDuration & 0xFF );
            pdu.PayloadData[ 3 ] = (byte) ( ( scanDuration << 8 ) & 0xFF );
            pdu.PayloadData[ 4 ] = (byte) ( scanInterval & 0xFF );
            pdu.PayloadData[ 5 ] = (byte) ( ( scanInterval << 8 ) & 0xFF );
            pdu.PayloadData[ 6 ] = (byte) ( scanWindow & 0xFF );
            pdu.PayloadData[ 7 ] = (byte) ( ( scanWindow << 8 ) & 0xFF );
            pdu.PayloadData[ 8 ] = Convert.ToByte( discoveryActiveScan );
            pdu.Header.PayloadLength = 9;
            return pdu;
        }

        public static CommandPdu CreateStopScanCommand() {
            return new CommandPdu( CommandCode.StopScan );
        }

        public static CommandPdu CreateWriteFirmwareHeaderCommand( byte[] encryptedFirmwareHeader ) {
            var pdu = new CommandPdu( CommandCode.WriteFirmwareHeader ) {
                PayloadData = encryptedFirmwareHeader
            };
            pdu.Header.PayloadLength = (byte) pdu.PayloadData.Length;
            return pdu;
        }

        public static CommandPdu CreateWriteFirmwareBlockCommand( byte[] encryptedFirmwareBlock ) {
            var pdu = new CommandPdu( CommandCode.WriteFirmwareBlock ) {
                PayloadData = encryptedFirmwareBlock
            };
            pdu.Header.PayloadLength = (byte) pdu.PayloadData.Length;
            return pdu;
        }

        public static CommandPdu CreateGoDfuCommand() {
            return new CommandPdu( CommandCode.GoDfu );
        }

    }

}