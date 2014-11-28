namespace ConsoleApp2.Entidades
{
    public partial class Actor
    {
        public void GetOlder(int years)
        {
            DateOfBirth = DateOfBirth.AddYears(-years);
        }
    }
}
