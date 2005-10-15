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
public class SeasarTestImpl implements SeasarTest {

    /* (non-Javadoc)
     * @see org.seasar.hessian.test.SeasarTest#intPlus(int, int)
     */
    public int intPlus(int i, int j) {

        return i+j;
    }

    /* (non-Javadoc)
     * @see org.seasar.hessian.test.SeasarTest#boolTest(boolean)
     */
    public boolean boolTest(boolean b){
        return !b;
    }

    /* (non-Javadoc)
     * @see org.seasar.hessian.test.SeasarTest#longPlus(long, long)
     */
    public long longPlus(long x, long y) {

        return x+y;
    }

    /* (non-Javadoc)
     * @see org.seasar.hessian.test.SeasarTest#doublePlus(double, double)
     */
    public double doublePlus(double x, double y) {

        return x+y;
    }

    /* (non-Javadoc)
     * @see org.seasar.hessian.test.SeasarTest#dateCheck(java.util.Date)
     */
    public String dateCheck1(Date d) {

        int m = d.getMonth();
        int dd = d.getDate();
        int h=d.getHours();
        int mm=d.getMinutes();
        String dds=d.toLocaleString();
        return dds;
    }

    /* (non-Javadoc)
     * @see org.seasar.hessian.test.SeasarTest#stringCat(java.lang.String, java.lang.String)
     */
    public String stringCat(String s1, String s2) {
        return s1+s2;
    }

    /* (non-Javadoc)
     * @see org.seasar.hessian.test.SeasarTest#argInt(int[])
     */
    public int[] intArray(int[] in) {

        return in;
    }

    /* (non-Javadoc)
     * @see org.seasar.hessian.test.SeasarTest#hashTable(java.util.Hashtable)
     */
    public Hashtable hashTable(Hashtable ht) {
        return ht;
    }
    public ArrayList arrayList(ArrayList ar){
        return ar;
    }
    /* (non-Javadoc)
     * @see org.seasar.s2hessian.example.SeasarTest#dateCheck2()
     */
    public Date dateCheck2() {
        Date nd=new Date();
        return nd;
    }
    /* (non-Javadoc)
     * @see org.seasar.s2hessian.example.SeasarTest#myObjectTest(org.seasar.s2hessian.example.MyObject)
     */
    public MyObject myObjectTest(MyObject mx) {

        return mx;
    }
    /* (non-Javadoc)
     * @see org.seasar.s2hessian.example.SeasarTest#myObjectTest2(org.seasar.s2hessian.example.MyObject2)
     */
    public MyObject2 myObjectTest2(MyObject2 mo2) {
        MyObject2 mores= new MyObject2();
        MyObject mo= new MyObject();
        mo=mo2.getMo1();
        mores.setMo1(mo);
        return mores;

    }

    /* (non-Javadoc)
     * @see org.seasar.s2hessian.example.SeasarTest#hashMap(java.util.HashMap)
     */
    public HashMap hashMap(HashMap ht) {
        return ht;
    }
    public BigDecimal bigDecimal(BigDecimal bd){
        return bd;
    }
    public Object[] ObjectArrrayTest(Object[] ot){
        return ot;
    }
    public MyObject[] MyObjectArrrayTest(MyObject[] ot){
        return ot;
    }
}

