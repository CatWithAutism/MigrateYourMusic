namespace WebHandlers.Models
{
    /// <summary>
    /// The default model of tracks.
    /// </summary>
    public class Track
    {
        public string Artist { get; set; }
        
        public string Title { get; set; }
        
        public int Duration { get; set; }
    }
}