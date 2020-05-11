using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace RoutesWebAppDemo.Models
{
    public class Route
    {
        public double distance = 0;
        public double time = 0;
        public List<Coordinates> maneuverPoints = new List<Coordinates>();

        public Route(string data)
        {
            initByJson(data);
        }

        public Route()
        {
            
        }

        public async Task initByCors(Coordinates toCor, Coordinates fromCor)
        {
            string url = GenerateRequestUrl(toCor, fromCor);
            string json = await GetJsonResponse(url);
            initByJson(json);
        }

        private void initByJson(string data)
        {
            Data d = JsonSerializer.Deserialize<Data>(data);
            this.distance = d.resourceSets[0].resources[0].travelDist;
            this.time = d.resourceSets[0].resources[0].travelDuration;
            this.maneuverPoints = new List<Coordinates>();

            List<ItineraryItem> items = d.resourceSets[0].resources[0].routeLegs[0].itineraryItems;
            foreach (var item in items)
            {
                double[] cor = item.maneuverPoint.coordinate;
                this.maneuverPoints.Add(new Coordinates(cor[0], cor[1]));
            }
        }

        public static string GenerateRequestUrl(Coordinates toCor, Coordinates fromCor)
        {
            string url = "http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={0}&wp.1={1}&avoid=minimizeTolls&key={2}";
            string apiKey = "AlHYPJRqRJFnyV05Dp_SmcfZgGUI5BNP6ljRLVvV43Rzm3-duATXyiG0rHJeH4oS";
            string result = String.Format(url, fromCor.ToString(), toCor.ToString(), apiKey);

            return result;
        }

        public static async Task<string> GetJsonResponse(string url)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync(url);

            var msg = await stringTask;

            return msg;
        }
    }

    partial class Data
    {
        [JsonPropertyName("resourceSets")]
        public List<ResourceSet> resourceSets { get; set; }
    }

    partial class ResourceSet
    {
        [JsonPropertyName("resources")]
        public List<Resource> resources { get; set; }
    }
    partial class Resource
    {
        [JsonPropertyName("travelDistance")]
        public double travelDist { get; set; }

        [JsonPropertyName("travelDuration")]
        public double travelDuration { get; set; }

        [JsonPropertyName("routeLegs")]
        public List<RouteLeg> routeLegs { get; set; }
    }

    partial class RouteLeg
    {
        [JsonPropertyName("itineraryItems")]
        public List<ItineraryItem> itineraryItems { get; set; }
    }

    partial class ItineraryItem
    {
        [JsonPropertyName("maneuverPoint")]
        public ManeuverPoint maneuverPoint { get; set; }
    }

    partial class ManeuverPoint
    {
        [JsonPropertyName("coordinates")]
        public double[] coordinate { get; set; }
    }
}