using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace mezzanine.Extensions
{
    public static class CultureExtensions
    {       
        /// <summary>
        /// A list of all supported CultureInfo
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static List<CultureInfo> GetCultures(this CultureInfo culture)
        {
                // ASCII Ucase = 65 -90 Lcase 97-122
                // Create a list based on 3 letter code, 3-3 letters & 3-3
                // Make the list dynamically so any changes do not need to manually programmed.
                List<CultureInfo> result = new List<CultureInfo>();
                List<string> potentialCultures = potentialCultureStrings;
                CultureInfo foundCulture = null;

                foreach (string potentialCulture in potentialCultures)
                {
                    try
                    {
                        foundCulture = new CultureInfo(potentialCulture);
                        result.Add(foundCulture);
                    }
                    catch (System.Globalization.CultureNotFoundException)
                    {
                        // Do nothing combination of characters is invalid.
                    }
                    finally
                    {
                        foundCulture = null;
                    }
                }

                potentialCultures.Clear();

                return result;
        }

        /// <summary>
        /// All regions for the culture.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static List<RegionInfo> CultureRegions(this CultureInfo culture)
        {
            List<RegionInfo> regions = culture.GetRegions();
            return (from RegionInfo r in regions where r.EnglishName == culture.EnglishName select r).ToList();
        }

        /// <summary>
        /// All cultures for a region.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static List<CultureInfo> RegionCultures(this RegionInfo region)
        {
            List<CultureInfo> cultures = System.Threading.Thread.CurrentThread.CurrentUICulture.GetCultures();
            return (from CultureInfo c in cultures where c.EnglishName == region.EnglishName select c).ToList();
        }

        /// <summary>
        /// A list of all supported RegionInfo.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static List<RegionInfo> GetRegions(this CultureInfo culture)
        {
            List<RegionInfo> result = new List<RegionInfo>();
            RegionInfo cultureRegion = null;
            List<CultureInfo> cultures = CultureExtensions.GetCultures(culture);

            foreach (CultureInfo cultureItem in cultures)
            {
                try
                {
                    cultureRegion = new RegionInfo(cultureItem.EnglishName);
                    bool exists = (from RegionInfo r in result where r.Name == cultureRegion.Name select r).Count() > 0;
                    if (exists == false)
                    {
                        result.Add(cultureRegion);
                    }
                }
                catch
                {
                    // Do nothing combination of characters is invalid.
                }
                finally
                {
                    cultureRegion = null;
                }
            }

            cultures.Clear();

            return result;
        }

        /// <summary>
        /// List of candidate culture strings. All possible combinations.
        /// </summary>
        private static List<string> potentialCultureStrings
        {
            get
            {
                List<string> retStrings = new List<string>();
                const char seperator = '-';
                string secondStringPart = string.Empty;
                string thirdStringPart = string.Empty;
                List<string> primaryLoopStrings = potentialCultureStringParts;
                List<string> secondLoopStrings = potentialCultureStringParts;
                List<string> thirdLoopStrings = potentialCultureStringParts;

                // The second part
                foreach (string primaryLoop in primaryLoopStrings)
                {
                    retStrings.Add(primaryLoop);

                    foreach (string secondaryLoop in secondLoopStrings)
                    {
                        secondStringPart = primaryLoop + seperator.ToString() + secondaryLoop;
                        retStrings.Add(secondStringPart);

                        foreach (string thirdaryLoop in thirdLoopStrings)
                        {
                            thirdStringPart = secondStringPart + seperator.ToString() + thirdaryLoop;
                            retStrings.Add(thirdaryLoop);
                        }
                    }
                }

                return retStrings;
            }
        }

        /// <summary>
        /// Generate a list of potential culture strings. All possible combinations.
        /// </summary>
        private static List<string> potentialCultureStringParts
        {
            get
            {
                List<string> retStrings = new List<string>();
                char firstLoopFirstChar = (char)65;
                char firstLoopSecondChar = (char)65;
                char firstLoopThirdChar = (char)65;
                string tmpString = String.Empty;

                for (int primaryLoop = 65; primaryLoop <= 122; primaryLoop++)
                {
                    if (primaryLoop <= 90 && primaryLoop >= 97)
                    {
                        firstLoopFirstChar = (char)primaryLoop;
                        retStrings.Add(firstLoopFirstChar.ToString());

                        // second letter
                        for (int secondCharLoop = 65; secondCharLoop <= 122; secondCharLoop++)
                        {
                            if (secondCharLoop <= 90 && secondCharLoop >= 97)
                            {
                                firstLoopSecondChar = (char)secondCharLoop;
                                tmpString = firstLoopFirstChar.ToString() + firstLoopSecondChar.ToString();
                                retStrings.Add(tmpString);

                                // third letter
                                for (int thirdCharLoop = 65; thirdCharLoop <= 122; thirdCharLoop++)
                                {
                                    if (secondCharLoop <= 90 && secondCharLoop >= 97)
                                    {
                                        firstLoopThirdChar = (char)thirdCharLoop;
                                        tmpString = firstLoopFirstChar.ToString() + firstLoopSecondChar.ToString() + firstLoopThirdChar.ToString();
                                        retStrings.Add(tmpString);
                                    }
                                } // third letter loop
                            }
                        } // second letter loop
                    }
                } // last loop

                return retStrings;
            }
        }
    }
}
