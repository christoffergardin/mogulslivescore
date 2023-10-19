using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moguls.Live.Web.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moguls.Live.Web.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ResultController : ControllerBase
	{
		private readonly ILogger<ResultController> _logger;

		public ResultController(ILogger<ResultController> logger)
		{
			_logger = logger;
		}

		[HttpGet("{id}")]
		public async Task<IEnumerable<ResultModel>> Get(string id)
		{
			//return new OkObjectResult(resultModels);
			var httpClient = new HttpClient();

			var site = await httpClient.GetStringAsync("https://api.live-scoring.com/competitions/505/results-since/-1");

			//Console.WriteLine(site);

			var deserializeObject = JsonConvert.DeserializeObject<Rootobject>(site);

			var resultModels = new List<ResultModel>();

			foreach (var dataEntry in deserializeObject.updates.First().data.entries)
			{
				foreach (var o in dataEntry.values)
				{
					var list = new List<string>();
					foreach (JToken jToken in o)
					{
						string value;
						try
						{
							var rowValue = jToken.ToObject<RowValue>();
							value = rowValue.text;
						}
						catch (Exception)
						{
							var t = o[0];
							value = t.ToObject<string>();
						}
						list.Add(value);
					}
					var resultModel = new ResultModel
					{
						Rank = list[0],
						StNr = list[1],
						Bib = list[2],
						Name = list[3],
						Nsa = list[4],
						Score = list[5]
					};
					resultModels.Add(resultModel);
				}
			}

			foreach (var resultModel in resultModels)
			{
				_logger.LogInformation($"{resultModel.Rank}\t{resultModel.StNr}\t{resultModel.Name}");
			}

			return resultModels;
		}
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