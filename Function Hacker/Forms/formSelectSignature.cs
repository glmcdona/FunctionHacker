using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FunctionHacker.Classes;

namespace FunctionHacker.Forms
{
    public partial class formSignatures : Form
    {
        private readonly SortedList<string, int> index = new SortedList<string, int>();
        private string[] lines;
        public formSignatures()
        {
            InitializeComponent();
        }

        public void TryToApplySignature(DataGridViewSelectedRowCollection selectedRows, oFunctionList functionList)
        {
            DialogResult showDialog = ShowDialog();
            if (showDialog == DialogResult.OK)
            {
                foreach (ListViewItem item in listSignatures.Items)
                {
                    if (item.Checked)
                    {
                        EntpackSignature(item.SubItems[1].Text);
                        if (!BuildIndex()) return;
                        foreach (DataGridViewRow selectedRow in selectedRows)
                        {
                            oFunction function = functionList.getFunction(selectedRow.Index);
                            string functionName = GetFunctionName(function.getSignature());
                            if (functionName != string.Empty)
                                function.name = functionName;
                        }
                    }
                }
            }
        }

        private void EntpackSignature(string SignatureFileName)
        {
            
            string dumpSigPath = Properties.Settings.Default.DumpSigPath;
            if (File.Exists(dumpSigPath))
            {
                string tempPath = System.IO.Path.GetTempPath();
                string arguments = SignatureFileName + " " + tempPath + @"\sigdump.txt";
                Process p = new Process();
                p.StartInfo.FileName = dumpSigPath;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            }
        }

        private string GetFunctionName(string signature)
        {
            string key = signature.Substring(0, 2);
            if (index.ContainsKey(key))
            {
                int startLine = index[key];
                int endLine = index.IndexOfKey(key) <= (index.Count - 2) ? index.ElementAt(index.IndexOfKey(key) + 1).Value : lines.Length;
                return FindName(signature, startLine, endLine, lines);
            }
            return string.Empty;
        }

        private bool BuildIndex()
        {
            index.Clear();
            string fileName = System.IO.Path.GetTempPath() + @"\sigdump.txt";
            if (!File.Exists(fileName))
            {
                Console.WriteLine(@"{0} does not exist.", fileName);
                return false;
            }
            lines = File.ReadAllLines(fileName);
            int lineNumber = 0;
            int maxLength = 0;
            foreach (string line in lines)
            {
                string substring = line.Substring(0, 2);
                if (substring != "  ")
                    index.Add(substring, lineNumber);
                if (line[line.Length - 1] == ':')
                    if (line.Length > maxLength)
                        maxLength = line.Length;
                lineNumber++;
            }
            return true;
        }

        private string FindName(string signature, int startLine, int endLine, string[] lines)
        {
            int pos = 0;
            int offset = 0;
            string localSearch = signature.Substring(pos);
            string foundedName = string.Empty;
            for (int i = startLine; i < endLine; i++)
            {
                string line = lines[i];
                if (offset < line.Length && line[offset] != ' ')
                {
                    if (line.EndsWith(":"))
                    {
                        line = line.TrimEnd(':').Trim();
                        if (StartsWithSpecial(localSearch, line, pos))
                        {
                            offset += 2;
                            pos += line.Length;
                            foundedName = lines[i + 1];
                            if (foundedName.Contains("0."))
                                break;
                        }
                    }
                }
            }
            string name = foundedName.Substring(foundedName.IndexOf(':') + 1);
            if (name.StartsWith("?"))
                return string.Empty;
            return name;
        }

        private bool StartsWithSpecial(string word, string mask, int pos)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                if ((mask[i] != word[i + pos]) && (mask[i] != '.'))
                {
                    return false;
                }
            }
            return true;
        }

        private void formSelectSignature_Load(object sender, EventArgs e)
        {
            FindSignatures();
        }

        private void FindSignatures()
        {
            listSignatures.Items.Clear();
            string signaturePath = Properties.Settings.Default.SignaturePath;
            if (Directory.Exists(signaturePath))
            {
                foreach (string file in Directory.GetFiles(signaturePath))
                {
                    AddSignatureInfo(file);
                }
            }
        }

        private void AddSignatureInfo(string file)
        {
            byte[] buffer;
            //FileStream fileStream = new FileStream(@"r:\b32vcl.sig", FileMode.Open, FileAccess.Read);
            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            try
            {
                var length = (int)fileStream.Length; // get file length
                buffer = new byte[512]; // create buffer
                int count; // actual number of bytes read
                int sum = 0; // total number of bytes read
                count = fileStream.Read(buffer, sum, 512);
                if (count > 0)
                {
                    string s = Encoding.ASCII.GetString(buffer, 0, 6);
                    if (s == "IDASGN")
                    {
                        int entries;

                        if (buffer[6] <= 5)
                        {
                        }
                        else
                        {
                            int functions = BitConverter.ToInt32(buffer, 0x25);
                            int fileType = buffer[0x9] << 8 + buffer[0x8];
                            uint nameLength = buffer[0x22];
                            string name = string.Empty;
                            for (int j = 0; j < nameLength; j++)
                            {
                                name += (char)buffer[0x29 + j];
                            }
                            //Console.WriteLine(name);
                            if (checkBoxOnlyWindows.Checked && (fileType & 0x0800) != 0x800)
                                return;
                            listSignatures.Items.Add(
                                new ListViewItem(new[] { name, file, functions.ToString()}));

                        }
                    }
                }
            }
            finally
            {
                fileStream.Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBoxOnlyWindows_CheckedChanged(object sender, EventArgs e)
        {
            FindSignatures();    
        }

    }
}
