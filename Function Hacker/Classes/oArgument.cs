using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionHacker.Classes
{
    public enum DISPLAY_TYPE
    {
        DOUBLE,
        FLOAT,
        LONG,
        UCHAR,
        USHORT,
        ULONG,
        ULONG_DATA,
        ULONG_ASCII,
        ULONG_ASCII_F,
        ULONG_UNICODE,
        ULONG_STRUCT
    }

    public enum DISPLAY_TYPE_NO_DEREF
    {
        DOUBLE,
        FLOAT,
        LONG,
        UCHAR,
        USHORT,
        ULONG
    }

    public class oArgument
    {
        private string weakName = "ebp-4";
        private string strongName = "";
        private string code = "push ";
        public DISPLAY_TYPE displayMethod = DISPLAY_TYPE.ULONG;
        private bool detectedType = false;

        public oArgument(string code)
        {
            this.code = code;
            this.weakName = "";
            this.displayMethod = DISPLAY_TYPE.ULONG;
        }

        public oArgument(string code, string weakName)
        {
            this.code = code;
            this.weakName = weakName;
            this.displayMethod = DISPLAY_TYPE.ULONG;
        }

        public oArgument(string code, string weakName, DISPLAY_TYPE displayMethod)
        {
            this.code = code;
            this.weakName = weakName;
            this.displayMethod = displayMethod;
        }

        public void setType(DISPLAY_TYPE displayMethod)
        {
            this.displayMethod = displayMethod;
        }

        public string getName()
        {
            if (strongName != null && strongName.Length > 0)
                return strongName;
            return weakName;
        }

        public string getValueString(uint data)
        {
            string result = "";

            // Build the data string depending on the data type
            bool dereferenceData = false;
            switch (displayMethod)
            {
                case DISPLAY_TYPE.UCHAR:
                    result = (data & 0xff).ToString("X");
                    break;
                case DISPLAY_TYPE.USHORT:
                    result = (data & 0xffff).ToString("X");
                    break;
                case DISPLAY_TYPE.DOUBLE:
                    result = oMemoryFunctions.IntToFloat(data).ToString();
                    break;
                case DISPLAY_TYPE.FLOAT:
                    result = oMemoryFunctions.IntToFloat(data).ToString();
                    break;
                case DISPLAY_TYPE.LONG:
                    result = ((int)data).ToString("X");
                    break;
                case DISPLAY_TYPE.ULONG:
                    result = data.ToString("X");
                    break;
                case DISPLAY_TYPE.ULONG_ASCII_F:
                case DISPLAY_TYPE.ULONG_STRUCT:
                case DISPLAY_TYPE.ULONG_UNICODE:
                case DISPLAY_TYPE.ULONG_ASCII:
                case DISPLAY_TYPE.ULONG_DATA:
                    dereferenceData = true;
                    result = data.ToString("X");
                    break;
                default:
                    result = "invalid DISPLAY_TYPE";
                    break;

            }

            return result;
        }

        /// <summary>
        /// Interprets the specified value and dereference according to the current interpretation method.
        /// </summary>
        /// <param name="data">The argument value.</param>
        /// <param name="dereference">The dereferenced data. This should be null if there are no dereferences.</param>
        /// <param name="autodetermine">If true, this funciton will change DISPLAY_TYPE depending on the dereference.</param>
        /// <returns></returns>
        public string getValueString(uint data, dereference dereference)
        {
            // Autodetect this type if required
            if (!detectedType)
            {
                // Detect the type of dereference
                byte[] derefData = dereference.data;
                if (derefData[0] >= 0x20 && derefData[0] <= 0x7E && derefData[1] == 0 && derefData[2] >= 0x20 && derefData[2] <= 0x7E && derefData[3] == 0)
                {
                    // Unicode with no offset
                    displayMethod = DISPLAY_TYPE.ULONG_UNICODE;
                }
                else if (derefData[0] >= 0x20 && derefData[0] <= 0x7E && derefData[1] >= 0x20 && derefData[1] <= 0x7E && derefData[2] >= 0x20 && derefData[2] <= 0x7E)
                {
                    // Ascii with no offset
                    displayMethod = DISPLAY_TYPE.ULONG_ASCII;
                }
                else
                {
                    // Binary dereference
                    displayMethod = DISPLAY_TYPE.ULONG_DATA;
                }
                detectedType = true;
            }

            string result = getValueString(data);

            if (displayMethod == DISPLAY_TYPE.ULONG_ASCII ||
                displayMethod == DISPLAY_TYPE.ULONG_ASCII_F ||
                displayMethod == DISPLAY_TYPE.ULONG_DATA ||
                displayMethod == DISPLAY_TYPE.ULONG_STRUCT ||
                displayMethod == DISPLAY_TYPE.ULONG_UNICODE)
            {
                // Setup the dereference
                result = result + "=";

                // Add the dereference string according to the type
                byte[] derefData = dereference.data;
                int i = 0;
                switch(displayMethod)
                {
                    case DISPLAY_TYPE.ULONG_UNICODE:
                        // Print until a null or invalid character
                        result = result + "\"";
                        i = 0;
                        while (i < derefData.Length && derefData[i] >= 32 && derefData[i] <= 126)
                        {
                            result = result + (char)derefData[i];
                            i += 2;
                        }
                        result = result + "\"";
                        break;
                    case DISPLAY_TYPE.ULONG_ASCII:
                        // Print until a null or invalid character
                        result = result + "'";
                        i = 0;
                        while (i < derefData.Length && derefData[i] >= 32 && derefData[i] <= 126)
                        {
                            result = result + (char)derefData[i];
                            i++;
                        }
                        result = result + "'";
                        break;
                    case DISPLAY_TYPE.ULONG_ASCII_F:
                        // Force the printing of the whole buffer as ascii
                        result = result + "'";
                        i = 0;
                        while (i < derefData.Length )
                        {
                            if( derefData[i] > 31 )
                            {
                                result = result + (char) derefData[i];
                            }else
                            {
                                result = result + '.';
                            }
                            i++;
                        }
                        result = result + "'";
                        break;

                    default:
                        // Interpret as a data pointer
                        result = result + "[";
                        i = 0;
                        while (i < derefData.Length )
                        {
                            string byteData = derefData[i].ToString("X");
                            while( byteData.Length < 2 )
                                byteData = "0" + byteData;
                            result = result + byteData + " ";
                            i += 1;
                        }
                        result = result.TrimEnd(new char[] {' '}) + "]";
                        break;

                }

            }

            return result;

        }

        /// <summary>
        /// Gets the code for preparing this argument
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string getCode(string value)
        {
            return this.code + value;
        }
        public string getCode(uint value)
        {
            return this.code + oMemoryFunctions.IntToDwordString(value);
        }

        public void setStrongName(string strongName)
        {
            this.strongName = strongName;
        }

        public string getStrongName()
        {
            return strongName;
        }
    }
}
