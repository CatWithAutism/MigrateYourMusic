namespace WebHandlers.Models
{
    public class Track
    {
        /// <summary>
        /// Исполнитель.
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// Наименование.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Длинна трека.
        /// </summary>
        public int Duration { get; set; } 

    }
}
