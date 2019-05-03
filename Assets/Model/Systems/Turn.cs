public class Turn : Movement
{
    public Turn (string blueprint, string[][] unitStrings) : base(blueprint, unitStrings)
    {
        TurnIndex = 0;
    }

    public Turn(int size, string[][] unitStrings) : base (size, unitStrings)
    {
        TurnIndex = 0;
    }

    public int TurnIndex;

    public void NewTurn()
    {
        if (TurnIndex == 0)
        {
            TurnIndex = 1;
        }
        else
        {
            TurnIndex = 0;
        }

        Armies[TurnIndex].UpdateTurn();
    }
}