/*
 * Created on 2004/10/09
 *
 * To change the template for this generated file go to
 * Window&gt;Preferences&gt;Java&gt;Code Generation&gt;Code and Comments
 */
package org.seasar.s2hessian.example;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.Hashtable;
/**
 * @author shimura
 *
 * To change the template for this generated type comment go to
 * Window&gt;Preferences&gt;Java&gt;Code Generation&gt;Code and Comments
 */
public interface SeasarTest {
	
    public int intPlus(int i,int j);
    public boolean boolTest(boolean b);
    public long longPlus(long x,long y);
    public double doublePlus(double x,double y);
    public String dateCheck1(Date d);
    public Date dateCheck2();
    public String stringCat(String s1,String s2);
    public int[] intArray(int[] in);
    public Hashtable hashTable(Hashtable ht);
    public ArrayList arrayList(ArrayList ar);
    public MyObject myObjectTest(MyObject mx);
    public MyObject2 myObjectTest2(MyObject2 mo2);
    public HashMap hashMap(HashMap ht);
    public BigDecimal bigDecimal(BigDecimal bd);
    public Object[] ObjectArrrayTest(Object[] ot);
}
