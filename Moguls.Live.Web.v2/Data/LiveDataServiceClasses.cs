using Newtonsoft.Json.Linq;

namespace Moguls.Live.Web.v2.Data
{
    public class LiveDataServiceClasses
    {
        public class StatusResult
        {
            public int[] activeResultTables { get; set; }
            public int competitionRevision { get; set; }
            public int resultsRevision { get; set; }
        }

        public class ParaResult
        {
            public Update[] updates { get; set; }

            public class Update
            {
                public int competitionResultRevision { get; set; }
                public int resultTableId { get; set; }
                public Data data { get; set; }
            }

            public class Data
            {
                public Entry[] entries { get; set; }
            }

            public class Entry
            {
                public JToken[][] values { get; set; }
                public string athleteUuid { get; set; }
                public Highlightcolor highlightColor { get; set; }
            }

            public class Highlightcolor
            {
                public int r { get; set; }
                public int g { get; set; }
                public int b { get; set; }
                public float a { get; set; }
            }
        }

        public class ResultDualCompetition
        {
            public string Place { get; set; }
            public string Bib { get; set; }
            public string Name { get; set; }
            public string Nationality { get; set; }
            public string NationalityFlag { get; set; }
            public string Score { get; set; }
            public ParaResult.Highlightcolor highlightColor { get; set; }
            public bool IsBlue => highlightColor.b == 204;
        }
    }
}
