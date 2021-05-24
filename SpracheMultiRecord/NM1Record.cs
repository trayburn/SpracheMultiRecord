namespace SpracheMultiRecord
{
    public class NM1Record
    {
        public NM1Record(string firstName, string middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }
    }
}
