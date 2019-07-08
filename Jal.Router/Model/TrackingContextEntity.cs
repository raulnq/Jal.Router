using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class TrackingContextEntity
    {
        public List<Track> Tracks { get; }

        public TrackingContextEntity()
        {

        }

        public TrackingContextEntity(List<Track> tracks)
        {
            Tracks = tracks;
        }
    }
}
