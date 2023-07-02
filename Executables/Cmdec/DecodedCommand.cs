namespace Arc.Cmdec
{
    internal record DecodedCommand(long Location, byte[] RawData, string Description)
    {
    }
}
