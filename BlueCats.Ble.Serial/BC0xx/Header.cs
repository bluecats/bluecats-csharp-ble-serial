namespace BlueCats.Ble.Serial.BC0xx {

    public abstract class Header {

        public PduType PduType { get; set; }
        public ClassId ClassId { get; set; }
        public byte PayloadLength { get; set; }
        public byte PayloadCrc8 { get; set; }
        public byte HeaderCrc8 { get; set; }

    }

}