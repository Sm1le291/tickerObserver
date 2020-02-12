using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace TickerObserver.Core
{
    public static class XElementExtensions
    {
        public static string GetSafeElementString(this XElement item, string elementName)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return GetSafeElementString(item, elementName, item.GetDefaultNamespace());
        }
        
        public static string GetSafeElementString(this XElement item, string elementName, XNamespace xNamespace)
        {
            if (item == null)
            {
                return string.Empty;
            }

            XElement value = item.Element(xNamespace + elementName);
            if (value != null)
            {
                return value.Value;
            }

            return string.Empty;
        }
        
        public static DateTime GetSafeElementDate(this XElement item, string elementName)
        {
            DateTime date;
            XElement element = item.Element(elementName);
            
            if (element == null)
            {
                return DateTime.Now;
            }

            if (TryParseDateTime(element.Value, out date))
            {
                return date;
            }

            return DateTime.Now;
        }
        
        private static bool TryParseDateTime(string s, out DateTime result)
        {
            if (DateTime.TryParse(s, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces, out result))
            {
                return true;
            }

            int tzIndex = s.LastIndexOf(" ");
            if (tzIndex >= 0)
            {
                string tz = s.Substring(tzIndex, s.Length - tzIndex);
                string offset = TimeZoneToOffset(tz);
                if (offset != null)
                {
                    string offsetDate = string.Format("{0} {1}", s.Substring(0, tzIndex), offset);
                    return TryParseDateTime(offsetDate, out result);
                }
            }

            result = default(DateTime);
            return false;
        }
        
        private static string TimeZoneToOffset(string tz)
        {
            if (tz == null)
            {
                return null;
            }

            tz = tz.ToUpper().Trim();

            if (timeZones.ContainsKey(tz))
            {
                return timeZones[tz].First();
            }

            return null;
        }
        
        /// <summary>
        /// Dictionary of timezones.
        /// </summary>
        private static Dictionary<string, string[]> timeZones = new Dictionary<string, string[]>
        {
            { "ACDT", new[] { "-1030", "Australian Central Daylight" } },
            { "ACST", new[] { "-0930", "Australian Central Standard" } },
            { "ADT", new[] { "+0300", "(US) Atlantic Daylight" } },
            { "AEDT", new[] { "-1100", "Australian East Daylight" } },
            { "AEST", new[] { "-1000", "Australian East Standard" } },
            { "AHDT", new[] { "+0900", string.Empty } },
            { "AHST", new[] { "+1000", string.Empty } },
            { "AST", new[] { "+0400", "(US) Atlantic Standard" } },
            { "AT", new[] { "+0200", "Azores" } },
            { "AWDT", new[] { "-0900", "Australian West Daylight" } },
            { "AWST", new[] { "-0800", "Australian West Standard" } },
            { "BAT", new[] { "-0300", "Bhagdad" } },
            { "BDST", new[] { "-0200", "British Double Summer" } },
            { "BET", new[] { "+1100", "Bering Standard" } },
            { "BST", new[] { "+0300", "Brazil Standard" } },
            { "BT", new[] { "-0300", "Baghdad" } },
            { "BZT2", new[] { "+0300", "Brazil Zone 2" } },
            { "CADT", new[] { "-1030", "Central Australian Daylight" } },
            { "CAST", new[] { "-0930", "Central Australian Standard" } },
            { "CAT", new[] { "+1000", "Central Alaska" } },
            { "CCT", new[] { "-0800", "China Coast" } },
            { "CDT", new[] { "+0500", "(US) Central Daylight" } },
            { "CED", new[] { "-0200", "Central European Daylight" } },
            { "CET", new[] { "-0100", "Central European" } },
            { "CST", new[] { "+0600", "(US) Central Standard" } },
            { "EAST", new[] { "-1000", "Eastern Australian Standard" } },
            { "EDT", new[] { "+0400", "(US) Eastern Daylight" } },
            { "EED", new[] { "-0300", "Eastern European Daylight" } },
            { "EET", new[] { "-0200", "Eastern Europe" } },
            { "EEST", new[] { "-0300", "Eastern Europe Summer" } },
            { "EST", new[] { "+0500", "(US) Eastern Standard" } },
            { "FST", new[] { "-0200", "French Summer" } },
            { "FWT", new[] { "-0100", "French Winter" } },
            { "GMT", new[] { "+0000", "Greenwich Mean" } },
            { "GST", new[] { "-1000", "Guam Standard" } },
            { "HDT", new[] { "+0900", "Hawaii Daylight" } },
            { "HST", new[] { "+1000", "Hawaii Standard" } },
            { "IDLE", new[] { "-1200", "Internation Date Line East" } },
            { "IDLW", new[] { "+1200", "Internation Date Line West" } },
            { "IST", new[] { "-0530", "Indian Standard" } },
            { "IT", new[] { "-0330", "Iran" } },
            { "JST", new[] { "-0900", "Japan Standard" } },
            { "JT", new[] { "-0700", "Java" } },
            { "MDT", new[] { "+0600", "(US) Mountain Daylight" } },
            { "MED", new[] { "-0200", "Middle European Daylight" } },
            { "MET", new[] { "-0100", "Middle European" } },
            { "MEST", new[] { "-0200", "Middle European Summer" } },
            { "MEWT", new[] { "-0100", "Middle European Winter" } },
            { "MST", new[] { "+0700", "(US) Mountain Standard" } },
            { "MT", new[] { "-0800", "Moluccas" } },
            { "NDT", new[] { "+0230", "Newfoundland Daylight" } },
            { "NFT", new[] { "+0330", "Newfoundland" } },
            { "NT", new[] { "+1100", "Nome" } },
            { "NST", new[] { "-0630", "North Sumatra" } },
            { "NZ", new[] { "-1100", "New Zealand " } },
            { "NZST", new[] { "-1200", "New Zealand Standard" } },
            { "NZDT", new[] { "-1300", "New Zealand Daylight" } },
            { "NZT", new[] { "-1200", "New Zealand" } },
            { "PDT", new[] { "+0700", "(US) Pacific Daylight" } },
            { "PST", new[] { "+0800", "(US) Pacific Standard" } },
            { "ROK", new[] { "-0900", "Republic of Korea" } },
            { "SAD", new[] { "-1000", "South Australia Daylight" } },
            { "SAST", new[] { "-0900", "South Australia Standard" } },
            { "SAT", new[] { "-0900", "South Australia Standard" } },
            { "SDT", new[] { "-1000", "South Australia Daylight" } },
            { "SST", new[] { "-0200", "Swedish Summer" } },
            { "SWT", new[] { "-0100", "Swedish Winter" } },
            { "USZ3", new[] { "-0400", "Volga Time (Russia)" } },
            { "USZ4", new[] { "-0500", "Ural Time (Russia)" } },
            { "USZ5", new[] { "-0600", "West-Siberian Time (Russia) " } },
            { "USZ6", new[] { "-0700", "Yenisei Time (Russia)" } },
            { "UT", new[] { "+0000", "Universal Coordinated" } },
            { "UTC", new[] { "+0000", "Universal Coordinated" } },
            { "UZ10", new[] { "-1100", "Okhotsk Time (Russia)" } },
            { "WAT", new[] { "+0100", "West Africa" } },
            { "WET", new[] { "+0000", "West European" } },
            { "WST", new[] { "-0800", "West Australian Standard" } },
            { "YDT", new[] { "+0800", "Yukon Daylight" } },
            { "YST", new[] { "+0900", "Yukon Standard" } }
        };
    }
}