using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private HashSet<string> properties = new HashSet<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string className = textBox1.Text.Trim();

            string pathToDLL = textBox2.Text.Trim();

            Assembly assembly = Assembly.LoadFile(pathToDLL);

            string nameSpace = assembly.GetName().Name;

            Type aType = assembly.GetType($"{nameSpace}.{className}");

            var sb = new StringBuilder();

            foreach (var property in aType.GetProperties())
            {
                var name = property.Name;
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                    var type = GetCSharpType(propertyType.FullName);
                    if (type == "object") continue;
                    sb.AppendLine($"public {type}? {name}" + " { get; set; }");
                    properties.Add(name);
                }
                else
                {
                    var type = GetCSharpType(propertyType.FullName);
                    if (type == "object") continue;
                    sb.AppendLine($"public {type} {name}" + " { get; set; }");
                    properties.Add(name);
                }
            }

            richTextBox1.Text = sb.ToString();

            Clipboard.SetText(sb.ToString());

            MessageBox.Show("OK");

            Cursor.Current = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            var arr = (from x in richTextBox2.Text.Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) select x.Trim()).ToList();

            var sb = new StringBuilder();
            string className = textBox1.Text.Trim();
            string classNameDTO = textBox1.Text.Trim() + "DTO";
            sb.AppendLine($"public class {classNameDTO}");
            sb.AppendLine("{");

            sb.AppendLine(richTextBox1.Text);

            sb.AppendLine($"public {classNameDTO}()");
            sb.AppendLine("{");
            sb.AppendLine("}");

            sb.AppendLine($"public {classNameDTO}({className} obj{className})");
            sb.AppendLine("{");

            var list = properties.ToList<string>();

            if (checkBox1.Checked)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sb.AppendLine($"{list[i]} = obj{className}.{list[i]};");
                }
            }
            else
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    sb.AppendLine($"{arr[i]} = obj{className}.{arr[i]};");
                }
            }

            sb.AppendLine("}");

            sb.AppendLine("}");

            Clipboard.SetText(sb.ToString());

            MessageBox.Show("OK");

            Cursor.Current = Cursors.Default;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string pathToDLL = textBox2.Text.Trim();

            Assembly mscorlib = Assembly.LoadFile(pathToDLL);

            var lst = new List<string>();

            var sb = new StringBuilder();

            foreach (Type type in mscorlib.GetTypes())
            {
                lst.Add(type.FullName);
                sb.AppendLine(type.FullName);
            }

            richTextBox1.Text = sb.ToString();

            Clipboard.SetText(sb.ToString());

            MessageBox.Show("OK");

            Cursor.Current = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string pathToDLL = textBox2.Text.Trim();

            Assembly mscorlib = Assembly.LoadFile(pathToDLL);

            var sb = new StringBuilder();

            foreach (Type aType in mscorlib.GetTypes())
            {
                sb.AppendLine();

                string className = aType.Name;

                sb.AppendLine($"public class {className}");
                sb.AppendLine("{");

                foreach (var property in aType.GetProperties())
                {
                    var name = property.Name;
                    var propertyType = property.PropertyType;
                    if (propertyType.IsGenericType &&
                        propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                        var type = GetCSharpType(propertyType.FullName);
                        if (type == "object") continue;
                        sb.AppendLine($"    public {type}? {name}" + " { get; set; }");
                        properties.Add(name);
                    }
                    else
                    {
                        var type = GetCSharpType(propertyType.FullName);
                        if (type == "object") continue;
                        sb.AppendLine($"    public {type} {name}" + " { get; set; }");
                        properties.Add(name);
                    }
                }

                sb.AppendLine($"    public {className}()");
                sb.AppendLine("    {");
                sb.AppendLine("    }");

                sb.AppendLine("}");
            }

            richTextBox1.Text = sb.ToString();

            Clipboard.SetText(sb.ToString());

            MessageBox.Show("OK");

            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(-1);
        }

        private string GetCSharpType(string fullName)
        {
            string type = "object";

            switch (fullName)
            {
                case "System.Object":
                    type = "object";
                    break;

                case "System.String":
                    type = "string";
                    break;

                case "System.Boolean":
                    type = "bool";
                    break;

                case "System.Byte":
                    type = "byte";
                    break;

                case "System.SByte":
                    type = "sbyte";
                    break;

                case "System.Int16":
                    type = "short";
                    break;

                case "System.UInt16":
                    type = "ushort";
                    break;

                case "System.Int32":
                    type = "int";
                    break;

                case "System.UInt32":
                    type = "uint";
                    break;

                case "System.Int64":
                    type = "long";
                    break;

                case "System.UInt64":
                    type = "ulong";
                    break;

                case "System.Single":
                    type = "float";
                    break;

                case "System.Double":
                    type = "double";
                    break;

                case "System.Decimal":
                    type = "decimal";
                    break;

                case "System.Char":
                    type = "char";
                    break;

                case "System.DateTime":
                    type = "DateTime";
                    break;

                default:
                    break;
            }

            return type;
        }
    }
}