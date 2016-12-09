namespace XOUND.Models
{
    public class TrackFile : ModelBase
    {
        public byte[] Track { get; internal set; }
        public int TrackID { get; internal set; }
    }
}