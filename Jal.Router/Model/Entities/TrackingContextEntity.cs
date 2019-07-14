using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class TrackingContextEntity
    {
        public List<Tracking> Trackings { get; private set; }

        private TrackingContextEntity()
        {

        }

        public TrackingContextEntity(List<Tracking> trackings)
        {
            Trackings = trackings;
        }
    }
}
