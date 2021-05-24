using Sprache;

namespace SpracheMultiRecord
{
    public class NMRecordSet
    {
        public NMRecordSet(NM1Record nm1, IOption<NM2Record> nm2)
        {
            Nm1 = nm1;
            Nm2 = nm2;
        }

        public NM1Record Nm1 { get; }
        public IOption<NM2Record> Nm2 { get; }
    }
}
