// <copyright file="AssemblyInfo.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Reflection;

namespace duncans.Utility
{
    /// <summary>
    /// Provides information about the running assembly. eg: Publisher name and version.
    /// </summary>
    public class AssemblyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class.
        /// </summary>
        public AssemblyInfo()
        {
            Assembly runningAssembly = null;
            runningAssembly = Assembly.GetEntryAssembly();
            Get_AllVariables(runningAssembly);
            runningAssembly = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class. Supply the parameter with 'typeof(mything).Assembly'
        /// </summary>
        /// <param name="runningAssembly"></param>
        public AssemblyInfo(Assembly runningAssembly)
        {
            Get_AllVariables(runningAssembly);
            runningAssembly = null;
        }

        public CultureInfo Culture { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string Product { get; private set; }

        public string Copywrite { get; private set; }

        public string Trademark { get; private set; }

        public Version Version { get; private set; }

        public string Company { get; private set; }

        private void Get_AllVariables(Assembly a)
        {
            AssemblyTitleAttribute titleAttr = null;
            AssemblyDescriptionAttribute descAttr = null;
            AssemblyCopyrightAttribute copyAttr = null;
            AssemblyProductAttribute prodAttr = null;
            AssemblyTrademarkAttribute tradeAttr = null;
            AssemblyCultureAttribute cultureAttr = null;
            AssemblyVersionAttribute versionAttr = null;
            AssemblyCompanyAttribute companyAttr = null;
            AssemblyFileVersionAttribute fileVersionAttr = null;

            try
            {
                titleAttr = a.GetCustomAttribute<AssemblyTitleAttribute>() ?? new AssemblyTitleAttribute("No assembly title");
                descAttr = a.GetCustomAttribute<AssemblyDescriptionAttribute>() ?? new AssemblyDescriptionAttribute("No description");
                copyAttr = a.GetCustomAttribute<AssemblyCopyrightAttribute>() ?? new AssemblyCopyrightAttribute("No copyright");
                prodAttr = a.GetCustomAttribute<AssemblyProductAttribute>() ?? new AssemblyProductAttribute("No product information");
                tradeAttr = a.GetCustomAttribute<AssemblyTrademarkAttribute>() ?? new AssemblyTrademarkAttribute(string.Empty); // Trademark was in older .Net 3.5 apps but has not made its way into .Net Core
                cultureAttr = a.GetCustomAttribute<AssemblyCultureAttribute>() ?? new AssemblyCultureAttribute("en-US");
                versionAttr = a.GetCustomAttribute<AssemblyVersionAttribute>() ?? new AssemblyVersionAttribute("0.0.0.0");
                companyAttr = a.GetCustomAttribute<AssemblyCompanyAttribute>() ?? new AssemblyCompanyAttribute("No company specified");
                fileVersionAttr = a.GetCustomAttribute<AssemblyFileVersionAttribute>() ?? new AssemblyFileVersionAttribute("0.0.0.0");

                this.Title = titleAttr.Title;
                this.Description = descAttr.Description;
                this.Copywrite = copyAttr.Copyright;
                this.Product = prodAttr.Product;
                this.Trademark = tradeAttr.Trademark;
                this.Culture = new CultureInfo(cultureAttr.Culture);
                this.Version = new Version(versionAttr.Version.ToString() == "0.0.0.0" ? fileVersionAttr.Version : versionAttr.Version);
                this.Company = companyAttr.Company;
            }
            catch (Exception ex)
            {
                // Oh dear ...
                this.Title= "An error happened trying to get the application information.";
                this.Description = string.Format("{0}. {1}", ex.GetType().ToString(), ex.Message);
            }
            finally
            {
                a = null;
                descAttr = null;
                copyAttr = null;
                prodAttr = null;
                tradeAttr = null;
                cultureAttr = null;
                versionAttr = null;
                companyAttr = null;
                descAttr = null;
                titleAttr = null;
            }
        }
    }
 }
