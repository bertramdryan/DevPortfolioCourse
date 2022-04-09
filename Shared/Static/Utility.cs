using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Static
{
    public static class UtilityFunctions
    {
        /// <summary>
        ///  Extension for "Object" that copies the properties to a destination object.
        /// </summary>
        /// <param name="source"> the source</param>
        /// <param name="destination"> The destination</param>
        public static void CopyObjectProperties(this object source, object destination)
        {
            // if any this is null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");

            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the properties of the source instance and
            // populate them from their destination counterparts
            PropertyInfo[] srcProps = typeSrc.GetProperties();

            foreach (PropertyInfo srcProp in srcProps)
            {
                if(!srcProp.CanRead)
                {
                    continue;
                }

                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);

                if (targetProperty == null)
                {
                    continue;
                }

                if (!targetProperty.CanWrite)
                {
                    continue;
                }

                if (targetProperty.GetSetMethod(true) != null || targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }

                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }

                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }

                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        public static string ConvertURLToTitle(this string str)
        {
            string stringWithHyphensReplacedBySpaces = str.Replace('-', ' ');
             if (ContainsSpaceThreeTimesInARow(stringWithHyphensReplacedBySpaces))
            {
                return stringWithHyphensReplacedBySpaces.Replace("   ", " - ");
            }
             else
            {
                return stringWithHyphensReplacedBySpaces;
            }
        }

        public static string ConvertTitleToURL(this string str) => str.Replace(" ", "-");

        private static bool ContainsSpaceThreeTimesInARow(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ' && str[i] == str[i - 1] && str[i] == str[i - 2])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
