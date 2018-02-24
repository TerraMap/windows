using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TerraMap
{
	public class ReleaseInfo
	{
		public Version Version { get; set; }
		public string Url { get; set; }
		public ReleaseStatus Status { get; set; }

    public static List<ReleaseInfo> FromJsonStream(Stream stream)
    {
      var releases = new List<ReleaseInfo>();

      var serializer = new JsonSerializer();
      using(var streamReader = new StreamReader(stream))
      {
        using (var jsonTextReader = new JsonTextReader(streamReader))
        {
          dynamic data = serializer.Deserialize(jsonTextReader);

          string tag_name = data.tag_name;
          if (string.IsNullOrWhiteSpace(tag_name))
            return releases;
          
          var version = new Version(tag_name);

          var release = new ReleaseInfo()
          {
            Status = ReleaseStatus.Stable,
            Url = "https://terramap.github.io/windows.html",
            Version = version,
          };

          releases.Add(release);
        }
      }

      return releases;
    }

		public static List<ReleaseInfo> FromRssStream(Stream stream)
		{
			var releases = new List<ReleaseInfo>();

			// regex to match a valid release version
			Regex regex = new Regex(@"\d+.\d+.\d+.\d+");

			XDocument document = XDocument.Load(stream);

			foreach (var item in document.Element("rss").Element("channel").Descendants("item"))
			{
				string title = item.Element("title").Value;
				Match match = regex.Match(title);
				if (!match.Success)
					continue;

				string titleLower = title.ToLower();

				if (titleLower.Contains("deleted") || titleLower.Contains("removed"))
					continue;

				ReleaseStatus status = ReleaseStatus.Stable;

				if (titleLower.Contains("alpha"))
					status = ReleaseStatus.Alpha;
				else if (titleLower.Contains("beta"))
					status = ReleaseStatus.Beta;

				Version version = new Version(match.Groups[0].Value);

				if (releases.Exists(r => r.Version == version))
					continue;

				string link = item.Element("link").Value;

				ReleaseInfo release = new ReleaseInfo()
				{
					Status = status,
					Url = link,
					Version = version,
				};

				releases.Add(release);
			}

			return releases;
		}

		public static ReleaseInfo GetLatest(List<ReleaseInfo> releases, ReleaseStatus minimumStatus)
		{
			if (releases.Count < 1)
				return null;

			return releases.Where(r => ((int)r.Status) >= ((int)minimumStatus)).OrderByDescending(r => r.Version).FirstOrDefault();
		}
	}

	public enum ReleaseStatus
	{
		Alpha = 0,
		Beta = 1,
		Stable = 2,
	}
}
