using System.Linq;
using BlueCats.Ble.Serial.BC0xx.Commands.Base;

namespace BlueCats.Ble.Serial.BC0xx.Commands {

    public class ReadStatusResponse : CommandResponsePdu {

        public override void ParsePayload() {
            Status = PayloadData.Skip( 1 ).ToArray();
        }

        public byte[] Status { get; set; }

    }

}