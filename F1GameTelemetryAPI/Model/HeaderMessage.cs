﻿namespace F1GameTelemetryAPI.Model;

using F1GameTelemetry.Enums;

public struct HeaderMessage
{
    public PacketId PacketId { get; set; }
    public int Total { get; set; }
}
