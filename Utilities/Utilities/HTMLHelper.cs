using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ista.Utilities
{
    /// <summary>
    /// This class format code to HTML.
    /// </summary>
    public class HTMLHelper
    { 
        /// <summary>
        /// private const string: html Header.
        /// </summary>
        private const string _htmlHeader = "<html><head>"
                + "<style type=\"text/css\">"
                + "body {font:15px arial,sans-serif;}"
                + "table {border:1px solid black;border-collapse:collapse;}"
                + "th {border:1px solid black;border-collapse:collapse;background-color:yellow;padding:3px;white-space:nowrap;}"
                + "td {border:1px solid black;border-collapse:collapse;padding:3px;white-space:nowrap;}"
                + ".error {color:red;font-weight:bold;}"
                + ".warning {color:orange;font-weight:bold;}"
                + ".success {color:green;font-weight:bold;}"
                + "</style>"
                + "</head><body>";
        /// <summary>
        /// private const string: html Footer
        /// </summary>
        private const string _htmlFooter = "</body></html>";
        /// <summary>
        /// private const string: html Line break
        /// </summary>
        private const string _htmlLinebreak = "<br />";
        /// <summary>
        /// Gets or sets the HTML header.
        /// </summary>
        public string HtmlHeader        { get; set; }
        /// <summary>
        /// Gets or sets the HTML Footer.
        /// </summary>
        public string HtmlFooter        { get; set; }
        /// <summary>
        /// Gets or sets the HTML Line break.
        /// </summary>
        public string HtmlLineBreak     { get; set; }
        /// <summary>
        /// Consturctor
        /// </summary>
        public HTMLHelper()
        {
            HtmlHeader    = _htmlHeader;
            HtmlFooter    = _htmlFooter;
            HtmlLineBreak = _htmlLinebreak;
        }
        /// <summary>
        /// Table header.
        /// </summary>
        /// <param name="headerList">List of column separated by a comma.</param>
        /// <returns>Html string that create the header of a table.</returns>
        public string TableHeader(string headerList)
        {
            string[] headers = headerList.Split(',');
            StringBuilder result = new StringBuilder();

            result.Append("<table><tr>");
            foreach (string header in headers)
            {
                result.Append("<th>" + header + "</th>");
            }
            result.Append("</tr>");
            return result.ToString();
        }
        /// <summary>
        /// Table header.
        /// </summary>
        /// <param name="columname">List of name of column.</param>
        /// <returns>Html string that create the header of a table.</returns>
        public string TableHeader(List<string> columname)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<table><tr>");
            foreach (string header in columname)
            {
                result.Append("<th>" + header + "</th>");
            }
            result.Append("</tr>");
            return result.ToString();
        }
        /// <summary>
        /// Table Data.
        /// </summary>
        /// <param name="dataline">List of data.</param>
        /// <returns>Html string that create the data line.</returns>
        public string TableData(List<List<string>> dataline)
        {
            StringBuilder result = new StringBuilder();
            
            foreach (List<string> line in dataline)
            {
                result.Append("<tr>");
                foreach (string data in line)
                {
                    result.Append("<td>" + data + "</td>");
                }
                result.Append("</tr>");
            }
            return result.ToString();
        }
        /// <summary>
        /// Table footer.
        /// </summary>
        /// <returns>Html footer</returns>
        public string TableFooter()
        {
            return "</table>";
        }
        /// <summary>
        /// Html text
        /// </summary>
        /// <param name="s">Html text</param>
        /// <returns>Html formatted text.</returns>
        public string HtmlText(string s)
        {
            string result = s;
            result = result.Replace("\n", HtmlLineBreak);
            if (!result.StartsWith("<html>"))
            {
                // Ensure HTML Formatting
                result = HtmlHeader + "<p>" + result + "</p>";
            }
            return result;
        }
        /// <summary>
        /// End of Html file.
        /// </summary>
        /// <param name="s">Html text</param>
        /// <returns>Html formatted text.</returns>
        public string HtmlEnd(string s)
        {
            string result = s;
            result = result.Replace("\n", HtmlLineBreak);
            if (!result.EndsWith("</html>"))
            {
                // Ensure HTML Formatting
                result = "<p>"+result+"</p>" + HtmlFooter;
            }
            return result;
        }
        /// <summary>
        /// TextToHTML
        /// </summary>
        /// <param name="s">html body.</param>
        /// <returns>return formatted html.</returns>
        public string TextToHTML(string s)
        {
            string result = s;
            result = result.Replace("\n", HtmlLineBreak);
            if (!result.StartsWith("<html>"))
            {
                // Ensure HTML Formatting
                result = HtmlHeader + result;
            }
            if (!result.EndsWith("</html>"))
            {
                // Ensure HTML Formatting
                result = result + HtmlFooter; ;
            }
            return result;
        }
    }
}
