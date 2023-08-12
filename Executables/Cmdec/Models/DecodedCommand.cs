namespace Arc.Cmdec.Models
{
    internal record DecodedCommand(long Location, byte[] RawData, string Description)
    {
    }
}
