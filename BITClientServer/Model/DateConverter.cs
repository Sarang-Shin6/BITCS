namespace BITClientServer.Model
{
    static class DateConverter
    {
        /// <summary>
        /// Change DD/MM/YYYY to YYYY/MM/DD to add to the database.
        /// </summary>

        private static string dt;

        public static string ToSQL(string source)
        {
            if (source.Length == 10)
            {
                dt = source.Substring(6, 4) + "/" + source.Substring(3, 2) + "/" + source.Substring(0, 2);
            }
            else
            {
                dt = source.Substring(5, 4) + "/" + source.Substring(2, 2) + "/" + source.Substring(0, 1);
            }
            

            return dt;
        }

        public static string ToDDMMYYY(string source)
        {
            if (source.Length == 10)
            {
                dt = source.Substring(0, 2) + "/" + source.Substring(3, 2) + "/" + source.Substring(6, 4);
            }
            else
            {
                dt = source.Substring(0, 1) + "/" + source.Substring(2, 2) + "/" + source.Substring(5, 4);
            }


            return dt;
        }

    }
}
