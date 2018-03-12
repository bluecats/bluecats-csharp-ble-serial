namespace BlueCats.Ble.Serial.BC0xx {

    public enum PduType : byte {

        Command = 0x00,
        Event = 0x80

    }

    public enum ClassId : byte {

        BlueCats = 0xBC

    }

    public enum CommandCode : byte {

        Meow = 0x01,
        ReadBluetoothAddress = 0x02,
        ReadFirmwareVersion = 0x03,
        ReadFirmwareUID = 0x04,
        ReadModelNumber = 0x05,
        ReadStatus = 0x06,
        ReadEncryptedStatus = 0x07,
        StartScan = 0x08,
        StopScan = 0x09,
        WriteFirmwareHeader = 0x0A,
        WriteFirmwareBlock = 0x0B,
        GoDfu = 0x0C,
        EraseFlash = 0x7F,

    }

    public enum CommandResponseCode : byte {

        Ack = 0x00,
        Busy = 0x01,
        AdvertisingDisabled = 0x02,
        BufferOverflow = 0x03,
        RemoteHungup = 0x04,
        GattWrite = 0x05,
        InvalidParameter = 0x06,
        NotSupported = 0x07,
        NoPendingChanges = 0x08,
        PduHeaderCrc8Failed = 0x09,
        PduPayloadCrc8Failed = 0x0A,
        FirmwareHeaderValidationFailed = 0x0B

    }

    public enum EventCode : byte {

        BLEConnected = 0x01,
        BLEDisconnected = 0x02,
        Debug = 0x03,
        BLEAuthSucceeded = 0x04,
        BLEAuthFailed = 0x05,
        Error = 0x06,
        SettingsSaved = 0x07,
        BLEDataRequested = 0x08,
        DataBlockSent = 0x09,
        BLEDataIndicated = 0x0A,
        ScanStarted = 0x0B,
        ScanStopped = 0x0C,
        DeviceRanged = 0x0D,
        DeviceEntered = 0x0E,
        DeviceExited = 0x0F,
        DeviceDiscovered = 0x10,
        Boot = 0x11,

    }

    public enum ErrorEventCode {

        UartTxQueueOverflow = 0x01,
        UnrecognizedCommand = 0x84,
        PermissionDenied = 0x85,

    }

}