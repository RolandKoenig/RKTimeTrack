using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.StaticProjectRepositoryAdapter;

class StaticProjectRepository : IProjectRepository
{
    private List<Project> _projects;
    
    public StaticProjectRepository()
    {
        _projects = new List<Project>
        {
            // Learning
            new Project("Weiterbildung", "Technische Spezialthemen"),
            new Project("Weiterbildung", "Allgemeine Spezialthemen"),
            new Project("Weiterbildung", "Gui"),
            new Project("Weiterbildung", "Zeitschriften, Bücher, etc."),
            new Project("Weiterbildung", "Zertifizierungslehrgang"),
            
            // Development community
            new Project("DevCommunity", "DCN"),
            new Project("DevCommunity", "Vorträge (Vorbereitung)"),
            new Project("DevCommunity", "Artikel (Vorbereitung)"),
            new Project("DevCommunity", "Teilnahme Konferenz"),
            new Project("DevCommunity", "Teilnahme Usergroup"),
            new Project("DevCommunity", "Sonstige Veranstaltungen"),
            new Project("DevCommunity", "Networking allgemein"),
            
            // ...
        };
    }
    
    public IReadOnlyCollection<Project> GetAllProjects()
    {
        return _projects;
    }
}