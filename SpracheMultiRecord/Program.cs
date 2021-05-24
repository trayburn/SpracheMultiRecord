using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpracheMultiRecord
{
    class Program
    {
        static Parser<string> Field =
            from fd in Parse.Char('|').Named("Field Delimeter")
            from content in Parse.CharExcept(new char[] { '|', '~' }).Many().Named("Content")
            select new string(content.ToArray());

        static Parser<NM1Record> NM1 =
            from leading in Parse.WhiteSpace.Many()
            from name in Parse.String("NM1").Named("NM1")
            from firstName in Field.Named("First Name")
            from middleName in Field.Named("Middle Name")
            from lastName in Field.Named("LastName")
            from record in Parse.Char('~').Named("Record Delimeter")
            select new NM1Record(firstName, middleName, lastName);

        static Parser<NM2Record> NM2 =
            from leading in Parse.WhiteSpace.Many()
            from name in Parse.String("NM2").Named("NM2")
            from address1 in Field.Named("Address 1")
            from address2 in Field.Named("Address 2")
            from city in Field.Named("City")
            from state in Field.Named("State")
            from zip in Field.Named("Zip")
            from record in Parse.Char('~').Named("Record Delimeter")
            select new NM2Record(address1, address2, city, state, zip);

        static Parser<TXTRecord> TXT =
            from leading in Parse.WhiteSpace.Many()
            from name in Parse.String("TXT").Named("TXT")
            from text in Field.Named("Text")
            from record in Parse.Char('~').Named("Record Delimeter")
            select new TXTRecord(text);

        static Parser<NMRecordSet> NMRecordSet =
            from nm1 in NM1
            from nm2 in NM2.Optional()
            select new NMRecordSet(nm1, nm2);

        static Parser<IEnumerable<object>> FileParser =
            from txt1 in TXT.Optional().Named("PreTXT")
            from nm in NMRecordSet
            from txt2 in TXT.Optional().Named("PostTXT")
            select new object[] { 
                txt1.GetOrDefault(),
                nm.Nm1, 
                nm.Nm2.GetOrDefault(),
                txt2.GetOrDefault()
            }.Where(e => e is not null);

        static void Main(string[] args)
        {
            // The rules of our multi-record format.
            // fields are delimited with |
            // records are delimited with ~
            // One and only one NM1 may exist
            // NM1 may optionally be followed by NM2
            // NM2 cannot exist without NM1 preceeding
            // TXT can exist before or after NM1/NM2 but not between NM1 and NM2.
            // Record names can have leading white space

            string test1 = "NM1|Timothy|John|Rayburn~NM2|123 Main St||Plano|TX|75023~TXT|Freeform Record~";
            Dump(FileParser.Parse(test1));

            string test2 = "NM1|Timothy|John|Rayburn~TXT|Freeform Record~";
            Dump(FileParser.Parse(test2));

            string test3 = "TXT|First Freeform Record~    NM1|Timothy|John|Rayburn~    NM2|123 Main St||Plano|TX|75023~TXT|Freeform Record~";
            Dump(FileParser.Parse(test3));
        }

        public static void Dump<T>(IEnumerable<T> results)
        {
            foreach (var result in results)
            {
                switch (result)
                {
                    case TXTRecord t:
                        Console.WriteLine($"TXT Record with value {t.Text}");
                        break;
                    case NM1Record nm1:
                        Console.WriteLine($"NM1 Record with First:{nm1.FirstName}, Middle:{nm1.MiddleName}, Last:{nm1.LastName}");
                        break;
                    case NM2Record nm2:
                        Console.WriteLine($"NM2 Record with Address1:{nm2.Address1}, Address2:{nm2.Address2}, City:{nm2.City}, State:{nm2.State}, Zip:{nm2.State}");
                        break;
                    default:
                        Console.WriteLine("Unknown record type");
                        break;
                }
            }
            Console.WriteLine("---------------------------------------------------------------");
        }
    }
}
