/*
 * Created on 2004/11/30
 *
 * To change the template for this generated file go to
 * Window&gt;Preferences&gt;Java&gt;Code Generation&gt;Code and Comments
 */
package org.seasar.s2hessian.example;

import java.util.ArrayList;
import java.util.Date;
import java.util.Hashtable;

import org.seasar.framework.container.S2Container;
import org.seasar.framework.container.factory.S2ContainerFactory;

import org.seasar.s2hessian.example.SeasarTest;
/**
 * @author shimura
 *
 * To change the template for this generated type comment go to
 * Window&gt;Preferences&gt;Java&gt;Code Generation&gt;Code and Comments
 */
public class TestAop {

    public static void main(String[] args) {
        
		S2Container container  = S2ContainerFactory.create("app.dicon");
		
		SeasarTest seasarTest = (SeasarTest)container.getComponent("test");
	    /* Test1 */
	    int ires=seasarTest.intPlus(30,50);
	    System.out.println("Test1 intPlus result: " +ires);
	    
	    /* Test2 */
	    boolean bres=seasarTest.boolTest(true);
	    System.out.println("Test2 boolTest result: " +bres);
	    
	    /* Test3 */
	    long lres=seasarTest.longPlus(222222L,4000000L);
	    System.out.println("Test3 longPlus result: " +lres);   
	    
	    /* Test4 */
	    double dres=seasarTest.doublePlus(111.111111,2000000.0);
	    System.out.println("Test4 doublePlus result: " +dres); 
	    
	    /* Test5 */
	    String sres=seasarTest.stringCat("SeaserAOP=","TestCase");
	    System.out.println("Test5 stringCat result: " +sres);
	    
	    /* Test6 */ 
	    sres=seasarTest.dateCheck1(new Date());
	    System.out.println("Test6 dateCheck1 result: " +sres);
	    
	    /* Test7 */ 
	    Date dtres=seasarTest.dateCheck2();
	    System.out.println("Test7 dateCheck2 result: " +dtres);
	    
	    /* Test8 */
	    int it[]={5,4,3,2,1};
	    int[] iares=seasarTest.intArray(it);
	    System.out.print("Test8 intArray result: ");
	    for (int i=0;i<5;i++) {
	        System.out.print(" "+iares[i]);
	    }
	    System.out.println();
	    
	    /* Test9 */
	    Hashtable ht=new Hashtable();
	    ht.put("test1",new Integer(100));
	    ht.put("test2","t200");
	    Hashtable htres=seasarTest.hashTable(ht);
	    System.out.println("Test9 Hashtable result: " +htres);    
	    
	    /* Test10 */
	    ArrayList al=new ArrayList();
	    al.add(new Integer(100));
	    al.add("t200");
	    ArrayList alres=seasarTest.arrayList(al);
	    System.out.println("Test10 ArrayList result: " +alres); 
	    
	    /* Test11 */
	    MyObject mo=new MyObject();
	    mo.setInt1(1);
	    mo.setString1("Seasar");
	    MyObject mores=seasarTest.myObjectTest(mo);
	    System.out.println("Test11 MyObjectTest result: "
	            + mores.getString1() + " " + mores.getInt1());

		S2Container containerb  = S2ContainerFactory.create("burlap.dicon");
		
		SeasarTest seasarTestb = (SeasarTest)containerb.getComponent("test");	    
		/* Test21 */
		ires=seasarTestb.intPlus(83,5);
		System.out.println("BurlapTest21 intPlus result: " +ires);
	    
		/* Test22 */
	    bres=seasarTestb.boolTest(false);
	    System.out.println("BurlapTest22 boolTest result: " +bres);
	    
	    /* Test23 */
	    lres=seasarTestb.longPlus(385555L,1000000L);
	    System.out.println("BurlapTest23 longPlus result: " +lres);   
	    
	    /* Test24 */
	    dres=seasarTestb.doublePlus(555.385555,1000000.0);
	    System.out.println("BurlapTest24 doublePlus result: " +dres); 
	    
	    /* Test25 */
	    sres=seasarTestb.stringCat("Seaser=","Test");
	    System.out.println("BurlapTest25 stringCat result: " +sres);
	    
	    /* Test26 */ 
	    sres=seasarTestb.dateCheck1(new Date());
	    System.out.println("BurlapTest26 dateCheck1 result: " +sres);
	    
	    /* Test27 */ 
	    dtres=seasarTestb.dateCheck2();
	    System.out.println("BurlapTest27 dateCheck2 result: " +dtres);
	    
	    /* Test28 */
	    int itb[]={1,2,3,4,5};
	    iares=seasarTestb.intArray(itb);
	    System.out.print("BurlapTest28 intArray result: ");
	    for (int i=0;i<5;i++) {
	        System.out.print(" "+iares[i]);
	    }
	    System.out.println();
	    
	    /* Test 29*/
	    ht=new Hashtable();
	    ht.put("test1",new Integer(1));
	    ht.put("test2","t2");
	    htres=seasarTestb.hashTable(ht);
	    System.out.println("BurlapTest29 Hashtable result: " +htres);    
	    
	    /* Test30 */
	    al=new ArrayList();
	    al.add(new Integer(1));
	    al.add("t2");
	    alres=seasarTestb.arrayList(al);
	    System.out.println("BurlapTest30 ArrayList result: " +alres); 
	    
	    /* Test31 */
	    mo=new MyObject();
	    mo.setInt1(100);
	    mo.setString1("Seasar");
	    mores=seasarTest.myObjectTest(mo);
	    System.out.println("BurlapTest31 MyObjectTest result: "
	            + mores.getString1() + " " + mores.getInt1());		

    }
}
