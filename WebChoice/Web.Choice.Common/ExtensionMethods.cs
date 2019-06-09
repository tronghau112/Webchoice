namespace Web.Choice.Common
{
    public static class ExtensionMethods
    {
        public static bool IsEmpty(this object obj)
        {
            if (obj == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(obj.ToString());
        }
        public static bool IsEmpty(this string str)
        {
            if (str == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(str);
        }
    }
}
