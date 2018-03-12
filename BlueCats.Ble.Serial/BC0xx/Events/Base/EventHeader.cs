namespace BlueCats.Ble.Serial.BC0xx.Events.Base {

    public class EventHeader : Header {

        public EventCode EventCode { get; set; }

        public EventHeader() {
            PduType = PduType.Event;
            ClassId = ClassId.BlueCats;
        }

    }

}