using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AtsenergoGrabber.Core
{
    public static class Helper
    {
        public static string GetQueryString(string html)
        {
            var query = string.Empty;

            Regex regex = new Regex(@"href=\""(\?fid=\w+)\""");
            MatchCollection matches = regex.Matches(html);
            if (matches.Count > 0)
            {
                query = matches[0].Groups[1].Value;
            }


            return query;
        }

        public static string GetUploadDir()
        {
            return "~/App_Data/Upload/";
        }
        public static string GetLocalFile(string date)
        {
            return GetUploadDir()  + date + "_eur_losses_regions.xls";
            //return "~/Tmp/" + date + "_eur_losses_regions.xls";
        }

        public static string GetUrl(string date)
        {
            return @"https://www.atsenergo.ru/nreport?rname=losses_regions&rdate=" + date;
        }

        public static string GetUrlFile(string queryString)
        {
            return @"https://www.atsenergo.ru/nreport" + queryString;
        }
    }
}