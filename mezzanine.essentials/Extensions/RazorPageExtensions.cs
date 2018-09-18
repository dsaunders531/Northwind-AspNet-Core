using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Text;

namespace mezzanine.Extensions
{
    public static class RazorPageExtensions
    {
        /// <summary>
        /// Write random lorem ipsum text to generate test content.
        /// credit : kEnobus https://stackoverflow.com/questions/4286487/is-there-any-lorem-ipsum-generator-in-c
        /// </summary>
        /// <param name="minWords"></param>
        /// <param name="maxWords"></param>
        /// <param name="minSentences"></param>
        /// <param name="maxSentences"></param>
        /// <param name="numLines"></param>
        /// <returns></returns>
        public static string LoremIpsum(this RazorPage page, int minWords, int maxWords, int minSentences, int maxSentences, int numLines)
        {
            var words = new[] { "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat" };

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences;
            int numWords = rand.Next(maxWords - minWords) + minWords;

            var sb = new StringBuilder();
            for (int p = 0; p < numLines; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { sb.Append(" "); }
                        string word = words[rand.Next(words.Length)];
                        if (w == 0) { word = word.Substring(0, 1).Trim().ToUpper() + word.Substring(1); }
                        sb.Append(word);
                    }
                    sb.Append(". ");
                }
                if (p < numLines - 1) sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
