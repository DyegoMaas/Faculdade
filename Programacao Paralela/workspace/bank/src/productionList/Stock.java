package productionList;

import java.util.Stack;

public class Stock {

	private Stack<String> productsStack;
	
	public Stock() {
		productsStack = new Stack<String>();
	}
	
	public synchronized void put(String product){
		System.out.println("Product was stored: " + product);
		productsStack.push(product);
	}
	
	public synchronized String get(){
		String product = productsStack.pop();
		System.out.println("Product was removed: " + product);
		return product;
	}
	
	public int getStackSize(){
		return productsStack.size();
	}
}
