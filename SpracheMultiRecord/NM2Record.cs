namespace SpracheMultiRecord
{
    public class NM2Record
    {
        public NM2Record(string address1, string address2, string city, string state, string zip)
        {
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Zip = zip;
        }

        public string Address1 { get; }
        public string Address2 { get; }
        public string City { get; }
        public string State { get; }
        public string Zip { get; }
    }
}
