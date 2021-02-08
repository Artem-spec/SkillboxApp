namespace ModuleFour.Model
{
    internal class MonthMoneyData
    {
        public TypeMonth Month { get; set; }

        public int Income { get; set; }

        public int Consumption { get; set; }

        public int Diff => Income - Consumption;

        public MonthMoneyData(TypeMonth month, int income, int consumption)
        {
            Month = month;
            Income = income;
            Consumption = consumption;
        }
    }
}
