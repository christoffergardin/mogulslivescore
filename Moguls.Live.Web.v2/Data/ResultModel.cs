using Newtonsoft.Json.Linq;

namespace Moguls.Live.Web.v2.Data
{

	public class ResultModel
	{
		public string Rank { get; set; }
		public string StartNumber { get; set; }
		public string Bib { get; set; }
		public string Name { get; set; }
		public string Nationality { get; set; }
        public string NationalityFlag { get; set; }
        public string Score { get; set; }
		public string Status { get; set; }
	}

	public class RowValue
	{
		public string text;
	}

	public class Rootobject
	{
		public Update[] updates { get; set; }
	}

	public class Update
	{
		public int competitionResultRevision { get; set; }
		public int resultTableId { get; set; }
		public Data data { get; set; }
	}

	public class Data
	{
		public int showSeparatorAt { get; set; }
		public Entry[] entries { get; set; }
	}

	public class Entry
	{
		public JToken[][] values { get; set; }
		public string athleteUuid { get; set; }
	}
}