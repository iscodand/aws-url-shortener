using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UrlShortener.Models
{
    public class ShortUrl
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }

        public ShortUrl()
        {
            CreatedAt = DateTime.Now;
        }

        public static ShortUrl Create(string url)
        {
            return new()
            {
                Url = url,
                Code = GenerateCode(),
            };
        }

        private static string GenerateCode()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string base62 = ToBase62(timestamp);

            return base62.Length > 8
                ? base62.Substring(base62.Length - 8, 8)
                : base62.PadLeft(8, '0');
        }

        private static string ToBase62(long value)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new StringBuilder();

            while (value > 0)
            {
                int remainder = (int)(value % 62);
                sb.Insert(0, alphabet[remainder]);
                value /= 62;
            }

            return sb.ToString();
        }
    }
}