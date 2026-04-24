using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SWE_AutomationJs_UI_Design.Data
{
    [Serializable]
    public sealed class OverrideRequest
    {
        public string RequestId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string RequestedAction { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAtUtc { get; set; }
        public string ReviewedByEmployeeId { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }
    }

    [Serializable]
    public sealed class OverrideRequestList
    {
        public List<OverrideRequest> Items { get; set; } = new List<OverrideRequest>();
    }

    internal static class OverrideRequestStore
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "override-requests.xml");
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(OverrideRequestList));
        private static readonly object SyncRoot = new object();

        public static IReadOnlyList<OverrideRequest> GetAll()
        {
            lock (SyncRoot)
            {
                return Load().Items
                    .OrderByDescending(x => x.RequestedAtUtc)
                    .ToList();
            }
        }

        public static void Add(OverrideRequest request)
        {
            lock (SyncRoot)
            {
                OverrideRequestList list = Load();
                if (string.IsNullOrWhiteSpace(request.RequestId))
                {
                    request.RequestId = Guid.NewGuid().ToString("N");
                }

                request.Status = "Pending";
                request.RequestedAtUtc = DateTime.UtcNow;
                list.Items.Add(request);
                Save(list);
            }
        }

        public static void SetStatus(string requestId, string status, string reviewedByEmployeeId)
        {
            lock (SyncRoot)
            {
                OverrideRequestList list = Load();
                OverrideRequest request = list.Items.FirstOrDefault(x => x.RequestId == requestId);
                if (request == null)
                {
                    return;
                }

                request.Status = status;
                request.ReviewedByEmployeeId = reviewedByEmployeeId;
                request.ReviewedAtUtc = DateTime.UtcNow;
                Save(list);
            }
        }

        private static OverrideRequestList Load()
        {
            if (!File.Exists(FilePath))
            {
                return new OverrideRequestList();
            }

            using (FileStream stream = File.OpenRead(FilePath))
            {
                return (OverrideRequestList)Serializer.Deserialize(stream);
            }
        }

        private static void Save(OverrideRequestList list)
        {
            using (FileStream stream = File.Create(FilePath))
            {
                Serializer.Serialize(stream, list);
            }
        }
    }
}
