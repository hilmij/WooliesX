namespace WooliesXFunctionApp.Helpers
{
    using System.Collections.Generic;

    public static class ListExtensions
    {
        public static void InitSpecialCounts(this List<int> list, int number, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                list.Add(number);
            }
        }
    }
}
