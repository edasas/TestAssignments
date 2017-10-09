using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerTest
{
    public static class ValidationHelper
    {
        #region ----------------------------------- Public methods -------------------------------------------------

        public static int ValidateInt(string valueToCheck, Action<string> logger, int minValue, int maxValue)
        {
            int toReturn = -1;
            int currentValue = -1;
            try
            {
                if (System.Int32.TryParse(valueToCheck, out currentValue))
                {

                    if ((minValue > -1 && currentValue >= minValue) && (maxValue > -1 && currentValue <= maxValue))
                    {
                        toReturn = currentValue;
                    }
                }
            }
            catch (Exception ex)
            {
                logger(ex.Message);
            }

            return toReturn;

        }

        public static string ValidateDirectoryPath(string valueToCheck, Action<string> logger)
        {
            string toReturn = string.Empty;

            try
            {
                if (System.IO.Directory.Exists(valueToCheck))
                {
                    toReturn = valueToCheck;
                }
            }
            catch (Exception ex)
            {
                logger(ex.Message);
            }

            return toReturn;
        }

        #endregion
    }
}
