using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcMusicStore.Models
{
    public static class CoverStyle
    {
        static readonly string[] Ignorar =
        {
            "of", "the", "a", "an", "and", "in", "to", "for",
            "de", "del", "la", "las", "el", "los", "y", "en", "un", "una", "al"
        };

        // Cada género recibe su propio color; uno nuevo se ve distinto automáticamente
        public static string Gradient(int seed)
        {
            int h = ((seed * 47 + 200) % 360 + 360) % 360;
            int h2 = (h + 40) % 360;
            return string.Format("linear-gradient(135deg, hsl({0},70%,48%), hsl({1},75%,28%))", h, h2);
        }

        // "Kind of Blue" => KB, "Led Zeppelin IV" => LZ, "Ni un centavo" => NC
        public static string Initials(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "♪";

            var words = text
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(w => char.IsLetterOrDigit(w[0]))
                .ToList();

            var significant = words
                .Where(w => !Ignorar.Contains(w.ToLowerInvariant()))
                .Take(2)
                .ToList();

            if (significant.Count == 0)
                significant = words.Take(2).ToList();

            var result = string.Concat(significant.Select(w => char.ToUpper(w[0])));
            return result.Length == 0 ? "♪" : result;
        }
    }
}