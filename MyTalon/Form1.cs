using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MyTalon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class ListBoxItem
        {
            public string Text { get; set; }
            public List Tag { get; set; }


            public override string ToString()
            {
                return Text;
            }
        }

        class ListBoxItem2
        {
            public string Text { get; set; }
            public List2 Tag { get; set; }


            public override string ToString()
            {
                return Text;
            }
        }

        public class List
        {
            public string id { get; set; }
            public string name { get; set; }
            public object fullName { get; set; }
            public object address { get; set; }
            public object regPhone { get; set; }
            public object note { get; set; }
            public object hint { get; set; }
            public string className { get; set; }
            public object children { get; set; }
        }

        public class Raion
        {
            public List<List> list { get; set; }
            public int count { get; set; }
            public int size { get; set; }
            public int page { get; set; }
        }


        public class Children
        {
            public List<List> list { get; set; }
            public int count { get; set; }
            public int size { get; set; }
            public int page { get; set; }
        }

        public class Bolnitsa
        {
            public string id { get; set; }
            public string name { get; set; }
            public object fullName { get; set; }
            public object address { get; set; }
            public object regPhone { get; set; }
            public object note { get; set; }
            public object hint { get; set; }
            public string className { get; set; }
            public Children children { get; set; }
        }


        public class List2
        {
            public int id { get; set; }
            public string fullName { get; set; }
            public object room { get; set; }
            public object hint { get; set; }
            public object note { get; set; }
            public string className { get; set; }
            public int serviceId { get; set; }
            public object planning { get; set; }
            public List<object> blocks { get; set; }
            public bool needReferral { get; set; }
            public object fio { get; set; }
            public object speciality { get; set; }
            public object districts { get; set; }
            public string formtattedDistricts { get; set; }
        }

        public class Resources
        {
            public List<List2> list { get; set; }
            public int count { get; set; }
            public int size { get; set; }
            public int page { get; set; }
        }

        public class RootObject
        {
            public int id { get; set; }
            public string name { get; set; }
            public string fullName { get; set; }
            public string note { get; set; }
            public object hint { get; set; }
            public int clinicId { get; set; }
            public object departmentId { get; set; }
            public Resources resources { get; set; }
            public string className { get; set; }
            public object districtAttachment { get; set; }
        }


        public class Interval
        {
            public int id { get; set; }
            public bool disabled { get; set; }
            public object disabilityReason { get; set; }
            public bool free { get; set; }
            public object start { get; set; }
            public object finish { get; set; }
            public object hint { get; set; }
            public string className { get; set; }
            public string formattedDate { get; set; }
        }

        public class Planning
        {
            public int date { get; set; }
            public int month { get; set; }
            public int year { get; set; }
            public bool disabled { get; set; }
            public bool blocked { get; set; }
            public string disabilityReason { get; set; }
            public object hint { get; set; }
            public int intervalCount { get; set; }
            public int? freeIntervalCount { get; set; }
            public object jobPeriodStart { get; set; }
            public object jobPeriodFinish { get; set; }
            public string className { get; set; }
            public IList<Interval> intervals { get; set; }
            public object finish { get; set; }
            public string formattedStart { get; set; }
            public string formattedFinish { get; set; }
            public object start { get; set; }
        }

        public class Example
        {
            public int id { get; set; }
            public string fullName { get; set; }
            public object room { get; set; }
            public object hint { get; set; }
            public object note { get; set; }
            public string className { get; set; }
            public int serviceId { get; set; }
            public IList<Planning> planning { get; set; }
            public IList<object> blocks { get; set; }
            public bool needReferral { get; set; }
            public object fio { get; set; }
            public object speciality { get; set; }
            public object districts { get; set; }
            public string formtattedDistricts { get; set; }
        }

        Encoding stan = Encoding.UTF8;
        public string url = "https://rmis52.cdmarf.ru/pp/clinics?&size=12&salt=1463820537924&page=";

        public string url2 = "https://rmis52.cdmarf.ru/pp/group/clinicGroup_0?size=12&salt=1463834576217&page=";
        DateTime date = new DateTime();
        string file_setting = Application.StartupPath + "\\data.txt";

        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        
        bool done = false;


        int total_day = 0;
        int total_one_vr = 0;

        void refresh_list()
        {
            linkLabel1.Text = "";
            if (File.Exists(file_setting))
            {
                listView1.Items.Clear();
                string[] massiveOfString = System.IO.File.ReadAllLines(file_setting, stan);
                foreach (string g in massiveOfString)
                {
                    if (g != "")
                    {
                        string[] words = g.Split(';');
                        string[] sm = new string[2];
                        sm[0] = words[1];
                        sm[1] = words[0];
                        ListViewItem lw = new ListViewItem(sm);
                        if (words[3] != "0") lw.BackColor = Color.Aqua;
                        lw.Tag = words[2];
                        listView1.Items.Add(lw);
                    }
                }


            }
        }

        void delete_item(string id)
        {
            if (File.Exists(file_setting))
            {
                string old_value = "";
                string[] massiveOfString = System.IO.File.ReadAllLines(file_setting);
                foreach (string g in massiveOfString)
                {
                    string[] words = g.Split(';');
                    if (words[0] != id) old_value += g + Environment.NewLine;
                }

                File.Delete(file_setting);
                System.IO.File.AppendAllText(file_setting, old_value, stan);
            }
        }

        void set_item(string id, string val)
        {
            if (File.Exists(file_setting))
            {
                string old_value = "";
                string[] massiveOfString = System.IO.File.ReadAllLines(file_setting);
                foreach (string g in massiveOfString)
                {
                    string[] words = g.Split(';');
                    if (words[0] == id)
                    {
                        old_value += words[0] + ";" + words[1] + ";" + words[2] + ";" + val + Environment.NewLine;
                    }
                    else
                    {
                        old_value += words[0] + ";" + words[1] + ";" + words[2] + ";" + words[3] + Environment.NewLine;
                    }
                }

                File.Delete(file_setting);
                System.IO.File.AppendAllText(file_setting, old_value, stan);
            }
        }

        string get_count(string id)
        {
            string res = "";
            if (File.Exists(file_setting))
            {

                string[] massiveOfString = System.IO.File.ReadAllLines(file_setting);
                foreach (string g in massiveOfString)
                {
                    string[] words = g.Split(';');
                    if (words[0] == id) return words[3];

                }
            }
            return res;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refresh_list();

        }

        public byte[] download(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "GET";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Win64; x64; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; .NET4.0C; .NET4.0E)";
            req.ContentType = "application/json;charset=UTF-8";
            string source = "";
            //linkLabel1.Text = url;
            try
            {
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    source = reader.ReadToEnd();
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
            return stan.GetBytes(source);

        }

        void add_listbox(ListBox listbox)
        {
            linkLabel1.Text = "";
            int total_elem = 0;
            int tek_elem = 0;
            int tek_page = 1;
            byte[] b = download(url + tek_page);

            Raion person_tmp = JsonConvert.DeserializeObject<Raion>(stan.GetString(b));

            if (person_tmp != null)
            {
                total_elem = person_tmp.count;
                while (tek_elem < total_elem)
                {
                    foreach (List l in person_tmp.list)
                    {
                        ListBoxItem q = new ListBoxItem();
                        q.Text = l.name;
                        q.Tag = l;

                        listbox.Items.Add(q);
                        tek_elem++;
                        Application.DoEvents();
                    }
                    tek_page++;
                    b = download(url + tek_page);
                    person_tmp = JsonConvert.DeserializeObject<Raion>(stan.GetString(b));
                }
            }
        }

        void add_listbox(ListBox sel_listbox, ListBox listbox)
        {
            linkLabel1.Text = "";
            ListBoxItem l = (ListBoxItem)sel_listbox.SelectedItem;

            if (l != null)
            {
                int total_elem = 0;
                int tek_elem = 0;
                int tek_page = 1;
                string new_url = "";
                new_url = url2.Replace("clinicGroup_0", l.Tag.id);


                string data = stan.GetString(download(new_url + tek_page));
                if (data != "")
                {
                    Bolnitsa person_tmp = JsonConvert.DeserializeObject<Bolnitsa>(data);
                    total_elem = person_tmp.children.count;
                    while (tek_elem < total_elem)
                    {
                        foreach (var li in person_tmp.children.list)
                        {
                            ListBoxItem q = new ListBoxItem();
                            q.Text = li.name;
                            q.Tag = li;

                            listbox.Items.Add(q);
                            tek_elem++;
                            Application.DoEvents();
                        }
                        if (tek_elem < total_elem)
                        {
                            tek_page++;
                            data = stan.GetString(download(new_url + tek_page));

                            person_tmp = JsonConvert.DeserializeObject<Bolnitsa>(data);
                        }
                    }
                }

            }
        }

        void add_listbox_cat(ListBox sel_listbox, ListBox listbox)
        {
            linkLabel1.Text = "";
            ListBoxItem l = (ListBoxItem)sel_listbox.SelectedItem;

            if (l != null)
            {
                int total_elem = 0;
                int tek_elem = 0;
                int tek_page = 1;
                string new_url = "";

                new_url = url2.Replace("clinicGroup_0", l.Tag.id);

                string data = stan.GetString(download(new_url + tek_page));


                RootObject person_tmp = JsonConvert.DeserializeObject<RootObject>(data);
                total_elem = person_tmp.resources.list.Count;

                while (tek_elem < total_elem)
                {
                    foreach (var li in person_tmp.resources.list)
                    {
                        ListBoxItem2 q = new ListBoxItem2();
                        q.Text = li.fullName;
                        q.Tag = li;

                        listbox.Items.Add(q);

                        tek_elem++;
                        Application.DoEvents();
                    }
                    if (tek_elem < total_elem)
                    {
                        tek_page++;
                        data = stan.GetString(download(new_url + tek_page));
                        person_tmp = JsonConvert.DeserializeObject<RootObject>(data);
                    }
                }

            }
        }

        void add_text(ListBoxItem2 lq, ListBoxItem l)
        {

            string new_url = "https://rmis52.cdmarf.ru/pp/group/" + l.Tag.id + "/resource/" + lq.Tag.id + "/planning/" + date.Year + "/" + date.Month + "?_salt=1463923646808";
            string data = stan.GetString(download(new_url));


            string label = "https://rmis52.cdmarf.ru/pp/#!/group/" + l.Tag.id + "/resource/" + lq.Tag.id + "/planning/" + date.Year + "/" + date.Month + "/!/";
            linkLabel1.Text = label;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            listBox1.Items.Clear();
            add_listbox(listBox1);
            button1.Enabled = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            add_listbox(listBox1, listBox2);

        }


        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();

            add_listbox(listBox2, listBox3);

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            listBox4.Items.Clear();
            listBox5.Items.Clear();

            ListBoxItem l = (ListBoxItem)listBox3.SelectedItem;
            if (l != null)
            {
                if (l.Tag.id.Contains("department"))
                {
                    listBox5.Enabled = true;
                    add_listbox(listBox3, listBox4);

                }
                else
                {
                    listBox5.Enabled = false;
                    add_listbox_cat(listBox3, listBox4);
                }
            }



        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
            ListBoxItem lq = (ListBoxItem)listBox3.SelectedItem;

            if (lq != null)
            {
                if (!lq.Tag.id.Contains("department"))
                {
                    ListBoxItem2 l = (ListBoxItem2)listBox4.SelectedItem;
                    add_text(l, lq);

                }
                else
                {
                    add_listbox_cat(listBox4, listBox5);
                }
            }


        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxItem2 lq = (ListBoxItem2)listBox5.SelectedItem;
            ListBoxItem l = (ListBoxItem)listBox4.SelectedItem;

            if (lq != null && l != null)
            {
                add_text(lq, l);

            }

        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            if (linkLabel1.Text.Contains("https://rmis52.cdmarf.ru"))
            {

                System.Diagnostics.Process.Start(linkLabel1.Text);
            }
        }

        private void listBox4_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox listbox = (ListBox)sender;

            Type tip = listbox.SelectedItem.GetType();

            if (tip.Name == "ListBoxItem2")
            {
                ListBoxItem2 hj = (ListBoxItem2)listbox.SelectedItem;
                int indexOfItem = listbox.IndexFromPoint(e.X, e.Y);


                if (e.Button == MouseButtons.Left)
                {
                    if (indexOfItem >= 0 && indexOfItem < listbox.Items.Count)// check we clicked down on a string
                    {

                        listbox.DoDragDrop(listbox.Items[indexOfItem], DragDropEffects.Copy);

                    }
                }
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == DragDropEffects.Copy)
            {
                e.Effect = DragDropEffects.Copy;

            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            object player = e.Data.GetData(typeof(ListBoxItem2));

            ListBoxItem2 lb = (ListBoxItem2)player;

            if (!is_listview(lb))
            {

                string servID = "";


                ListBoxItem lq = (ListBoxItem)listBox3.SelectedItem;
                if (lq.Tag.id.ToString().Contains("department")) servID = "/service/" + lb.Tag.serviceId;
                string[] st = new string[2];
                st[0] = lb.Text;
                st[1] = lb.Tag.id.ToString();

                ListViewItem lw = new ListViewItem(st);
                lw.Tag = "https://rmis52.cdmarf.ru/pp/group/" + lq.Tag.id + servID + "/resource/" + lb.Tag.id + "/planning/";
                listView1.Items.Add(lw);

                string new_value = "https://rmis52.cdmarf.ru/pp/group/" + lq.Tag.id + servID + "/resource/" + lb.Tag.id + "/planning/".Replace(Environment.NewLine, "");
                System.IO.File.AppendAllText(file_setting, lb.Tag.id + ";" + lb.Text + ";" + new_value + ";0" + Environment.NewLine, stan);



            }

        }


        bool is_listview(ListBoxItem2 lw)
        {
            foreach (ListViewItem l in listView1.Items)
            {
                if (lw.Tag.id.ToString() == l.SubItems[1].Text) return true;
            }

            return false;
        }


        void add_listview_day(int year, int month, string url_tag)
        {
            string url = url_tag + "/" + year + "/" + month + "?_salt=1463923646808";
            string data = stan.GetString(download(url));


            Example person_tmp = JsonConvert.DeserializeObject<Example>(data);
            if (person_tmp != null)
            {

                foreach (Planning ll in person_tmp.planning)
                {
                    if (!ll.disabled && ll.freeIntervalCount != 0)
                    {
                        total_day++;
                        total_one_vr++;
                    }

                }

                notifyIcon1.Text = total_day.ToString();
                if (total_day == 0)
                {
                    notifyIcon1.Icon = Properties.Resources.down_3949;

                }
                else
                {
                    notifyIcon1.Icon = Properties.Resources.stop;

                }
            } else
            {
                notifyIcon1.Text = "NULL";
            }

        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                date = DateTime.Now;
                ListViewItem lw = (ListViewItem)listView1.SelectedItems[0];

                string label = lw.Tag.ToString().Replace("/pp/group/", "/pp/#!/group/") + date.Year + "/" + date.Month + "/!";
                linkLabel1.Text = label;

            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.CheckedItems.Count != 0)
            {
                DialogResult result = MessageBox.Show("Точно удалить?", "", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    foreach (ListViewItem lw in listView1.CheckedItems)
                    {
                        delete_item(lw.SubItems[1].Text);
                        listView1.Items.Remove(lw);
                    }

                }
            }



        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
                timer1.Start();
                timer1_Tick(sender, e);
                
                notifyIcon1.Visible = true;
                if (total_day == 0)
                {
                    notifyIcon1.Icon = Properties.Resources.down_3949;
                } else
                {
                    notifyIcon1.Icon = Properties.Resources.stop;

                }
 
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.Show();
                notifyIcon1.Visible = false;
                timer1.Stop();
                
                refresh_list();

            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Show();
            this.WindowState = FormWindowState.Normal;


        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Произошла ошибка");
            }

            notifyIcon1.Text = total_day.ToString();


            done = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox mm = (CheckBox)sender;
            if (mm.Checked)
            {
                rkApp.SetValue("MyTalon", Application.ExecutablePath.ToString());

            }
            else
            {
                rkApp.DeleteValue("MyTalon", false);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            total_day = 0;
            done = false;
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            bg.RunWorkerAsync();
            while (!done)
            {
                Application.DoEvents();
            }







        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = proverka();


        }

        bool proverka()
        {
           
            if (File.Exists(file_setting))
            {
               
                string[] massiveOfString = System.IO.File.ReadAllLines(file_setting, stan);
                date = DateTime.Now;
                DateTime date_add = DateTime.Now.AddMonths(1);
                foreach (string g in massiveOfString)
                {
                    total_one_vr = 0;
                    if (g != "")
                    {
                        string[] words = g.Split(';');

                        add_listview_day(date.Year, date.Month, words[2]);
                        if (date_add.Month != date.Month || date_add.Year != date.Year) add_listview_day(date_add.Year, date_add.Month, words[2]);
                        set_item(words[0], total_one_vr.ToString());


                    }
                }


            }

            return true;
        }


        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(comboBox1.Items[comboBox1.SelectedIndex]) * 60000;


        }

        private void Form1_Shown(object sender, EventArgs e)
        {


            for (int m = 1; m < 60; m++)
            {
                comboBox1.Items.Add(m.ToString());
            }
            comboBox1.SelectedIndex = 0;


            date = DateTime.Now;

            Object cc = rkApp.GetValue("MyTalon");

            if (cc == null)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
                notifyIcon1.Visible = true;

                this.Hide();

                timer1.Start();
                timer1_Tick(sender, e);
            }

        }

    }
}
