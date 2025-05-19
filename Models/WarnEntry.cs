using System;
using Newtonsoft.Json;

namespace WarnSystem.Models
{
    public class WarnEntry
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string PlayerUserId { get; set; }
        public string PlayerNickname { get; set; }
        public string AdminUserId { get; set; }
        public string AdminNickname { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }

        public WarnEntry() { }

        public WarnEntry(int id, string type, string playerUserId, string playerNickname, string adminUserId, string adminNickname, string reason)
        {
            Id = id;
            Type = type ?? "Unknown";
            PlayerUserId = playerUserId ?? "Unknown@?";
            PlayerNickname = playerNickname ?? "Unknown Nick";
            AdminUserId = adminUserId ?? "Unknown@?";
            AdminNickname = adminNickname ?? "Unknown Admin";
            Reason = reason ?? string.Empty;
            Timestamp = DateTime.UtcNow;
        }
        public override string ToString()
        {
            string formattedTimestamp = Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            return $"ID {Type}: {Id}\n" +
                   $"Player Nickname: {PlayerNickname} ({PlayerUserId})\n" +
                   $"Admin Nickname: {AdminNickname} ({AdminUserId})\n" +
                   $"Reason/Content: {Reason}\n" +
                   $"Date: {formattedTimestamp} UTC\n" +
                   "--------------------";
        }
    }
}