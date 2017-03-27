using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreHtmlPostSample
{
    class NicoNicoDic
    {
        private static string url = "http://dic.nicovideo.jp/s";

        public async Task<List<ResponseData>> searchResultList(string keyword)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("search_category", "a"),
                new KeyValuePair<string, string>("query_type", "b"),
                new KeyValuePair<string, string>("query", keyword)
            });

            using (var client = new HttpClient())
            using (var stream = await client.PostAsync(url, content))
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                stream.EnsureSuccessStatusCode();
                htmlDoc.LoadHtml(stream.Content.ReadAsStringAsync().Result);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//table[@class=\"menulist\"]/tr");
                if (nodes == null)
                {
                    return new List<ResponseData>();
                }

                nodes.RemoveAt(0);
                var responseDataList = new List<ResponseData>();
                foreach (var node in nodes)
                {
                    var n = node.SelectNodes("td")
                                .Select(s => s.InnerText.Replace(" ", "").Replace("\n", ""))
                                .ToList<string>();

                    var responseData = new ResponseData()
                    {
                        title = n[1],
                        category = n[2],
                        res = n[3],
                        lastUpdate = n[4],
                        lastRes = n[5]
                    };

                    responseDataList.Add(responseData);
                }

                return responseDataList;

            }
        }
    }

    class ResponseData
    {
        public string title { get; set; }
        public string category { get; set; }
        public string res { get; set; }
        public string lastUpdate { get; set; }
        public string lastRes { get; set; }
    }
}
