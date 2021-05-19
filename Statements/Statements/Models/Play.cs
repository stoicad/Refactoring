namespace Statements.Models
{
    public class Play
    {
        public Play(string playId, string name, PlayTypeEnum type)
        {
            PlayId = playId;
            Name = name;
            Type = type;
        }

        public string PlayId { get; set; }
        public string Name { get; set; }
        public PlayTypeEnum Type { get; set; }
    }
}