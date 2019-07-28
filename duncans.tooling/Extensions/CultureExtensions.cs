// <copyright file="CultureExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace duncans
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
                return System.Globalization.CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        }

        /// <summary>
        /// All regions for the culture.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static RegionInfo CultureRegion(this CultureInfo culture)
        {
            if (culture.IsInvariantCulture() == true)
            {
                return new RegionInfo("EN-US");
            }
            else
            {
                return new RegionInfo(culture.Name);
            }
        }

        /// <summary>
        /// A list of all supported RegionInfo.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static List<RegionInfo> GetRegions(this CultureInfo culture)
        {
            List<RegionInfo> result = new List<RegionInfo>();
            List<CultureInfo> cultures = CultureExtensions.GetCultures(culture);

            foreach (CultureInfo cultureItem in cultures)
            {
                try
                {
                    if (cultureItem.IsNeutralCulture == false)
                    {
                        RegionInfo region = new RegionInfo(cultureItem.LCID);

                        bool exists = result.Where(r => r.Name == region.Name).Count() > 0;

                        if (exists == false)
                        {
                            result.Add(region);
                        }
                    }
                }
                catch
                {
                    // Do nothing combination of characters is invalid.
                }
            }

            cultures.Clear();

            return result;
        }

        /// <summary>
        /// Returns true if the culture is the invariant. (LCID = 127 or 0x7F)
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool IsInvariantCulture(this CultureInfo culture)
        {
            return culture.LCID == CultureInfo.InvariantCulture.LCID;
        }
    }
}
