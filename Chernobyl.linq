<Query Kind="Program">
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

using System.Text.Json;

void Main()
{
	string url = "https://api.saveecobot.com/output.json";
	
	string json = GetURLResponseAsString(url);
	
	JsonSerializerOptions options = new();
	options.Converters.Add(new DateTimeConverter());
	List<City> cities = JsonSerializer.Deserialize<List<City>>(json, options);

	ChartData(cities);
}

string GetURLResponseAsString(string url)
{
	string result = string.Empty;

	WebRequest request = WebRequest.Create(url);
	WebResponse response = request.GetResponse();
	Stream data = response.GetResponseStream();

	using (StreamReader sr = new StreamReader(data))
	{
		result = sr.ReadToEnd();
		result = System.Net.WebUtility.HtmlDecode(result);
	}

	return result;
}

void ChartData(List<City> cities)
{
	//cities.Dump();
	
	List<City> today = new();
	
	foreach (City city in cities)
	{
		if (city.pollutants.Where(p => p.pol.Equals("PM2.5")).FirstOrDefault()?.time >= DateTime.Today)
		{
			city.Dump();
			today.Add(city);
		}
	}

	today
		.Chart(c => c.id.Replace("SAVEDNIPRO_", ""))
		.AddYSeries(c => c.pollutants.Where(x => x.pol.Equals("PM2.5")).FirstOrDefault()?.value, Util.SeriesType.Point, name: "PM2.5 (ug/m3)")
		//.AddYSeries(c => c.pollutants.Where(x => x.pol.Equals("PM10")).FirstOrDefault()?.value, Util.SeriesType.Point, name: "PM10 (ug/m3)")
		//.AddYSeries(c => c.pollutants.Where(x => x.pol.Equals("Temperature")).FirstOrDefault()?.value, Util.SeriesType.Point, name: "Temperature (C)")
		//.AddYSeries(c => c.pollutants.Where(x => x.pol.Equals("Humidity")).FirstOrDefault()?.value, Util.SeriesType.Point, name: "Humidity (%)")
		//.AddYSeries(c => c.pollutants.Where(x => x.pol.Equals("Pressure")).FirstOrDefault()?.value, Util.SeriesType.Point, name: "Pressure (hPa)")
		//.AddYSeries(c => c.pollutants.Where(x => x.pol.Equals("Air Quality Index")).FirstOrDefault()?.value, Util.SeriesType.Point, name: "Air Quality Index (aqi)")
		.Dump();
}

// Simple models for the JSON data
public class City
{
	public string id { get; set; }
	public string cityName { get; set; }
	public string stationName { get; set; }
	public string localName { get; set; }
	public string timezone { get; set; }
	public string latitude { get; set; }
	public string longitude { get; set; }
	public List<Pollutant> pollutants { get; set; } = new();
}

public class Pollutant
{
	public string pol { get; set; }
	public string unit { get; set; }
	public DateTime? time { get; set; }
	public double? value { get; set; }
	public string averaging { get; set; }
}

public class DateTimeConverter : JsonConverter<DateTime?>
{
	public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		
		return DateTime.Parse(reader.GetString());
	}

	public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value?.ToString() ?? "");
	}
}