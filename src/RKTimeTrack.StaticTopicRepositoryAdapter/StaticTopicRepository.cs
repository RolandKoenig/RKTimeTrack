using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.StaticTopicRepositoryAdapter;

class StaticTopicRepository : ITopicRepository
{
    private List<TimeTrackingTopic> _topics;
    
    public StaticTopicRepository()
    {
        _topics = new List<TimeTrackingTopic>
        {
            // Learning
            new TimeTrackingTopic("Weiterbildung", "Technische Spezialthemen"),
            new TimeTrackingTopic("Weiterbildung", "Allgemeine Spezialthemen"),
            new TimeTrackingTopic("Weiterbildung", "Gui"),
            new TimeTrackingTopic("Weiterbildung", "Zeitschriften, Bücher, etc."),
            new TimeTrackingTopic("Weiterbildung", "Zertifizierungslehrgang"),
            
            // Development community
            new TimeTrackingTopic("DevCommunity", "DCN"),
            new TimeTrackingTopic("DevCommunity", "Vorträge (Vorbereitung)"),
            new TimeTrackingTopic("DevCommunity", "Artikel (Vorbereitung)"),
            new TimeTrackingTopic("DevCommunity", "Teilnahme Konferenz"),
            new TimeTrackingTopic("DevCommunity", "Teilnahme Usergroup"),
            new TimeTrackingTopic("DevCommunity", "Sonstige Veranstaltungen"),
            new TimeTrackingTopic("DevCommunity", "Networking allgemein"),
            
            // ...
        };
    }
    
    public IReadOnlyCollection<TimeTrackingTopic> GetAllTopics()
    {
        return _topics;
    }
}