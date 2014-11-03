using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using BufferOverflowProtection;

namespace FunctionHacker.Classes
{

    static public class oSymbolTable
    {
        private static Hashtable md5Lookup;
        private static int md5_length = 50;


        public static void loadSymbolTableFromFile()
        {
            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream("symbols.dat", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                md5Lookup = (Hashtable)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                oConsole.printMessageShow("Failed to decode symbols database. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }

        }

        public static void saveSymbolTableToFile()
        {
            // Open the symbol table to write
            FileStream fs = new FileStream("symbols.dat", FileMode.OpenOrCreate, FileAccess.Write);

            try
            {
                // Serialize the hashtable to a file
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, md5Lookup);
            }
            finally
            {
                fs.Close();
            }
        }

        private static UInt32 generateTrimmedMD5(byte[] md5)
        {
            // We group it into 4-byte groupings, then xor them.
            UInt32[] data = oMemoryFunctions.ByteArrayToUintArray(ref md5);

            // xor coding
            UInt32 result = 0;
            foreach (UInt32 u in data)
            {
                result = result ^ u;
            }

            return result;
        }

        public static string lookupSymbol(byte[] md5)
        {
            // Lookup the specified md5
            UInt32 trimmedMd5 = generateTrimmedMD5(md5);
            if (md5Lookup.ContainsKey(trimmedMd5))
            {
                return (string)md5Lookup[trimmedMd5];
            }
            return "";
        }

        public static string lookupSymbol(byte[] data, int offset)
        {
            // First create the MD5 object
            MD5 md5 = MD5.Create();

            // Add the symbol with known MD5
            return lookupSymbol(md5.ComputeHash(data, offset, md5_length));
        }

        public static string lookupSymbol(Process process, uint address)
        {
            // Read in the memory
            byte[] data = oMemoryFunctions.ReadMemory(process, address, (uint) md5_length);

            // Create the MD5 object
            MD5 md5 = MD5.Create();

            // Lookup the symbol
            return lookupSymbol(md5.ComputeHash(data));
        }

        public static void addSymbol(string name, byte[] md5)
        {
            // See if this symbol already exists
            UInt32 trimmedMd5 = generateTrimmedMD5(md5);
            if (md5Lookup.ContainsKey(trimmedMd5))
            {
                // Symbol already exists. But lets overwrite it anways?
                md5Lookup[trimmedMd5] = name;
            }else
            {
                // Add this symbol lookup
                md5Lookup.Add(trimmedMd5, name);
            }
        }

        public static void addSymbol(string name, byte[] data, int offset)
        {
            // First create the MD5 object
            MD5 md5 = MD5.Create();

            // Add the symbol with known MD5
            addSymbol(name, md5.ComputeHash(data, offset, md5_length));
        }

        public static void addSymbol(string name, Process process, uint address)
        {
            // Read in the memory
            byte[] data = oMemoryFunctions.ReadMemory(process, address, (uint) md5_length);

            // Create the MD5 object
            MD5 md5 = MD5.Create();
            
            // Add the symbol with known MD5
            addSymbol(name, md5.ComputeHash(data));
        }
    }
}
