namespace Models
{
    public struct PlayerWithScore
    {
        public string Name;
        public int Score;

        public PlayerWithScore(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}