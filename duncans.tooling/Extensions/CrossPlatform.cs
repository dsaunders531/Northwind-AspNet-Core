// <copyright file="CrossPlatform.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace duncans
{
    public static class CrossPlatform
    {
        /// <summary>
        /// Returns true when the operating system is Windows.
        /// </summary>
        /// <returns></returns>
        public static bool IsWinOS()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        /// Build a path which should work on different OS.
        /// </summary>
        /// <param name="pathParts"></param>
        /// <returns></returns>
        public static string BuildUniversalPath(string[] pathParts)
        {
            string result = string.Empty;

            char seperator = System.IO.Path.DirectorySeparatorChar;

            if (pathParts.Length > 0)
            {
                if (CrossPlatform.IsWinOS())
                {
                    if (!pathParts[0].Contains(":"))
                    {
                        throw new ArgumentException("Paths targeting windows require a drive.");
                    }

                    result = pathParts[0] + seperator.ToString();

                    for (int i = 1; i < pathParts.Length; i++)
                    {
                        result += pathParts[i];

                        if (i < pathParts.Length - 1)
                        {
                            result += seperator.ToString();
                        }
                    }
                }
                else
                {
                    // *nix paths
                    int startPos = 0;

                    if (pathParts[0].Contains(":"))
                    {
                        // a windows drive - to ignore
                        startPos = 1;
                    }

                    for (int i = startPos; i < pathParts.Length; i++)
                    {
                        result += pathParts[i];

                        if (i < pathParts.Length - 1)
                        {
                            result += seperator.ToString();
                        }
                    }
                }
            }

            return result;
        }
    }
}
