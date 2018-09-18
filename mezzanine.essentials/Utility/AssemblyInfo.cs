using System;
using System.Globalization;
using System.Reflection;

namespace mezzanine.Utility
{
    /// <summary>
    /// Provides information about the running assembly. eg: Publisher name and version.
    /// </summary>
    public class AssemblyInfo
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _product = string.Empty;
        private string _copywrite = string.Empty;
        private string _trademark = string.Empty;
        private string _culture = string.Empty;
        private CultureInfo _appCultureInfo = null;
        private string _version = string.Empty;
        private Version _AppVersion = null;
        private string _company = string.Empty;

        /// <summary>
        /// Constructor for the AssemblyInfo class.
        /// </summary>
        public AssemblyInfo()
        {
            Assembly runningAssembly = null;
            runningAssembly = Assembly.GetEntryAssembly();
            Get_AllVariables(runningAssembly);
            runningAssembly = null;
        }

        /// <summary>
        /// Constructor for the AssemblyInfo class. Supply the parameter with 'typeof(mything).Assembly'
        /// </summary>
        /// <param name="runningAssembly"></param>
        public AssemblyInfo(Assembly runningAssembly)
        {
            Get_AllVariables(runningAssembly);
            runningAssembly = null;
        }

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

                this._title = titleAttr.Title;
                this._description = descAttr.Description;
                this._copywrite = copyAttr.Copyright;
                this._product = prodAttr.Product;
                this._trademark = tradeAttr.Trademark;
                this._culture = cultureAttr.Culture;
                this._appCultureInfo = new CultureInfo(this._culture);
                this._version = versionAttr.Version;
                this._AppVersion = new Version(this._version);
                this._company = companyAttr.Company; 
            }
            catch (Exception ex)
            {
                // Oh dear ...
                this._title = "An error happened trying to get the application information.";
                this._description = string.Format("{0}. {1}", ex.GetType().ToString(), ex.Message);
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

        public CultureInfo Culture
        {
            get
            {
                return this._appCultureInfo;
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
        }

        public string Product
        {
            get
            {
                return this._product;
            }
        }

        public string Copywrite
        {
            get
            {
                return this._copywrite;
            }
        }

        public string Trademark
        {
            get
            {
                return this._trademark;
            }
        }

        public Version Version
        {
            get
            {
                return this._AppVersion;
            }
        }

        public string Company
        {
            get
            {
                return this._company;
            }
        }
    }
 }
