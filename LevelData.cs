namespace eMastercamRateMyCode
{
    public struct LevelData
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public LevelData(int number, string name)
        {
            Number = number;
            Name = name;
        }
    }
}
