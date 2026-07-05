
namespace CodingTracker.Model;

internal class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public String Duration { get; set; }
}
