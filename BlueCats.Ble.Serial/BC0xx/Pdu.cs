namespace BlueCats.Ble.Serial.BC0xx {

    public abstract class Pdu< T > where T : Header, new() {

        public T Header { get; set; }
        public byte[] PayloadData { get; set; }

        protected Pdu() {
            Header = new T();
        }

    }

}