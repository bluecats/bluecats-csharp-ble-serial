namespace BlueCats.Ble.Serial.BC0xx.Commands.Base {

    public class CommandHeader : Header {

        public CommandCode CommandCode { get; set; }

        public CommandHeader() {
            PduType = PduType.Command;
            ClassId = ClassId.BlueCats;
        }

    }

}