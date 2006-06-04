using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using org.seasar.s2hessian.client;
using System.Collections;

namespace nethessian.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            DotNetProxy dnp = new DotNetProxy(textBox1.Text);




            //Test1
            int resi = (int)dnp.invoke("test", "intPlus", typeof(int), 300, 5);
            textBox2.Text += "Test1 intPlus result: " + resi.ToString() + "\r\n";

            //Test2
            Boolean bres = (Boolean)dnp.invoke("test", "boolTest", typeof(Boolean), false);
            textBox2.Text += "Test2 boolTest result: " + bres.ToString() + "\r\n";

            //Test3
            long lres = (long)dnp.invoke("test", "longPlus", typeof(long), 385555L, 1000000L);
            textBox2.Text += "Test3 longPlus result: " + lres.ToString() + "\r\n";

            //Test4
            double dres = (double)dnp.invoke("test", "doublePlus", typeof(double), 555.385555, 1000000.0);
            textBox2.Text += "Test4 doublePlus result: " + dres.ToString() + "\r\n";

            //Test5
            string sres = (string)dnp.invoke("test", "stringCat", typeof(string), "Seaser=", "Test");
            textBox2.Text += "Test5 stringCat result: " + sres + "\r\n";

            //Test6
            sres = (string)dnp.invoke("test", "dateCheck1", typeof(string), DateTime.Now);
            textBox2.Text += "Test6 dateCheck1 result: " + sres + "\r\n";

            //Test7
            DateTime dtres = (DateTime)dnp.invoke("test", "dateCheck2", typeof(DateTime));
            textBox2.Text += "Test7 dateCheck2 result: " + dtres.ToLongDateString() + ""
                + dtres.ToLongTimeString() + "\r\n";

            //Test8

            int[] it ={ 1, 2, 3, 4, 5 };
            int[] iares = (int[])dnp.invoke("test", "intArray", typeof(int[]), it);
            textBox2.Text += "Test8 intArray result: ";
            for (int i = 0; i < iares.Length; i++)
            {
                textBox2.Text += " " + iares[i].ToString();
            }
            textBox2.Text += "\r\n";

            //Test9
            Hashtable ht = new Hashtable();
            ht.Add("test1", 1);
            ht.Add("test2", "t2");
            Hashtable htres = (Hashtable)dnp.invoke("test", "hashTable", typeof(Hashtable), ht);
            textBox2.Text += "Test9 hashTable result: ";
            textBox2.Text += " " + htres["test1"].ToString();
            textBox2.Text += " " + htres["test2"].ToString();
            textBox2.Text += "\r\n";

            //Test10
            ArrayList al = new ArrayList();
            al.Add(100);
            al.Add("t200");
            ArrayList alres = (ArrayList)dnp.invoke("test", "arrayList", typeof(ArrayList), al);
            textBox2.Text += "Test10 arrayList result: ";
            textBox2.Text += " " + alres[0].ToString();
            textBox2.Text += " " + alres[1].ToString();
            textBox2.Text += "\r\n";

            //Test11
            //dnp.setClassConv(typeof(nethessian.Test.MyObject1),"org.seasar.s2hessian.example.MyObject");

            MyObject1 mo = new MyObject1();
            mo.setInt1(100);
            mo.setString1("Seasar");
            MyObject1 mores = (MyObject1)dnp.invoke("test", "myObjectTest", typeof(MyObject1), mo);
            textBox2.Text += "Test11 myObjectTest result: ";
            textBox2.Text += " " + mo.getInt1().ToString();
            textBox2.Text += " " + mo.getString1().ToString();
            textBox2.Text += "\r\n";

            //Test12
            dnp.setClassConv(typeof(nethessian.Test.MyObject2), "org.seasar.s2hessian.example.MyObject2");
            MyObject1 mon = new MyObject1();
            mon.setInt1(200);
            mon.setString1("Seasar3");
            MyObject2 mo2 = new MyObject2();
            mo2.setMo1(mon);
            MyObject2 mo2res = (MyObject2)dnp.invoke("test", "myObjectTest2", typeof(MyObject2), mo2);
            MyObject1 mofrom2 = new MyObject1();
            mofrom2 = mo2res.getMo1();
            textBox2.Text += "Test12 myObjectTest2result: ";
            textBox2.Text += " " + mofrom2.getInt1().ToString();
            textBox2.Text += " " + mofrom2.getString1().ToString();
            textBox2.Text += "\r\n";

            //Test13
            al = new ArrayList();
            al.Add(mon);
            al.Add(mon);
            ArrayList alres2 = (ArrayList)dnp.invoke("test", "arrayList", typeof(ArrayList), al);
            textBox2.Text += "Test13 arrayList result: ";
            textBox2.Text += " " + alres2[0].ToString();
            textBox2.Text += " " + alres2[1].ToString();
            Boolean bxres = (alres2[0] == alres2[1]);
            textBox2.Text += " " + bxres.ToString();
            textBox2.Text += "\r\n";


            //Test14
            Hashtable hm = new Hashtable();
            hm.Add("test1", 1);
            hm.Add("test2", "t2");
            hm.Add("HM", hm);
            hm.Add("mon1", mon);
            hm.Add("mon2", mon);
            Hashtable hmres = (Hashtable)dnp.invoke("test", "hashMap", typeof(Hashtable), hm);
            textBox2.Text += "Test14 hashTable result: ";
            textBox2.Text += " " + hmres["test1"].ToString();
            textBox2.Text += " " + hmres["test2"].ToString();
            textBox2.Text += " " + hmres["HM"].ToString();
            textBox2.Text += " " + hmres["mon1"].ToString();
            textBox2.Text += " " + hmres["mon2"].ToString();
            textBox2.Text += "\r\n";

            //Test15
            SqlDate sqldt = new SqlDate();
            sqldt.setValue(DateTime.Now);
            SqlDate dt = (SqlDate)dnp.invoke("test", "SqlDateTest", typeof(SqlDate), sqldt);
            DateTime dt2 = dt.getValue();
            textBox2.Text += "Test15 SqlDate 2006/06/04 will return result: ";
            textBox2.Text += dt2.ToString();
            textBox2.Text += "\r\n";
        }
    }
}