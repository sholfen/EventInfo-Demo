using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventInfoDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnTestButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked!");
        }

        private static void PrintEventMethods(Control control, string eventname)
        {
            if (control == null) return;
            if (string.IsNullOrEmpty(eventname)) return;

            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;
            Type controlType = typeof(System.Windows.Forms.Control);
            PropertyInfo propertyInfo = controlType.GetProperty("Events", mPropertyFlags);
            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(control, null);
            FieldInfo fieldInfo = (typeof(Control)).GetField("Event" + eventname, mFieldFlags);

            Delegate d = null;
            if (fieldInfo != null)
            {
                d = eventHandlerList[fieldInfo.GetValue(control)];
            }

            if (d == null)
            {
                return;
            }
            EventInfo eventInfo = controlType.GetEvent(eventname);

            foreach (Delegate dx in d.GetInvocationList())
            {
                MethodInfo method = dx.Method;

                Console.WriteLine("Method Name: " + method.Name);
                Console.WriteLine("Method Source Type: " + method.DeclaringType.FullName);
                Console.WriteLine();
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TestClass testObj = new TestClass();
            this.btnTestButton.Click += testObj.btnTestButton_Click;

            Type controlType = typeof(Control);
            foreach (Control control in this.Controls)
            {
                Type type = control.GetType();
                Console.WriteLine("Control Type: " + type.FullName);
                Console.WriteLine("Control Name: " + control.Name);

                foreach (EventInfo eventInfo in type.GetEvents())
                {
                    PrintEventMethods(control, eventInfo.Name);
                }

                Console.WriteLine();
            }
        }
    }
}
