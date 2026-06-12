using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Models
{
    public class ConnectionInfo
    {
        public string ConnectionName { get; set; }
        public string ConnectionUN { get; set; }
        public string ConnectionPW { get; set; }

        public static string EncodeConnections(List<ConnectionInfo> connections)
        {
            var json = JsonConvert.SerializeObject(connections);

            var bytes = Encoding.UTF8.GetBytes(json);
            var base64 = Convert.ToBase64String(bytes);

            return base64;
        }

        public static List<ConnectionInfo> DecodeConnections(string base64Blob)
        {
            if (string.IsNullOrWhiteSpace(base64Blob))
                return new List<ConnectionInfo>();

            var bytes = Convert.FromBase64String(base64Blob);
            var json = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<List<ConnectionInfo>>(json);
        }
    }
}
